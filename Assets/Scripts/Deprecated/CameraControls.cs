using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	public float cameraSpeed = 12.0f;			// Speed of the camera movement

	private bool mouseViewMode;					// We check whether the camera is in the view mode
	private GameObject cameraNode;				// Camera node for movement
	private bool isRotating;					// Camera is rotating
	private Vector3 cameraNodeDirection;		// Where the Camera node is looking at
	private Quaternion newCameraRotation;			// New Camera rotation

	public float rotationSpeed = 70.0f;

	void Awake(){
		cameraNode = transform.parent.gameObject;
	}


	void Update (){
		// Cache the attention attracting input.
		if (Input.GetMouseButtonDown (1))
			mouseViewMode = true;
		else
			mouseViewMode = false;

		if (Input.GetKey("w") || Input.GetKey("up")){
			cameraNode.transform.Translate(Vector3.forward * Time.deltaTime * cameraSpeed);
		}
		if (Input.GetKey("s") || Input.GetKey("down")){
			cameraNode.transform.Translate(Vector3.back * Time.deltaTime * cameraSpeed);
		}
		if (Input.GetKey("a") || Input.GetKey("left")){
			cameraNode.transform.Translate(Vector3.left * Time.deltaTime * cameraSpeed);
		}
		if (Input.GetKey("d") || Input.GetKey("right")){
			cameraNode.transform.Translate(Vector3.right * Time.deltaTime * cameraSpeed);
		}
		if (Input.GetKey("q") && !isRotating){
			isRotating = true;
			newCameraRotation = new Quaternion(cameraNode.transform.rotation.x,cameraNode.transform.rotation.y,cameraNode.transform.rotation.z,cameraNode.transform.rotation.w);;
			newCameraRotation *= Quaternion.Euler(0,90,0);
		}
		if (Input.GetKey("e") && !isRotating){
			isRotating = true;
			newCameraRotation = new Quaternion(cameraNode.transform.rotation.x,cameraNode.transform.rotation.y,cameraNode.transform.rotation.z,cameraNode.transform.rotation.w);;
			newCameraRotation *= Quaternion.Euler(0,-90,0);
		}

		if (isRotating) {
			cameraNode.transform.rotation = Quaternion.RotateTowards(cameraNode.transform.rotation, newCameraRotation, rotationSpeed * Time.deltaTime);
			if(cameraNode.transform.rotation == newCameraRotation){
				isRotating = false;
			}

			/*
			// Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
			cameraNode.transform.rotation = Quaternion.Lerp(cameraNode.transform.rotation, newCameraRotation, 1.5f * Time.deltaTime);
			if (Quaternion.Angle(cameraNode.transform.rotation, newCameraRotation)<0.1){
				isRotating = false;
				cameraNode.transform.rotation=newCameraRotation;
			}*/
		}
	}
}
