using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	public bool inputBlocked;
	public LayerMask rayLayerMask;

	private CameraController cameraController;						// Get reference to a camera controller
	private HashIDs hash;
	private GameObject cursor;
	private float gridXdef = 1.5f;
	private float gridZdef = -1.5f;
	private float gridXcenterDef = 2.0f;
	private float gridZcenterDef = -1.0f;
	private CharModel charModel;
	private bool charSelected;
	private GameController gameController;
	enum inputStates {CHAR_SELECTED, CHAR_DESELECTED, INPUT_BLOCKED};
	private inputStates iState;
	private inputStates previousState;

	void Awake(){
		cameraController = GameObject.Find("camera_main").GetComponent<CameraController>();
		gameController = GameObject.Find("game_controller").GetComponent<GameController>();
		cursor = GameObject.Find("mouse_cursor_move");
		//inputBlocked = false;
		//charSelected = false;
		iState = inputStates.CHAR_DESELECTED;
		previousState = inputStates.CHAR_DESELECTED;
		cursor.SetActive(true);
	}

	// Update is called once per frame
	void Update () {

		//switch


		if (iState!=inputStates.INPUT_BLOCKED){
			if (Input.GetKey("w") || Input.GetKey("up")){
				cameraController.moveCamera("w");
			}
			if (Input.GetKey("s") || Input.GetKey("down")){
				cameraController.moveCamera("s");
			}
			if (Input.GetKey("a") || Input.GetKey("left")){
				cameraController.moveCamera("a");;
			}
			if (Input.GetKey("d") || Input.GetKey("right")){
				cameraController.moveCamera("d");
			}
			if (Input.GetKey("q")){
				cameraController.rotateCamera("q");
			}
			if (Input.GetKey("e")){
				cameraController.rotateCamera("e");
			}
			if (Input.GetAxis("Mouse ScrollWheel")!=0f){
				cameraController.moveCameraVertical(Input.GetAxis("Mouse ScrollWheel"));
			}

			MouseUpdate ();

		}
		else {
			cursor.SetActive(false);
		}
	}

	Vector3 getGridCenter(Vector3 p){
		Vector3 gridCenter; 
		gridCenter.x = 2.5f - Mathf.Ceil (Mathf.Abs(p.x - gridXdef));
		gridCenter.z = Mathf.Ceil (p.z - gridZdef) - 2.5f;
		gridCenter.y = 0.5f;
		//Debug.Log(p.x);
		//Debug.Log(gridCenter.x);
		//Debug.Log(gridCenter.y);

		return gridCenter;
	}
	void MouseUpdate()
	{
		//cursor.SetActive(true);
		Ray vRay = cameraController.hitRay();
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(vRay, out hit, 100, rayLayerMask)) 
		{	
			//Debug.DrawLine(vRay.origin, hit.point, Color.red);
			//Debug.Log(hit.transform.tag);
			if(hit.transform.tag == "Player")
			{
				cursor.SetActive(false);
			}
			else if((hit.transform.tag == "Floor") && (iState==inputStates.CHAR_SELECTED))
			{
				//cursor.SetActive(false);
				cursor.transform.position = getGridCenter(hit.point);
				cursor.SetActive(true);
			}
		}

		//cursor.SetActive(false);
		if(Input.GetMouseButtonDown(0)){
			if(Physics.Raycast(vRay, out hit, 100, rayLayerMask)){
				if(hit.transform.tag == "Player"){
					charModel = hit.transform.gameObject.GetComponent<CharModel>();
					charModel.Select();
					gameController.SelectPlayerObject(hit.transform.gameObject);
					iState = inputStates.CHAR_SELECTED;
					//cursor.SetActive(true);
				}
			}

		}

		if(Input.GetMouseButtonDown(1)){
			if(Physics.Raycast(vRay, out hit, 100, rayLayerMask)){
				if((hit.transform.tag == "Floor") && (iState==inputStates.CHAR_SELECTED)){
					gameController.MoveSelectedCharacter(getGridCenter(hit.point));
				}
			}
		}

	}

	public void BlockInput(bool t){
		if (t) {
			previousState = iState;
			iState = inputStates.INPUT_BLOCKED;
		}
		else iState = previousState;
	}
}
