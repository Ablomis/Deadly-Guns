using UnityEngine;
using System.Collections;

public class CinematicCameraController : MonoBehaviour {
	
	public float smooth = 1.5f;         			// The relative speed at which the camera will catch up.
	public float cameraYOffset = 3f;				// Distance from camera to player on the Y axis
	public float cameraZOffset = -2f;				// Distance from camera to player on the Y axis
	
	private Transform character;           			// Reference to the player's transform.
	private Vector3 relCameraPos;      				// The relative position of the camera from the player.
	private float relCameraPosMag;     				// The distance of the camera from the player.
	private Vector3 newPos;             			// The position the camera is trying to reach.
	enum CameraActions {FOLLOW, CHARACTER_SHOT, IDLE};
	private CameraActions cameraAction;

	// Use this for initialization
	void Awake () {
		cameraAction = CameraActions.IDLE;
	}
	
	// Update is called once per frame
	void Update () {
		switch(cameraAction){
			case CameraActions.FOLLOW:
				UpdatePosition();
				break;	
		}
	}

	public void CinematicMovement(GameObject fObject){
		cameraAction = CameraActions.FOLLOW;
		character = fObject.transform;
		transform.position = new Vector3 (fObject.transform.position.x, fObject.transform.position.y + cameraYOffset, fObject.transform.position.z + cameraZOffset);
		transform.LookAt (new Vector3(fObject.transform.position.x,fObject.transform.position.y + 1f,fObject.transform.position.z + 5f));
		relCameraPos = transform.position - fObject.transform.position;
	}

	private void UpdatePosition(){

		// The standard position of the camera is the relative position of the camera from the player.
		Vector3 standardPos = character.position + relCameraPos;
		
		// The abovePos is directly above the player at the same distance as the standard position.
		Vector3 abovePos = character.position + Vector3.up * relCameraPosMag;
		
		// An array of 5 points to check if the camera can see the player.
		Vector3[] checkPoints = new Vector3[5];
		
		// The first is the standard position of the camera.
		checkPoints[0] = standardPos;
		
		// The next three are 25%, 50% and 75% of the distance between the standard position and abovePos.
		checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
		checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
		checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);
		
		// The last is the abovePos.
		checkPoints[4] = abovePos;
		
		// Run through the check points...
		for(int i = 0; i < checkPoints.Length; i++)
		{
			// ... if the camera can see the player...
			if(ViewingPosCheck(checkPoints[i]))
				// ... break from the loop.
				break;
		}
		
		// Lerp the camera's position between it's current position and it's new position.
		transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
		
		// Make sure the camera is looking at the player.
		SmoothLookAt();
	
	}

	bool ViewingPosCheck (Vector3 checkPos)
	{
		RaycastHit hit;
		
		// If a raycast from the check position to the player hits something...
		if(Physics.Raycast(checkPos, character.position - checkPos, out hit, relCameraPosMag))
			// ... if it is not the player...
			if(hit.transform != character)
				// This position isn't appropriate.
				return false;
		
		// If we haven't hit anything or we've hit the player, this is an appropriate position.
		newPos = checkPos;
		return true;
	}

	void SmoothLookAt ()
	{
		Vector3 v = new Vector3(character.transform.position.x,character.transform.position.y - 1f, character.transform.position.z + 4f);

		// Create a vector from the camera towards the player.
		Vector3 relPlayerPosition = v - transform.position;
		
		// Create a rotation based on the relative position of the player being the forward vector.
		Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
		
		// Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
		transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.deltaTime);
	}
}
