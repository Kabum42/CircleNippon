using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	public float AnglesPerSecond = 90f;
	
	// Update is called once per frame
	void Update () {

		this.transform.Rotate (new Vector3 (0f, 0f, -1f) * Time.deltaTime * AnglesPerSecond);

	}
}
