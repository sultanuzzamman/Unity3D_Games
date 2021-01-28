using UnityEngine;
using System.Collections;

public class teamname_script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		GetComponent<GUITexture>().texture = GameManager.SharedObject().playerTeamTexture;
		//guiTexture.texture = GameManager.SharedObject ().opponentTeamTexture;
	}
}
