using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TestScene : MonoBehaviour {

	public Camera texCamera;
	public GameObject elf;

	[HideInInspector]
	public RenderTexture tex;
	public MeshRenderer plane;
	Material mat;

	void Awake(){
		tex = new RenderTexture (32, 32, 24);
		//tex = new RenderTexture (256, 256, 24);
		tex.enableRandomWrite = false;
		//tex.filterMode = FilterMode.Bilinear;
		tex.filterMode = FilterMode.Point;
		//tex.antiAliasing = 2;
		texCamera.targetTexture = tex;
		mat = new Material( Shader.Find("Sprites/Default") );
		mat.mainTexture = tex;
		plane.material = mat;
	}

	// Use this for initialization
	void OnEnable () {

		//elf.transform.positionTo (4f, new Vector3 (0, 5, 0), true);

		//plane.material.mainTexture = tex;

		aa = Time.time;
	}

	void OnDisable(){
		if (tex != null) {
			tex.Release ();
			tex = null;
		}
	}

	float aa;
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
		if( !Application.isPlaying ) return;
#endif
		var anim = elf.GetComponent<Animation> ();
		foreach (AnimationState state in anim) {
			state.time = Mathf.Floor(Time.time * 8 ) / 8f;
		}
		if (aa > 1.0f) {
			aa -= 1.0f;
			elf.transform.Rotate (0, 360f/16, 0);
		}
		aa += Time.deltaTime;
	}
	
	void LateUpdate(){
		//texCamera.Render ();
		//texCamera.Render ();
		//texCamera.Render ();
	}
}
