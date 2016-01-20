using UnityEngine;
using System.Collections;

public class DotImageScene : MonoBehaviour {

	float angle = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		angle += Time.deltaTime * 30;
		Camera.main.transform.position = Quaternion.AngleAxis(angle,new Vector3(0,1,0)) * new Vector3 (8, 0, 0) + new Vector3(0,4,0);
		Camera.main.transform.LookAt (Vector3.zero);
	}
}
