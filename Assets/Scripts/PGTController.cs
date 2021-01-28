using UnityEngine;
using System.Collections;

public class PGTController : MonoBehaviour
{
	float lastTriggerTime = 0f;
	public GameObject Golie;
	public BallScript ballScript;
	float camDistance=60;
	public GameObject actcamera;


	public GameObject starterPlayer;

	void Start()
	{
		ballScript = GameObject.FindGameObjectWithTag("TheSoccerBall").GetComponent<BallScript>();
	}

	void StartPlay()
	{

		//GameManager.SharedObject().IsGameReady = true;
	}


	void Reset()
	{
		if(Time.time - lastTriggerTime > 5)
		{
			actcamera.SetActive(false);

			//Camera.main.GetComponent<SmoothFollow> ().height=13f;
			//Camera.main.fieldOfView=40;

			GoalCeleberationManager.PlayCeleberation(1);
			
			
			ballScript.isKicked = false;
			
			lastTriggerTime = Time.time;
			GameManager.SharedObject().opponentTeamGoals += 1;
			PlayerPosition.PlayerTurn = true;
		
			ballScript.PlaceOnInitialPositon();
			starterPlayer.GetComponent<PlayerPosition>().enabled = true;

			
//			Invoke("StartPlay",7f);
			
			//			Invoke("StartPlay",5f);
		}
	}



//	void StartPlay()
//	{
//		GameManager.SharedObject().IsGameReady = true;
//	}

	void OnTriggerEnter(Collider other)
	{
		actcamera.SetActive(true);
		actcamera.GetComponent<SmoothFollow>().distance=15;
		actcamera.GetComponent<Camera>().fieldOfView=25;
		//Camera.main.GetComponent<SmoothFollow> ().height=7f;
		//camDistance += Time.deltaTime*(1500);
		//Camera.main.fieldOfView=30;
		//Camera.main.GetComponent<SmoothFollow> ().distance = camDistance;
	//	Camera.main.GetComponent<SmoothFollow> ().height=9f;
		print (camDistance);
		AudioManager.PlayOnGoalRoar();
		GameManager.SharedObject ().isTimeActive = false;
		GameManager.SharedObject().IsGameReady = false;
		Invoke ("Reset",1.0f);

	}
}