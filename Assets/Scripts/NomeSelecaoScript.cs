using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NomeSelecaoScript : MonoBehaviour {

    public GameObject Ayah, Jeshi, Violetta, Momoto;
    private Text texto;

	// Use this for initialization
	void Start () {
        texto = GetComponent<Text>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
        switch (texto.text)
        {
            case "Ayah":
                {
                    Ayah.SetActive(true);
                    Jeshi.SetActive(false);
                    Violetta.SetActive(false);
                    Momoto.SetActive(false);
                }
                break;
            case "Violetta":
                {
                    Ayah.SetActive(false);
                    Jeshi.SetActive(false);
                    Violetta.SetActive(true);
                    Momoto.SetActive(false);
                }
                break;
            case "Jeshi":
                {
                    Ayah.SetActive(false);
                    Jeshi.SetActive(true);
                    Violetta.SetActive(false);
                    Momoto.SetActive(false);
                }
                break;
            case "Momoto":
                {
                    Ayah.SetActive(false);
                    Jeshi.SetActive(false);
                    Violetta.SetActive(false);
                    Momoto.SetActive(true);
                }
                break;

        }

	}
}
