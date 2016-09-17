using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControllerScript : MonoBehaviour
{
    #region Variáveis/Atributos

    public bool drift = false;
    public bool jogando = false;
    public Text colocacaoText;
    public Text contadorVoltas;
    public Text textVelocimetro;
    public Camera camKart;
    public string Player;

    private string direção = "Frente";
    private KartScript Kart;
    private KartCameraScript CameraScript;
    private GerenciadorScript Gerenciador;
    private float vertical, horizontal;

    #endregion

    void Start()
    {
        CameraScript = camKart.GetComponent<KartCameraScript>();                        //Localiza o script da camera
        Kart = gameObject.GetComponent<KartScript>();                                   //Localiza o script do kart
        Gerenciador = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>(); //Localiza o gerenciador da pista
        Kart.Jogando = true;                                                            //Inicia o estado como jogando

        Player = " Player1";

        //Inicializa os textos de interface vazios
        contadorVoltas.text = "";
        textVelocimetro.text = "";
        colocacaoText.text = "";
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
                colocacaoText.text = "";
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
            contadorVoltas.text = "Lap: " + Kart.lap.ToString() + "/" + Gerenciador.Laps.ToString();
        else if (Kart.lap > Gerenciador.Laps)
            contadorVoltas.text = "Lap: " + Gerenciador.Laps.ToString() + "/" + Gerenciador.Laps.ToString();
        else
            contadorVoltas.text = "Lap: " + "1" + "/" + Gerenciador.Laps.ToString();
        textVelocimetro.text = "Velocidade: " + Kart.velAtual.ToString();
        colocacaoText.text = "Posição: " + Kart.posicao.ToString();
    }

    #endregion

}
