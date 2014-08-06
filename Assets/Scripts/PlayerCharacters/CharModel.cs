﻿using UnityEngine;
using System.Collections;

public class CharModel : MonoBehaviour {

	public float speedDampTime = 0.1f;  					// The damping for the speed parameter
	public Texture2D portrait;
	public float walkSpeed = 2f;
	public float deadZone = 5f;            					// The number of degrees for which the rotation isn't controlled by Mecanim.

	
	private bool selected;									// Tells us if the char is selected
	private GameObject selector;							// Mesh for player selection
	private int currentAP;
	private string name;									// Character name	
	private NavMeshAgent nav;                               // Reference to the nav mesh agent.
	private float runSpeed = 0.7f;
	enum States {IDLE, WALKING, RUNNING, SHOOTING };
	private States charState;
	private Animator anim; 
	
	// Update is called once per frame
	void Awake () {

		// Setting up the references.
		anim = GetComponent<Animator>();
		selector = transform.Find("char_selector").gameObject;
		selector.SetActive (false);
		nav = GetComponent<NavMeshAgent>();
		currentAP = 2;


		// We need to convert the angle for the deadzone from degrees to radians.
		deadZone *= Mathf.Deg2Rad;
		
	}
	
	void FixedUpdate () {
		switch (charState){
			case States.IDLE:
				//nav.speed = 0f;
				break;
			case States.WALKING:
				Walking ();
				if (nav.destination == transform.position)
					Stop();
				break;
				
		}
		//Debug.Log(anim.GetFloat("Speed"));
	}

	void OnAnimatorMove ()
	{
		// Set the NavMeshAgent's velocity to the change in position since the last frame, by the time it took for the last frame.
		nav.velocity = anim.deltaPosition / Time.deltaTime;
	}

	public void Select()
	{
		selected = true;
		selector.SetActive (true);
	}
	public void Deselect()
	{
		selected = false;
		selector.SetActive (false);
	}
	public Texture2D GetPortrait()
	{
		return portrait;
	}
	public string GetName()
	{
		return name;
	}
	public void MoveTo(Vector3 destination){
		//if(currentAP > 0){
			transform.LookAt (destination);
			nav.destination = destination;
			charState = States.WALKING;
			nav.speed = walkSpeed;
			currentAP -= 1;
		//}
	}
	public void Walking(){

		float speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
		anim.SetFloat ("Speed", speed, speedDampTime, Time.deltaTime);
		//anim.SetFloat("Speed", walkSpeed);
	
	}
	public void Stop(){
		nav.speed = 0f;
		nav.Stop ();
		anim.SetFloat ("Speed", 0f);
		charState = States.IDLE;
	}

	public int GetCurrentAP()
	{
		return currentAP;
	}
	public void ResetAP()
	{
		currentAP = 2;
	}
	public void CharacterSetup(string n, Texture2D p, Vector3 v)
	{
		name = n;
		portrait = p;
		transform.position = v;
	}
	
}