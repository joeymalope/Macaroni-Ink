using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class godScript : MonoBehaviour {
	public GameObject[] squad;
	public GameObject type;
	public GameObject tmp;
	public GameObject halo= null;
	public int active=0;
	public int squadCount=1; 
	public Component controllerScript;
	public Image foodRations;
	public int maxSize = 3;
	//public bool facingRight= true;
	// Use this for initialization
	void Start () {
		squad = new GameObject[maxSize];
		for (int i = 0;i<maxSize; i++) {
			squad[i]= Instantiate(type,transform.position+ new Vector3(5f*squadCount,5f,0),transform.rotation);
		//	squad[i].GetComponent<playerControls>().thoughts= Instantiate(GameObject.Find("thoughts"),transform.position+ new Vector3(5f*squadCount,5f,0),transform.rotation);
		//	squad[i].GetComponent<playerControls>().thoughts.transform.parent=squad[i].transform;
		//	squad[i].GetComponent<playerControls>().thoughts.transform.position= new Vector3(0.0f,1.025f,0.0f);
			if(i==0)
				squad [i].GetComponent<playerControls> ().activatePlayer ();
			squadCount++;
		}
		//	squad[active%8].AddComponent<playerControls>().activatePlayer();
		//		addTag(squad[active%8]);

		squadCount = 0;
		//	halo = GameObject.FindGameObjectWithTag ("halo");
		//halo.transform.parent = GameObject.Find("Player").transform;
		InvokeRepeating("decrease",2,1);
	}

	void decrease(){
		foodRations.GetComponent<HealthBar> ().decreaseHealth (0.75f);
	}
	// Update is called once per frame
	void Update () {
		//	halo=GameObject.FindGameObjectWithTag ("halo");
		//	halo.GetComponent<followPlayer>().resetHalo();

			if (Input.GetButtonUp ("Fire2")) {
				makePlayerNextActive ();	
			}


	}

	void makePlayerNextActive(){

		squad[active%maxSize].GetComponent<playerControls>().deactivatePlayer();
		//removeTag (squad [active % 8]);
		try{
			squad[active%squad.Length].GetComponent<playerControls>().deactivatePlayer();			
			squad[++active%squad.Length].GetComponent<playerControls>().activatePlayer();
			//addTag(squad[active%8]);
			//squad[active%8].GetComponent<followPlayer>().newCharacter=true;
			//if(halo==null)
			// halo =Instantiate(halo,transform.position+ new Vector3(0,5f,0),transform.rotation);
			//halo.transform.parent= squad[active].transform;
		}
		catch(System.FormatException e){
			Debug.Log (e);
		}
		//}
	}


	void addTag(GameObject o){
		o.tag = "active";
	}

	void removeTag(GameObject o){
		o.tag = "Untagged";
	}
}
