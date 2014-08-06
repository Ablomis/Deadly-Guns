using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {

	public Texture blackTexture;
	public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
	public int guiState;					// 1 - player turn, 2 - enemy turn
	public GUIStyle labelStyle;

	private float alphaFadeValue;
	private GameController gameController;

	void Awake(){
		gameController = GameObject.Find("game_controller").GetComponent<GameController>();
		alphaFadeValue = 0f;
		guiState = 1;
	}

	void OnGUI () {

		switch (guiState){
			case 1:
				if (alphaFadeValue>0f){
					alphaFadeValue -= Mathf.Clamp01(Time.deltaTime / 2);
					if (alphaFadeValue<0f) alphaFadeValue = 0f;
				}
			else DrawTacticalGUI();
				oppositionTurnSplashScreen(alphaFadeValue);
				break;
			case 2:
				if (alphaFadeValue<0.6)
					alphaFadeValue += Mathf.Clamp01(Time.deltaTime / 2);
				oppositionTurnSplashScreen(alphaFadeValue);
				break;
		}
	}

	void oppositionTurnSplashScreen(float f){
				
		GUI.color = new Color(0, 0, 0, f);
		GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), blackTexture );

		GUI.color = new Color(1, 0, 0, alphaFadeValue);
		GUI.Label (new Rect (Screen.width/2, Screen.height/2, 200, 80), "Opposition Turn");
	}

	void DrawTacticalGUI()
	{
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(Screen.width-110,Screen.height-160,100,150),"End Turn")) {
			Debug.Log("Turn ended");
			gameController.EndPlayerTurn();
		}
		// Create character Box
		if(gameController.GetSelectedPlayerObject()!= null){

			GUI.Box(new Rect(10,Screen.height-160,100,150), gameController.GetSelectedPlayerObject().GetComponent<CharModel>().GetName());
			GUI.Label (new Rect (22,Screen.height-140,76,87), gameController.GetSelectedPlayerObject().GetComponent<CharModel>().GetPortrait());
			GUI.Label (new Rect (30,Screen.height-40,76,87), "AP Left " + gameController.GetSelectedPlayerObject().GetComponent<CharModel>().GetCurrentAP().ToString());
		}
	}

}
