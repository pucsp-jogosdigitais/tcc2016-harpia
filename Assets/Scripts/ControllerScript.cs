using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControllerScript : MonoBehaviour
{
    #region Variáveis/Atributos

    public bool drift = false;
    public bool jogando = false;
    public Text contadorVoltas;
    public Text textVelocimetro;
    public GameObject imagensColocacao;
    public Camera camKart;
    public string Player;

    private string direção = "Frente";
    private KartScript Kart;
    private KartCameraScript CameraScript;
    private GerenciadorScript Gerenciador;
    private float vertical, horizontal;
    private GameObject imgPos1, imgPos2, imgPos3, imgPos4;

    #endregion

    void Start()
    {
        CameraScript = camKart.GetComponent<KartCameraScript>();                        //Localiza o script da camera
        Kart = gameObject.GetComponent<KartScript>();                                   //Localiza o script do kart
        Gerenciador = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>(); //Localiza o gerenciador da pista
        Kart.Jogando = true;                                                            //Inicia o estado como jogando

        //Acha a referencia das imagens de colocacao que irão na interface
        imgPos1 = imagensColocacao.transform.Find("Primeiro").gameObject;
        imgPos2 = imagensColocacao.transform.Find("Segundo").gameObject;
        imgPos3 = imagensColocacao.transform.Find("Terceiro").gameObject;
        imgPos4 = imagensColocacao.transform.Find("Quarto").gameObject;

        Player = " Player1";

        //Inicializa os textos de interface vazios
        contadorVoltas.text = "";
        textVelocimetro.text = "";
    }

    void FixedUpdate()
    {
                  //Chama a função com as mecanicas básicas do kart
    }

    void Update()
    {
        Controle();  
            InserirInterface(); //coloca informações na tela (velocimetro, contagem de voltas...)
            verificaCamera();   //Verifica se a camera está reversa ou não

            if (Input.GetButtonDown("Confirmar/PowerUp Comum" + Player)) //Atira o powerup comum
                Kart.powerUpComum(direção);

            if (Input.GetButtonDown("Voltar/PowerUp Especial" + Player)) //Atira o powerup especial
                Kart.powerUpEspecial();

        if (Kart.Terminou) //Se o jogador terminou, desativa os controles, tira os textos de interface e ativa a inteligencia artificial
        {
            this.gameObject.GetComponent<Rigidbody>().drag = 0.4f;
            this.gameObject.GetComponentInChildren<AIScript>().enabled = true;
            this.gameObject.GetComponentInChildren<NavMeshObstacle>().enabled = false;
            this.gameObject.GetComponentInChildren<NavMeshAgent>().enabled = true;
            contadorVoltas.text = "";
            textVelocimetro.text = "";
            contadorVoltas.gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            this.enabled = false;
        }
    }

    #region Funções de movimentação

    private void Controle() //Função que passa os controles do jogador para o script do kart
    {
        Kart.Velocimetro();

        vertical = Input.GetAxis("Acelerador/Re" + Player);
        horizontal = Input.GetAxis("Volante" + Player);

        if (Input.GetButtonDown("Drift" + Player))
            drift = true;
        if (Input.GetButtonUp("Drift" + Player))
            drift = false;

        //Kart.Acelerar(1);
       
        if (vertical != 0)
            Kart.Acelerar(vertical);
        else
            Kart.Desacelerar();

        Kart.Direção(horizontal, drift);
        Kart.ExecutarDrift(drift);
    }

    #endregion

    #region Funções Auxiliares

    private void verificaCamera() //Muda a direção que a camera está marcada de acordo com o script da camera
    {
        if (CameraScript.cameraReversa)
            direção = "Trás";
        else
            direção = "Frente";
    }

    private void InserirInterface() //Atualiza a interface do jogador
    {
        if (Kart.lap != 0)
            contadorVoltas.text = Kart.lap.ToString() + "/" + Gerenciador.Laps.ToString();
        else if (Kart.lap > Gerenciador.Laps)
            contadorVoltas.text = Gerenciador.Laps.ToString() + "/" + Gerenciador.Laps.ToString();
        else
            contadorVoltas.text = "1" + "/" + Gerenciador.Laps.ToString();
        textVelocimetro.text = Mathf.RoundToInt(Kart.KartRigidbody.velocity.magnitude*1.9f).ToString();

        #region Imagem de colocacao
        switch (Kart.posicao)
        {            
            case 1:
                {
                    imgPos1.SetActive(true);
                    imgPos2.SetActive(false);
                    imgPos3.SetActive(false);
                    imgPos4.SetActive(false);
                }
                break;
            case 2:
                {
                    imgPos1.SetActive(false);
                    imgPos2.SetActive(true);
                    imgPos3.SetActive(false);
                    imgPos4.SetActive(false);
                }
                break;
            case 3:
                {
                    imgPos1.SetActive(false);
                    imgPos2.SetActive(false);
                    imgPos3.SetActive(true);
                    imgPos4.SetActive(false);
                }
                break;
            case 4:
                {
                    imgPos1.SetActive(false);
                    imgPos2.SetActive(false);
                    imgPos3.SetActive(false);
                    imgPos4.SetActive(true);
                }
                break;
        }
        #endregion
    }

    #endregion

}
