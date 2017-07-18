using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour {
	public GameObject gunEnd;
	public GameObject bullet,tmp;
//	public Bullet2 bullet,tmp;
	public Animator anim;
	bool facingRight=true,allowfire=true;
	float speed= 6f,sightDistance=8f;
	public bool isDead=false;
	float attackDuration=5f;
	bool reIterate=true;
	public enum State{
		Patrol,alert,attack
	}
	public float lastshort=0.0f,fireRate= 0.5f;

	public GameObject player;
	public playerControls.direction currDirection;

	public int directionIndex =0;
	public State state = State.Patrol;
	// Use this for initialization
	void Start () {
	//	StartCoroutine ("shoot");
	}

	int getIndex(){
		return directionIndex%4;
	}

	public void resetParams(){
		anim.SetFloat ("vSpeed",0);
		anim.SetFloat ("hSpeed",0);
		anim.SetBool ("recruiting", false);
		anim.SetBool ("usingSpecialSkill", false);

	}

	// Update is called once per frame
	void Update () {
		//StartCoroutine ("shoot");
		//shoot();
		resetParams ();
		switch(currDirection){
		case playerControls.direction.up:{
				anim.SetFloat ("vSpeed", 2);
			}break;
		case playerControls.direction.right:{
				anim.SetFloat ("hSpeed", 2);
				goRight ();
			}break;
		case playerControls.direction.down:{
				anim.SetFloat ("vSpeed", -2);
			}break;
		case playerControls.direction.left:{
				anim.SetFloat ("hSpeed", 2);
				goLeft ();
			}break;
		}

		if (playerScan() && playerInSight ()) {
			//	Debug.Log ("Player insight");
			if (attackDuration <= 0.5 && reIterate)
				attackDuration = 5f;
			else
				state = State.Patrol;
			
			if(state!=State.attack)	
				state = State.attack;
		} else
			state = State.Patrol;

		switch(state){
			case State.Patrol :
			patrol();
			break;
			case State.alert :
			alert();break;
			case State.attack :
			attack();break;
		};	

	//	Debug.Log("upAhead=="+upAhead());
	//	Debug.Log("canIGoUp=="+canIGoUp());
	//	Debug.Log("canIGoDown=="+canIGoDown());
	//	Debug.Log("canIGoRight=="+canIGoRight());
	//	Debug.Log("canIGoLeft=="+canIGoLeft());
//			Debug.DrawRay (transform.position,transform.position+new Vector3(5,0,0),Color.green,20);
//			Debug.DrawRay (transform.position,transform.position+new Vector3(-5,0,0),Color.green,20);
//			Debug.DrawRay (transform.position,transform.position+new Vector3(0,5,0),Color.green,20);
//			Debug.DrawRay (transform.position,transform.position+new Vector3(0,-5,0),Color.green,20);
	}


	void attack(){
	//	print (attackDuration);
		if (attackDuration > 0.5)
			attackDuration -= Time.deltaTime;
		else
			state = State.Patrol;

		if (reIterate)
			reIterate = false;	
		 shoot();
	}


	void Move(){
	//	Debug.Log ("in move");
		switch (directionIndex) {
		case 0:
			{
				GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position+ new Vector2(0,1)*speed*Time.deltaTime);
				break;
			}

		case 1:
			{
				GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position+ new Vector2(1,0)*speed*Time.deltaTime);
				break;
			}

		case 2:
			{
				GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position+ new Vector2(0,-1)*speed*Time.deltaTime);
				break;
			}

		case 3:
			{
				GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position+ new Vector2(-1,0)*speed*Time.deltaTime);
				break;	
			}
		}
	}



	void alert(){
		
	}

	void patrol(){
		if(!reIterate){
			reIterate=true;
		}

		if (upAhead ()) {
			print ("direction should change!!");
			if (canIGoUp () && validDirection (0)) {
				currDirection = playerControls.direction.up;
			} else if (canIGoRight () && validDirection (1)) {
				currDirection = playerControls.direction.right;
			} else if (canIGoDown () && validDirection (2)) {
				currDirection = playerControls.direction.down;
			} else if (canIGoLeft () && validDirection (3)) {
				currDirection = playerControls.direction.left;
			}
			else
				currDirection = Reverse (currDirection);
		}
		directionIndex = directionToIndex ();
		walk ();

		}

	int directionToIndex(){
		switch(currDirection){
		case playerControls.direction.up:{
				return 0;
			}
		case playerControls.direction.right:{
				return 1;
			}
		case playerControls.direction.down:{
				return 2;
			}
		case playerControls.direction.left:{
				return 3;
			}
		}

		return -1;
	}

	bool validDirection(int i){

		switch(i){
			case 0:{
					return playerControls.direction.up != Reverse (currDirection);
				}
			case 1:{
					return playerControls.direction.right != Reverse (currDirection);
				}
			case 2:{
					return playerControls.direction.down != Reverse (currDirection);
				}
			case 3:{
					return playerControls.direction.left != Reverse (currDirection);
				}
		}

		return false;
	}

	playerControls.direction Reverse(playerControls.direction d){
		if (playerControls.direction.up == d)
			return playerControls.direction.down;
		else if (playerControls.direction.right == d)
			return playerControls.direction.left;
		else if (playerControls.direction.down == d)
			return playerControls.direction.up;
		else
			return playerControls.direction.right;
	}

	//walking
	void walk(){
		Move ();
	}

	void shoot(){
		/*
     allowfire = false;
     tempBullet = Instantiate(bullet, transform.position, transform.rotation);
     yield WaitForSeconds(rate with which you want player to fire);
     allowfire = true;

		*/


		if (Time.time > lastshort+ fireRate) {
			tmp = Instantiate (bullet, gunEnd.transform.position, transform.rotation);
			allowfire = false;
			lastshort=Time.time;
		}


		if (tmp != null) {
			tmp.GetComponent<bullet> ().SetDirection (currDirection);
			tmp.GetComponent<bullet> ().SetHurtPlayer (true);
		}

		//yield return new WaitForSeconds(0.5f);
		//allowfire = true;
	}

	bool upAhead(){
		//Debug.Log ("in upAhead()");
		switch(currDirection){
		case playerControls.direction.up:{
			//	Debug.Log ("-- up && linecast =="+Physics2D.Linecast (transform.position, transform.position+new Vector3(0,4,0), 1 << LayerMask.NameToLayer("wall")));
//				Debug.DrawRay(transform.position, transform.position+new Vector3(0,4,0))
				return Physics2D.Linecast (transform.position, transform.position+new Vector3(0,4,0), 1 << LayerMask.NameToLayer("wall")) && Physics2D.Linecast (transform.position, transform.position+new Vector3(0,4,0), 1 << LayerMask.NameToLayer("enemy")) ;
			}
		case playerControls.direction.right:{
			//	Debug.Log ("-- right");
				return Physics2D.Linecast (transform.position, transform.position+new Vector3(4,0,0), 1 << LayerMask.NameToLayer("wall")) && Physics2D.Linecast (transform.position, transform.position+new Vector3(4,0,0), 1 << LayerMask.NameToLayer("enemy"));
			}
		case playerControls.direction.down:{
		//		Debug.Log ("-- down");
				return Physics2D.Linecast (transform.position, transform.position+new Vector3(0,-4,0), 1 << LayerMask.NameToLayer("wall")) &&  Physics2D.Linecast (transform.position, transform.position+new Vector3(0,-4,0), 1 << LayerMask.NameToLayer("enemy"));
			}
		case playerControls.direction.left:{
		//		Debug.Log ("-- left");
				return Physics2D.Linecast (transform.position, transform.position+new Vector3(-4,0,0), 1 << LayerMask.NameToLayer("wall")) && Physics2D.Linecast (transform.position, transform.position+new Vector3(-4,0,0), 1 << LayerMask.NameToLayer("enemy")) ;
			}
		}

		return false;
	}

	protected void goLeft(){
	//	currDirection=playerControls.direction.left;
		FlipLeft ();	
	}

	protected void goRight(){
		FlipRight ();
		facingRight = true;
	//	currDirection=playerControls.direction.right;
	}

	protected void FlipLeft ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = false;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		if(theScale.x>0)
			theScale.x *= -1;
		transform.localScale = theScale;
	}

	protected void FlipRight ( )
	{
		// Switch the way the player is labelled as facing.
		facingRight = false;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		//Debug.Log ("transform.localScale == " + transform.localScale);
		if(theScale.x<0)
			theScale.x *= -1;
		//theScale.x *= scale;
		transform.localScale = theScale;
	}


	void OnCollisionEnter2D (Collision2D col)
	{
		//if (col.gameObject.tag == "wall")
		//	directionIndex++;
	}



	bool canIGoUp(){
		return !Physics2D.Linecast (transform.position, transform.position + new Vector3 (0, 5, 0), 1 << LayerMask.NameToLayer ("wall")) ||  !Physics2D.Linecast (transform.position, transform.position + new Vector3 (0, 5, 0), 1 << LayerMask.NameToLayer ("enemy"));
	}
	bool canIGoDown(){
		return !Physics2D.Linecast (transform.position, transform.position+new Vector3(0,-5,0), 1 << LayerMask.NameToLayer("wall")) || !Physics2D.Linecast (transform.position, transform.position+new Vector3(0,-5,0), 1 << LayerMask.NameToLayer("enemy"));
	}

	bool canIGoRight(){
		return !Physics2D.Linecast (transform.position, transform.position+new Vector3(5,0,0), 1 << LayerMask.NameToLayer("wall")) || !Physics2D.Linecast (transform.position, transform.position+new Vector3(5,0,0), 1 << LayerMask.NameToLayer("enemy"));
	}

	bool canIGoLeft(){
		return !Physics2D.Linecast (transform.position, transform.position+new Vector3(-5,0,0), 1 << LayerMask.NameToLayer("wall")) ||  !Physics2D.Linecast (transform.position, transform.position+new Vector3(-5,0,0), 1 << LayerMask.NameToLayer("enemy"));
	}

	bool playerInSight(){
	//	player = Physics2D.Linecast (transform.position, transform.position+new Vector3(7,0,0), 1 << LayerMask.NameToLayer("Player")); 

		switch(currDirection){
		case playerControls.direction.up:{
				return Physics2D.Linecast (transform.position, transform.position+new Vector3(0,sightDistance,0), 1 << LayerMask.NameToLayer("Player"));
			}
		case playerControls.direction.right:{
				return Physics2D.Linecast (transform.position, transform.position+new Vector3(sightDistance,0,0), 1 << LayerMask.NameToLayer("Player"));
			}
		case playerControls.direction.down:{
				return Physics2D.Linecast (transform.position, transform.position+new Vector3(0,-sightDistance,0), 1 << LayerMask.NameToLayer("Player"));
			}
		case playerControls.direction.left:{
				return Physics2D.Linecast (transform.position, transform.position+new Vector3(-sightDistance,0,0), 1 << LayerMask.NameToLayer("Player"));
			}
		}

		return player != null;
	}

	bool playerScan(){
		Debug.DrawRay (transform.position,transform.position+new Vector3 (0, sightDistance, 0),Color.green);
		Debug.DrawRay (transform.position,transform.position+new Vector3 (sightDistance,0, 0),Color.green);
		Debug.DrawRay (transform.position,transform.position+new Vector3 (-sightDistance,0, 0),Color.green);
		Debug.DrawRay (transform.position,transform.position+new Vector3 (0, -sightDistance, 0),Color.green);
		if (Physics2D.Linecast (transform.position, transform.position + new Vector3 (0, sightDistance, 0), 1 << LayerMask.NameToLayer ("Player")))
			currDirection = playerControls.direction.up;
		else if (Physics2D.Linecast (transform.position, transform.position + new Vector3 (sightDistance, 0, 0), 1 << LayerMask.NameToLayer ("Player")))
			currDirection = playerControls.direction.right;
		else if (Physics2D.Linecast (transform.position, transform.position + new Vector3 (0, -sightDistance, 0), 1 << LayerMask.NameToLayer ("Player")))
			currDirection = playerControls.direction.down;
		else if (Physics2D.Linecast (transform.position, transform.position + new Vector3 (-sightDistance, 0, 0), 1 << LayerMask.NameToLayer ("Player")))
			currDirection = playerControls.direction.left;
		else
			return false;
	//	Debug.Log ("playerScan()=="+true);
		return true;
	} 


	public void dead(){
		anim.SetTrigger ("dead");	
		anim.SetBool ("isDead",true);	
		isDead = true;
	//	thoughts.GetComponent<SpriteRenderer>().enabled=false;
		Destroy (GetComponent<BoxCollider2D> ());
		removeScript ();

	}


	public void removeScript(){
		Destroy (this);
	}
}
