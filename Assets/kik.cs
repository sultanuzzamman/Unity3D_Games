using UnityEngine;
using System.Collections;

public class kik : MonoBehaviour {

	private float progress = 0;
	private GameObject theBall;
	private BallScript theBallScript;
	// Use this for initialization
	void Start () {

		theBall = GameObject.FindGameObjectWithTag("TheSoccerBall");
		theBallScript = theBall.GetComponent<BallScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKey ("k"))
		{
			StartCoroutine(KickTheBall());
		}

	}

	IEnumerator KickTheBall()
	{
		if (GetComponent<Animation>() ["tiro"].enabled == false)
			GetComponent<Animation>().Play ("tiro", PlayMode.StopAll);
		
		progress = progress < 0.3f ? 0.3f : progress;
		AudioManager.PlayKickSound ();
		yield return new WaitForSeconds (0.3f);
		
		theBall.GetComponent<BallScript> ().SetFree ();
		theBallScript.isKicked = true;
		
		Quaternion shotAngle = Quaternion.Euler (new Vector3 (transform.rotation.eulerAngles.x - 15 * progress, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
		theBall.transform.rotation = shotAngle;
		theBall.GetComponent<Rigidbody> ().AddForce (theBall.transform.forward * 5000 * progress, ForceMode.Impulse);
		
		progress = 0;
	}
}
