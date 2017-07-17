using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour {

	public GameObject player;
	//public float test;
	public float vh=0.1f;
	bool offsetAdded=false;
	public Vector3 offset;
	public bool newCharacter = true;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("active");
		this.tag="halo";
		//newChar ();
		newCharacter = true;
	}

	bool newChar(){
		if(newCharacter){
			newCharacter = false;
			return true;
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
		player = GameObject.FindGameObjectWithTag ("active");//get the player

		Vector3 offset=this.transform.position;
		if(newChar()){
			//offset= new Vector3(this.transform.position.x,this.transform.position.y+4f,0); 
			offset=resetHalo();
			offsetAdded=true;

		}
	//	else offset=
		if(offsetAdded)
			this.transform.position=offset;
	//	else 
	//		offset= new Vector3(this.transform.position.x,this.transform.position.y,0); 
		

		//this.transform.position.x=player.transform.position.x;
	//	Debug.Log ("target x == "+player.transform.position.x);
	//	Debug.Log ("follower x == "+transform.position.x);
	}



	public Vector3 resetHalo(){
	//	player = GameObject.FindGameObjectWithTag ("active");
		return new Vector3(this.transform.position.x,this.transform.position.y+4f,0); 
	}
}
