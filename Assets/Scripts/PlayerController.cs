﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public int playerid;
	public float speed;
	public float jumpforce = 10.0f;
	public Text HUDTxt;
	public GameObject emp;
	public Animator animator;
	public float attackDuration;
	public float jump;
	public AudioClip jmpsfx;
	public AudioClip eatmealsfx;
	public AudioClip hurtsfx;
	public AudioClip kosfx;
	public AudioClip smallsfx;
	public AudioClip midsfx;
	public AudioClip bigsfx;

	//ref to the otehr player
	public GameObject otherPlayer;
	private Animator otherPlayerAnimator;
	public Text WinTxt;

	private Rigidbody2D rb2d;
	private BoxCollider2D bc2d;
	private AudioSource asrc;

	private float energy;
	private int life;
	private float jmptime;

    //stop move on fire
    private float stopMove;


	public GameObject lifeBar;
    private float lifeBarScaleXFactor;
    public GameObject powerBar;
    private float powerBarScaleXFactor;

    public GameObject EMKingBar;

	void Start () {
		asrc = GetComponent<AudioSource> ();
		//Physic
		rb2d = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		bc2d = GetComponent<BoxCollider2D> ();



		//Animator of other player for anim on win
		otherPlayerAnimator = otherPlayer.GetComponent<Animator> ();

		//PLAYER
		energy = 1;
		life = 10;
        lifeBarScaleXFactor = lifeBar.transform.localScale.x / life;
        powerBarScaleXFactor = 25f;

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

		//Exit
		if (Input.GetButtonDown ("Cancel")) {
			SceneManager.LoadScene("Start");
			SceneManager.UnloadSceneAsync("Main");
		};


		//Move X
		float moveHoriz = Input.GetAxis ("Horizontal" + playerid);

		if (moveHoriz != 0 && Time.time > stopMove && life > 0) {
			animator.SetTrigger ("playerWalk");
			transform.position += new Vector3 (moveHoriz, 0.0f, 0.0f) * Time.deltaTime * speed;
		} else {
			animator.SetTrigger ("playerIdle");
		}

		//float moveVert = Input.GetAxis ("Vertical" + playerid);

		if (Input.GetButtonDown ("Fire1" + playerid) && energy >= 1 && life > 0)  {
			//play attack animation
			animator.SetTrigger ("playerAttack");
			//set stop move duration
			stopMove = Time.time + attackDuration;

			//EMP
			GameObject empinst = Instantiate (emp, transform.position, transform.rotation);
            float realEnergy = energy;
            realEnergy = Mathf.Clamp(realEnergy, 0f, 3f);
            //fix energy level
            if (energy >= 3) {
				energy = 3;
				asrc.PlayOneShot (bigsfx, 0.5f);
			} else if (energy >= 2) {
				energy = 2;
				asrc.PlayOneShot (midsfx, 0.5f);
			} else {
				energy = 1;
				asrc.PlayOneShot (smallsfx, 0.5f);
			}
			empinst.SendMessage("SetEnergy", energy);
			empinst.SendMessage("SetOwnerId", playerid);

            //Reset EMP
            //reset scale
            powerBar.transform.localScale = new Vector3(0, 1, 1);
            //resert position
            int sign = 1;
            if (playerid == 1) sign = -1;

            float newPosX = powerBar.transform.position.x + (sign * 0.125f * realEnergy);
            powerBar.transform.position = new Vector3(newPosX, powerBar.transform.position.y, 1);

            //reset energy value
            energy = 0;

            SetHUDText ();
		}

		//Jump Move Y
		float jump = Input.GetAxis ("Vertical" + playerid);
		if (jump > 0 && jmptime <= 0.0f && life > 0) {
			rb2d.AddForce(Vector2.up * jumpforce);
			jmptime = 0.8f;
			asrc.PlayOneShot (jmpsfx, 0.5f);
		}

	}
		
	void OnTriggerEnter2D(Collider2D other) {

		if (other.CompareTag ("PickUp") || other.CompareTag ("PickUp2") || other.CompareTag ("PickUp3")) {

			animator.SetTrigger ("playerEat");
            asrc.PlayOneShot(eatmealsfx, 0.5f);
            Destroy(other.gameObject);

            if (energy < 3)
            {
                float pickupValue = 0;

                if (other.CompareTag("PickUp"))
                {
                    energy += 0.15f;
                    pickupValue = 0.15f;
                }
                if (other.CompareTag("PickUp2"))
                {
                    energy += 0.25f;
                    pickupValue = 0.25f;
                }
                if (other.CompareTag("PickUp3"))
                {
                    energy += 0.5f;
                    pickupValue = 0.5f;
                }

                //scale power bar
                float energyPowerBar = Mathf.Clamp(energy, 0f, 3f);
                powerBar.transform.localScale = new Vector3(powerBarScaleXFactor * energyPowerBar, 1, 1);

                int sign = -1;
                if (playerid == 1) sign = 1;
                if (energy > 3)
                    pickupValue -= energy - 3;

                float newPosX = powerBar.transform.position.x + (sign * 0.125f * pickupValue);
                powerBar.transform.position = new Vector3(newPosX, powerBar.transform.position.y, 1);

                SetHUDText();
            }
		}

	}

	void HurtPlayer(int damage) {

        //lifeBar scale X linear
        lifeBar.transform.localScale = new Vector3(lifeBar.transform.localScale.x - lifeBarScaleXFactor, 1, 1);
        //Debug.Log("orthographicSize=" + Camera.main.orthographicSize);
        //float newPosX = lifeBar.transform.position.x + ((((lifeBar.transform.position.x / life) )) / 10) * Camera.main.orthographicSize;

        int sign = 1;
        if (playerid == 1) sign = -1;

        //lifebar pos X linear
        float newPosX = lifeBar.transform.position.x + (sign * 0.0375f * damage);
        lifeBar.transform.position = new Vector3(newPosX, lifeBar.transform.position.y, 1);

        //decrease life value
        life -= damage;

		if (life > 0) {
			asrc.PlayOneShot (hurtsfx, 1.0f);
			animator.SetTrigger ("playerHit");
		} else {
			//Dead
			asrc.PlayOneShot (kosfx, 1.0f);
			animator.SetTrigger ("playerDead");


			//for win player
			otherPlayerAnimator.SetTrigger ("playerWin");
			int idWin = 1;
			if (playerid == 1) idWin = 2;
				
			WinTxt.text = "PLAYER " + idWin.ToString () + " WINS !!!";

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
		//HUDTxt.text = "Player " + playerid.ToString() + " EM " + energy.ToString () + " Life " + life.ToString();
		HUDTxt.text = energy.ToString ();
	}
}
