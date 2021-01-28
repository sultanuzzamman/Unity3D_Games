using UnityEngine;
using System.Collections;

public class pingggg : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<GUITexture>().color=new Color(50,Mathf.PingPong (Time.time, 0.5f),Mathf.PingPong (Time.time, 0.25f));
	}
}
