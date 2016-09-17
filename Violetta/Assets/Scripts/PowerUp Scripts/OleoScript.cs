using UnityEngine;
using System.Collections;

public class OleoScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<KartScript>().Rodar();
            Destroy(this.gameObject);
        }
    }
}
