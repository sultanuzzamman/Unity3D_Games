using UnityEngine;
using System.Collections;

public class OGTController : MonoBehaviour
{

	public GameObject Golie;
	public BallScript ballScript;
	float camDistance=60;
	public GameObject opactcam;
	public AI_Striker ppp;
	public GameObject halfTimeDialog,matchCompleteDialog;

	float lastTriggerTime = 0f;
	
	void Start()
	{
		ballScript = GameObject.FindGameObjectWithTag("TheSoccerBall").GetComponent<BallScript>();
	}
//
	void StartPlay()
	{
		if(!halfTimeDialog.activeSelf&&!matchCompleteDialog.activeSelf)
		{

		ppp.MakeAPassMethod ();
			GameManager.SharedObject().isTimeActive=true;
		GameManager.SharedObject ().IsGameReady = true;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		GameManager.SharedObject ().isTimeActive = false;
		GameManager.SharedObject().IsGameReady = false;

		//camDistance += Time.deltaTime*9075;
		//Camera.main.GetComponent<SmoothFollow> ().distance = camDistance;
		//Camera.main.GetComponent<SmoothFollow> ().height=9f;
		//Camera.main.fieldOfView=30;
		opactcam.SetActive (true);
		opactcam.GetComponent<Camera>().fieldOfView=30;
		GameManager.SharedObject().IsGameReady = false;

		Invoke ("playSound",0.2f);

		//Invoke ("Reset",2.5f);

	}

	void playSound()
	{
		GameManager.SharedObject().IsGameReady = false;
		AudioManager.PlayOnGoalRoar();
	
		Invoke ("Reset",1.0f);
	}

	void Reset()
	{
		if(Time.time - lastTriggerTime > 1)
		{
			GameManager.SharedObject().IsGameReady = false;
			opactcam.SetActive (false);

			//Camera.main.fieldOfView=40;
			//Camera.main.GetComponent<SmoothFollow> ().height=13f;
			GoalCeleberationManager.PlayCeleberation(0);
			ballScript.isKicked = false;
			
			lastTriggerTime = Time.time;
			GameManager.SharedObject().playerTeamGoals += 1;

			PlayerPosition.PlayerTurn = false;
			//Golie.GetComponent<OpponentGolie>().enabled = false;
			//Golie.GetComponent<OpponentGolieKick>().enabled = true;
			ballScript.PlaceOnInitialPositon();

			Invoke("StartPlay",7f);

		}
	}

}
