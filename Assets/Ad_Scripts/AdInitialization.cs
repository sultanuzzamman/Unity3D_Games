using UnityEngine;
using System.Collections;

public class AdInitialization : MonoBehaviour
{
	[SerializeField]private string BannerId, InterstitialId, unityid, starapptid, heyzappid;
	//	[SerializeField]private string AppLovin_sdk_Key="dBoH-NJJ4I8JQWH0sCtnyaxFQLbQKfOysAEqTT8drGxGoCGeO8QKkMa8iAZ1rz-g8DVgmdIavDsgQ7KpFl8-5k";
	//
	[SerializeField]float loadingtime = 4;


	void Awake ()
	{
		Time.timeScale = 1;
//		StartApp.StartAppWrapper.applicationId=starapptid;
		AdsManager.Instance.InitAdmob (BannerId, InterstitialId);
		AdsManager.Instance.ShowBannerAtTop ();
		AdsManager.Instance.LoadInterstitial ();
		AdsManager.Instance.InitUnityadd (unityid);
//		AdsManager.Instance.InitStartApp();
//		AdsManager.Instance.InitHeyzappadd (heyzappid);
//		AdsManager.Instance.InitAppLovin (AppLovin_sdk_Key);
//		AdsManager.Instance.LoadAppLovin();
//		AdsManager.Instance.LoadApLovinRewardedVideo();
		Invoke ("ShowVideoAd", 0.5f);
//		Invoke ("ShowNextLevel", loadingtime);

	}

	void ShowBanner ()
	{
		AdsManager.Instance.ShowBannerAtBottom ();

	}

	void ShowNextLevel ()
	{

		//Application.LoadLevel (Application.loadedLevel + 1);

	}

	void ShowInterstitial ()
	{

		AdsManager.Instance.ShowInterstitial ();

	}

	void ShowVideoAd ()
	{
		AdsManager.Instance.ShowUnityAdd ();
	}

}
