using UnityEngine;
using System.Collections;

public class DelayScript : MonoBehaviour {

    public BoxCollider Collider;

	// Use this for initialization
	void Start () {
        StartCoroutine(Delay());
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(10);
        gameObject.tag = "Poca";
        //Collider.enabled = true;
        //gameObject.AddComponent<Rigidbody>();

    }
}
