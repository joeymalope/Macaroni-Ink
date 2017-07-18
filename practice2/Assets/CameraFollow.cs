using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	GameObject player;       //Public variable to store a reference to the player game object
	public GameObject gameManager; 

	private Vector3 offset;         //Private variable to store the offset distance between the player and camera

	// Use this for initialization
	void Start () 
	{
	//	offset = new Vector3 (10f,10f,0); 
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		player= gameManager.GetComponent<godScript>().squad[gameManager.GetComponent<godScript>().active];
		if(player!=null)
			offset = transform.position - player.transform.position;
		else 
			offset= new Vector3 (10f,10f,0); 
	}

	void Update(){
		print (gameManager.GetComponent<godScript>().active);
		player= gameManager.GetComponent<godScript>().squad[gameManager.GetComponent<godScript>().active % gameManager.GetComponent<godScript>().maxSize];
	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		transform.position = player.transform.position + offset;
	}
}