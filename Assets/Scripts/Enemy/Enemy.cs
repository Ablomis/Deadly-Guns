using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, ITargetable {

	public int health = 100;
	public enum States {IDLE, WALKING, RUNNING, SHOOTING, DYING };
	public bool active;	

	private States state;
	private Animator anim;

	void Awake () {
		state = States.IDLE;
		// Setting up the references.
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		switch(state){
			case States.IDLE:
				break;
			case States.DYING:
				break;
		}
	}

	public void TakeHit(int damage){
		if (health>0)
			health -= damage;
		Debug.Log (health.ToString());

		if (health<0){
			state = States.DYING;
			Die ();
		}
	}
	public void Die(){
		anim.SetBool ("Dead", true);
		active = false;
	}
	public bool IsActive(){
		return active;
	}
}
