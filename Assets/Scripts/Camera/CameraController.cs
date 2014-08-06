using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float cameraSpeed = 12.0f;			// Speed of the camera movement
	
	private bool mouseViewMode;					// We check whether the camera is in the view mode
	private GameObject cameraNode;				// Camera node for movement
	private bool isBusy;						// Camera is busy and does not react to inputs
	private Quaternion newCameraRotation;		// New Camera rotation
	private Vector3 newCameraPosition;

	public float rotationSpeed = 70.0f;

	// Use this for initialization
	void Awake () {
		cameraNode = transform.parent.gameObject;
		newCameraPosition = cameraNode.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (isBusy) {
			cameraNode.transform.rotation = Quaternion.RotateTowards(cameraNode.transform.rotation, newCameraRotation, rotationSpeed * Time.deltaTime);
			cameraNode.transform.position = Vector3.MoveTowards(cameraNode.transform.position, newCameraPosition, Time.deltaTime * cameraSpeed);
			if((cameraNode.transform.rotation == newCameraRotation) && (cameraNode.transform.position == newCameraPosition)) {
				isBusy = false;
			}
		}
	}

	public void moveCamera(string s){
		if (!isBusy){
			switch (s) {
				case "w":
					cameraNode.transform.Translate(Vector3.forward * Time.deltaTime * cameraSpeed);
					break;
				case "s":
					cameraNode.transform.Translate(Vector3.back * Time.deltaTime * cameraSpeed);
					break;
				case "a":
					cameraNode.transform.Translate(Vector3.left * Time.deltaTime * cameraSpeed);
					break;
				case "d":
					cameraNode.transform.Translate(Vector3.right * Time.deltaTime * cameraSpeed);
					break;
			}
			newCameraPosition = cameraNode.transform.position;
		}
	}

	public void rotateCamera(string s){
		if (!isBusy){
			isBusy = true;
			newCameraRotation = new Quaternion(cameraNode.transform.rotation.x,cameraNode.transform.rotation.y,cameraNode.transform.rotation.z,cameraNode.transform.rotation.w);;
			if (s=="q")
				newCameraRotation *= Quaternion.Euler(0,90,0);
			else if (s=="e")
				newCameraRotation *= Quaternion.Euler(0,-90,0);
		}
	}

	public void moveCameraVertical(float f){
		if (!isBusy){
			isBusy=true;
			if ((f>0f) && cameraNode.transform.position.y<5f){
				//Vector3 v = new Vector3(0,10f,0);
				newCameraPosition = cameraNode.transform.position;
				newCameraPosition.y = newCameraPosition.y + 5f;
			}
			else if ((f<0) && cameraNode.transform.position.y>0f){
				//Vector3 v = new Vector3(0,-10f,0);
				newCameraPosition = cameraNode.transform.position;
				newCameraPosition.y = newCameraPosition.y - 5f;
			}
		}
	}

	public Ray hitRay(){
		return camera.ScreenPointToRay(Input.mousePosition);
	}
}
