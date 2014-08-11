using UnityEngine;
using System.Collections;

public class Weapon {

	private string type;						// Weapon type: Pistol / SMG/ AR/ SR/ Shotgun
	private int damage;
	private int maneuverability;
	private	int range;
	private int accuracy;


	// Use this for initialization
	public Weapon (string t, int d, int m, int r, int a) {
		type = t;
		damage = d;
		maneuverability = m;
		range = r;
		accuracy = a;
	}

	public int MakeShot (int skill){
		if (Random.Range (0, 100) <= skill)
						return damage;
		else return -1;
	}
}
