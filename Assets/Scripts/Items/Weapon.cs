using UnityEngine;
using System.Collections;

public class Weapon : Item {

	public AudioClip shotClip;                          // An audio clip to play when a shot happens.
	public float flashIntensity = 3f;                   // The intensity of the light when the shot happens.
	public float fadeSpeed = 10f;                       // How fast the light will fade after the shot.
	public bool shooting;


	private string type;								// Weapon type: Pistol / SMG/ AR/ SR/ Shotgun
	private int min_damage = 110;
	private int max_damage = 125;
	private int maneuverability;
	private	int range;
	private int accuracy;
	private LineRenderer laserShotLine;                 // Reference to the laser shot line renderer.
	private Light laserShotLight;                       // Reference to the laser shot light.
	private float shot_timer = 0.5f;

	public void Awake(){
		laserShotLine = GetComponentInChildren<LineRenderer>();
		laserShotLight = laserShotLine.gameObject.light;

		shooting = false;
	}

	public void Update(){

		if (shooting){
			if (shot_timer>0)
				shot_timer -= Time.deltaTime;
			else {
				shot_timer = 0.5f;
				shooting = false;
			}
		}
		// Fade the light out.
		laserShotLight.intensity = Mathf.Lerp(laserShotLight.intensity, 0f, fadeSpeed * Time.deltaTime);
	}

	// Use this for initialization
	public void SetWeapon (string t, int d, int d2, int m, int r, int a) {
		type = t;
		min_damage = d;
		max_damage = d2;
		maneuverability = m;
		range = r;
		accuracy = a;
	}

	public int MakeShot (int skill){
		if (Random.Range (0, 100) <= skill)
						return min_damage;
		else return -1;
	}

	public int Shoot(Vector3 target)
	{
		// The enemy is shooting.
		shooting = true;

		int damage = min_damage + Mathf.FloorToInt ((max_damage - min_damage) * Random.Range (0, 100) / 100);
		ShotEffects (target);
		return damage;
	}

	public void Idle(){
		shooting = false;

		// Turn on the line renderer.
		laserShotLine.enabled = false;
	}

	public int GetAccuracy(){
		return accuracy;
	}

	public int GetRange(){
		return range;
	}

	void ShotEffects (Vector3 target)
	{

		// Set the initial position of the line renderer to the position of the muzzle.
		laserShotLine.SetPosition(0, laserShotLine.transform.position);
		
		// Set the end position of the player's centre of mass.
		laserShotLine.SetPosition(1, target + Vector3.up * 1.5f);
		
		// Turn on the line renderer.
		laserShotLine.enabled = true;
		
		// Make the light flash.
		laserShotLight.intensity = flashIntensity;
		
		// Play the gun shot clip at the position of the muzzle flare.
		AudioSource.PlayClipAtPoint(shotClip, laserShotLight.transform.position);
	}
	
}
