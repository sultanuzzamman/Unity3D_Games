﻿using UnityEngine;
using System.Collections;

public class Playernew : MonoBehaviour
{
	public OtherDialoguesActive ode;
	public GameObject matchComplete, halfComplete;
	public Texture barTexture;
	//punlic gameob
	public Texture sprintButton;
	public Texture passButton;
	public Texture shootButton;
	public Texture tackleButton;
	public GameObject sprintbar, acb;
	public Texture sprintButtonSel;
	public Texture passButtonSel;
	public Texture shootButtonSel;
	public Texture tackleButtonSel;
	
	private bool sprintButtonPressed;
	private bool passButtonPressed;
	private bool shootButtonPressed;
	private bool tackleButtonPressed;
	//
	//public Transform player;
	private Rect sprintButtonRect;
	private Rect passButtonRect;
	private Rect shootButtonRect;
	private Rect tackleButtonRect;
	
	int moveSpeed = 5;
	
	[HideInInspector]
	public bool isMoving = false;
	
	private float progress = 0;
	private Rect touchRect;
	private GameObject theBall;
	private BallScript theBallScript;
	
	private Transform[] players;
	private Transform highlight;
	
	public Vector3 initialPosition, targetPosition;

	public enum PlayerType
	{
		AttackerLeft,
		AttackerRight,
		DefenderLeft,
		DefenderRight,
		MidFielderLeft,
		MidFielderRight}

	;

	public PlayerType playerType = PlayerType.AttackerLeft;
	
	private float sprintStamina = 10f;
	float lastTackleTime = 0f;

	public static bool noControls = false;

	void Start ()
	{
		acb.SetActive (false);
		ode = GameObject.Find ("Main Camera").GetComponent<OtherDialoguesActive> ();
//		noControls = false;

		lastTackleTime = Time.time;
		
		sprintButtonRect = new Rect (Screen.width - GetValue (150), Screen.height - GetValue (150), GetValue (130), GetValue (130));
		shootButtonRect = new Rect (Screen.width - GetValue (150) - GetValue (130), Screen.height - GetValue (150), GetValue (110), GetValue (110));
		tackleButtonRect = new Rect (Screen.width - GetValue (150) - GetValue (130), Screen.height - GetValue (150), GetValue (110), GetValue (110));
		passButtonRect = new Rect (Screen.width - GetValue (150), Screen.height - GetValue (150) - GetValue (130), GetValue (110), GetValue (110));
		
		initialPosition = transform.position;
		targetPosition = initialPosition;
		
		highlight = GameObject.Find ("Highlight").transform;
		touchRect = new Rect (Screen.width / 3 * 2, 0, Screen.width / 3, Screen.height);
		theBall = GameObject.FindGameObjectWithTag ("TheSoccerBall");
		
		GameObject[] playersT = GameObject.FindGameObjectsWithTag ("Player");
		
		players = new Transform[playersT.Length];

		for (int i = 0; i < playersT.Length; i++)
			players [i] = playersT [i].transform;
		
		theBallScript = theBall.GetComponent<BallScript> ();
	}

	Transform ControllablePlayer ()
	{
		if (HasTheBall ())
			return transform;
		
		Transform idealPlayer = transform;
		
		foreach (Transform player in players) {
			if (Vector3.Distance (theBall.transform.position, player.position) < Vector3.Distance (theBall.transform.position, idealPlayer.position)) {
				idealPlayer = player;
//				Camera.main.GetComponent<SmoothFollow>().target=player;
			}
				
		}
		
		return idealPlayer;
	}

	bool HasTheBall ()
	{
		return (theBall.GetComponent<BallScript> ().ownerPlayer == transform);
	}

	private void MoveForward ()
	{
		isMoving = false;

		float minZ = 0f;
		float maxZ = 0f;

		if (playerType == PlayerType.AttackerLeft || playerType == PlayerType.AttackerRight) {
			if (playerType == PlayerType.AttackerLeft) {
				minZ = 5f;
				maxZ = 36f;
			} else {
				minZ = -36f;
				maxZ = -5f;
			}

			if ((theBall.transform.position.x > transform.position.x || Vector3.Distance (theBall.transform.position, targetPosition) > 15f) && Vector3.Distance (transform.position, targetPosition) < 1f)
				targetPosition = new Vector3 (theBall.transform.position.x + 10, transform.position.y, Random.Range (minZ, maxZ));
		} else if (playerType == PlayerType.MidFielderLeft || playerType == PlayerType.MidFielderRight) {
			if (playerType == PlayerType.MidFielderLeft) {
				minZ = 5f;
				maxZ = 36f;
			} else {
				minZ = -36f;
				maxZ = -5f;
			}

			if ((theBall.transform.position.x < transform.position.x || Vector3.Distance (theBall.transform.position, targetPosition) > 15f) && Vector3.Distance (transform.position, targetPosition) < 1f)
				targetPosition = new Vector3 (theBall.transform.position.x - 10, transform.position.y, Random.Range (minZ, maxZ));
		} else if (playerType == PlayerType.DefenderLeft || playerType == PlayerType.DefenderRight) {
			if (playerType == PlayerType.DefenderLeft) {
				minZ = 5f;
				maxZ = 36f;
			} else {
				minZ = -36f;
				maxZ = -5f;
			}

			if ((theBall.transform.position.x < transform.position.x || Vector3.Distance (theBall.transform.position, targetPosition) > 25f) && Vector3.Distance (transform.position, targetPosition) < 1f && Vector3.Distance (initialPosition, targetPosition) < 40f)
				targetPosition = new Vector3 (theBall.transform.position.x - 20, transform.position.y, Random.Range (minZ, maxZ));
			else if (Vector3.Distance (initialPosition, targetPosition) > 42f)
				targetPosition = initialPosition;
		}

		if (!GameManager.SharedObject ().IsGameReady)
			targetPosition = initialPosition;

		float RotationSpeed = 100f;
		if (Vector3.Distance (transform.position, targetPosition) > 1f) {
			isMoving = true;

			//values for internal use
			Quaternion _lookRotation;
			Vector3 _direction;

			//find the vector pointing from our position to the target
			_direction = (targetPosition - transform.position).normalized;

			//create the rotation we need to be in to look at the target
			_lookRotation = Quaternion.LookRotation (_direction);
			_lookRotation.x = _lookRotation.z = 0f;
			//rotate us over time according to speed until we are in the required rotation
			transform.rotation = Quaternion.Slerp (transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);

			Vector3 currentPosition = this.transform.position;
			//first, check to see if we're close enough to the target
			if (Vector3.Distance (currentPosition, targetPosition) > .5f) { 
				Vector3 directionOfTravel = targetPosition - currentPosition;
				directionOfTravel.Normalize ();

				this.transform.Translate (
					(directionOfTravel.x * moveSpeed * Time.deltaTime * 0.65f),
					(directionOfTravel.y * moveSpeed * Time.deltaTime * 0.65f),
					(directionOfTravel.z * moveSpeed * Time.deltaTime * 0.65f),
					Space.World);
			}
		} else {
			isMoving = false;
			Quaternion _lookRotation;
			Vector3 _direction;

			_direction = (theBall.transform.position - transform.position).normalized;
			_lookRotation = Quaternion.LookRotation (_direction);
			_lookRotation.x = _lookRotation.z = 0f;
			transform.rotation = Quaternion.Slerp (transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
		}
		
		if (isMoving == false && GetComponent<Animation> () ["entrada"].enabled == false)
			GetComponent<Animation> ().Play ("reposo", PlayMode.StopAll);
		else if (GetComponent<Animation> () ["corriendo"].enabled == false)
			GetComponent<Animation> ().Play ("corriendo", PlayMode.StopAll);
	}

	private void MoveTowardsTheBall ()
	{
		if (GetComponent<Animation> () ["tiro"].enabled == true || GetComponent<Animation> () ["pase"].enabled == true || GetComponent<Animation> () ["entrada"].enabled == true)
			return;
		
		isMoving = false;
		
		//move towards the center of the world (or where ever you like)
		Vector3 targetPosition = theBall.transform.position;
		targetPosition.y = transform.position.y;
		float RotationSpeed = 100f;

		if (!GameManager.SharedObject ().IsGameReady)
			targetPosition = initialPosition;

		//values for internal use
		Quaternion _lookRotation;
		Vector3 _direction;
		
		//find the vector pointing from our position to the target
		_direction = (targetPosition - transform.position).normalized;
		
		//create the rotation we need to be in to look at the target
		_lookRotation = Quaternion.LookRotation (_direction);
		_lookRotation.x = _lookRotation.z = 0f;
		//rotate us over time according to speed until we are in the required rotation
		transform.rotation = Quaternion.Slerp (transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
		
		Vector3 currentPosition = this.transform.position;
		//first, check to see if we're close enough to the target
		if (Vector3.Distance (currentPosition, targetPosition) > .5f) { 
			isMoving = true;
			Vector3 directionOfTravel = targetPosition - currentPosition;
			//now normalize the direction, since we only want the direction information
			directionOfTravel.Normalize ();
			//scale the movement on each axis by the directionOfTravel vector components
			
			this.transform.Translate (
				(directionOfTravel.x * moveSpeed * Time.deltaTime * 0.65f),
				(directionOfTravel.y * moveSpeed * Time.deltaTime * 0.65f),
				(directionOfTravel.z * moveSpeed * Time.deltaTime * 0.65f),
				Space.World);
		}
		
		if (isMoving == true && GetComponent<Animation> () ["corriendo"].enabled == false)
			GetComponent<Animation> ().Play ("corriendo", PlayMode.StopAll);
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (noControls) {
			if (GetComponent<Animation> () ["reposo"].enabled == false)
				GetComponent<Animation> ().Play ("reposo", PlayMode.StopAll);
			return;
		}

		if (GameManager.SharedObject ().OpponentMadeFoul || GameManager.SharedObject ().PlayerMadeFoul) {
			gameObject.GetComponent<Player> ().enabled = false;
			gameObject.GetComponent<PlayerFoulHandler> ().enabled = true;
			return;
		}

		if (GameManager.SharedObject ().PlayerGotCornerKick || GameManager.SharedObject ().OpponentGotCornerKick) {
			gameObject.GetComponent<Player> ().enabled = false;
			gameObject.GetComponent<PCornerKickHandler> ().enabled = true;
			return;
		}
		/////****0.5/////3
		if (Vector3.Distance (theBall.transform.position, transform.position) < 3f && GameManager.SharedObject ().IsGameReady)
			theBall.GetComponent<BallScript> ().SetOwnerIfPossible (transform);


		isMoving = false;
		if (transform == ControllablePlayer ()) {
			Vector3 position = transform.position;
			position.y = 0.01f;
			highlight.position = position;
			//gameObject.transform.position=position;

			float multiplier = 1f;

			if (GameManager.SharedObject ().IsFirstHalf)
				multiplier = 1f;
			else
				multiplier = -1f;

			if (GameManager.SharedObject ().IsGameReady == false)
				multiplier = 0f;

			#if !UNITY_EDITOR
			float x = multiplier*GameObject.Find("Single Joystick").GetComponent<Joystick>().position.x;
			float y = multiplier*GameObject.Find("Single Joystick").GetComponent<Joystick>().position.y;
			
			if((Mathf.Abs(x) > 0.1f) || (Mathf.Abs(y) > 0.1f))
			{	
				if(GetComponent<Animation> ()["tiro"].enabled == false)
					transform.Translate(Vector3.forward*Time.deltaTime*moveSpeed * 0.65f);
				
				isMoving = true;
				
				if (GetComponent<Animation> ()["corriendo"].enabled == false && GetComponent<Animation> ()["tiro"].enabled == false)
					GetComponent<Animation> ().Play("corriendo", PlayMode.StopAll);
				
				transform.eulerAngles = new Vector3(0, 90 + Mathf.Atan2(-y, x) * 180 / Mathf.PI, 0);
			}
			
			if(transform == ControllablePlayer()){
				foreach(Touch touch in Input.touches)
				{
					//acb.SetActive(true);

					Vector2 inputGuiPosition = touch.position;
					inputGuiPosition.y = Screen.height - inputGuiPosition.y;
				
					if (touch.phase != TouchPhase.Canceled && HasTheBall())
					{
						//acb.SetActive(true);
						if(sprintButtonRect.Contains(inputGuiPosition) && touch.phase != TouchPhase.Ended)
						{
							//acb.SetActive(true);
							sprintButtonPressed = true;
							break;
						}
						else if(touch.phase == TouchPhase.Ended)
						{
							if(sprintButtonPressed == true)
							{
								sprintButtonPressed = false;
								//acb.SetActive(false);
								break;
							}
						}
					
						if(passButtonRect.Contains(inputGuiPosition) && touch.phase != TouchPhase.Ended)
							passButtonPressed = true;
						else if(touch.phase == TouchPhase.Ended)
						{
							if(passButtonPressed == true)
							{
								passButtonPressed = false;
								//PASS CODE HERE..
								StartCoroutine(PassTheBall());
								break;
							}
						}
					
						if(shootButtonRect.Contains(inputGuiPosition) && touch.phase != TouchPhase.Ended)
							shootButtonPressed = true;
						else if(touch.phase == TouchPhase.Ended)
						{
							if(shootButtonPressed == true)
							{
								shootButtonPressed = false;
								//SHOOT CODE HERE..
								progress = 1;
								StartCoroutine(KickTheBall());
								break;
							}
						}
					}
					if (touch.phase != TouchPhase.Canceled && !HasTheBall())
					{
						//acb.SetActive(false);
						if(sprintButtonRect.Contains(inputGuiPosition) && touch.phase != TouchPhase.Ended)
						{
						/////////////////////
							//acb.SetActive(true);
							sprintButtonPressed = true;
							break;
						}
						else if(touch.phase == TouchPhase.Ended)
						{
							
							if(sprintButtonPressed == true)
							{

								sprintButtonPressed = false;
								break;
							}
						}
					
						if(tackleButtonRect.Contains(inputGuiPosition) && touch.phase != TouchPhase.Ended)
							tackleButtonPressed = true;
						else if(touch.phase == TouchPhase.Ended)
						{
							if(tackleButtonPressed == true)
							{
								tackleButtonPressed = false;
								//TACKLE CODE HERE..
								if(Time.time - lastTackleTime > 3 && theBallScript.ownerPlayer != null && Vector3.Distance(theBallScript.ownerPlayer.position, transform.position)<1 && GameManager.SharedObject().IsGameReady)
								{
									theBallScript.ownerPlayer.gameObject.GetComponent<Animation> ().Play("entrada", PlayMode.StopAll);
									theBallScript.SetOwner(transform);
									lastTackleTime = Time.time;
								}
								break;
							}
						}
					}
					else if(touch.phase == TouchPhase.Canceled)
					{
						passButtonPressed = false;
						sprintButtonPressed = false;

						shootButtonPressed = false;
					}
					else if(progress > 0)
					{
						StartCoroutine(KickTheBall());
					}
				}}else{
				//acb.SetActive(false);
			}
			
				if(sprintButtonPressed && sprintStamina > 0)
				{
					//acb.SetActive(true);
					sprintStamina -= Time.deltaTime;
					sprintbar.transform.localScale-=new Vector3(0,0,Time.deltaTime*0.015f);
					moveSpeed = 8;
					GetComponent<Animation> ()["corriendo"].speed= 1.5f;
				}
				else
				{
				//	acb.SetActive(false);
					GetComponent<Animation> ()["corriendo"].speed= 1.0f;
					moveSpeed = 5;
				}
			#endif


			if (moveSpeed > 5 && sprintStamina > 0 && transform == ControllablePlayer ()) {
				acb.SetActive (true);
			} else {
				acb.SetActive (false);
			}
			if (transform != ControllablePlayer () || !HasTheBall ()) {
				acb.SetActive (false);
			}

			#if UNITY_EDITOR

			if (Input.GetAxis ("Vertical") > 0.1) {
				if (GetComponent<Animation> () ["tiro"].enabled == false && GetComponent<Animation> () ["pase"].enabled == false && GetComponent<Animation> () ["entrada"].enabled == false)
					transform.Translate (Vector3.forward * Time.deltaTime * moveSpeed * 0.65f);
				
				isMoving = true;
				
				if (GetComponent<Animation> () ["corriendo"].enabled == false && GetComponent<Animation> () ["tiro"].enabled == false && GetComponent<Animation> () ["pase"].enabled == false && GetComponent<Animation> () ["entrada"].enabled == false)
					GetComponent<Animation> ().Play ("corriendo", PlayMode.StopAll);
			}
			
			if (Input.GetAxis ("Horizontal") < 0) {
				if (GetComponent<Animation> () ["tiro"].enabled == false && GetComponent<Animation> () ["pase"].enabled == false && GetComponent<Animation> () ["entrada"].enabled == false)
					transform.Rotate (0, -20 * Time.deltaTime * moveSpeed, 0);
				
				if (GetComponent<Animation> () ["corriendo"].enabled == false && GetComponent<Animation> () ["tiro"].enabled == false && GetComponent<Animation> () ["pase"].enabled == false && GetComponent<Animation> () ["entrada"].enabled == false)
					GetComponent<Animation> ().Play ("corriendo", PlayMode.StopAll);
			}
			
			if (Input.GetAxis ("Horizontal") > 0) {
				if (GetComponent<Animation> () ["tiro"].enabled == false && GetComponent<Animation> () ["pase"].enabled == false && GetComponent<Animation> () ["entrada"].enabled == false)
					transform.Rotate (0, 20 * Time.deltaTime * moveSpeed, 0);
				
				if (GetComponent<Animation> () ["corriendo"].enabled == false && GetComponent<Animation> () ["tiro"].enabled == false && GetComponent<Animation> () ["pase"].enabled == false && GetComponent<Animation> () ["entrada"].enabled == false)
					GetComponent<Animation> ().Play ("corriendo", PlayMode.StopAll);
			}
			
			if (Input.GetKey ("space") && HasTheBall ()) {
				progress += Time.deltaTime;
				progress = (progress > 1) ? 1 : progress;
			} else if (progress > 0 && HasTheBall ()) {
				if (progress > 0.5f)
					StartCoroutine (KickTheBall ());
				else
					StartCoroutine (PassTheBall ());
			}
			#endif
		}
		if (isMoving == false && transform == ControllablePlayer () && !HasTheBall () && GetComponent<Animation> () ["entrada"].enabled == false/* && Vector3.Distance(theBall.transform.position, transform.position)>5f*/) {
			MoveTowardsTheBall ();
			//acb.SetActive(false);
		}
		
		if (transform != ControllablePlayer () && !HasTheBall () && GetComponent<Animation> () ["entrada"].enabled == false) {
			MoveForward ();
			//acb.SetActive(false);
		}
		
		if (isMoving == false && GetComponent<Animation> () ["reposo"].enabled == false && GetComponent<Animation> () ["tiro"].enabled == false && GetComponent<Animation> () ["pase"].enabled == false && GetComponent<Animation> () ["entrada"].enabled == false)
			GetComponent<Animation> ().Play ("reposo", PlayMode.StopAll);
		
		if (GetComponent<Animation> () ["corriendo"].enabled == false && isMoving == true && GetComponent<Animation> () ["tiro"].enabled == false && GetComponent<Animation> () ["entrada"].enabled == false)
			GetComponent<Animation> ().Play ("corriendo", PlayMode.StopAll);

		transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
	}

	IEnumerator KickTheBall ()
	{
		if (GetComponent<Animation> () ["tiro"].enabled == false)
			GetComponent<Animation> ().Play ("tiro", PlayMode.StopAll);
		
		progress = progress < 0.3f ? 0.3f : progress;
		AudioManager.PlayKickSound ();
		yield return new WaitForSeconds (0.3f);

		theBall.GetComponent<BallScript> ().SetFree ();
		theBallScript.isKicked = true;

		Quaternion shotAngle = Quaternion.Euler (new Vector3 (transform.rotation.eulerAngles.x - 15 * progress, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
		theBall.transform.rotation = shotAngle;
		theBall.GetComponent<Rigidbody> ().AddForce (theBall.transform.forward * 2500 * progress, ForceMode.Impulse);

		progress = 0;
	}

	IEnumerator PassTheBall ()
	{
		float x = -100;
		
		Transform idealPlayer = null;
		float distance = 0f;
		foreach (Transform player in players) {
			if (player != transform && player.position.x > x && Vector3.Distance (player.transform.position, transform.position) < 80 && Vector3.Distance (player.transform.position, transform.position) > 2) {
				x = player.position.x;
				idealPlayer = player;
				distance = Vector3.Distance (player.transform.position, transform.position);
				//if(Input.GetKey("v")){
				//	print (distance);
				//}
			}
		}
		
		if (idealPlayer != null) {
			if (GetComponent<Animation> () ["pase"].enabled == false)
				GetComponent<Animation> ().Play ("pase", PlayMode.StopAll);
		
			progress = progress < 0.3f ? 0.3f : progress;
		
			progress = 1;
		
			transform.rotation = Quaternion.LookRotation ((idealPlayer.position - transform.position).normalized);
			Quaternion shotAngle = Quaternion.Euler (new Vector3 (transform.rotation.eulerAngles.x * progress, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
			theBall.transform.rotation = shotAngle;
		
			yield return new WaitForSeconds (0.3f);

			AudioManager.PlayKickSound ();

			transform.rotation = Quaternion.LookRotation ((idealPlayer.position - transform.position).normalized);
			shotAngle = Quaternion.Euler (new Vector3 (transform.rotation.eulerAngles.x * progress, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
			theBall.transform.rotation = shotAngle;
		
			theBall.GetComponent<BallScript> ().SetFree ();
			theBall.GetComponent<Rigidbody> ().AddForce (theBall.transform.forward * distance * 95 * progress, ForceMode.Impulse);
			print (distance * 95);
			progress = 0;
		}
		progress = 0;
	}

	void OnGUI ()
	{

	
		if (!PauseController.isPaused && !InitGame.matchcomplete && !InitGame.halfComplete) {
			if (!GameManager.SharedObject ().IsGameReady || PauseController.isPaused)
				return;

			if (HasTheBall ()) {

				GUI.BeginGroup (new Rect (Screen.width - GetValue (53), GetValue (236) + GetValue (10) - GetValue (236 * progress), GetValue (43), GetValue (236 * progress)));
				GUI.DrawTexture (new Rect (GetValue (0), GetValue (0), GetValue (43), GetValue (236)), barTexture);
				GUI.EndGroup ();
			
				if (sprintButtonPressed)
					GUI.DrawTexture (sprintButtonRect, sprintButtonSel);
				else
					GUI.DrawTexture (sprintButtonRect, sprintButton);
			
				if (passButtonPressed)
					GUI.DrawTexture (passButtonRect, passButtonSel);
				else
					GUI.DrawTexture (passButtonRect, passButton);
			
				if (shootButtonPressed)
					GUI.DrawTexture (shootButtonRect, shootButtonSel);
				else
					GUI.DrawTexture (shootButtonRect, shootButton);
			
				/*if(GUI.Button(shootButtonRect,"",shootButton))
			{
				progress = 1;
				StartCoroutine(KickTheBall());
			}*/
			} else if (ControllablePlayer () == transform && !HasTheBall ()) {


				if (sprintButtonPressed) {
					GUI.DrawTexture (sprintButtonRect, sprintButtonSel);
					//acb.SetActive(true);
				} else {

					GUI.DrawTexture (sprintButtonRect, sprintButton);
				}
				if (Time.time - lastTackleTime > 3) {
					if (tackleButtonPressed)
						GUI.DrawTexture (tackleButtonRect, tackleButtonSel);
					else
						GUI.DrawTexture (tackleButtonRect, tackleButton);
				}
			}
		}

	}

	float GetValue (float value)
	{
		return value * Screen.height / 640f;
	}
}
