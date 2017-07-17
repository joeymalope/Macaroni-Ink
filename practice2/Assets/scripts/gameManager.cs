using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {
	public GameObject[] squad;
	public GameObject type;
	public GameObject ptr;
	public GameObject halo= null;
	public int active=0;
	public int squadCount=1; 
	public Component controllerScript;
	public Image foodRations;
	//public bool facingRight= true;
	// Use this for initialization
	void Start () {
		squad = new GameObject[8];
		/*for (int i = 0;i<8; i++) {
			squad[i]= Instantiate(type,transform.position+ new Vector3(5f*squadCount,5f,0),transform.rotation);
			squadCount++;
		}*/
//		squad[active%8].AddComponent<playerControls>();
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

	//	if (Input.GetButtonUp ("Fire1")) {
	//		makePlayerActive ();	
	//	}


	}

	void makePlayerActive(){
		
/*		squad[active%8].GetComponent<playerControls>().removeScript();
		removeTag (squad [active % 8]);
		try{
			
			squad[++active%squad.Length].AddComponent<playerControls>();
			addTag(squad[active%8]);
			squad[active%8].GetComponent<followPlayer>().newCharacter=true;
			if(halo==null)
			 halo =Instantiate(halo,transform.position+ new Vector3(0,5f,0),transform.rotation);
			//halo.transform.parent= squad[active].transform;
		}
		catch(System.FormatException e){
			Debug.Log (e);
		}
		//}
*/	}


	void addTag(GameObject o){
		o.tag = "active";
	}

	void removeTag(GameObject o){
		o.tag = "Untagged";
	}
}
