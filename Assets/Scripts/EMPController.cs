using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPController : MonoBehaviour {

	public float increment = 0.00075f;
	public float timeratio = 0.02f;
	private int energy = 1;
	private float time;
	private int ownerid;

	void Start () {
		time = 0.0f;
	}

	void SetEnergy(int e) {
		Debug.Log ("SetEnergy=" + e);
		energy = e;
	}

	void SetOwnerId(int id) {
		ownerid = id;
	}

	void Update() {
		time += Time.deltaTime;
		transform.localScale += new Vector3 (increment, increment, 0.0f) * energy * 10.0f * Time.deltaTime;
		if (time >= timeratio * energy) {
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player") && (other.gameObject.GetComponent<PlayerController>().playerid != ownerid)) {
			other.gameObject.SendMessage("HurtPlayer", 1);
		}
	}
}
