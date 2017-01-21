using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {

	public float gravityScale;
	private GameObject fridgectrl;
	private CircleCollider2D cc2d;
	private Rigidbody2D rb2d;
	private float fallspeed;
	private float rndmlateral;
	private bool falling;

	void Start() {
		rb2d = GetComponent<Rigidbody2D> ();
		cc2d = GetComponent<CircleCollider2D> ();
		fallspeed = 0.5f;
		falling = true;
		rndmlateral = (Random.value * 2.0f - 1.0f) / 4.0f;
	}

	void Update() {
		if (falling) {
			fallspeed -= gravityScale * Time.deltaTime;
			transform.position += new Vector3 (rndmlateral, 1.0f * fallspeed, 0.0f) ;
		}
	}

	void FixedUpdate() {
		RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * cc2d.radius, -Vector2.up, 0.01f);
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
