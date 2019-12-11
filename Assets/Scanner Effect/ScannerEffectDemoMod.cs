using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class ScannerEffectDemoMod : MonoBehaviour {
	
	public GameObject indicator;
	public Material effectMaterial;
	public float scanDistance;
	public float scanWidth;
	public Color color;
	private Camera _camera;
	private Transform scannerOrigin;
	


	void Update() {
		
		scannerOrigin = indicator.transform;
		
	}

	void OnEnable()
	{
		_camera = GetComponent<Camera>();
		_camera.depthTextureMode = DepthTextureMode.Depth;
	}

	[ImageEffectOpaque]
	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		 
		Matrix4x4 rotationMatrix = Matrix4x4.Rotate(indicator.transform.rotation);
		Matrix4x4 translationMatrix = Matrix4x4.Translate(indicator.transform.position);
		Matrix4x4 composition =  rotationMatrix.inverse * translationMatrix;
		
		effectMaterial.SetMatrix("_TransformationMatrix",composition);
		effectMaterial.SetFloat("_ScanDistance", scanDistance);
		effectMaterial.SetFloat("_ScanWidth", scanWidth);
		effectMaterial.SetColor("_Color",color);
		
		effectMaterial.SetVector("_WorldSpaceScannerPos", indicator.transform.position);
		
		
		RaycastCornerBlit(src, dst, effectMaterial);
	}

	void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat) {
		// Compute Frustum Corners
		float camFar = _camera.farClipPlane;
		float camFov = _camera.fieldOfView;
		float camAspect = _camera.aspect;

		float fovWHalf = camFov * 0.5f;

		Vector3 toRight = camAspect * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) *_camera.transform.right ;
		Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
		float camScale = topLeft.magnitude * camFar;

		topLeft.Normalize();
		topLeft *= camScale;

		Vector3 topRight = (_camera.transform.forward + toRight + toTop);
		topRight.Normalize();
		topRight *= camScale;

		Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= camScale;

		Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= camScale;

		// Custom Blit, encoding Frustum Corners as additional Texture Coordinates
		RenderTexture.active = dest;

		mat.SetTexture("_MainTex", source);

		GL.PushMatrix();
		GL.LoadOrtho();

		mat.SetPass(0);

		GL.Begin(GL.QUADS);

		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.MultiTexCoord(1, bottomLeft);
		GL.Vertex3(0.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.MultiTexCoord(1, bottomRight);
		GL.Vertex3(1.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.MultiTexCoord(1, topRight);
		GL.Vertex3(1.0f, 1.0f, 0.0f);

		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.MultiTexCoord(1, topLeft);
		GL.Vertex3(0.0f, 1.0f, 0.0f);

		GL.End();
		GL.PopMatrix();
	}
}
