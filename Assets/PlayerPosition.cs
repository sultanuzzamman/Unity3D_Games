using UnityEngine;
using System.Collections;

public class PlayerPosition : MonoBehaviour
{
	public GUIStyle passButtonStyle;
//	public Vector3 direction;
	public static bool PlayerTurn = true;

	public Transform InitialPositonTransform, SecondaryPositonTransform;
	private Vector3 InitialPosition, SecondaryPosition;

//	private Player playerScript;

	public Transform passingPlayer;
	public Vector3 dir;
	GameObject ball;

	void Start ()
	{
		ball = GameObject.FindGameObjectWithTag("TheSoccerBall");
//		playerScript = InitialPositonTransform.GetComponent<Player> ();
		InitialPosition = InitialPositonTransform.position;
		SecondaryPosition = SecondaryPositonTransform.position;

		if(PlayerTurn){

			transform.position = InitialPosition;
		}
		else {

			transform.position = SecondaryPositonTransform.position;
		}
	}
	
	void Update ()
	{

	}
	void gr()
	{
		GameManager.SharedObject().IsGameReady = true;
	}

	void OnGUI()
	{
		if(!PauseController.isPaused)
		{


		if(PlayerTurn && GameManager.SharedObject().IsGameReady == false && Vector3.Distance(transform.position,ball.transform.position)<1.5f)
		{
			if(GUI.Button(new Rect (Screen.width - GetValue(150), Screen.height - GetValue(150) - GetValue(130), GetValue(110), GetValue(110)),"",passButtonStyle))
			{
					StartCoroutine(initialPass());
				//GameManager.SharedObject().IsGameReady = true;
			}
		}
		}


	
		//GUI.Label (new Rect (Screen.width / 2, Screen.height / 2, 300, 20), "player Position: "+playerScript.initialPosition);
		//GUI.Label (new Rect (Screen.width / 2, Screen.height / 2, 300, 20), "player Position: "+dir);

	}

	float GetValue(float value)
	{
		return value * Screen.height/640f;
	}
	IEnumerator initialPass()
	{
		Vector3 direction = (passingPlayer.position-ball.transform.position).normalized;
		//dir=direction+new Vector3(1,1,1);
		if (GetComponent<Animation>() ["pase"].enabled == false)
			GetComponent<Animation>().Play ("pase", PlayMode.StopAll);
		//					Invoke("initialPass",0.3f);
		
		yield return new WaitForSeconds (0.3f);
		ball.GetComponent<Rigidbody>().AddForce(direction*1200, ForceMode.Impulse);
		AudioManager.PlayResumeWhistle();
		GameManager.SharedObject().isTimeActive=true;
		Invoke ("gr",0.25f);
	}
}
