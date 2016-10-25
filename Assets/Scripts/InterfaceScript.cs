using UnityEngine;
using System.Collections.Generic;

public class InterfaceScript : MonoBehaviour {

    private int Velocidade;
    private int Volta;
    private int PowerUpComum;
    private int PowerUpEspecial;
    private int Unidade, Decimo;
    public List<GameObject> decimosVelocidade, unidadesVelocidade;
    public List<GameObject> numVoltas;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Unidade = Velocidade % 10;
        Decimo = Velocidade / 10;

        foreach (GameObject ImgNumero in numVoltas)
        {
            if (ImgNumero.name == Volta.ToString())
            {
                ImgNumero.SetActive(true);
            }
            else
            {
                ImgNumero.SetActive(false);
            }
        }

        foreach (GameObject ImgNumero in decimosVelocidade)
        {
            if (ImgNumero.name == Decimo.ToString())
            {
                ImgNumero.SetActive(true);
            }
            else
            {
                ImgNumero.SetActive(false);
            }
        }

        foreach (GameObject ImgNumero in unidadesVelocidade)
        {
            if (ImgNumero.name == Unidade.ToString())
            {
                ImgNumero.SetActive(true);
            }
            else
            {
                ImgNumero.SetActive(false);
            }
        }


    }

    public void setVelocidade(int Vel)
    {
        Velocidade = Vel;
    }

    public void setVolta(int volta)
    {
        Volta = volta;
    }

    public void Finaliza()
    {

    }
}
