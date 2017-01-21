using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject plyr;

	private Vector3 offset;

	void Start () {
		offset = transform.position - plyr.transform.position;
	}

	void LateUpdate () {
		transform.position = plyr.transform.position + offset;
	}
}
