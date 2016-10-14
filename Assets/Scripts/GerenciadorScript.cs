using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class GerenciadorScript : MonoBehaviour
{
    public int Laps = 1;
    public bool Fim;
    public int TempoPreview = 25;
    public int progTotal;
    public Camera CamPreview;
    public Text regressiva, resultado;

    private KartScript script;
    private List<KartScript> ScriptsKarts;
    private List<Transform> PosIniciais;
    private Transform[] Posições;
    private GameObject[] Karts;
    private GameObject[] Checkpoints;
    private List<Camera> CamerasPlayers;
    public List<AIScript> ScriptsAI;
    public List<ControllerScript> ScriptsControllers;
    private float count = 0, count2;
    private int numTotalCheckPoints;
    private int numeroJogadores;
    private string player1, player2, player3, player4;
    private bool Iniciou = false;

    // Use this for initialization
    void Start()
    {
        numeroJogadores = Escolhas.Numjogadores;
        player1 = Escolhas.player1;
        player2 = Escolhas.player2;
        player3 = Escolhas.player3;
        player4 = Escolhas.player4;

        if (numeroJogadores == 0)
        {
            numeroJogadores = 1;
            player1 = "Jeshi";
        }

        Karts = GameObject.FindGameObjectsWithTag("Karts");
        Checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        PosIniciais = new List<Transform>();
        ScriptsKarts = new List<KartScript>();
        CamerasPlayers = new List<Camera>();
        Posições = transform.GetComponentsInChildren<Transform>();
        CamPreview.gameObject.SetActive(true);
        numTotalCheckPoints = Checkpoints.Length;

        #region Posiciona os Karts
        foreach (Transform posição in Posições) //Adiciona todas as posições iniciais na lista de posições
        {
            if (posição != transform)
                PosIniciais.Add(posição);
        }

        for (int i = 0; i < Karts.Count(); i++)
        {
            Karts[i].transform.position = PosIniciais[i].position; //Coloca os karts nas posições pré definidas
            ScriptsKarts.Add(Karts[i].GetComponent<KartScript>()); //Salva a referência do script de cada Kart
            ScriptsAI.Add(Karts[i].GetComponent<AIScript>()); //Salva a referência do script de AI de cada Kart
            ScriptsKarts[i].contProgresso = -i;     
        }
        #endregion

        #region Ajusta as cameras na tela de acordo com o número de jogadores
        switch (numeroJogadores) 
        {
            #region Somente um jogador
            case 1:
                {
                    configCameras("KartCamera_" + player1);
                }
                break;
            #endregion
            #region Dois Jogadores
            case 2:
                {
                    configCameras("KartCamera_" + player1, "KartCamera_" + player2);
                }
                break;
            #endregion
            #region Três Jogadores
            case 3:
                {
                    configCameras("KartCamera_" + player1, "KartCamera_" + player2, "KartCamera_" + player3);
                }
                break;
            #endregion
            #region Quatro Jogadores
            case 4:
                {
                    configCameras("KartCamera_" + player1, "KartCamera_" + player2, "KartCamera_" + player3, "KartCamera_" + player4);
                }
                break;
                #endregion
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (Fim)
        {
            #region Mostra Ranking
            for (int i = 0; i < Karts.Count(); i++)
            {
                script = Karts[i].GetComponent<KartScript>();

                if (i == 0)
                    resultado.text = "         Ranking: \n1. " + script.Nome + " ...... " + script.minutos + ":" + script.segundos;
                else
                    resultado.text = resultado.text + "\n" + (i + 1) + ". " + script.Nome + " ...... " + script.minutos + ":" + script.segundos;
            }
            #endregion
        }
        else
        {
            if (!Iniciou)
            contagemInicial();

            #region Atualiza as colocações/posições na corrida
            Karts = Karts.OrderByDescending(go => go.GetComponent<KartScript>().contProgresso).ToArray();
            for (int i = 0; i < Karts.Count(); i++)
            {
                Karts[i].GetComponent<KartScript>().posicao = i + 1;
                
            }
            #endregion

            #region Verifica se cada kart terminou a corrida e atualiza o seu progresso
            foreach (KartScript script in ScriptsKarts)
            {
                if ((script.contProgresso >= progTotal) && (script.lap >= Laps) && (script.ContCP >= numTotalCheckPoints / 2))
                    script.Terminou = true;
            }
            #endregion

            #region Verifica se todos os karts já completaram a corrida
            foreach (KartScript script in ScriptsKarts)
            {
                if (script.Terminou)
                {
                    Fim = true;
                }
                else
                {
                    Fim = false;
                    break;
                }
            }
            #endregion
        }
    }

    private void contagemInicial()
    {
        if (count >= TempoPreview)
        {
            #region Fim do Preview

            #region Ativa todas as cameras de jogadores
            foreach (Camera CamKart in CamerasPlayers)
            {
                //CamKart.gameObject.GetComponent<AudioListener>().enabled = true;
                CamKart.gameObject.GetComponent<Camera>().enabled = true;
            }
            #endregion

            CamPreview.gameObject.SetActive(false); //Desativa a Camera do Preview

            if (count2 >= 5)
            {
                #region Inicia o Jogo
                regressiva.text = "";
                switch (numeroJogadores) //Ativa os karts como jogáveis
                {
                    #region Somente um jogador
                    case 1:
                        {
                            ativarComoJogador("Kart_" + player1, " Player1");
                        }
                        break;
                    #endregion
                    #region Dois Jogadores
                    case 2:
                        {
                            ativarComoJogador("Kart_" + player1, " Player1");
                            ativarComoJogador("Kart_" + player2, " Player2");
                        }
                        break;
                    #endregion
                    #region Três Jogadores
                    case 3:
                        {
                            ativarComoJogador("Kart_" + player1, " Player1");
                            ativarComoJogador("Kart_" + player2, " Player2");
                            ativarComoJogador("Kart_" + player3, " Player3");
                        }
                        break;
                    #endregion
                    #region Quatro Jogadores
                    case 4:
                        {
                            ativarComoJogador("Kart_" + player1, " Player1");
                            ativarComoJogador("Kart_" + player2, " Player2");
                            ativarComoJogador("Kart_" + player3, " Player3");
                            ativarComoJogador("Kart_" + player4, " Player4");
                        }
                        break;
                        #endregion
                }

                foreach (AIScript AI in ScriptsAI)
                {
                    AI.enabled = true;
                }

                Iniciou = true;

                #endregion
            }
            else
            {
                #region Contagem Regressiva na Tela
                count2 += Time.deltaTime/Time.timeScale;
                switch ((int)count2)
                {
                    case 0:
                        regressiva.text = "Ready?";
                        break;
                    case 1:
                        regressiva.text = "3";
                        break;
                    case 2:
                        regressiva.text = "2";
                        break;
                    case 3:
                        regressiva.text = "1";
                        break;
                    case 4:
                        regressiva.text = "GO!";
                        break;
                }
                #endregion
            }
            
            #endregion
        }
        else
        {
            #region Contagem do Preview + Interrupção
            count += Time.deltaTime/Time.timeScale;
            if (Input.anyKey) //Interrompe o Preview
            {
                count = TempoPreview;
            }
            #endregion
        }
    }

    private void ativarComoJogador(string kart, string Player)
    {
        AIScript ControladorAI = GameObject.Find(kart).GetComponent<AIScript>();        
        ControllerScript ControladorJogador = GameObject.Find(kart).GetComponent<ControllerScript>();
        ControladorJogador.Player = Player;
        ScriptsAI.Remove(ControladorAI);
        ControladorAI.enabled = false;
        GameObject.Find(kart).GetComponentInChildren<NavMeshAgent>().enabled = false;
        GameObject.Find(kart).GetComponent<NavMeshObstacle>().enabled = true;
        ControladorJogador.enabled = true;
        ScriptsControllers.Add(ControladorJogador);
    }

    #region Métodos para configurar cameras de acordo com os jogadores

    private void configCameras(string Player1)
    {
        Camera cameraP1 = GameObject.Find(Player1).GetComponentInChildren<Camera>();
        cameraP1.rect = new Rect(0, 0, 1, 1);
        CamerasPlayers.Add(cameraP1);
    }

    private void configCameras(string Player1, string Player2)
    {    
        Camera cameraP1 = GameObject.Find(Player1).GetComponentInChildren<Camera>();
        Camera cameraP2 = GameObject.Find(Player2).GetComponentInChildren<Camera>();
        cameraP1.rect = new Rect(0, 0.5f, 1, 1);
        cameraP2.rect = new Rect(0, -0.5f, 1, 1);
        CamerasPlayers.Add(cameraP1);
        CamerasPlayers.Add(cameraP2);
    }

    private void configCameras(string Player1, string Player2, string Player3)
    {
        Camera cameraP1 = GameObject.Find(Player1).GetComponentInChildren<Camera>();
        Camera cameraP2 = GameObject.Find(Player2).GetComponentInChildren<Camera>();
        Camera cameraP3 = GameObject.Find(Player3).GetComponentInChildren<Camera>();
        cameraP1.rect = new Rect(-0.5f, 0.5f, 1, 1);
        cameraP2.rect = new Rect(0.5f, 0.5f, 1, 1);
        cameraP3.rect = new Rect(0f, -0.5f, 1, 1);
        CamerasPlayers.Add(cameraP1);
        CamerasPlayers.Add(cameraP2);
        CamerasPlayers.Add(cameraP3);
    }

    private void configCameras(string Player1, string Player2, string Player3, string Player4)
    {
        Camera cameraP1 = GameObject.Find(Player1).GetComponentInChildren<Camera>();
        Camera cameraP2 = GameObject.Find(Player2).GetComponentInChildren<Camera>();
        Camera cameraP3 = GameObject.Find(Player3).GetComponentInChildren<Camera>();
        Camera cameraP4 = GameObject.Find(Player4).GetComponentInChildren<Camera>();
        cameraP1.rect = new Rect(-0.5f, 0.5f, 1, 1);
        cameraP2.rect = new Rect(0.5f, 0.5f, 1, 1);
        cameraP3.rect = new Rect(-0.5f, -0.5f, 1, 1);
        cameraP4.rect = new Rect(0.5f, -0.5f, 1, 1);
        CamerasPlayers.Add(cameraP1);
        CamerasPlayers.Add(cameraP2);
        CamerasPlayers.Add(cameraP3);
        CamerasPlayers.Add(cameraP4);
    }

    #endregion

}

