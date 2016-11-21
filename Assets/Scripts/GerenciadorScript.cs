using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GerenciadorScript : MonoBehaviour
{
    public int Laps = 1;
    public bool Fim;
    public int TempoPreview = 25;
    public int progTotal;
    public Camera CamPreview;
    public GameObject resultado;
    public Image NomeRankAyah, NomeRankVioletta, NomeRankJeshi, NomeRankMomoto;
    public List<GameObject> Resultados;

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
    private int contAI;
    public Image Ready, img1, img2, img3, Go;
    public AudioSource Audio;
    public AudioClip AudioContagem, AudioGo, MusicaTema, AudioQuartoLugar, AudioTerceiroLugar, AudioPrimeiroSegundoLugar;

    // Use this for initialization
    void Start()
    {
        Audio.PlayOneShot(MusicaTema, 0.4f);

        numeroJogadores = Escolhas.Numjogadores;
        player1 = Escolhas.player1;
        player2 = Escolhas.player2;
        player3 = Escolhas.player3;
        player4 = Escolhas.player4;

        if (numeroJogadores == 0)
        {
            numeroJogadores = 1;
            player1 = "Ayah";
            player2 = "Jeshi";
            player3 = "Violetta";
            player4 = "Momoto";
        }

        Ready.gameObject.SetActive(false);
        img1.gameObject.SetActive(false);
        img2.gameObject.SetActive(false);
        img3.gameObject.SetActive(false);
        Go.gameObject.SetActive(false);
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
            ScriptsKarts[i].ProgNoFim = -i;     
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
            MostraRaking();

            if (Input.GetKey(KeyCode.Space)) //Interrompe o Preview
            {
                VoltaMenu();
            }
        }
        else
        {
            if (!Iniciou)
            contagemInicial();

            #region Atualiza as colocações/posições na corrida
            Karts = Karts.OrderByDescending(go => go.GetComponent<KartScript>().ProgNoFim).ToArray();
            for (int i = 0; i < Karts.Count(); i++)
            {
                Karts[i].GetComponent<KartScript>().posicao = i + 1;
                
            }
            #endregion

            #region Verifica se cada kart terminou a corrida
            contAI = 0;
            foreach (KartScript script in ScriptsKarts)
            {
                if ((script.ProgNoFim >= progTotal) && (script.lap > Laps) && (script.ContCP >= numTotalCheckPoints / 2))
                {
                    script.Terminou = true;
                    script.Ganhou.Play();
                }

                if (script.gameObject.GetComponent<AIScript>().enabled)
                {
                    contAI++;
                }
            }

            if (contAI == 4)
            {
                foreach (KartScript script in ScriptsKarts)
                {
                    if (!script.Terminou)
                    {
                        script.Terminou = true;
                        //script.ProgNoFim = script.contProgresso;
                    }
                }
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
                Go.gameObject.SetActive(false);
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
                        {
                            Ready.gameObject.SetActive(true);
                        }
                        break;
                    case 1:
                        {
                            Ready.gameObject.SetActive(false);
                            img3.gameObject.SetActive(true);
                            Audio.PlayOneShot(AudioContagem);
                        }
                        break;
                    case 2:
                        {
                            img3.gameObject.SetActive(false);
                            img2.gameObject.SetActive(true);
                            Audio.PlayOneShot(AudioContagem);
                        }
                            break;
                    case 3:
                        {
                            img2.gameObject.SetActive(false);                            
                            img1.gameObject.SetActive(true);
                            Audio.PlayOneShot(AudioContagem);
                        }
                        break;
                    case 4:
                        {
                            img1.gameObject.SetActive(false);
                            Go.gameObject.SetActive(true);
                            Audio.PlayOneShot(AudioGo);
                        }
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

    private void MostraRaking()
    {
        #region Mostra Ranking
        for (int i = 0; i < Karts.Count(); i++)
        {
            script = Karts[i].GetComponent<KartScript>();

            #region Posiciona os nomes
            switch (script.Nome)
            {
                case "Violetta":
                    {
                        NomeRankVioletta.transform.parent = Resultados[i].transform;
                        RectTransform Rect = NomeRankVioletta.GetComponent<RectTransform>();
                        Rect.offsetMin = new Vector2(0, 0);
                        Rect.offsetMax = new Vector2(0, 0);
                        Rect.sizeDelta = new Vector2(0, 0);
                        if (!script.SomouTempoExtra)
                            script.SomaTempoExtra((250 - script.ProgNoFim) * 0.4f);
                        Resultados[i].GetComponentInChildren<EscreveTempoScript>().SetTempos(script.minutos, script.segundos, script.milisegundos);
                    }
                    break;
                case "Jeshi":
                    {
                        NomeRankJeshi.transform.parent = Resultados[i].transform;
                        RectTransform Rect = NomeRankJeshi.GetComponent<RectTransform>();
                        Rect.offsetMin = new Vector2(0, 0);
                        Rect.offsetMax = new Vector2(0, 0);
                        Rect.sizeDelta = new Vector2(0, 0);
                        if (!script.SomouTempoExtra)
                            script.SomaTempoExtra((250 - script.ProgNoFim) * 0.4f);
                        Resultados[i].GetComponentInChildren<EscreveTempoScript>().SetTempos(script.minutos, script.segundos, script.milisegundos);
                    }
                    break;
                case "Ayah":
                    {
                        NomeRankAyah.transform.parent = Resultados[i].transform;
                        RectTransform Rect = NomeRankAyah.GetComponent<RectTransform>();
                        Rect.offsetMin = new Vector2(0, 0);
                        Rect.offsetMax = new Vector2(0, 0);
                        Rect.sizeDelta = new Vector2(0, 0);
                        if (!script.SomouTempoExtra)
                            script.SomaTempoExtra((250 - script.ProgNoFim) * 0.4f);
                        Resultados[i].GetComponentInChildren<EscreveTempoScript>().SetTempos(script.minutos, script.segundos, script.milisegundos);
                    }
                    break;
                case "Momoto":
                    {
                        NomeRankMomoto.transform.parent = Resultados[i].transform;
                        RectTransform Rect = NomeRankMomoto.GetComponent<RectTransform>();
                        Rect.offsetMin = new Vector2(0, 0);
                        Rect.offsetMax = new Vector2(0, 0);
                        Rect.sizeDelta = new Vector2(0, 0);
                        if (!script.SomouTempoExtra)
                            script.SomaTempoExtra((250 - script.ProgNoFim) * 0.4f);
                        Resultados[i].GetComponentInChildren<EscreveTempoScript>().SetTempos(script.minutos, script.segundos, script.milisegundos);
                    }
                    break;
            }
            #endregion
            resultado.SetActive(true);
        }
        #endregion
    }

    #region Métodos para configurar cameras de acordo com os jogadores

    private void configCameras(string Player1)
    {
        //Cam Principal
        Camera cameraP1 = GameObject.Find(Player1).GetComponentInChildren<Camera>();
        cameraP1.rect = new Rect(0, 0, 1, 1);
        CamerasPlayers.Add(cameraP1);
        //Map
        cameraP1 = GameObject.Find(Player1 + "_Map").GetComponentInChildren<Camera>();
        cameraP1.rect = new Rect(0.8f, 0.6f, 1, 1);
        CamerasPlayers.Add(cameraP1);
    }

    private void configCameras(string Player1, string Player2)
    {   
        //Cam Principal
        Camera cameraP1 = GameObject.Find(Player1).GetComponentInChildren<Camera>();
        Camera cameraP2 = GameObject.Find(Player2).GetComponentInChildren<Camera>();
        cameraP1.rect = new Rect(0, 0.5f, 1, 1);
        cameraP2.rect = new Rect(0, -0.5f, 1, 1);
        CamerasPlayers.Add(cameraP1);
        CamerasPlayers.Add(cameraP2);
        //Map
        cameraP1 = GameObject.Find(Player1 + "_Map").GetComponentInChildren<Camera>();
        cameraP2 = GameObject.Find(Player2 + "_Map").GetComponentInChildren<Camera>();
        cameraP1.rect = new Rect(0.8f, 0.6f, 1, 1);
        cameraP2.rect = new Rect(0.8f, 0.1f, 0.5f, 0.415f);
        CamerasPlayers.Add(cameraP1);
        CamerasPlayers.Add(cameraP2);
    }

    private void configCameras(string Player1, string Player2, string Player3)
    {
        //Cam Principal
        Camera cameraP1 = GameObject.Find(Player1).GetComponentInChildren<Camera>();
        Camera cameraP2 = GameObject.Find(Player2).GetComponentInChildren<Camera>();
        Camera cameraP3 = GameObject.Find(Player3).GetComponentInChildren<Camera>();
        cameraP1.rect = new Rect(-0.5f, 0.5f, 1, 1);
        cameraP2.rect = new Rect(0.5f, 0.5f, 1, 1);
        cameraP3.rect = new Rect(0f, -0.5f, 1, 1);
        CamerasPlayers.Add(cameraP1);
        CamerasPlayers.Add(cameraP2);
        CamerasPlayers.Add(cameraP3);
        //Cam Map
        cameraP1 = GameObject.Find(Player1 + "_Map").GetComponentInChildren<Camera>();
        cameraP2 = GameObject.Find(Player2 + "_Map").GetComponentInChildren<Camera>();
        cameraP3 = GameObject.Find(Player3 + "_Map").GetComponentInChildren<Camera>();
        cameraP1.rect = new Rect(-0.15f, 0.75f, 1, 0.25f);
        cameraP2.rect = new Rect(0.85f, 0.75f, 1, 0.25f);
        cameraP3.rect = new Rect(0.85f, 0.25f, 1, 0.25f);
        CamerasPlayers.Add(cameraP1);
        CamerasPlayers.Add(cameraP2);
        CamerasPlayers.Add(cameraP3);
    }

    private void configCameras(string Player1, string Player2, string Player3, string Player4)
    {
        //Cam Principal
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
        //Cam Map
        cameraP1 = GameObject.Find(Player1 + "_Map").GetComponentInChildren<Camera>();
        cameraP2 = GameObject.Find(Player2 + "_Map").GetComponentInChildren<Camera>();
        cameraP3 = GameObject.Find(Player3 + "_Map").GetComponentInChildren<Camera>();
        cameraP4 = GameObject.Find(Player4 + "_Map").GetComponentInChildren<Camera>();
        cameraP1.rect = new Rect(-0.15f, 0.75f, 1, 0.25f);
        cameraP2.rect = new Rect(0.85f, 0.75f, 1, 0.25f);
        cameraP3.rect = new Rect(-0.15f, 0.25f, 1, 0.25f);
        cameraP4.rect = new Rect(0.85f, 0.25f, 1, 0.25f);
        CamerasPlayers.Add(cameraP1);
        CamerasPlayers.Add(cameraP2);
        CamerasPlayers.Add(cameraP3);
        CamerasPlayers.Add(cameraP4);
    }

    #endregion

    public GameObject BuscarKartAlvo(int colocacao)
    {
        foreach (KartScript script in ScriptsKarts)
        {
            if (script.posicao == colocacao)
                return script.gameObject;
        }
        if (colocacao >= 5)
            return BuscarKartAlvo(1);
        if (colocacao <= 0)
            return BuscarKartAlvo(4);
        return null;
    }

    public void VoltaMenu()
    {
        SceneManager.LoadScene("Menu Principal");
    }

}

