using UnityEngine;
using System.Collections;

public class TeamSelectionController : MonoBehaviour
{

	public static int teamIndex = 0;
	public Texture[] teams;
	public Texture[] clothes;
	public Texture[] textures_team_names;
	//public Texture[] textures_oppponent_team_names;
	public Texture[] HDTextures;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		if(teamIndex > 31) teamIndex = 0;
		if(teamIndex < 0) teamIndex = 31;

		if(GetComponent<GUITexture>())
			GetComponent<GUITexture>().texture = teams[teamIndex];
		//GameManager

		GameManager.SharedObject ().playerTeamFlag = teams[teamIndex];
		GameManager.SharedObject ().cloth = clothes[teamIndex];
		GameManager.SharedObject ().playerTeamTexture = textures_team_names[teamIndex];
		GameManager.SharedObject ().playerTeamHDTexture = HDTextures[teamIndex];
	}
}
