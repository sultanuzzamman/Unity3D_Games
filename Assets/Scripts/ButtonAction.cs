using UnityEngine;
using System.Collections;

public class ButtonAction : MonoBehaviour
{
	//	public GameObject player1;
	// arif	OtherDialoguesActive oda;
	void Start ()
	{



	}

	//	public GameObject matchCompleted, halfCompleted;
	public enum Buttons
	{
		Play,
		MoreGames,
		RateUs,
		QuickMatch,
		InternationalCup,
		Back,
		Next,
		PrevTeam,
		NextTeam,
		KickOff,
		MainMenu,
		PlaySecondHalf,
		Replay,
		YES_QUIT,
		NO_QUIT,
		Resume,
		Share,
		Pause,
		None}
	;

	public Buttons buttonType = Buttons.None;

	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.Escape)) {
			//backPressed ();
		}
		switch (buttonType) {
		case Buttons.QuickMatch:
			
			AudioManager.PlayButtonClickSound ();
			GameManager.SharedObject ().isQuickMatch = true;
			GameManager.SharedObject ().IsFirstHalf = true;
			PlayerPosition.PlayerTurn = true;
			Application.LoadLevel ("1stTeamSelection");

			break;
			
		case Buttons.InternationalCup:
			AudioManager.PlayButtonClickSound ();
			GameManager.SharedObject ().isQuickMatch = false;
			GameManager.SharedObject ().IsFirstHalf = true;
			PlayerPosition.PlayerTurn = true;
			
			if (MatchesSceneController.HasPendingCup ())
				Application.LoadLevel ("MatchesScene");
			else
				Application.LoadLevel ("1stTeamSelection");
			break;
		}

	}

	void OnMouseDown ()
	{
		GetComponent<GUITexture> ().texture = gameObject.GetComponent<ButtonController> ().hoverTexture;
	}



	//	void OnMouseUpAsButton()
	void OnMouseUpAsButton ()
	{
		GetComponent<GUITexture> ().texture = gameObject.GetComponent<ButtonController> ().normalTexture;

		switch (buttonType) {
		case Buttons.Pause:
			#if !UNITY_EDITOR
			AdsManager.Instance.ShowAdmobUnityHeyzap ();//sultanad
			#endif
			AudioManager.PlayButtonClickSound ();
			PauseController.isPaused = true;

	//		}
			break;
		case Buttons.Play:
						
			#if !UNITY_EDITOR
			AdsManager.Instance.ShowAdmobUnityHeyzap ();//sultanad
			Time.timeScale = 1;
			#endif
			AudioManager.PlayButtonClickSound ();

			Application.LoadLevel ("GameSelectionScene");

			break;

		case Buttons.MoreGames:
			AudioManager.PlayButtonClickSound ();
			Application.OpenURL ("https://play.google.com/store/apps/developer?id=SUZ+Games+Studio");
			break;

		case Buttons.RateUs:
			AudioManager.PlayButtonClickSound ();
			Application.OpenURL ("https://play.google.com/store/apps/details?id=com.ssggames.real.football.game.fifa.soccer");
			break;
		case Buttons.Share:
			AudioManager.PlayButtonClickSound ();
			break;

		

		case Buttons.Back:
			AudioManager.PlayButtonClickSound ();
			backPressed ();
			break;

		case Buttons.Next:
			#if !UNITY_EDITOR
			AdsManager.Instance.ShowAdmobUnityHeyzap ();//sultanad
			Time.timeScale = 1;
			#endif
			AudioManager.PlayButtonClickSound ();
			if (Application.loadedLevelName == "1stTeamSelection" && GameManager.SharedObject ().isQuickMatch)
				Application.LoadLevel ("2ndTeamSelection");
			else if (Application.loadedLevelName == "1stTeamSelection" && !GameManager.SharedObject ().isQuickMatch)
				Application.LoadLevel ("GroupsScene");
			else if (Application.loadedLevelName == "GroupsScene" && !GameManager.SharedObject ().isQuickMatch)
				Application.LoadLevel ("MatchesScene");
			else if (Application.loadedLevelName == "MatchesScene" && !GameManager.SharedObject ().isQuickMatch)
				Application.LoadLevel ("KickOffScene");
			else if (Application.loadedLevelName == "2ndTeamSelection")
				Application.LoadLevel ("KickOffScene");
			
			break;

		case Buttons.PrevTeam:
			AudioManager.PlayButtonClickSound ();
			if (Application.loadedLevelName == "1stTeamSelection")
				TeamSelectionController.teamIndex -= 1;
			else if (Application.loadedLevelName == "2ndTeamSelection") {
				if (TeamSelectionController.teamIndex + 1 == TeamSelectionController2.teamIndex) {
					TeamSelectionController2.teamIndex -= 2;
				} else {
					TeamSelectionController2.teamIndex -= 1;
				}	
			}
				
			break;

		case Buttons.NextTeam:
			AudioManager.PlayButtonClickSound ();
			if (Application.loadedLevelName == "1stTeamSelection")
				TeamSelectionController.teamIndex += 1;
			else if (Application.loadedLevelName == "2ndTeamSelection")
				TeamSelectionController2.teamIndex += 1;
			break;

		case Buttons.KickOff:
			
			AudioManager.PlayButtonClickSound ();
			PlayerPrefs.Save ();

			Application.LoadLevel ("MatchScene");
			#if !UNITY_EDITOR
			AdsManager.Instance.HideBanner ();
			#endif
			break;

		case Buttons.MainMenu:
			#if !UNITY_EDITOR
			AdsManager.Instance.ShowAdmobUnityHeyzap ();//sultanad
			AdsManager.Instance.ShowBannerAtTop ();
			Time.timeScale = 1;
			#endif

			AudioManager.PlayButtonClickSound ();
			InitGame.matchcomplete = false;
			InitGame.halfComplete = false;


			PauseController.isPaused = false;
			Time.timeScale = 1f;
			PlayerPosition.PlayerTurn = !PlayerPosition.PlayerTurn;
			if (Application.loadedLevelName == "FinalCeleberation") {
				Application.LoadLevel ("MainMenu");
				#if !UNITY_EDITOR
				AdsManager.Instance.ShowAdmobUnityHeyzap ();//sultanad
				Time.timeScale = 1f;
				#endif

			} else if (GameManager.SharedObject ().isQuickMatch == false && PlayerPrefs.GetInt ("matchNumber") > 7) {
				PlayerPrefs.SetInt ("HasPendingCup", 0);
				Application.LoadLevel ("FinalCeleberation");
			} else if (GameManager.SharedObject ().isQuickMatch == false && PlayerPrefs.GetInt ("matchNumber") < 7) {
				//	AdsManager.SharedObject().HideBanner();
				AudioManager.StopAudienceSound ();
				AudioManager.StopOnGoalRoar ();
				Application.LoadLevel ("MatchesScene");
			} else {
				//	AdsManager.SharedObject().ShowBanner();
				//ads AdsAPI.instance.PauseMenuAD();
				AudioManager.StopOnGoalRoar ();
				Application.LoadLevel ("MainMenu");
			}

			//if(GameManager.SharedObject().isQuickMatch)	Application.LoadLevel("MainMenu");
			//else	Application.LoadLevel("MatchesScene");
			//ads AdsAPI.instance.PauseMenuAD();
			break;

		case Buttons.PlaySecondHalf:
			AudioManager.PlayButtonClickSound ();
			InitGame.halfComplete = false;
//			AdsManager.SharedObject().HideBanner();
			PlayerPosition.PlayerTurn = false;
//			player1.GetComponent<PlayerPosition>().enabled=false;//////////////****************\\\\\\\\\\\\\\\\\
			GameManager.SharedObject ().GameTime = 0;
			GameManager.SharedObject ().IsFirstHalf = false;
			GameManager.SharedObject ().IsGameReady = true;

			if (GameManager.SharedObject ().isQuickMatch) {
				Application.LoadLevel ("MatchScene");
			}
				//Application.LoadLevel("MatchScene");
			else
				Application.LoadLevel ("KickOffScene");
			break;

		case Buttons.Replay:

			AudioManager.PlayButtonClickSound ();
			//sultan ManagerAds.Instance.AdmobDestroyBannerAd();
			//sultan ManagerAds.Instance.AdmobDestroyNativeAd();
			GameManager.SharedObject ().IsFirstHalf = true;
			GameManager.SharedObject ().IsGameReady = true;
			GameManager.SharedObject ().ShowHalfTimeDialog = false;
			GameManager.SharedObject ().ShowMatchEndDialog = false;
			GameManager.SharedObject ().playerTeamGoals = 0;
			GameManager.SharedObject ().opponentTeamGoals = 0;
			Application.LoadLevel ("MatchScene");
			break;

		case Buttons.YES_QUIT:
			AudioManager.PlayButtonClickSound ();
			Application.Quit ();
			break;

		case Buttons.NO_QUIT:
			AudioManager.PlayButtonClickSound ();
			if (AudioManager.isMusicOn)
				AudioListener.volume = 1;
			GameObject.Find ("QuitDialog").SetActive (false);
			break;

		case Buttons.Resume:

			AudioManager.PlayButtonClickSound ();
			PauseController.isPaused = false;
			break;
		}
	}

	public void quickMode ()
	{
		buttonType = Buttons.QuickMatch;
	}

	public void worldCup ()
	{
		buttonType = Buttons.InternationalCup;
	}

	bool adshow;

	public void backPressed ()
	{
		#if !UNITY_EDITOR
		AdsManager.Instance.ShowAdmobUnityHeyzap ();//sultanad
		Time.timeScale = 1;
		#endif
		adshow = true;
		if (adshow) {
			
			adshow = false;
		}
		AudioManager.PlayButtonClickSound ();

		if (Application.loadedLevelName == "GameSelectionScene") {
			Application.LoadLevel ("MainMenu");
		} else if (Application.loadedLevelName == "1stTeamSelection") {
			Application.LoadLevel ("GameSelectionScene");
		} else if (Application.loadedLevelName == "2ndTeamSelection") {
			Application.LoadLevel ("1stTeamSelection");
		} else if (Application.loadedLevelName == "MatchesScene" && MatchesSceneController.HasPendingCup ()) {
			Application.LoadLevel ("GameSelectionScene");
		} else if (Application.loadedLevelName == "KickOffScene" && !GameManager.SharedObject ().isQuickMatch && MatchesSceneController.HasPendingCup ()) {
			Application.LoadLevel ("MatchesScene");
		} else if (Application.loadedLevelName == "KickOffScene" && !GameManager.SharedObject ().isQuickMatch && !MatchesSceneController.HasPendingCup ()) {
			Application.LoadLevel ("1stTeamSelection");
		} else if (Application.loadedLevelName == "KickOffScene" && GameManager.SharedObject ().isQuickMatch) {
			Application.LoadLevel ("2ndTeamSelection");
		} else if (Application.loadedLevelName == "GroupsScene" && !GameManager.SharedObject ().isQuickMatch) {
			Application.LoadLevel ("1stTeamSelection");
		}
			
	}
}