﻿using UnityEngine;
using System.Collections;

public class flag1script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<GUITexture>().texture = GameManager.SharedObject ().playerTeamFlag;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
