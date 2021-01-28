using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour
{

	public static bool isPaused = false;
	OtherDialoguesActive oda;
	// Use this for initialization
	void Start ()
	{
		Time.timeScale = 1f;
		isPaused = false;
		oda = GameObject.Find ("Main Camera").GetComponent<OtherDialoguesActive> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!oda.isOtherDialogueActive) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				isPaused = !isPaused;

				if (isPaused) {
					//AdsManager.SharedObject().ShowBanner();
					//AdsManager.SharedObject().ShowUnityADS();
					#if !UNITY_EDITOR
					AdsManager.Instance.ShowAdmobUnityHeyzap ();//sultanad
					#endif
				} else {
					//AdsManager.SharedObject().HideBanner();
//				AdsManager.SharedObject().HideAdmobInterstitial();
				}
			}
		}
		if (isPaused) {
			AudioListener.volume = 0;
			Time.timeScale = 0f;
			transform.position = new Vector3 (0.5f, 0.48f, 30);
		} else {
			AudioListener.volume = 1;
			Time.timeScale = 1f;
			transform.position = new Vector3 (3, 3, 5);
		}
	
	}

	public static void pause ()
	{
		isPaused = !isPaused;
	}
	//	void OnApplicationFocus(bool focusStatus) {
	//		isPaused = focusStatus;
	//	}
}
