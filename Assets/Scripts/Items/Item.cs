using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	private float weight;
	private int size;
	private Texture2D icon;
	private int cost;

	// Update is called once per frame
	void Update () {
	
	}

	public void SetIcon(string s){
		icon = (Texture2D)Resources.Load ("Textures/" + s, typeof(Texture2D));
	}

	public Texture2D GetIcon(){
		return icon;
	}
}
