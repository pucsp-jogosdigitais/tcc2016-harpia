using UnityEngine;
using System.Collections;

public class BalaScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (WaitAndDestroy ());
	}

	IEnumerator WaitAndDestroy()
	{
		yield return new WaitForSeconds (16);
		Destroy (this.gameObject);

	}
}
