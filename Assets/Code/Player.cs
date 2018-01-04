using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Color color1;
	public Color color2;

	public float secondsPerCycle = 3f;
	private float pos = 0f;
	private bool inside = false;
	private float distance = 0f;
	private float velocity = 0f;
	private static float distanceOutside = 2.3f;
	private static float distanceInside = 1.75f;

	private GameObject lastAnim;
	private Dictionary<string, GameObject> dictionaryAnimations = new Dictionary<string, GameObject> ();

	// Use this for initialization
	void Start () {

		ChangeAnimation ("Run");
		distance = distanceOutside;

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {

			if (inside) {

				if (distance == distanceInside) {
					ChangeAnimation ("Jump");
					velocity = -5f;
				}

			} else {

				inside = true;
				this.transform.localScale = new Vector3 (this.transform.localScale.x, -this.transform.localScale.y, this.transform.localScale.z);
				distance = distanceInside;
				StartCoroutine(ReproduceEffect ("Smoke", this.transform.position, this.transform.eulerAngles));

			}


		}

		pos += (Time.deltaTime / secondsPerCycle);
		if (pos > 1f)
			pos -= 1f;


		if (inside && (distance < distanceInside || velocity != 0f)) {

			velocity += 10f * Time.deltaTime;
			distance += velocity * Time.deltaTime;

			if (velocity > 0f && lastAnim.name != "Fall_Visual") {
				ChangeAnimation ("Fall");
			}

			if (distance > distanceInside) {
				distance = distanceInside;
				velocity = 0f;
				ChangeAnimation ("Run");
			}

		}


		this.transform.eulerAngles = Vector3.zero;
		this.transform.position = new Vector3 (0f, distance, 0f);
		this.transform.RotateAround (Vector3.zero, -Vector3.forward, pos * 360f);

	}


	private void ChangeAnimation(string animName) {

		if (lastAnim != null)
			lastAnim.SetActive (false);

		lastAnim = GetAnimation (animName);
		lastAnim.transform.SetParent (this.transform);
		lastAnim.transform.localScale = Vector3.one;
		lastAnim.transform.localPosition = Vector3.zero;
		lastAnim.transform.localEulerAngles = Vector3.zero;

	}

	private GameObject GetAnimation(string animName) {

		if (!dictionaryAnimations.ContainsKey (animName)) {

			GameObject source = Resources.Load ("Animations/" + animName + "/" + animName + "_Visual") as GameObject;
			GameObject newAnim = Instantiate (source);
			newAnim.name = source.name;
			dictionaryAnimations.Add (animName, newAnim);

		}

		dictionaryAnimations [animName].SetActive (true);
		return dictionaryAnimations [animName];

	}

	private IEnumerator ReproduceEffect(string effectName, Vector3 position, Vector3 eulerAngles) {

		GameObject anim = GetAnimation (effectName);
		anim.GetComponent<SpriteRenderer> ().color = color1;
		anim.transform.position = position;
		anim.transform.localEulerAngles = eulerAngles;

		yield return new WaitForSeconds (anim.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).length);

		anim.SetActive (false);

	}

}
