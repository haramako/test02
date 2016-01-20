using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DotSprite : MonoBehaviour {

	public MeshRenderer sprite;
	public GameObject target;
	public int pixels = 32;
	public float size = 4;
	public Vector3 origin = new Vector3(0,2,0);

	Quaternion backupRotation;
	RenderTexture texture;
	Material material;

	// Use this for initialization
	void Start () {
		if (sprite != null) {
			//material = new Material(sprite.material);
		}
	}
	
	void LateUpdate () {
		if (sprite != null) {
			backupRotation = sprite.transform.localRotation;
			if (Camera.main != null) {
				var t = sprite.transform;
				t.rotation = Camera.main.transform.rotation * Quaternion.AngleAxis(180, new Vector3(0,1,0)) * Quaternion.AngleAxis(90, new Vector3(1,0,0));
			}

			if( DotRenderer.hasInstance ){
				var dir = Camera.main.transform.forward;
				var ray = new Ray(origin - dir, dir);
				texture = DotRenderer.instance.RenderImage(target, pixels, size, ray);

				sprite.transform.localPosition = origin;
				sprite.transform.localScale = new Vector3(size/5,size/5,size/5);
				sprite.sharedMaterial.mainTexture = texture;
				texture.hideFlags = HideFlags.HideAndDontSave;
			}
		}
	}

	void OnPostRender(){
		//Debug.Log ("OnPostRender");
		if (sprite != null) {
			sprite.transform.localRotation = backupRotation;
			sprite.material.mainTexture = null;
		}
		if (texture != null) {
			RenderTexture.ReleaseTemporary (texture);
		}
	}
}
