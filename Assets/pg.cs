using UnityEngine;
using System.Collections;

public class pg : MonoBehaviour
{
	public Transform LeftHand;
	
	private GameObject FootBall;
	public BallScript ballScript;
	private bool anim = false;
	private bool dive = false;
	
	private bool playStandAnimation = false;
	private Vector3 InitialPosition;
	private Quaternion InitialRotation;
	
	float timeSinceCaught = 0f;
	
	// Use this for initialization
	void Start ()
	{
		timeSinceCaught = Time.time;
		
		InitialPosition = transform.position;
		InitialRotation = transform.rotation;
		
		FootBall = GameObject.FindGameObjectWithTag("TheSoccerBall");
		ballScript = FootBall.GetComponent<BallScript>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (ballScript.ownerPlayer == LeftHand) {
			if (GameManager.SharedObject ().IsGameReady) {
				GameManager.SharedObject ().IsGameReady = false;
			}
		}
		
		
		
		
		if (ballScript.ownerPlayer == LeftHand && Time.time - timeSinceCaught > 2f) {
			ballScript.isKicked = false;
			ballScript.ownerPlayer = null;
			gameObject.GetComponent<PlayerGolie> ().enabled = false;
			gameObject.GetComponent<PlayerGolieKick> ().enabled = true;
			return;
		}
		//		if (Input.GetKey ("b"))
		//			{
		//				print (transform.position.z - FootBall.transform.position.z);
		//			}
		if( Vector3.Distance(transform.position,FootBall.transform.position) <= 10f  && ballScript.ownerPlayer != LeftHand)
		{
			
			if(45f - FootBall.transform.position.z < -0.25f && FootBall.transform.position.y < 0.5f)
			{
				
				if(!dive)
					if(GetComponent<Animation>()["portero_despeje_lateral_izquierdo_raso"].enabled == false && GetComponent<Animation>()["portero_despeje_lateral_izquierdo_alto"].enabled == false && GetComponent<Animation>()["portero_despeje_lateral_derecho_raso"].enabled == false && GetComponent<Animation>()["portero_despeje_lateral_derecho_alto"].enabled == false)
						GetComponent<Animation>().Play("portero_despeje_lateral_izquierdo_raso", PlayMode.StopAll);
				
				transform.Translate(Vector3.right*Time.deltaTime*-5);
				dive=false;
			}
			else if(transform.position.z - FootBall.transform.position.z < -0.35f && FootBall.transform.position.y >= 0.5f)
			{
				
				if(GetComponent<Animation>()["portero_despeje_lateral_izquierdo_raso"].enabled == false && GetComponent<Animation>()["portero_despeje_lateral_izquierdo_alto"].enabled == false && GetComponent<Animation>()["portero_despeje_lateral_derecho_raso"].enabled == false && GetComponent<Animation>()["portero_despeje_lateral_derecho_alto"].enabled == false)
					GetComponent<Animation>().Play("portero_despeje_lateral_izquierdo_alto", PlayMode.StopAll);
				
				transform.Translate(Vector3.right*Time.deltaTime*-8);
			}
			else if(transform.position.z - FootBall.transform.position.z > 0.25f && FootBall.transform.position.y < 0.5f)
			{
				
				if(GetComponent<Animation>()["portero_despeje_lateral_izquierdo_raso"].enabled == false && GetComponent<Animation>()["portero_despeje_lateral_izquierdo_alto"].enabled == false && GetComponent<Animation>()["portero_despeje_lateral_derecho_raso"].enabled == false && GetComponent<Animation>()["portero_despeje_lateral_derecho_alto"].enabled == false)
					GetComponent<Animation>().Play("portero_despeje_lateral_derecho_raso", PlayMode.StopAll);
				
				transform.Translate(Vector3.right*Time.deltaTime*5);
			}
			else if(transform.position.z - FootBall.transform.position.z > 0.35f && FootBall.transform.position.y >= 0.5f)
			{
				
				if(GetComponent<Animation>()["portero_despeje_lateral_izquierdo_raso"].enabled == false && GetComponent<Animation>()["portero_despeje_lateral_izquierdo_alto"].enabled == false && GetComponent<Animation>()["portero_despeje_lateral_derecho_raso"].enabled == false && GetComponent<Animation>()["portero_despeje_lateral_derecho_alto"].enabled == false)
					GetComponent<Animation>().Play("portero_despeje_lateral_derecho_alto", PlayMode.StopAll);
				
				transform.Translate(Vector3.right*Time.deltaTime*8);
			}else
			{
				
			}
			
			if(Vector3.Distance(FootBall.transform.position, transform.position) <= .79f&&Time.time - timeSinceCaught > 2f)
			{
				timeSinceCaught = Time.time;
				//GameManager.SharedObject().IsGameReady = false;
				ballScript.SetOwner(LeftHand);
				playStandAnimation = true;
				ballScript.isKicked = false;
				//print ("caught");
			}
		}
		//////////////////////////////////////////////
		else if((FootBall.transform.position.x < -40 || FootBall.transform.position.x < transform.position.x) && ballScript.ownerPlayer != LeftHand) // run towards ball
		{
			if((Vector3.Distance(transform.position,FootBall.transform.position) > 1f && GetComponent<Animation>()["portero_parada_frontal_rasa"].enabled == false) &&( FootBall.transform.position.z > -16f && FootBall.transform.position.z < 16f))
			{
				if(GetComponent<Animation>()["corriendo"].enabled == false)
				{
					
					GetComponent<Animation>().Play("corriendo", PlayMode.StopAll);
				}
				Quaternion rot = Quaternion.LookRotation((FootBall.transform.position - transform.position).normalized);
				rot.x = 0;
				transform.rotation = rot;
				transform.Translate(Vector3.forward*Time.deltaTime*5);
				
				
			}
			else if(Vector3.Distance(transform.position,FootBall.transform.position) < 1f)
			{
				if(GetComponent<Animation>()["portero_parada_frontal_rasa"].enabled == false&&anim==false)
				{
					
					GetComponent<Animation>().Play("portero_parada_frontal_rasa", PlayMode.StopAll);
					//GetComponent<Animation> ().CrossFade("portero_parada_frontal_rasa",PlayMode.StopSameLayer);
					anim=true;
					//					if(Vector3.Distance(transform.position,FootBall.transform.position) < 4.5f)
					//					{
					//						timeSinceCaught = Time.time;
					//						GameManager.SharedObject().IsGameReady = false;
					//						ballScript.SetOwner(LeftHand);
					//						playStandAnimation = true;
					//						ballScript.isKicked = false;
					//						print ("case22222222222");
					//					}
					
				}
				else if (GetComponent<Animation>()["portero_parada_frontal_rasa"].enabled == true||anim==true)
				{
					//anim=false;
					if(Vector3.Distance(transform.position,FootBall.transform.position) < 2.5f&&Time.time - timeSinceCaught > 2f)
					{
						
						timeSinceCaught = Time.time;
						//GameManager.SharedObject().IsGameReady = false;
						ballScript.SetOwner(LeftHand);
						playStandAnimation = true;
						ballScript.isKicked = false;
						print ("case333333");
						//anim=false;
					}
				}
			}
		}
		else if(FootBall.transform.position.x < -43 && ballScript.ownerPlayer == LeftHand && playStandAnimation)
		{
			anim=false;
			playStandAnimation = false;
			if(GetComponent<Animation>()["portero_levanta_balon"].enabled == false)//&&(GetComponent<Animation> ()["portero_parada_frontal_rasa"].enabled == false))
				GetComponent<Animation>().Play("portero_levanta_balon", PlayMode.StopAll);
			
			ballScript.isKicked = false;
		}
		else if(FootBall.transform.position.x < -34f)
		{
			if(transform.position.z - FootBall.transform.position.z < -1 && FootBall.transform.position.z > -3.4f && FootBall.transform.position.z < 3.4f) // ball to left side
			{
				if(GetComponent<Animation>()["portero_guardia_izquierda"].enabled == false)
					GetComponent<Animation>().Play("portero_guardia_izquierda", PlayMode.StopAll);
				
				transform.Translate(Vector3.right*Time.deltaTime*-2);
			}
			else if(transform.position.z - FootBall.transform.position.z > 1 && FootBall.transform.position.z > -3.4f && FootBall.transform.position.z < 3.4f)// ball to right side
			{
				if(GetComponent<Animation>()["portero_guardia_derecha"].enabled == false)
					GetComponent<Animation>().Play("portero_guardia_derecha", PlayMode.StopAll);
				
				transform.Translate(Vector3.right*Time.deltaTime*2);
			}
			else if(GetComponent<Animation>()["portero_levanta_balon"].enabled == false)// ball in front
			{
				if(GetComponent<Animation>()["portero_guardia_reposo"].enabled == false)
					GetComponent<Animation>().Play("portero_guardia_reposo", PlayMode.StopAll);
			}
		}
		else
		{
			if(GetComponent<Animation>()["reposo"].enabled == false)
				GetComponent<Animation>().Play("reposo", PlayMode.StopAll);
			
			transform.rotation = InitialRotation;
			transform.position = InitialPosition;
		}
		
		Vector3 pos = transform.position;
		pos.y = 0;
		transform.position = pos;
	}
	//	void OnGUI()
	//	{
	//		GUI.Label (new Rect (Screen.width / 2, Screen.height / 2, 300, 20), "Football Position x: "+FootBall.transform.position.x);
	//	}
	//	void OnGUI()
	//	{
	//		if(ballScript.ownerPlayer!=null)
	//			GUI.Label (new Rect (Screen.width / 2, Screen.height / 2, 300, 20), "Owner Player: "+ballScript.ownerPlayer.gameObject.name);
	//			GUI.Label (new Rect (Screen.width / 2, Screen.height / 2-25, 300, 20), "IsGameReady: "+GameManager.SharedObject().IsGameReady);
	//
	//	}
}
