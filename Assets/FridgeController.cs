using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeController : MonoBehaviour {

	public GameObject pickup;
	public float spawntime;
	public int maxwaitingmeals;
	private float time;
	private int waitingmeals;

	void Start() {
		time = 0.0f;
		waitingmeals = 0;
	}

	void Update () {
		time += Time.deltaTime;
		if ((time >= spawntime) && (waitingmeals < maxwaitingmeals)) {
			time = 0.0f;
			GameObject pickupinst = Instantiate (pickup, transform.position, transform.rotation);
			pickupinst.SendMessage ("SetFridge", this.gameObject);
			waitingmeals++;
		}
	}

	void OneLessMeal() {
		waitingmeals--;
	}
}
