using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


	public float rollWaitTime = 2;

	float currentSpeed;
	                   
	Animator anim;                                

	float h;
	float v;

	private float groundDist;
	private float rollTime;


	private bool floored;
	private bool died;
	private bool fighting;
	private int nextAttack;
	private float timeIdle;


	
	void Awake ()
	{
		anim = GetComponent <Animator> ();
		died = false;


		currentSpeed = 0;
		floored = true;
		timeIdle = 0;
		

		groundDist = GetComponent<Collider> ().bounds.extents.y;

		rollTime = 2.1f;

	}
	
	
	void FixedUpdate ()
	{
		h = Input.GetAxis ("Horizontal");
		v = Input.GetAxis ("Vertical");

		Animating (h, v);
		
	}




	
	void Animating (float h, float v)
	{




		currentSpeed = Mathf.Sqrt (h * h + (v * v));
			

		anim.SetFloat("MoveSpeed", currentSpeed);
 




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