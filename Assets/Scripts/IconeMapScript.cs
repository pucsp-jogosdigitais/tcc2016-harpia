using UnityEngine;
using System.Collections;

public class IconeMapScript : MonoBehaviour {

    public Transform PosicaoKart;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(PosicaoKart.position.x, transform.position.y, PosicaoKart.position.z);
	}
}
