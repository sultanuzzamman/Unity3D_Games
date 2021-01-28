using UnityEngine;
using System.Collections;

public class GUI_MainMenu : MonoBehaviour
{
	public GameObject quitDialog;
	public GameObject btn_no;

	//	public Texture background;
	//	public GUIStyle playButton, moreButton, rateButton;

	// Use this for initialization
	void Start ()
	{
// arif		AdsManager.SharedObject().ShowBanner();
		AudioManager.PlayBackgroundMusic ();
//		audio.PlayOneShot (whistle);

	}

	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.Escape)) {

			btn_no.GetComponent<GUITexture> ().color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
			quitDialog.SetActive (!quitDialog.activeInHierarchy);
			if (quitDialog.activeSelf) {
				AudioListener.volume = 0;
				#if !UNITY_EDITOR
				AdsManager.Instance.ShowAdmobUnityHeyzap ();//sultanad
				Time.timeScale = 1;
				#endif
			} else {
				if (AudioManager.isMusicOn)
					AudioListener.volume = 1;

			}
		}
	}

	//	void OnGUI ()
	//	{
	//		GUI.DrawTexture (new Rect(0f,0f,Screen.width,Screen.height), background);
	//
	//		float y = Screen.height / 2 - GetValue (155f) / 2 - GetValue (155f);
	//
	//		if(GUI.Button(new Rect(0f,y,GetValue(606f),GetValue(155f)),"",playButton))
	//		{
	//
	//		}
	//
	//		y += GetValue (155f);
	//		if(GUI.Button(new Rect(0f,y,GetValue(606f),GetValue(155f)),"",moreButton))
	//		{
	//
	//		}
	//
	//		y += GetValue (155f);
	//		if(GUI.Button(new Rect(0f,y,GetValue(606f),GetValue(155f)),"",rateButton))
	//		{
	//
	//		}
	//	}
	//
	//	float GetValue(float value)
	//	{
	//		return value * Screen.height / 1024f;
	//	}
}
