using UnityEngine;
using System.Collections;

public class BalaScript : MonoBehaviour {

    bool derretendo = false;

	// Use this for initialization
	void Start () {
		StartCoroutine (WaitAndDestroy ());
	}

    void Update()
    {
        if (derretendo)
            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
        if (transform.localScale.x <= 0.05f)
            Destroy(this.gameObject);
    }

	IEnumerator WaitAndDestroy()
	{
		yield return new WaitForSeconds (Random.Range(15,30));
        derretendo = true;
	}
}
