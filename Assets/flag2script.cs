﻿using UnityEngine;
using System.Collections;

public class flag2script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<GUITexture>().texture = GameManager.SharedObject ().opponentTeamFlag;
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
