using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MolduraScript : MonoBehaviour {

    public GameObject ImgVioletta, ImgJeshi, ImgAyah, ImgMomoto;
    public Text Nome;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AtualizaMoldura(string Personagem)
    {
        Nome.text = Personagem;

        if (Personagem == "Ayah")
        {
            ImgVioletta.SetActive(false);
            ImgMomoto.SetActive(false);
            ImgJeshi.SetActive(false);
            ImgAyah.SetActive(true);
        }
            
        if (Personagem == "Violetta")
        {
            ImgVioletta.SetActive(true);
            ImgMomoto.SetActive(false);
            ImgJeshi.SetActive(false);
            ImgAyah.SetActive(false);
        }
        if (Personagem == "Jeshi")
        {
            ImgVioletta.SetActive(false);
            ImgMomoto.SetActive(false);
            ImgJeshi.SetActive(true);
            ImgAyah.SetActive(false);
        }
        if (Personagem == "Momoto")
        {
            ImgVioletta.SetActive(false);
            ImgMomoto.SetActive(true);
            ImgJeshi.SetActive(false);
            ImgAyah.SetActive(false);
        }
    }
}
