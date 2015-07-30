using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public Transform target;            // The position that that camera will be following.
	public float smoothing = 5f;        // The speed with which the camera will be following.
	
	Vector3 offset;                     // The initial offset from the target.

	public float distance = 5.0f; 
	public float xSpeed = 125.0f; 
	public float ySpeed = 50.0f;
	private float x = 0.0f; 
	private float y = 0.0f;

	public float cameraLowLimit = 0;
	public float cameraUpAngleLimit = 58;
	public float cameraCloseLimit = 1;
	private float lowLimitAngle;

	private Transform foundTarget;

	private Camera cam;

	//@script AddComponentMenu("Camera-Control/Mouse Orbit");
	
	void Start ()
	{
		Cursor.visible = false;
		x = transform.eulerAngles.y; 
		y = transform.eulerAngles.x;
	
		// Calculate the initial offset.
		offset = transform.position - target.position;

		cam = Camera.main;
	}
	
	void FixedUpdate ()
	{


		checkTarget ();
		 
		 if (target && !foundTarget) { 
			x += Input.GetAxis("Mouse X") * xSpeed * distance; //0.02f; 
			y -= Input.GetAxis("Mouse Y") * ySpeed; //0.02f; 


			if (y > cameraUpAngleLimit)
				y = cameraUpAngleLimit;
			
			if(y < -90)
				y = -90;



			Quaternion rotation = Quaternion.Euler(y, x, 0); 
			Vector3 position = rotation * new Vector3(0.0f, 2.0f, -distance) + target.position; 



			if (position.y < cameraLowLimit && lowLimitAngle == 0) {
				position.y = cameraLowLimit;
				lowLimitAngle = y;
				rotation = Quaternion.Euler(y, x, 0); 
			}else if(position.y<cameraLowLimit){
				position.y = cameraLowLimit;
				rotation = Quaternion.Euler (lowLimitAngle, x, 0);
			}


			
			 

			transform.position = position;
			transform.rotation = rotation;




		}
		if (target && foundTarget) {

			Vector3 lookPoint = (target.position + foundTarget.position)/2;

			Vector3 position = target.position;

			transform.position = position;

			transform.LookAt(lookPoint);

			//position = transform.rotation *  position + new Vector3(0.0f, 2.0f, -distance);

			transform.Translate( new Vector3(0.0f, 2.0f, -distance));


			transform.LookAt(lookPoint);
		}

	}



	void checkTarget(){

		if (!foundTarget) {
			for (int i = 2; i > -3; i--) {

				Ray ray;
				RaycastHit hit;

				//Horizontal raycast
				ray = cam.ScreenPointToRay (new Vector3 (cam.pixelWidth / 2 + (i * .5f), cam.pixelHeight / 2, 0));

				if (Physics.Raycast (ray, out hit)) {
					if (hit.transform.gameObject.tag == "Targetable" && Input.GetButtonDown ("Lock"))
						foundTarget = hit.transform;
				}

				//Vertical raycast
				ray = cam.ScreenPointToRay (new Vector3 (cam.pixelWidth / 2, cam.pixelHeight / 2 + (i * .5f), 0));

				if (Physics.Raycast (ray, out hit)) {
					if (hit.transform.gameObject.tag == "Targetable" && Input.GetButtonDown ("Lock"))
						foundTarget = hit.transform;
				}

			}
		} else {
			if (Input.GetButtonDown ("Lock"))
				foundTarget = null;
		}
	
		}





}