using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public int playerid;
	public float speed;
	public float jumpforce = 10.0f;
	public Text HUDTxt;
	public GameObject emp;
	public Animator animator;

	private Rigidbody2D rb2d;
	private BoxCollider2D cb2d;

	private int energy;
	private int life;


	void Start () {
		//Physic
		rb2d = GetComponent<Rigidbody2D> ();
		cb2d = GetComponent<BoxCollider2D> ();

		//Animation
		animator = GetComponent<Animator> ();

		//player def
		energy = 2;
		life = 10;

		SetHUDText ();
	}

	void Update () {
		
	}

	void FixedUpdate () {

		//Move X
		float moveHoriz = Input.GetAxis ("Horizontal" + playerid);
		if (moveHoriz != 0) {
			animator.SetTrigger ("playerWalk");
			transform.position += new Vector3 (moveHoriz, 0.0f, 0.0f) * Time.deltaTime * speed;
		} else {
			animator.SetTrigger ("playerIdle");
		}

		//float moveVert = Input.GetAxis ("Vertical" + playerid);

		if (Input.GetButton ("Fire1" + playerid)) {
			GameObject empinst = Instantiate (emp, transform.position, transform.rotation);
			empinst.SendMessage("SetEnergy", ((float) energy));
			empinst.SendMessage("SetOwnerId", playerid);
			energy = 0;
			SetHUDText ();
		}

		if (Input.GetButtonDown ("Jump" + playerid) && IsGrounded()) {
			rb2d.AddForce(Vector2.up * jumpforce);
		}
	}
		
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("PickUp")) {
			Destroy(other.gameObject);
			energy++;
			SetHUDText ();
		}
	}

	void HurtPlayer(int damage) {
		life -= damage;
		if (life <= 0) {
			// You lose !
		}
		SetHUDText ();
	}

	bool IsGrounded() {
		RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * cb2d.size.y / 2, -Vector2.up, 0.01f);

		if (hit.collider != null ) {
			return true;
		}
		return false;
	}

	void SetHUDText() {
		HUDTxt.text = "Player " + playerid.ToString() + " EM " + energy.ToString () + " Life " + life.ToString();
	}
}
