using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

//シーン遷移時のフェードイン・アウトを制御するためのクラス
public class FadeManager : MonoBehaviour
{

	#region Singleton

	private static FadeManager instance;

	public static FadeManager Instance {
		get {
			if (instance == null) {
				instance = (FadeManager)FindObjectOfType (typeof(FadeManager));

				if (instance == null) {
					Debug.LogError (typeof(FadeManager) + "is nothing");
				}
			}

			return instance;
		}
	}

	#endregion Singleton

	public bool DebugMode = true;
	//フェード中の透明度
	private float fadeAlpha = 0;
	//フェード中かどうか
	private bool isFading = false;
	//フェード色
	public Color fadeColor = Color.black;


	public void Awake ()
	{
		if (this != Instance) {
			Destroy (this.gameObject);
			return;
		}

		DontDestroyOnLoad (this.gameObject);
	}

	public void OnGUI ()
	{

		if (this.isFading) {
			//色と透明度を更新して白テクスチャを描画
			this.fadeColor.a = this.fadeAlpha;
			GUI.color = this.fadeColor;
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
		}
			
	}

	//画面遷移
	//シーン名
    //暗転にかかる時間(秒)
	public void LoadScene (string scene, float interval)
	{
		StartCoroutine (TransScene (scene, interval));
	}

	//シーン遷移用コルーチン
	//シーン名
	//暗転にかかる時間(秒)
	private IEnumerator TransScene (string scene, float interval)
	{
		//だんだん暗く
		this.isFading = true;
		float time = 0;
		while (time <= interval) {
			this.fadeAlpha = Mathf.Lerp (0f, 1f, time / interval);
			time += Time.deltaTime;
			yield return 0;
		}

		//シーン切替
		SceneManager.LoadScene (scene);

		//だんだん明るく
		time = 0;
		while (time <= interval) {
			this.fadeAlpha = Mathf.Lerp (1f, 0f, time / interval);
			time += Time.deltaTime;
			yield return 0;
		}

		this.isFading = false;
	}
}