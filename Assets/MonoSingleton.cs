/// <summary>
/// Generic Mono singleton.
/// </summary>
using UnityEngine;

/// <summary>
/// シングルトン
/// Sceneのヒエラルキに最初から生成してあるのが前提のシングルトン。
/// ヒエラルキ内にない場合は、instanceはnullを返す。
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>{

	/// <summary>
	/// インスタンスを返す。
	/// インスタンスがない場合は、空のオブジェクトを作成してアタッチする。
	/// </summary>
	public static T instance { get; private set; }

	public static bool hasInstance { get { return instance != null; } }

	protected virtual void Awake() {
		instance = this as T;
	}

	protected virtual void OnDestroy() {
		if( instance == this ) instance = null;
	}
 
#if false && !UNITY_EDITOR
	/// デバッグ用の破棄確認
	~MonoSingleton() {
		Debug.Log("Finalize MonoSingleton<" + typeof(T) + ">");
	}
#endif

}

/// <summary>
/// 自動作成型シングルトン
/// ヒエラルキ内にない場合は、空のGameObjectに自分をアタッチして生成する
/// </summary>
public abstract class MonoAutoSingleton<T> : MonoBehaviour where T : MonoAutoSingleton<T> {
	static T _instance;
	public static bool HasInstance { get { return _instance != null; } }
	public static T instance { 
		get {
			if( _instance != null ) return _instance;
			_instance = new GameObject("MonoAutoSingleton<" + typeof(T).ToString() + ">", typeof(T)).GetComponent<T>();
			return _instance;
		} 
	}
	public static T Instance { get { return instance; } }

	protected virtual void OnDestroy() {
		_instance = null;
	}

}
