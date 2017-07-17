using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : playerControls {
	public Animator enemyAnimator;
	bool right=true;
	RaycastHit2D player;
	float hA=0,vA=0;
	// Use this for initialization
	void Start () {
		InvokeRepeating("patrol",2,4);
	}

	void patrol(){
		
		//GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position+ new Vector2(hA,vA)*speed*Time.deltaTime);
		enemyAnimator.SetFloat ("hSpeed", hA);
	} 
	// Update is called once per frame
	public	override void Update () {
		movePlayer ();
		if (right)
			hA *= -1;
		
			GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position+ new Vector2(hA,vA)*speed*Time.deltaTime);
		 
		//	playerAnimator.SetFloat ("hSpeed", Mathf.Abs(Input.GetAxisRaw ("Horizontal")*5));
		enemyAnimator.SetFloat ("hSpeed", Input.GetAxisRaw ("Horizontal")*5);
		Debug.Log ("xxxx");
	}

	public override	void movePlayer(){


		if (playerInSight()) {
	//		enemyAnimator.SetBool ("usingSpecialSkill",true);	
			Instantiate(bullet,gunEnd.transform.position,transform.rotation);

		}
			
		
	}

	bool playerInSight(){
		player = Physics2D.Linecast (transform.position, transform.position+new Vector3(20,0,0), 1 << LayerMask.NameToLayer("Player")); 
		Debug.DrawRay (transform.position,transform.position+new Vector3(20,0,0),Color.green,20);
		return player != null;
	}
}
