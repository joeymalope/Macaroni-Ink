using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerControls : MonoBehaviour {
	public float speed=6f;
	Animator playerAnimator;
	public GameObject bullet,tmp;
	public bool facingRight= true;
	public float prev = 1;
	public float bulletOffset=-3f;
	public enum direction {up,right,down,left};
	public direction currDirection=direction.right;
	RaycastHit2D[] hits;
	public GameObject gunEnd;
	public GameObject thoughts;
	public bool isDead=false,awe=true;
	public float lastshort=0.0f,fireRate= 0.1f;
	public bool activePlayer = false;
	// Use this for initialization
	void Start () {
		playerAnimator = GetComponent<Animator> ();
		//healthBar = GameObject.FindGameObjectWithTag ("healthBar").GetComponent<Image>();
		InvokeRepeating("aweness",2,Random.Range(2,9));
	}

	void aweness(){
		awe = !awe;
	}

	public void activatePlayer(){
		activePlayer = true;
	}

	public void deactivatePlayer(){
		activePlayer = false;
	}

	public direction getDirection(){
		return currDirection;
	}

	// Update is called once per frame
	public virtual void Update () {

		hits = Physics2D.LinecastAll (transform.position, transform.position + new Vector3 (20, 0, 0), 1 << LayerMask.NameToLayer ("wall")); 
		//Debug.Log ("something on the right");
		foreach (RaycastHit2D hit in hits)
			hit.collider.gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
	}

	public void FixedUpdate(){
			movePlayer ();
	}


	protected void FlipLeft ( )
	{
		// Switch the way the player is labelled as facing.
		facingRight = false;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	protected void FlipRight ( )
	{
		// Switch the way the player is labelled as facing.
		facingRight = false;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		//theScale.x *= scale;
		transform.localScale = theScale;
	}

	protected void goLeft(){
		currDirection=direction.left;
		FlipLeft ();	
	}

	protected void goRight(){
		currDirection=direction.right;
		FlipRight ();
		facingRight = true;

	}

	public virtual void movePlayer(){
		if (activePlayer) {
			float hA = Input.GetAxisRaw ("Horizontal");
			float vA = Input.GetAxis ("Vertical");

			prev = hA;
			resetParams ();

			//if the player presses keys for horizontal movement
			if (Input.GetButton ("Horizontal")) {
				if (hA == -1 && facingRight == true) {
					goLeft ();
				} else if (hA == 1 && facingRight == false) {
					goRight ();
				}

				GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position + new Vector2 (hA, vA) * speed * Time.deltaTime);

				playerAnimator.SetFloat ("hSpeed", Input.GetAxisRaw ("Horizontal") * 5);
			}


			if (Input.GetButton ("Vertical")) {
				GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position + new Vector2 (hA, vA) * speed * Time.deltaTime);
				playerAnimator.SetFloat ("vSpeed", Input.GetAxis ("Vertical") * 5);

				//if the player presses keys for vertical movement
				if (vA < 0) {
					currDirection = direction.down;
				}

			}

			if (Input.GetButtonDown ("Vertical")) {
				if (vA > 0) {
					currDirection = direction.up;
				}
			}

			if (Input.GetButtonUp ("Vertical") || Input.GetButtonUp ("Horizontal")) {
				//currDirection=direction.down;
				if (facingRight)
					currDirection = direction.right;
				else
					currDirection = direction.left;
			}


			if (Input.GetButton ("Jump")) {
				playerAnimator.SetBool ("recruiting", true);	
				//dead ();
			}

			if (Input.GetButtonDown ("Fire1")) {
				playerAnimator.SetBool ("usingSpecialSkill", true);	
				if (Time.time > lastshort + fireRate) {
					tmp = Instantiate (bullet, gunEnd.transform.position, transform.rotation);
					lastshort = Time.time;
				}
				tmp.GetComponent<bullet> ().SetHurtEnemy (true);
				tmp.GetComponent<bullet> ().SetDirection (currDirection);
			}


			prev = hA;

			if (!isDead)
				thinking ();
			else
				Debug.Log ("is dead :)");

		}
	}

	public void dead(){
		playerAnimator.SetTrigger ("dead");	
		playerAnimator.SetBool ("isDead",true);	
		isDead = true;
		thoughts.GetComponent<SpriteRenderer>().enabled=false;
		removeScript ();
		Destroy (GetComponent<BoxCollider2D> ());

	}

	//make thoughts object visible during idle mode
	void thinking(){
		
		if(!Input.anyKeyDown && !Input.anyKey && awe)
			thoughts.GetComponent<SpriteRenderer>().enabled=true;
		else 		
			thoughts.GetComponent<SpriteRenderer>().enabled=false;

	}

	//the parameter resets should be done in the should be done on onButtonUp
	public void resetParams(){
		playerAnimator.SetFloat ("vSpeed",0);
		playerAnimator.SetFloat ("hSpeed",0);
		playerAnimator.SetBool ("recruiting", false);
		playerAnimator.SetBool ("usingSpecialSkill", false);

	}

	public void removeScript(){
		Destroy (this);
	}

}