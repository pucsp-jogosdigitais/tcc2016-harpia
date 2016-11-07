using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InterfaceScript : MonoBehaviour {

    private int Velocidade;
    private int Volta;
    private int PowerUpComum;
    private int PowerUpEspecial;
    private int Unidade, Decimo;
    private float rodando;
    public Image ComumVazio, ImgPoca, ImgBoost, ImgFalso, ImgMisselVerm, ImgMisselAzul;
    public Image ImgEspecial, EspecialVazio;
    public Camera CameraMiniMapa;
    public List<GameObject> decimosVelocidade, unidadesVelocidade;
    public List<GameObject> numVoltas;
    
	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

        #region Numero de Voltas
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
        #endregion

        #region Velocidade
        Unidade = Velocidade % 10;
        Decimo = Velocidade / 10;

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
        #endregion

        #region PowerUp Especial
        if (PowerUpEspecial == 0)
        {
            EspecialVazio.gameObject.SetActive(true);
            ImgEspecial.gameObject.SetActive(false);
        }
        else
        {
            EspecialVazio.gameObject.SetActive(false);
            ImgEspecial.gameObject.SetActive(true);
        }
        #endregion

        #region Power Up Comum
        MostraPowerUp(PowerUpComum);
        #endregion

    }

    private void MostraPowerUp(int num)
    {
        switch (num)
        {
            #region Vazio
            case 0: //Nenhum
                {
                    ImgPoca.gameObject.SetActive(false);
                    ImgBoost.gameObject.SetActive(false);
                    ImgFalso.gameObject.SetActive(false);
                    ImgMisselVerm.gameObject.SetActive(false);
                    ImgMisselAzul.gameObject.SetActive(false);
                    ComumVazio.gameObject.SetActive(true);
                }
                break;
            #endregion
            #region Missel Comum
            case 1: //Missel Comum
                {
                    ImgPoca.gameObject.SetActive(false);
                    ImgBoost.gameObject.SetActive(false);
                    ImgFalso.gameObject.SetActive(false);
                    ImgMisselVerm.gameObject.SetActive(false);
                    ImgMisselAzul.gameObject.SetActive(true);
                    ComumVazio.gameObject.SetActive(false);
                }
                break;
            #endregion
            #region Boost de Velocidade
            case 2: //Boost de Velocidade
                {
                    ImgPoca.gameObject.SetActive(false);
                    ImgBoost.gameObject.SetActive(true);
                    ImgFalso.gameObject.SetActive(false);
                    ImgMisselVerm.gameObject.SetActive(false);
                    ImgMisselAzul.gameObject.SetActive(false);
                    ComumVazio.gameObject.SetActive(false);
                }
                break;
            #endregion
            #region Missel Teleguiado
            case 3: //Missel Teleguiado
                {
                    ImgPoca.gameObject.SetActive(false);
                    ImgBoost.gameObject.SetActive(false);
                    ImgFalso.gameObject.SetActive(false);
                    ImgMisselVerm.gameObject.SetActive(true);
                    ImgMisselAzul.gameObject.SetActive(false);
                    ComumVazio.gameObject.SetActive(false);
                }
                break;
            #endregion
            #region Oleo
            case 4:  //Oleo
                {
                    ImgPoca.gameObject.SetActive(true);
                    ImgBoost.gameObject.SetActive(false);
                    ImgFalso.gameObject.SetActive(false);
                    ImgMisselVerm.gameObject.SetActive(false);
                    ImgMisselAzul.gameObject.SetActive(false);
                    ComumVazio.gameObject.SetActive(false);
                }
                break;
            #endregion
            #region Power Up Box Falsa
            case 5: //Power Up Box Falsa
                {
                    ImgPoca.gameObject.SetActive(false);
                    ImgBoost.gameObject.SetActive(false);
                    ImgFalso.gameObject.SetActive(true);
                    ImgMisselVerm.gameObject.SetActive(false);
                    ImgMisselAzul.gameObject.SetActive(false);
                    ComumVazio.gameObject.SetActive(false);
                }
                break;
                #endregion
        }

        /*
        if (rodando <= 2)
        {
            rodando += Time.deltaTime / Time.timeScale;
        }
        else
        {
            rodando = 0;

        }*/
    }

    public void setVelocidade(int Vel)
    {
        Velocidade = Vel;
    }

    public void setVolta(int volta)
    {
        Volta = volta;
    }

    public void setPowerUp(int PU)
    {
        PowerUpComum = PU;
    }

    public void setPowerUpEspecial(bool disponivel)
    {
        if (!disponivel)
        {
            PowerUpEspecial = 0;
        }
        else
        {
            PowerUpEspecial = 1;
        }
    }

    public void Finaliza()
    {
        CameraMiniMapa.enabled = false;
        this.gameObject.transform.parent.gameObject.SetActive(false);
        this.enabled = false;
    }
}
