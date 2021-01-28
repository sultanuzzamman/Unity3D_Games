using UnityEngine;
using System.Collections;

public class tnscript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		GetComponent<GUITexture>().texture = GameManager.SharedObject ().opponentTeamTexture;
		//guiTexture.texture=GameManager.SharedObject ().opponentt
	}
}
