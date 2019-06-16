using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView,
RequireComponent(typeof(ComputeShader)), RequireComponent(typeof(Light))]
public class RaymarchingManager : MonoBehaviour
{
  struct ShapeDataStruct
  {
    public int     shapeType;
    public int     blendType;
    public float   blendStrength;
    public Vector3 pos;
    public Vector3 scale;
    public Vector3 rotation;
    public Vector4 color;

    public static int GetBytes() { return sizeof(float)*(1 + 3*3 + 4) + sizeof(int)*2; }
  }

  [SerializeField] private Light         m_mainLight;
  [SerializeField] private ComputeShader m_raymarchingShader;

  private Camera                 m_camera;
  private RenderTexture          m_tmpRenderTex;
  private ComputeBuffer          m_shapesBuffer;
  private ShapeDataStruct[]      m_shapesData;
  private List<RayMarchingShape> m_shapes;

  void OnRenderImage(RenderTexture srcRenderTex, RenderTexture outRenderTex)
  {
    m_camera = Camera.current;
    CleanOrCreateRenderTexture();

    m_shapes = new List<RayMarchingShape>( FindObjectsOfType<RayMarchingShape>() );
    Debug.Log(m_shapes.Count + " shapes found");
    if (m_shapes.Count > 0)
    {
      // Convert shapes into structs and then to a compute buffer
      m_shapesData   = new ShapeDataStruct[m_shapes.Count];
      m_shapesBuffer = new ComputeBuffer(m_shapesData.Length, ShapeDataStruct.GetBytes());
      ProcessShapes(ref m_shapes, ref m_shapesData, ref m_shapesBuffer);

      SetupComputeShader(srcRenderTex, m_tmpRenderTex);


      // Launch kernel
      // Get the proper grid size
      int gridSizeX = Mathf.CeilToInt(m_camera.pixelWidth / 8.0f);
      int gridSizeY = Mathf.CeilToInt(m_camera.pixelHeight / 8.0f);
      // "Run" the compute shader
      m_raymarchingShader.Dispatch(/*Entry kernel:*/0, gridSizeX, gridSizeY, /*Num Blocks:*/1);

      // Copy the processed texture onto the output
      Graphics.Blit(m_tmpRenderTex, outRenderTex);

      // Clean buffer
      m_shapesBuffer.Dispose();
    }
    else {
      Graphics.Blit(srcRenderTex, outRenderTex);
    }
  }

  // Cleans the render texture or creates a new one if it doesn't exist
  void CleanOrCreateRenderTexture()
  {
    if (m_tmpRenderTex == null ||
        m_tmpRenderTex.width == m_camera.pixelWidth || m_tmpRenderTex.height == m_camera.pixelHeight)
    {
      if (m_tmpRenderTex != null) m_tmpRenderTex.Release();
 
      m_tmpRenderTex = new RenderTexture(m_camera.pixelWidth, m_camera.pixelHeight, 0,
                                         RenderTextureFormat.ARGBFloat,
                                         RenderTextureReadWrite.Linear);
      m_tmpRenderTex.enableRandomWrite = true;
      m_tmpRenderTex.Create();
    }
  }

  // Sorts and transforms the shapes into structs to being able to pass them as a buffer to the shader
  void ProcessShapes(ref List<RayMarchingShape> shapes, ref ShapeDataStruct[] data, ref ComputeBuffer buffer)
  {
    // Sort the shapes by blending operation (This way we should minimize the creation order bug)
    shapes.Sort((a,b) => a.GetBlendType().CompareTo(b.GetBlendType()));

    for (int i=0; i<shapes.Count; i++)
    {
      data[i].shapeType     = shapes[i].GetShapeType();
      data[i].blendType     = shapes[i].GetBlendType();
      data[i].blendStrength = shapes[i].GetBlendStrength();
      data[i].pos           = shapes[i].GetPos();
      data[i].scale         = shapes[i].GetScale();
      data[i].rotation      = shapes[i].GetRot();
      data[i].color         = shapes[i].GetColor();
    }

    buffer.SetData(data);
  }

  // Passes all the needed uniforms to the shader
  // TODO: Add more light types and the possibility of multiple lights
  void SetupComputeShader(RenderTexture srcTex, RenderTexture outTex)
  {
    // Pass the shapes buffer
    m_raymarchingShader.SetBuffer(0, "_shapes", m_shapesBuffer);
    m_raymarchingShader.SetInt("_numShapes", m_shapesData.Length);

    // Pass the needed matrices
    m_raymarchingShader.SetMatrix("_Camera2WorldMatrix", m_camera.cameraToWorldMatrix);
    m_raymarchingShader.SetMatrix("_InverseProjectionMatrix", m_camera.projectionMatrix.inverse);

    // Pass the textures
    m_raymarchingShader.SetTexture(0, "_srcTex", srcTex);
    m_raymarchingShader.SetTexture(0, "_outTex", outTex);

    // Set light(s) NOTE: For now, just directional light(s)
    m_raymarchingShader.SetVector("_lightDir", m_mainLight.transform.forward);
  }
}
