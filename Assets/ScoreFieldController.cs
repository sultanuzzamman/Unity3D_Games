﻿using UnityEngine;
using System.Collections;

public class ScoreFieldController : MonoBehaviour
{
	public bool playerGoals;
	public bool oppositeGoals;
	// Update is called once per frame
	void FixedUpdate ()
	{
		GameManager manager = GameManager.SharedObject ();

		///*** In order to display score always in double digits.***\\\
//		string pGoals = manager.playerTeamGoals<10?"0"+manager.playerTeamGoals:""+manager.playerTeamGoals;
//		string oGoals = manager.opponentTeamGoals<10?"0"+manager.opponentTeamGoals:""+manager.opponentTeamGoals;

		string pGoals = "" + manager.playerTeamGoals;
		string oGoals = "" + manager.opponentTeamGoals;
		if (playerGoals && oppositeGoals) {
			if (manager.IsFirstHalf) { ///*** in order to switch teamNames and scores
//		guiText.text = manager.playerTeamShortName + "    "+pGoals+" - "+oGoals+"    "+manager.opponentTeamShortName;
				GetComponent<GUIText>().text = "" + pGoals + " - " + oGoals + "";
//			print("FixedUpdate test 123456789 123456789 123456789 123456789");
			} else
//			guiText.text = manager.opponentTeamShortName + "    "+oGoals+" - "+pGoals+"    "+manager.playerTeamShortName;
				GetComponent<GUIText>().text = "" + oGoals + " - " + pGoals + "";
		} else if (playerGoals && !oppositeGoals) {
			GetComponent<GUIText>().text = "" + pGoals;
			if (manager.IsFirstHalf) { ///*** in order to switch teamNames and scores
				if (transform.position.x != 0.54f) {
					//transform.position=new Vector3(0.54f,transform.position.y,transform.position.z);
				}
			} else {
				if (transform.position.x != 0.585f) {
					//transform.position=new Vector3(0.585f,transform.position.y,transform.position.z);
				}
			}
		} else if (!playerGoals && oppositeGoals) {
			GetComponent<GUIText>().text = "" + oGoals;
			if (manager.IsFirstHalf) { ///*** in order to switch teamNames and scores
				if (transform.position.x != 0.585f) {
					//transform.position = new Vector3 (0.585f, transform.position.y, transform.position.z);
				}
			} else {
				if (transform.position.x != 0.54f) {
					//transform.position = new Vector3 (0.54f, transform.position.y, transform.position.z);
				}
			}
		}
	}
}
