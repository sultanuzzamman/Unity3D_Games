using UnityEngine;
using System.Collections;

public class TeamSelectionController2:MonoBehaviour
{
	public static int teamIndex = 0;
	public Texture[] teams;
	public Texture[] clothes;
	public Texture[] textures_oppponent_team_names;
	public Texture[] HDTextures;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		//if (TeamSelectionController.teamIndex == teamIndex) 
	//	{

		//	print("matched");
		//	teamIndex++;
		//}

		if(teamIndex > 31) teamIndex = 0;
		if(teamIndex < 0) teamIndex = 31;

		if (GetComponent<GUITexture>()) 
		{
			if (TeamSelectionController.teamIndex == teamIndex){
				teamIndex++;
			}
			GetComponent<GUITexture>().texture = teams [teamIndex];
		}

		GameManager.SharedObject ().opponentTeamFlag = teams[teamIndex];
		GameManager.SharedObject ().opcloth = clothes[teamIndex];
		GameManager.SharedObject ().opponentTeamTexture = textures_oppponent_team_names[teamIndex];
		GameManager.SharedObject ().opponentTeamHDTexture = HDTextures[teamIndex];
	}
}