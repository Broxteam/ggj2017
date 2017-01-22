using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {

	public float gravityScale;
	private GameObject fridgectrl;
	private BoxCollider2D cc2d;
	private Rigidbody2D rb2d;
	private float fallspeed;
	private float rndmlateral;
	private bool falling;

	void Start() {
		rb2d = GetComponent<Rigidbody2D> ();
		cc2d = GetComponent<BoxCollider2D> ();
		fallspeed = 0.1f;
		falling = true;
		rndmlateral = (Random.value * 2.0f - 1.5f);

	}

	void Update() {
		if (falling) {
			fallspeed -= gravityScale * Time.deltaTime;
			transform.position += new Vector3 (rndmlateral / (Random.Range(8f, 25f)), 1.0f * fallspeed, 0.0f) ;
		}

		if (transform.position.y < -1.1f) {
			Destroy (this.gameObject);
		}


	}

	void FixedUpdate() {
		RaycastHit2D hit = Physics2D.Raycast(transform.position - (Vector3.up * (cc2d.size.y)/2), -Vector2.up, 0.001f);
		if (hit.collider != null) {
			if (hit.collider.gameObject.CompareTag ("Ground")) {
				fallspeed = 0.0f;
				falling = false;
			}
		}

	}

	void SetFridge(GameObject fctrl) {
		fridgectrl = fctrl;
	}

	void OnDestroy() {
		fridgectrl.SendMessage("OneLessMeal");
	}
}
