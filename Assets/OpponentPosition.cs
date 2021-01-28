using UnityEngine;
using System.Collections;

public class OpponentPosition : MonoBehaviour
{
	private bool initialpass=false;
	public Transform InitialPositonTransform, SecondaryPositonTransform;
	private Vector3 InitialPosition, SecondaryPosition;
	
	private AI_Striker playerScript;
//	private AI_MidfielderScript ppp;
	// Use this for initialization
	void Start () {
		playerScript = InitialPositonTransform.GetComponent<AI_Striker> ();
		InitialPosition = InitialPositonTransform.position;      ///////////**********On circle
		SecondaryPosition = SecondaryPositonTransform.position;  ///////////**********Center of circle
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (PlayerPosition.PlayerTurn)
			playerScript.InitialPosition = InitialPosition;
		else {
			playerScript.InitialPosition = SecondaryPositonTransform.position;
		}

		if(PlayerPosition.PlayerTurn == false && Vector3.Distance(transform.position, SecondaryPosition) < 1f)
		{
			if(!initialpass)
			{
				initialpass=true;
			Invoke("strtGme",1f);
			}
		}
	}
	void strtGme(){
		playerScript.MakeAPassMethod();
		GameManager.SharedObject().IsGameReady = true;
	}
}
