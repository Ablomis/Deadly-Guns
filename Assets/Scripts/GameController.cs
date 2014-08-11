using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	enum GameStates {PLAYER_TURN, OPPONENT_TURN, ACTION};
	//public int gameState;						// Is it player turn or enemy turn or script scene or other? (0 - scene; 1 - player; 2 -enemy)
	public float aiTimer = 3f;					// Debug stuff: timer which monitors opponent turn time;
	public GameObject spawnObject;				//Drag object prefab to variable in inspector

	private InputController inputController;	// Reference to input controller
	private GUIController guiController;		// Reference to GUI controller
	private GameObject selectedPlayerObject;	// Reference to selected character
	private GameObject[] playerCharList;		// list ofplayer Characters
	private GameObject cameraCinematic;			// Cinematic camera
	private GameObject cameraMain;
	private GameStates gameState;

	// Use this for initialization
	void Awake () {
		gameState = GameStates.PLAYER_TURN;

		// Setting up cameras: main and Cinematic
		cameraMain = GameObject.Find ("camera_main");
		cameraMain.camera.enabled = true;
		cameraCinematic = GameObject.Find ("camera_cinematic");
		cameraCinematic.camera.enabled = false;

		// Getting references to GUI and Input
		inputController = GameObject.Find("game_controller").GetComponent<InputController>();
		guiController = GameObject.Find("camera_main").GetComponent<GUIController>();

		// Initializng player characters
		playerCharList = new GameObject[2];
		playerCharList[0] = Instantiate(spawnObject,new Vector3(-7.5f, 0f, 1.5f),Quaternion.identity) as GameObject;
		playerCharList[0].GetComponent<CharModel>().CharacterSetup("Raven", 90, (Texture2D) Resources.Load("Textures/Raven", typeof(Texture2D)), new Vector3(-7.5f, 0f, 1.5f));
		playerCharList[1] = Instantiate(spawnObject,new Vector3(-7.5f, 0f, 0.5f),Quaternion.identity) as GameObject;
		playerCharList[1].GetComponent<CharModel>().CharacterSetup("Raider", 88, (Texture2D) Resources.Load("Textures/raider", typeof(Texture2D)), new Vector3(-7.5f, 0f, 0.5f));
	}
	
	// Update is called once per frame
	void Update () {
		switch (gameState){
			case GameStates.PLAYER_TURN:
				break;
			case GameStates.OPPONENT_TURN:
				// update AI
				aiTimer -= Time.deltaTime;
				if (aiTimer<0){
					startPlayerTurn();
				}
				break;
			case GameStates.ACTION:
				if(selectedPlayerObject.GetComponent<CharModel>().isIdle())
				{
					gameState = GameStates.PLAYER_TURN;
					cameraMain.GetComponent<CameraController>().UpdatePosition( selectedPlayerObject.GetComponent<CharModel>().GetPosition());
					cameraMain.camera.enabled = true;
					cameraCinematic.camera.enabled = false;
					inputController.BlockInput (false);
				}
				break;
		}
	}

	// Ends player turn and passes turn to AI
	public void EndPlayerTurn()
	{
		inputController.BlockInput (true);
		gameState = GameStates.OPPONENT_TURN;
		guiController.guiState = 2;
		//selectedPlayerObject.GetComponent<CharModel> ().ResetAP ();
		selectedPlayerObject.GetComponent<CharModel>().Deselect();
		for(int i = 0; i<playerCharList.Length; i++){
			playerCharList[i].GetComponent<CharModel> ().ResetAP ();
		}
		if(selectedPlayerObject!=null)
			selectedPlayerObject.GetComponent<CharModel>().Deselect();
		selectedPlayerObject = null;
	}

	// Starts player turn: unblocks input, gui etc.
	public void startPlayerTurn(){
		gameState = GameStates.PLAYER_TURN;
		guiController.guiState = 1;
		guiController.SetCursorSelect ();
		inputController.BlockInput (false);
		aiTimer=3;
	}

	// Marks a player charatcer selected for interaction
	public void SetSelectedPlayerObject(GameObject o)
	{
		selectedPlayerObject = o;
	}

	// Returns selected player character
	public GameObject GetSelectedPlayerObject()
	{
		return selectedPlayerObject;
	}
		public void MoveSelectedPlayerObject(Vector3 coordinates){
		selectedPlayerObject.GetComponent<CharModel> ().MoveTo(coordinates);
	}

	// Highlights player character as selected (Move to GUI????)
	public void SelectPlayerObject(GameObject o)
	{
		if(selectedPlayerObject!=null)
			selectedPlayerObject.GetComponent<CharModel>().Deselect();
		selectedPlayerObject = o;
		selectedPlayerObject.GetComponent<CharModel> ().Select ();
	}

	// Moves selected character at required direction
	public void MoveSelectedCharacter(Vector3 v){
		if(selectedPlayerObject.GetComponent <CharModel> ().GetCurrentAP()>0)
			selectedPlayerObject.GetComponent<CharModel> ().MoveTo(v);
		inputController.BlockInput (true);
		gameState = GameStates.ACTION;
		cameraMain.camera.enabled = false;
		cameraCinematic.camera.enabled = true;
		cameraCinematic.GetComponent<CinematicCameraController> ().CinematicMovement (selectedPlayerObject);
	}

	// Calculates chance to hit for a specific character and target
	public int GetChanceToHit(Transform t){
		return selectedPlayerObject.GetComponent<CharModel> ().profile.GetAimingSkill ();
	}
}
