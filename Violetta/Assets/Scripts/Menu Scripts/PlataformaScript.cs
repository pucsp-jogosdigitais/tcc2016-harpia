using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlataformaScript : MonoBehaviour {

    GameObject Plataforma;
    public Text Personagem;
    float lado, delay;

	// Use this for initialization
	void Start () {

        Plataforma = this.gameObject;
	
	}
	
	// Update is called once per frame
	void Update () {

        //Plataforma.transform.Rotate(new Vector3(0, 1, 0));

        lado = Input.GetAxis("Horizontal");
        Debug.Log(lado.ToString());
        if (delay <= 0)
        {
            if (lado == 1 || lado == -1)
            {
                Mudar(lado);
                delay = 0.8f;
            }
        }
        else
        {
            delay -= Time.deltaTime / Time.timeScale;
        }

            

        
	}

    void Mudar(float orientacao)
    {
        if (orientacao > 0)
        {
            switch (Personagem.text)
            {
                case "Violetta":
                    {
                        Personagem.text = "Momoto";
                        Debug.Log("momoto");
                    }
                    break;

                case "Momoto":
                    {
                        Personagem.text = "Ayah";
                    }
                    break;

                case "Ayah":
                    {
                        Personagem.text = "Jeshi";
                    }
                    break;

                case "Jeshi":
                    {
                        Personagem.text = "Violetta";
                    }
                    break;
            }
            Rotaciona(1);
        }
        else
        {
            switch (Personagem.text)
            {
                case "Violetta":
                    {
                        Personagem.text = "Jeshi";
                    }
                    break;

                case "Momoto":
                    {
                        Personagem.text = "Violetta";
                    }
                    break;

                case "Ayah":
                    {
                        Personagem.text = "Momoto";
                    }
                    break;

                case "Jeshi":
                    {
                        Personagem.text = "Ayah";
                    }
                    break;
            }
            Rotaciona(-1);
        }
    }

    void Rotaciona(float direção)
    {
        for (int i=0; i<=90; i++)
        Plataforma.transform.Rotate(new Vector3(0, direção, 0));
    }
}
