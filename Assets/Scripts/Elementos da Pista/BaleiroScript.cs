using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class BaleiroScript : MonoBehaviour {

	public List<GameObject> PrefabsBalas;
	public Transform PrefabPos;


	// Use this for initialization
	void Start () {
		InvokeRepeating ("Instanciar", 0.1f, 5f);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void Instanciar()
	{
		Instantiate (PrefabsBalas [UnityEngine.Random.Range (0, 6)], PrefabPos.position, 
			         PrefabPos.rotation);
	}
}
