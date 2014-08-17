using UnityEngine;
using System.Collections;

public class CharProfile {

	private string name;
	private int aimingSkill;
	public Texture2D portrait;

	// Update is called once per frame
	public CharProfile (string n, int a, Texture2D t) {
		name = n;
		aimingSkill = a;
		portrait = t;
	}

	public string GetName(){
		return name;
	}
	public Texture2D GetPortrait(){
		return portrait;
	}
	public int GetAimingSkill(){
		return aimingSkill;
	}
	public void SetName(string n){
		name = n;
	}
	public void SetPortrait(Texture2D t){
		portrait = t;
	}
	public void SetAimingSkill(int a){
		aimingSkill = a;
	}
}
