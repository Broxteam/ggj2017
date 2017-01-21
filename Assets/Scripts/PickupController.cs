using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {

	private GameObject fridgectrl;
	private CircleCollider2D cc2d;
	private Rigidbody2D rb2d;

	void Start() {
		rb2d = GetComponent<Rigidbody2D> ();
		cc2d = GetComponent<CircleCollider2D> ();
	}

	void FixedUpdate() {
		RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * cc2d.radius, -Vector2.up, 0.01f);
		if (hit.collider != null) {
			if (hit.collider.gameObject.CompareTag ("Ground")) {
				rb2d.gravityScale = 0;
				rb2d.velocity = new Vector2(0.0f, 0.0f);
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
