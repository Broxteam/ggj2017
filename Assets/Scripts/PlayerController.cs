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
	public float attackDuration;
	public float jump;

	private Rigidbody2D rb2d;
	private BoxCollider2D bc2d;

	private int energy;
	private int life;
	private float jmptime;

	//stop move on fire
	private float stopMove;

	void Start () {
		//Physic
		rb2d = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		bc2d = GetComponent<BoxCollider2D> ();

		energy = 3;
		life = 10;

		jmptime = 0.0f;
		stopMove = 0;

		SetHUDText ();
	}

	void Update () {
		if (jmptime > 0.0f) {
			jmptime -= Time.deltaTime;
			if (jmptime < 0.0f) {
				jmptime = 0.0f;
			}
		}
	}

	void FixedUpdate () {

		//Move X
		float moveHoriz = Input.GetAxis ("Horizontal" + playerid);

		if (moveHoriz != 0 && Time.time > stopMove) {
			animator.SetTrigger ("playerWalk");
			transform.position += new Vector3 (moveHoriz, 0.0f, 0.0f) * Time.deltaTime * speed;
		} else {
			animator.SetTrigger ("playerIdle");
		}

		//float moveVert = Input.GetAxis ("Vertical" + playerid);

		if (Input.GetButtonDown ("Fire1" + playerid) && energy > 0) {
			//play attack animation
			animator.SetTrigger ("playerAttack");
			//set stop move duration
			stopMove = Time.time + attackDuration;

			//EMP
			GameObject empinst = Instantiate (emp, transform.position, transform.rotation);
			empinst.SendMessage("SetEnergy", ((float) energy));
			empinst.SendMessage("SetOwnerId", playerid);

			//Reset EMP energy
			energy = 0;
			SetHUDText ();
		}

		//Jump Move Y
		float jump = Input.GetAxis ("Vertical" + playerid);
		if (jump > 0 && jmptime <= 0.0f) {
			rb2d.AddForce(Vector2.up * jumpforce);
			jmptime = 0.8f;
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
		RaycastHit2D hit = Physics2D.Raycast(transform.position - (Vector3.up * (bc2d.size.y / 2.0f)), -Vector2.up, 0.00001f);
		if (hit.collider != null) {
			if (hit.collider.gameObject.CompareTag ("Ground") != null) {
				return true;
			}
		}
		return false;
	}

	void SetHUDText() {
		HUDTxt.text = "Player " + playerid.ToString() + " EM " + energy.ToString () + " Life " + life.ToString();
	}
}
