using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	/*public float speed = 6f;            
	public float acceleration = 6f; 
	public float gravity = 10;
	public float jumpHeight = 8;**/

	public float idleWaitTime = 10;
	public float rollWaitTime = 2;
	public float turnSpeed = 2;

	float currentSpeed;
	
	//Vector3 movement;                   
	Animator anim;                      
	Rigidbody playerRigidbody;          
	//int floorMask;                      
	//float camRayLength = 100f; 

	float h;
	float v;

	private float groundDist;
	private float rollTime;

	private bool moving;
	private bool floored;
	private bool died;
	private bool fighting;
	private int nextAttack;
	private float timeIdle;

	private float turn;
	private float lastTurn;
	private Vector3 turnBegin;
	private float turnPoint;
	private Vector3 turnChangePoint;
	private Vector3 turnEnd;
	private Vector3 deltaCam;
	private float deltaCamTurnPoint;
	private Vector3 deltaCamEnd;

	
	void Awake ()
	{
		//floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
		died = false;


		currentSpeed = 0;
		floored = true;
		timeIdle = 0;
		

		groundDist = GetComponent<Collider> ().bounds.extents.y;

		rollTime = 2.1f;
		turn = 0;
		lastTurn = 0;
		turnPoint = 1;
		turnChangePoint = transform.localEulerAngles;
		deltaCam = Vector3.zero;

	}
	
	
	void FixedUpdate ()
	{
		h = Input.GetAxisRaw ("Horizontal");
		v = Input.GetAxisRaw ("Vertical");
				
		if (h != 0 || v != 0) {
			Move (h, v);
		} else if (deltaCam == Vector3.zero) {
			deltaCam = Camera.main.transform.localEulerAngles;
		}

		Animating (h, v);
		
	}



	//TÄHÄN EI SAA KOSKEA!!!! T:Konsta
	void Move (float h, float v)
	{

		if (deltaCam.y != Camera.main.transform.localEulerAngles.y && deltaCam != Vector3.zero) {

			deltaCam.y = Camera.main.transform.localEulerAngles.y - deltaCam.y;

			while(deltaCam.y > 360)
				deltaCam.y -= 360;
			while(deltaCam.y <= 0)
				deltaCam.y += 360;
		
			deltaCam.x = 0;deltaCam.z = 0;deltaCamEnd = deltaCam;deltaCam = Vector3.zero;
		}



		turn = 0;

		if (h != 0 || v != 0) {
			if (v == 1) {
				if (h == 1) {
					turn = 45;
					h = 0;
				}
				if (h == -1) {
					turn = -45;
					h = 0;
				}
			} else if (v == 0) {
				if (h == 1) {
					turn = 90;
					v = 1;
					h = 0;
				}
				if (h == -1) {
					turn = -90;
					v = 1;
					h = 0;
				}
			} else {
				if (h == 0) {
					turn = 180;
					v = 1;
				}
				if (h == 1) {
					turn = 135;
					h = 0;
					v = 1;
				}
				if (h == -1) {
					turn = -135;
					h = 0;
					v = 1; 
				}
			}
		} 

		if (turn < 0)
			turn += 360;





		if (lastTurn != turn || deltaCamEnd != Vector3.zero) {

			turnChangePoint -= deltaCamEnd;

			while(turnChangePoint.y >= 360)
				turnChangePoint.y -= 360;
			while(turnChangePoint.y < 0)
				turnChangePoint.y += 360;

			lastTurn = turn;
			turnPoint = 0;
			turnBegin = turnChangePoint;

			if (turnChangePoint.y > 180 && (turn -turnChangePoint.y)< -180)
				turnBegin.y -= 360;
			if (turnChangePoint.y < 180 && (turn -turnChangePoint.y)> 180)
				turn -= 360;

			turnEnd = new Vector3 (transform.localEulerAngles.x, turn, transform.localEulerAngles.z);

			deltaCamEnd = Vector3.zero;
		}


		if (turnPoint < 1) {
			turnPoint += Time.deltaTime *turnSpeed;
		} else {
			turnPoint = 1;
		}
		turnChangePoint = Vector3.Lerp(turnBegin, turnEnd,turnPoint); 


		/*float g = IncrementTowards (currentSpeedGrav, maxFallSpeed * Time.deltaTime * -1, gravity);

		currentSpeedGrav -= gravity*Time.deltaTime;

		if(jumped){
			currentSpeedGrav = jumpHeight;
		}*/

		transform.localEulerAngles = turnChangePoint + new Vector3(0, Camera.main.transform.localEulerAngles.y, 0);


		

	}




	
	void Animating (float h, float v)
	{
		moving = h != 0 || v != 0;

		anim.SetBool ("Moving", moving);



		if (moving) {
			if(currentSpeed != 1 && !Input.GetButton("Walk") && !Input.GetButton("Run")){
				float s = Mathf.Sign(currentSpeed - 1)*-1;
				currentSpeed += Time.deltaTime*s;
				if(Mathf.Sign(currentSpeed -1) == s)
					currentSpeed = 1;
			} if(Input.GetButton("Walk") && currentSpeed != 0){
				currentSpeed -= Time.deltaTime;
				if(currentSpeed < 0)
					currentSpeed = 0;
			} if(Input.GetButton("Run") && currentSpeed != 2){
				currentSpeed += Time.deltaTime;
				if(currentSpeed >2)
					currentSpeed =2;
			}

			anim.SetFloat("MoveSpeed", currentSpeed);
		} 

		if (!moving && !fighting && floored) {
			timeIdle += Time.deltaTime/idleWaitTime;
			if (timeIdle / idleWaitTime >= 1)
				timeIdle = 1;
			anim.SetFloat ("IdleWaitTime", timeIdle);
		} else {
			timeIdle = 0;
		}


		floored = Physics.Raycast (transform.position, - Vector3.up, (groundDist + 0.001f) / 13.5f);

		if (rollTime < rollWaitTime) {
			floored = false;
			rollTime += Time.deltaTime;
		} 
		

		if (Input.GetButtonDown ("Jump") && floored) {
			anim.SetTrigger("Jump");
			floored = false;
		}

		if (Input.GetButtonDown ("Roll") && floored) {
			anim.SetTrigger("Roll");
			floored = false;
			rollTime = 0;
		}


	

	}




}