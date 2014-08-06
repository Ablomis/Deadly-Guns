using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public int gameState;						// Is it player turn or enemy turn or script scene or other? (0 - scene; 1 - player; 2 -enemy)
	public float aiTimer = 3f;					// Debug stuff: timer which monitors opponent turn time;
	public GameObject spawnObject;				//Drag object prefab to variable in inspector

	private InputController inputController;	// Reference to input controller
	private GUIController guiController;		// Reference to GUI controller
	private GameObject selectedPlayerObject;	// Reference to selected character
	private GameObject[] playerCharList;		// list ofplayer Characters

	// Use this for initialization
	void Awake () {
		gameState = 1;
		inputController = GameObject.Find("game_controller").GetComponent<InputController>();
		guiController = GameObject.Find("camera_main").GetComponent<GUIController>();
		//selectedPlayerObject = GameObject.Find ("char_enemy_001");
		playerCharList = new GameObject[2];
		playerCharList[0] = Instantiate(spawnObject,new Vector3(-7.5f, 0f, 1.5f),Quaternion.identity) as GameObject;
		//Debug.Log (playerCharList [0]);
		playerCharList[0].GetComponent<CharModel>().CharacterSetup("Raven", (Texture2D) Resources.Load("Textures/Raven", typeof(Texture2D)), new Vector3(-7.5f, 0f, 1.5f));
		//Debug.Log (playerCharList [0].GetComponent<CharModel> ().GetName ());
		playerCharList[1] = Instantiate(spawnObject,new Vector3(-7.5f, 0f, 0.5f),Quaternion.identity) as GameObject;
		playerCharList[1].GetComponent<CharModel>().CharacterSetup("Raider", (Texture2D) Resources.Load("Textures/raider", typeof(Texture2D)), new Vector3(-7.5f, 0f, 0.5f));
	}
	
	// Update is called once per frame
	void Update () {
		switch (gameState){
			case 1:
				break;
			case 2:
				// update AI
				aiTimer -= Time.deltaTime;
				if (aiTimer<0){
					startPlayerTurn();
				}
				break;
		}
	}

	public void EndPlayerTurn()
	{
		inputController.BlockInput (true);
		gameState = 2;
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
	public void startPlayerTurn(){
		gameState = 1;
		guiController.guiState = 1;
		inputController.BlockInput (false);
		aiTimer=3;
	}
	public void SetSelectedPlayerObject(GameObject o)
	{
		selectedPlayerObject = o;
	}
	public GameObject GetSelectedPlayerObject()
	{
		return selectedPlayerObject;
	}
		public void MoveSelectedPlayerObject(Vector3 coordinates){
		selectedPlayerObject.GetComponent<CharModel> ().MoveTo(coordinates);
	}
	public void SelectPlayerObject(GameObject o)
	{
		if(selectedPlayerObject!=null)
			selectedPlayerObject.GetComponent<CharModel>().Deselect();
		selectedPlayerObject = o;
		selectedPlayerObject.GetComponent<CharModel> ().Select ();
	}
	public void MoveSelectedCharacter(Vector3 v){
		if(selectedPlayerObject.GetComponent <CharModel> ().GetCurrentAP()>0)
			selectedPlayerObject.GetComponent<CharModel> ().MoveTo(v);
	}	
}
