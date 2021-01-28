using UnityEngine;
using System.Collections;

public class AI_Striker : MonoBehaviour
{
	private Transform[] strikers;
	private Transform[] opponents;
	public static float opponentPassSpeed=1200;
	//public GameObject cam;

	public Transform goal;
	private int moveSpeed = 5;
	private bool FirstHalf = true;
	public Vector3 InitialPosition = Vector3.zero;
	private Vector3 targetPosition = Vector3.zero;

	private GameObject FootBall;
	private BallScript FootBallScript;

	static bool Position1Available = true;
	static bool Position2Available = true;
	static bool Position3Available = true;
	static bool Position4Available = true;

	private int zOffset = 0;
	private int xOffset = 0;

	private float progress = 0;
	
	private float timeToPass = 0;
	private float waitForPass = 0;

	[HideInInspector]
	public bool isMoving = false;
	private float lastTime = 0;

	private float AttackTime = 0;
	private bool Attack = false;

	// Use this for initialization
	void Start ()
	{
		opponentPassSpeed = 1200;
		/*AtPosition1 = false;
		AtPosition2 = false;
		AtPosition3 = false;*/

		InitialPosition = transform.position;
		targetPosition = transform.position;

		FootBall = GameObject.FindGameObjectWithTag("TheSoccerBall");
		FootBallScript = FootBall.GetComponent<BallScript> ();

		if(Position1Available)
		{
			xOffset = 5;
			zOffset = 15;
			Position1Available = false;
		}
		else if(Position2Available)
		{
			xOffset = 5;
			zOffset = -15;
			Position2Available = false;
		}
		else if(Position3Available)
		{
			xOffset = -5;
			zOffset = -15;
			Position3Available = false;
		}
		else if(Position4Available)
		{
			xOffset = -5;
			zOffset = 15;
			Position3Available = false;
		}

		GameObject[] playersT = GameObject.FindGameObjectsWithTag("AIStriker");
		GameObject[] opponentsT = GameObject.FindGameObjectsWithTag("Player");
		
		strikers = new Transform[playersT.Length];
		opponents = new Transform[opponentsT.Length];
		
		int i = 0;
		for(i = 0; i < playersT.Length; i++)
			strikers[i] = playersT[i].transform;
		
		for(i = 0; i < opponentsT.Length; i++)
			opponents[i] = opponentsT[i].transform;
	}

	bool TeamHasTheBall()
	{
		return (FootBallScript.ownerPlayer && FootBallScript.ownerPlayer.gameObject.name == transform.gameObject.name);
	}
	
	private void MoveForward()
	{
		float minZ = 0f;
		float maxZ = 0f;
		float xPos = 0f;
		
		targetPosition = FootBall.transform.position;
		 
		if(FootBall.transform.position.x < 30 && FootBall.transform.position.x > -30)
		{
			if(TeamHasTheBall())
				targetPosition = new Vector3(FootBall.transform.position.x-xOffset,0,((FootBall.transform.position.z+zOffset < 37 && FootBall.transform.position.z+zOffset > -37)?FootBall.transform.position.z+zOffset:targetPosition.z));
			else
				targetPosition = new Vector3(FootBall.transform.position.x+xOffset,0,((FootBall.transform.position.z+zOffset < 37 && FootBall.transform.position.z+zOffset > -37)?FootBall.transform.position.z+zOffset:targetPosition.z));
		}
		else
			targetPosition = InitialPosition;

		if(!GameManager.SharedObject().IsGameReady)
			targetPosition = InitialPosition;

		targetPosition.y = transform.position.y;
		float RotationSpeed = 100;
		
		Quaternion _lookRotation = transform.rotation;
		Vector3 _direction;
		
		_direction = (targetPosition - transform.position).normalized;
		
		if(_direction.magnitude > 0.2f)
			_lookRotation = Quaternion.LookRotation(_direction);
		
		Vector3 currentPosition = this.transform.position;
		
		if(Vector3.Distance(currentPosition, targetPosition) > 2f && GetComponent<Animation>()["entrada"].enabled == false)
		{ 
			isMoving = true;
			
			if(Time.time - lastTime > 0.3f)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
				lastTime = Time.time;
			}
			
			transform.Translate(Vector3.forward*Time.deltaTime*moveSpeed * 0.7f);
		}
		else
		{
			isMoving = false;
			
			_direction = (FootBall.transform.position - transform.position).normalized;
			_lookRotation = Quaternion.LookRotation(_direction);
			_lookRotation.x = _lookRotation.z = 0f;
			transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
		}
		
		if(isMoving == false && GetComponent<Animation>()["tiro"].enabled == false && GetComponent<Animation>()["entrada"].enabled == false)
			GetComponent<Animation>().Play("reposo", PlayMode.StopAll);
		else if(GetComponent<Animation>()["corriendo"].enabled == false && GetComponent<Animation>()["tiro"].enabled == false && GetComponent<Animation>()["entrada"].enabled == false)
			GetComponent<Animation>().Play("corriendo", PlayMode.StopAll);
	}
	
	private void MoveTowardsTheBall()
	{
		if(GetComponent<Animation>()["tiro"].enabled == true || GetComponent<Animation>()["pase"].enabled == true) return;
		isMoving = true;
		
		targetPosition = FootBall.transform.position;

		if(FootBall.transform.position.x < 0 && FootBall.transform.position.x > -55)
		{
			AttackTime += Time.deltaTime;

			if(Attack == false && AttackTime > 3)
			{
				Attack = true;
				AttackTime = 0;
			}
			else if(Attack == true && AttackTime>4)
			{
				AttackTime = 0;
				Attack = false;
			}

			if((Attack || FootBallScript.ownerPlayer == null) && !TeamHasTheBall())
				targetPosition = FootBall.transform.position;
			else if(TeamHasTheBall())
				targetPosition = new Vector3(FootBall.transform.position.x-10,0,FootBall.transform.position.z+zOffset/2);
			else
				targetPosition = new Vector3(FootBall.transform.position.x+10,0,FootBall.transform.position.z);
		}
		else
			targetPosition = InitialPosition;

		if(!GameManager.SharedObject().IsGameReady)
			targetPosition = InitialPosition;

		targetPosition.y = transform.position.y;
		float RotationSpeed = 100;
		
		Quaternion _lookRotation;
		Vector3 _direction;
		
		//find the vector pointing from our position to the target
		_direction = (targetPosition - transform.position).normalized;
		
		//create the rotation we need to be in to look at the target
		_lookRotation = Quaternion.LookRotation(_direction);
		
		//rotate us over time according to speed until we are in the required rotation
		if(Time.time - lastTime > 0.3f)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
			lastTime = Time.time;
		}
		
		Vector3 currentPosition = this.transform.position;
		
		if(Vector3.Distance(currentPosition, targetPosition) > 0.4f && GetComponent<Animation>()["entrada"].enabled == false)
			transform.Translate(Vector3.forward*Time.deltaTime*moveSpeed * 0.65f);
		else
		{
			isMoving = false;
			
			_direction = (FootBall.transform.position - transform.position).normalized;
			_lookRotation = Quaternion.LookRotation(_direction);
			_lookRotation.x = _lookRotation.z = 0f;
			transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
		}
		
		if(isMoving == false && GetComponent<Animation>()["entrada"].enabled == false)
			GetComponent<Animation>().Play("reposo", PlayMode.StopAll);
		else if(GetComponent<Animation>()["corriendo"].enabled == false && GetComponent<Animation>()["tiro"].enabled == false && GetComponent<Animation>()["entrada"].enabled == false)
			GetComponent<Animation>().Play("corriendo", PlayMode.StopAll);
	}
	
	//
	private void MoveForGoal()
	{
		if(GetComponent<Animation>()["tiro"].enabled == true || GetComponent<Animation>()["pase"].enabled == true) return;
		isMoving = true;
		
		if(GetComponent<Animation>()["corriendo"].enabled == false)
			GetComponent<Animation>().Play("corriendo", PlayMode.StopAll);
		
		Vector3 targetPosition = goal.transform.position;
		targetPosition.y = transform.position.y;
		float RotationSpeed = 100;

		if(!GameManager.SharedObject().IsGameReady)
			targetPosition = InitialPosition;

		Quaternion _lookRotation;
		Vector3 _direction;
		
		//find the vector pointing from our position to the target
		_direction = (targetPosition - transform.position).normalized;
		
		//create the rotation we need to be in to look at the target
		_lookRotation = Quaternion.LookRotation(_direction);
		_lookRotation.x = _lookRotation.z = 0f;
		transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
		
		if(Vector3.Distance(transform.position, targetPosition) > .5f && GetComponent<Animation>()["entrada"].enabled == false)
																			
			transform.Translate(Vector3.forward*Time.deltaTime*moveSpeed * 0.65f); //****StatesAndBehaviours.oppPlayerSpeed
																					//0.65f
	}																			///^^^^^^\\\\\
	
	// Update is called once per frame
	void Update()
	{
		if(GameManager.SharedObject().OpponentMadeFoul || GameManager.SharedObject().PlayerMadeFoul || GameManager.SharedObject().PlayerGotCornerKick || GameManager.SharedObject().OpponentGotCornerKick)
		{
			transform.rotation = Quaternion.LookRotation((FootBall.transform.position - transform.position));

			if(GetComponent<Animation>()["reposo"].enabled == false)
				GetComponent<Animation>().Play("reposo", PlayMode.StopAll);
			return;
		}

		if(GameManager.SharedObject().PlayerGotCornerKick || GameManager.SharedObject().OpponentGotCornerKick)
		{
			gameObject.GetComponent<AI_Striker>().enabled = false;
			gameObject.GetComponent<OCornerKickHandler>().enabled = true;
			return;
		}

		if(Vector3.Distance(FootBall.transform.position,transform.position) < 0.5f && GameManager.SharedObject().IsGameReady)
			FootBallScript.SetOwnerIfPossible(transform);
		
		isMoving = false;

		if((waitForPass <= 0 || Vector3.Distance(transform.position, FootBall.transform.position)<5f) && isMoving == false && transform == ControllablePlayer() && !HasTheBall()  && ((FootBallScript.ownerPlayer == null || !TeamHasTheBall() && GameManager.SharedObject().IsGameReady) /*&& Vector3.Distance(transform.position, FootBall.transform.position)<10f*/))
			MoveTowardsTheBall();
		
		if(waitForPass <= 0 && transform != ControllablePlayer() && !HasTheBall() && GameManager.SharedObject().IsGameReady)
			MoveForward();
		
		if(transform == ControllablePlayer() && HasTheBall() && GameManager.SharedObject().IsGameReady)
			MoveForGoal();
		
		if(isMoving == false && GetComponent<Animation>()["reposo"].enabled == false && GetComponent<Animation>()["tiro"].enabled == false && GetComponent<Animation>()["pase"].enabled == false && GetComponent<Animation>()["entrada"].enabled == false)
			GetComponent<Animation>().Play("reposo", PlayMode.StopAll);
		
		if(Vector3.Distance(transform.position, FootBall.transform.position)<0.5f && transform == ControllablePlayer() && !HasTheBall() && GetComponent<Animation>()["entrada"].enabled == false && GameManager.SharedObject().IsGameReady)
		{
			if(FootBallScript.ownerPlayer && FootBallScript.ownerPlayer.GetComponent<Animation>() != null)
				FootBallScript.ownerPlayer.gameObject.GetComponent<Animation>().Play("entrada", PlayMode.StopAll);
			
			FootBallScript.SetOwner(transform);
			timeToPass = 5f;
		}

		timeToPass -= Time.deltaTime;
		waitForPass -= Time.deltaTime;
		/*
		if(transform == ControllablePlayer() && HasTheBall())
			Debug.Log ("time: "+timeToPass);
		*/
		if(transform == ControllablePlayer() && HasTheBall() && OpponentNearBy() && timeToPass <0f && GameManager.SharedObject().IsGameReady)
			StartCoroutine(MakeAPass());
		else if(transform == ControllablePlayer() && HasTheBall() && (transform.position.x < -38f) && GameManager.SharedObject().IsGameReady)
		{
			transform.rotation = Quaternion.LookRotation((goal.position - transform.position).normalized);
			StartCoroutine(KickTheBall());
		}

		transform.position = new Vector3 (transform.position.x,0,transform.position.z);
	}
	
	void WaitForPass()
	{
		waitForPass = 1;
	}

	public void MakeAPassMethod()
	{
		StartCoroutine(MakeAPass());

	}

	IEnumerator MakeAPass()
	{
		Quaternion _lookRotation;
		Vector3 _direction;
		
		Transform idealPlayer = null;
		
		float dot = -1f;
		foreach(Transform player in strikers)
		{
			Vector3 heading = player.position - transform.position;
			if(Vector3.Dot(heading, transform.forward) > dot && player != transform)
			{
				dot = Vector3.Dot(heading, transform.forward);
				idealPlayer = player;
			}
		}

		if (idealPlayer != null)
		{
			if (GetComponent<Animation>() ["pase"].enabled == false)
				GetComponent<Animation>().Play ("pase", PlayMode.StopAll);
		
			yield return new WaitForSeconds (0.3f);



			FootBallScript.SetFree();

			AudioManager.PlayKickSound();
			//find the vector pointing from our position to the target
			_direction = (idealPlayer.position - transform.position).normalized;
		
			//create the rotation we need to be in to look at the target
			_lookRotation = Quaternion.LookRotation(_direction);
			_lookRotation.x = _lookRotation.z = 0f;
			transform.rotation = _lookRotation;//Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 9999999999);

			Quaternion shotAngle = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x - 25 * progress, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
			FootBall.transform.rotation = shotAngle;
		
			FootBall.transform.rotation = shotAngle;
			FootBall.GetComponent<Rigidbody>().AddForce(FootBall.transform.forward * opponentPassSpeed, ForceMode.Impulse);
			yield return new WaitForSeconds(3);
			opponentPassSpeed = 1500;
		
			progress = 0;
		}
	}
	
	IEnumerator KickTheBall()
	{
//		cam.SetActive (true);
		if(GetComponent<Animation>()["tiro"].enabled == false)
			GetComponent<Animation>().Play("tiro", PlayMode.StopAll);

		yield return new WaitForSeconds(0.3f);

		AudioManager.PlayKickSound();

		FootBallScript.SetFree();
		FootBallScript.isKicked = true;

		Quaternion shotAngle = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x - 5,transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z));
		FootBall.transform.rotation = shotAngle;
		FootBall.GetComponent<Rigidbody>().AddForce(FootBall.transform.forward*150, ForceMode.Impulse);
	}

	Transform ControllablePlayer()
	{
		if(HasTheBall()) return transform;
		
		Transform idealPlayer = transform;
		
		foreach(Transform player in strikers)
		{
			if(Vector3.Distance(FootBall.transform.position,player.position) - Vector3.Distance(FootBall.transform.position,idealPlayer.position) < 1f)
				idealPlayer = player;
		}
		return idealPlayer;
	}
	
	private bool OpponentNearBy()
	{
		Transform opponentNB = null;
		
		foreach(Transform opponent in opponents)
		{
			if(Vector3.Distance(transform.position,opponent.position) < 2f)
				opponentNB = opponent;
		}
		return opponentNB != null;
	}
	
	bool HasTheBall()
	{
		return (FootBallScript.ownerPlayer == transform);
	}

}