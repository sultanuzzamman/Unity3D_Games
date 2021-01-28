using UnityEngine;
using System.Collections;

public class getplayer : MonoBehaviour {

	public Transform player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//Vector3 temp=player.transform.position;
		gameObject.transform.position=new Vector3(player.position.x,transform.position.y,player.position.z);

		//print(player.transform.position);
	
	}
}
