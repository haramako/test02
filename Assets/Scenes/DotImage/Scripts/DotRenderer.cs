using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DotRenderer : MonoSingleton<DotRenderer> {

	public Camera dotCamera;

	// Use this for initialization
	void Start () {
		Debug.Log ("start");
	}

	public RenderTexture RenderImage(GameObject obj, int pixels, float size, Ray ray){
		var oldParent = obj.transform.parent;
		var oldPosision = obj.transform.localPosition;
		var oldScale = obj.transform.localScale;
		var oldRotation = obj.transform.localRotation;
		var oldLayer = obj.layer;

		RenderTexture tex = null;
		try {
			obj.transform.SetParent(null);
			obj.transform.localScale = Vector3.one;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.localPosition = Vector3.zero;
			obj.layer = LayerMask.NameToLayer("DotRenderer");

			tex = RenderTexture.GetTemporary(pixels,pixels,24);
			//tex.antiAliasing = 2;
			tex.filterMode = FilterMode.Point;
			dotCamera.targetTexture = tex;
			dotCamera.orthographicSize = size;
			dotCamera.transform.position=ray.origin;
			dotCamera.transform.LookAt(ray.origin + ray.direction, Vector3.up);
			dotCamera.Render();
			dotCamera.targetTexture = null;

		}catch{
			if( tex != null) RenderTexture.ReleaseTemporary(tex);
		}finally{
			obj.transform.SetParent (oldParent);
			obj.transform.localPosition = oldPosision;
			obj.transform.localScale = oldScale;
			obj.transform.localRotation = oldRotation;
			obj.layer = oldLayer;
		}

		return tex;
	}
}
