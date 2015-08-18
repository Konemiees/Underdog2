using UnityEngine;
using System.Collections;

public class PlayerTurning : MonoBehaviour {

	public float turnSpeed = 2;

	float h;
	float v;

	private float turn;
	private float lastTurn;
	private Vector3 turnBegin;
	private float turnPoint;
	private Vector3 turnChangePoint;
	private Vector3 turnEnd;
	private Vector3 deltaCam;
	private float deltaCamTurnPoint;
	private Vector3 deltaCamEnd;

	// Use this for initialization
	void Awake () {
		turn = 0;
		lastTurn = 0;
		turnPoint = 1;
		turnChangePoint = transform.localEulerAngles;
		deltaCam = Vector3.zero;
	}


	void FixedUpdate () {

		h = Input.GetAxis ("Horizontal");
		v = Input.GetAxis ("Vertical");

		if (h != 0 || v != 0) {
			if (deltaCam.y != Camera.main.transform.localEulerAngles.y && deltaCam != Vector3.zero) {
			
				deltaCam.y = Camera.main.transform.localEulerAngles.y - deltaCam.y;
			
				while (deltaCam.y > 360)
					deltaCam.y -= 360;
				while (deltaCam.y <= 0)
					deltaCam.y += 360;
			
				deltaCam.x = 0;
				deltaCam.z = 0;
				deltaCamEnd = deltaCam;
				deltaCam = Vector3.zero;
			}
		
		
		
			turn = 0;
		
			if (h >= 0 && v >= 0)
				turn = Mathf.Atan (h / v) * 180 / Mathf.PI;
			if (h < 0 && v >= 0)
				turn = Mathf.Atan (-(v / h)) * 180 / Mathf.PI + 270;
			if (h >= 0 && v < 0)
				turn = Mathf.Atan (-(v / h)) * 180 / Mathf.PI + 90;
			if (h < 0 && v < 0)
				turn = Mathf.Atan (h / v) * 180 / Mathf.PI + 180;
		
		
			if (lastTurn != turn || deltaCamEnd != Vector3.zero) {
			
				turnChangePoint -= deltaCamEnd;
			
				while (turnChangePoint.y >= 360)
					turnChangePoint.y -= 360;
				while (turnChangePoint.y < 0)
					turnChangePoint.y += 360;
			
				lastTurn = turn;
				turnPoint = 0;
				turnBegin = turnChangePoint;
			
				if (turnChangePoint.y > 180 && (turn - turnChangePoint.y) < -180)
					turnBegin.y -= 360;
				if (turnChangePoint.y < 180 && (turn - turnChangePoint.y) > 180)
					turn -= 360;
			
				turnEnd = new Vector3 (transform.localEulerAngles.x, turn, transform.localEulerAngles.z);
			
				deltaCamEnd = Vector3.zero;
			}
		
		
			if (turnPoint < 1) {
				turnPoint += Time.deltaTime * turnSpeed;
			} else {
				turnPoint = 1;
			}
			turnChangePoint = Vector3.Lerp (turnBegin, turnEnd, turnPoint); 
		
		

		
			transform.localEulerAngles = turnChangePoint + new Vector3 (0, Camera.main.transform.localEulerAngles.y, 0);
		}else if (deltaCam == Vector3.zero) {
			deltaCam = Camera.main.transform.localEulerAngles;
		}
	}
}
