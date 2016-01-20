using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/DotImageEffect2")]
public class DotImageEffect2 : ImageEffectBase
{
	[Range(1, 10)]
	public int Downsample = 3;  // 解像度を落とす回数+1。1回ごとに解像度を半分に落とす。
	
	[Range(0.00001f, 1.0f)]
	public float ValidDepthThreshold = 0.01f;  // 深度が近いドットを平均化するときの閾値
	
	[Range(0, 5f)]
	public float EdgeLuminanceThreshold = 0.8f; // エッジ部と判定するための輝度差。この値以上離れていたらエッジとみなす。
	
	[Range(0, 1f)]
	public float SharpEdge = 0.5f;  // 平均化する際のコントラスト強調パラメータ
	
	[Range(-5,5f)]
	public float SampleDistance =1f;  // テクスチャサンプリング距離
	
	public List<float> SliceDepth;
	
	public RenderTexture[] RT;
	void OnEnable()
	{
		// 深度の取得を有効にする
		GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
	}
	
	[ImageEffectOpaque]
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//Debug.Log ("s" + source.GetInstanceID () + " " + destination.GetInstanceID ());
		material.SetFloat("_ValidDepthThreshold", ValidDepthThreshold);
		material.SetFloat("_EdgeLuminanceThreshold", EdgeLuminanceThreshold);
		material.SetFloat("_SharpEdge", SharpEdge);

		//var rt = RenderTexture.GetTemporary (source.width / 2, source.height / 2);
		//Graphics.Blit(source, rt2, material, 0);

		var rt2 = new RenderTexture (source.width, source.height, 0);
		rt2.enableRandomWrite = false;
		//rt2.filterMode = FilterMode.Point;
		rt2.filterMode = FilterMode.Point;
		Graphics.Blit(source, rt2, material, 0);

		//var rt3 = new RenderTexture (source.width/2, source.height/2, 0);
		//rt3.enableRandomWrite = false;
		//rt3.filterMode = FilterMode.Point;
		//Graphics.Blit(rt2, rt3, material, 0);

		Graphics.Blit(rt2, destination, material, 1);
		rt2.Release ();
		//rt3.Release ();

	}
	
}
