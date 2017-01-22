using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeController : MonoBehaviour {

	public GameObject pickupBurger;
	public GameObject pickupChiken;
	public GameObject pickupSalad;

	public float spawntime;
	public int maxwaitingmeals;
	private float time;
	private int waitingmeals;

	private GameObject pickupToUse;

	void Start() {
		time = 0.0f;
		waitingmeals = 0;
	}

	void Update () {
		time += Time.deltaTime;
		if ((time >= spawntime) && (waitingmeals < maxwaitingmeals)) {

			int foodNum = Random.Range (1, 4);
			if (foodNum == 1) {
				pickupToUse = pickupBurger;
			} else if (foodNum == 2) {
				pickupToUse = pickupChiken;
			} else {
				pickupToUse = pickupSalad;
			}



			time = 0.0f;
			GameObject pickupinst = Instantiate (pickupToUse, transform.position, transform.rotation);
			pickupinst.SendMessage ("SetFridge", this.gameObject);
			waitingmeals++;
		}
	}

	void OneLessMeal() {
		waitingmeals--;
	}
}
