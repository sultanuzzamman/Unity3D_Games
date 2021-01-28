﻿using UnityEngine;
using System.Collections;

public class scoreFieldFlagePosition : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameManager.SharedObject ().IsFirstHalf)
			transform.position = new Vector3 (0.45f, transform.position.y, transform.position.z);
		else
			transform.position = new Vector3 (0.68f, transform.position.y, transform.position.z);
	}
}
