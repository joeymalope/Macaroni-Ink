using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {
	public float speed=5f;
	public GameObject explosion;
	public GameObject player;
	Component tmpComp;
	//public enum direction {up,right,down,left};
	//bool facingRight=true;
//	playerControls controls;
	playerControls.direction currDirection;
	bool hurtPlayer =false;
	bool hurtEnemy=false;
	// Use this for initialization

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
//		controls = player.GetComponent<playerControls> ();
		//Debug.Log ("player =="+player);
		Destroy(gameObject, 1);
		//currDirection = controls.getDirection ();
	}

	public void SetDirection(playerControls.direction d){
		currDirection=d;
	}

	public void SetHurtPlayer(bool h){
		hurtPlayer=h;
	}

	public void SetHurtEnemy(bool h){
		hurtEnemy=h;
	}
	// Update is called once per frame
	void Update () {
		if (player != null) {
			//playerControls.direction currDirection = controls.getDirection ();
			if (currDirection==playerControls.direction.right) 
					GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position + new Vector2 (speed, 0) * Time.deltaTime);
			else if(currDirection== playerControls.direction.left)					
					GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position + new Vector2 (-speed, 0) * Time.deltaTime);
			else if(currDirection== playerControls.direction.down)					
				GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position + new Vector2 (0,-speed) * Time.deltaTime);
			else if(currDirection== playerControls.direction.up)					
			GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position + new Vector2 (0,speed) * Time.deltaTime);
	
			}
		}


	void OnCollisionEnter2D (Collision2D col)
	{
		//if(col.gameObject.tag== "wall")
		//Debug.Log ("haaibo!!!");
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "wall") {
			//Debug.Log ("haaibo!!! ke trigga ,trigga");
			explode ();
		} else if (col.tag == "Player" && hurtPlayer) {
			explode ();
			if(col.gameObject.GetComponent<playerControls> ()!=null)
				col.gameObject.GetComponent<playerControls> ().dead ();
		} else if (col.tag == "enemy" && hurtEnemy) {
			explode ();
			tmpComp = col.gameObject.GetComponent<enemyAI> ();

		if(tmpComp!=null)
				col.gameObject.GetComponent<enemyAI> ().dead ();
		}
	}

	void explode(){
		Instantiate(explosion,transform.position,transform.rotation);
		Destroy (gameObject);
	}
}
