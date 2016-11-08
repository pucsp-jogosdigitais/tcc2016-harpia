using UnityEngine;
using System.Collections;

public class DelayScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        gameObject.tag = "Poca";

    }
}
