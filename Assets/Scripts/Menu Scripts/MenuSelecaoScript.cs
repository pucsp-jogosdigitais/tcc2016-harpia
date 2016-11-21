using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSelecaoScript : MonoBehaviour {

    #region Variáveis
    int numero;
    public GameObject PainelCena, PainelLoading;
    public GameObject Painel1P, Painel2P, Painel3P, Painel4P;
    private string level = "Pista 4 - Doceria da Violetta";
    private string Player1, Player2, Player3, Player4;
    private bool p1selecionou, p2selecionou, p3selecionou, p4selecionou;
    private bool CarregarCena;
    private float inputP1, inputP2, inputP3, inputP4;
    private MolduraScript MoldP1, MoldP2, MoldP3, MoldP4;
    private PainelScript Aux;
    private AsyncOperation async;
    public Texture ProgressBarFull;
    #endregion

    void Start()
    {
        PainelLoading.SetActive(false);
        PainelCena.SetActive(true);

        p1selecionou = false;
        p2selecionou = false;
        p3selecionou = false;
        p4selecionou = false;
        CarregarCena = false;

        numero = Escolhas.Numjogadores;
        switch (numero)
        {
            #region Caso tenha um jogador selecionando
            case 1:
                {
                    Painel1P.SetActive(true);
                    p2selecionou = true;
                    p3selecionou = true;
                    p4selecionou = true;
                    Player1 = "Violetta";
                    Player2 = "";
                    Player3 = "";
                    Player4 = "";
                    Aux = Painel1P.GetComponent<PainelScript>();
                    MoldP1 = Aux.Player1;
                    MoldP1.AtualizaMoldura(Player1);
                }
                break;
            #endregion
            #region Caso tenha dois jogadores selecionando
            case 2:
                {
                    Painel2P.SetActive(true);
                    p3selecionou = true;
                    p4selecionou = true;
                    Player1 = "Violetta";
                    Player2 = "Ayah";
                    Player3 = "";
                    Player4 = "";
                    Aux = Painel2P.GetComponent<PainelScript>();
                    MoldP1 = Aux.Player1;
                    MoldP2 = Aux.Player2;
                    MoldP1.AtualizaMoldura(Player1);
                    MoldP2.AtualizaMoldura(Player2);
                }
                break;
            #endregion
            #region Caso tenha três jogadores selecionando
            case 3:
                {
                    Painel3P.SetActive(true);
                    p4selecionou = true;
                    Player1 = "Violetta";
                    Player2 = "Ayah";
                    Player3 = "Momoto";
                    Player4 = "";
                    Aux = Painel3P.GetComponent<PainelScript>();
                    MoldP1 = Aux.Player1;
                    MoldP2 = Aux.Player2;
                    MoldP3 = Aux.Player3;
                    MoldP1.AtualizaMoldura(Player1);
                    MoldP2.AtualizaMoldura(Player2);
                    MoldP3.AtualizaMoldura(Player3);
                }
                break;
            #endregion
            #region Caso tenha quatro jogadores selecionando
            case 4:
                {
                    Painel4P.SetActive(true);
                    Player1 = "Violetta";
                    Player2 = "Ayah";
                    Player3 = "Momoto";
                    Player4 = "Jeshi";
                    Aux = Painel4P.GetComponent<PainelScript>();
                    MoldP1 = Aux.Player1;
                    MoldP2 = Aux.Player2;
                    MoldP3 = Aux.Player3;
                    MoldP4 = Aux.Player4;
                    MoldP1.AtualizaMoldura(Player1);
                    MoldP2.AtualizaMoldura(Player2);
                    MoldP3.AtualizaMoldura(Player3);
                    MoldP4.AtualizaMoldura(Player4);
                }
                break;
                #endregion
        }
        InvokeRepeating("myUpdate", 0, 0.3f);
    }

    void Update()
    {
        if (Input.GetButtonDown("Confirmar/PowerUp Comum Player1"))
            ConfirmaPlayer1();

        if (Input.GetButtonDown("Confirmar/PowerUp Comum Player2"))
            ConfirmaPlayer2();

        if (Input.GetButtonDown("Confirmar/PowerUp Comum Player3"))
            ConfirmaPlayer3();

        if (Input.GetButtonDown("Confirmar/PowerUp Comum Player4"))
            ConfirmaPlayer4();

        if (Input.GetButtonDown("Voltar/PowerUp Especial Player1"))
            VoltarMenu();

        if (Input.GetButtonDown("Voltar/PowerUp Especial Player2"))
            VoltarMenu();

        if (Input.GetButtonDown("Voltar/PowerUp Especial Player3"))
            VoltarMenu();

        if (Input.GetButtonDown("Voltar/PowerUp Especial Player4"))
            VoltarMenu();
    }
	
	void myUpdate ()
    {
        #region Todos selecionaram
        if (p1selecionou && p2selecionou && p3selecionou && p4selecionou) //Se todos os jogadores já selecionaram os personagens
        {
            SalvaEscolhas();
            #region Se todos os jogadores selecionaram os personagens, carrega a pista
            if (CarregarCena) //Se o loading já foi iniciado 
            {
                //Pulsa a transparencia do texto de Loading pro jogador saber que o jogo não travou
            }
            else  //Se o jogador já selecionou o personagem e ainda não entrou no loading
            {
                CarregarCena = true; // Load scene é true para previnir que inicie o loading de mais de uma cena de uma vez
                StartCoroutine(CarregarNovaCena()); //Inicia uma coroutine que carrega a cena desejada
            }
            #endregion
        }
        #endregion
        else
        {
            #region Selecao do Player1
            inputP1 = Input.GetAxis("Volante Player1");
            if (inputP1 > 0.5f)
                DireitaPlayer1();
            else if (inputP1 < -0.5f)
                EsquerdaPlayer1();
            #endregion
            #region Selecao do Player2
            if (!p2selecionou)
            {
                inputP2 = Input.GetAxis("Volante Player2");
                if (inputP2 > 0.5f)
                    DireitaPlayer2();
                else if (inputP2 < -0.5f)
                    EsquerdaPlayer2();
            }
            #endregion
            #region Selecao do Player3
            if (!p3selecionou)
            {
                inputP3 = Input.GetAxis("Volante Player3");
                if (inputP3 > 0.5f)
                    DireitaPlayer3();
                else if (inputP3 < -0.5f)
                    EsquerdaPlayer3();
            }
            #endregion
            #region Selecao do Player4
            if (!p4selecionou)
            {
                inputP4 = Input.GetAxis("Volante Player4");
                if (inputP4 > 0.5f)
                    DireitaPlayer4();
                else if (inputP4 < -0.5f)
                    EsquerdaPlayer4();
            }
            #endregion
        }
    }

    private string Mover (string Atual, float Direcao)
    {
        print(Atual +", "+ Direcao);
        switch (Atual)
        {
            #region Atual é Violetta
            case "Violetta":
                {
                    if (Direcao > 0.5f)
                        return "Ayah";
                    if (Direcao < 0.5f)
                        return "Jeshi";
                }
                break;
            #endregion
            #region Atual é Ayah
            case "Ayah":
                {
                    if (Direcao > 0.5f)
                        return "Momoto";
                    if (Direcao < 0.5f)
                        return "Violetta";
                }
                break;
            #endregion
            #region Atual é Momoto
            case "Momoto":
                {
                    if (Direcao > 0.5f)
                        return "Jeshi";
                    if (Direcao < 0.5f)
                        return "Ayah";
                }
                break;
            #endregion
            #region Atual é Jeshi
            case "Jeshi":
                {
                    if (Direcao > 0.5f)
                        return "Violetta";
                    if (Direcao < 0.5f)
                        return "Momoto";
                }
                break;
            #endregion
        }
        return Atual;
    }
    

    private void SalvaEscolhas ()
    {
        Escolhas.player1 = Player1;
        Escolhas.player2 = Player2;
        Escolhas.player3 = Player3;
        Escolhas.player4 = Player4;
    }

    IEnumerator CarregarNovaCena()
    {
        //espera 3 segundos
        //yield return new WaitForSeconds(5);
        PainelCena.SetActive(false);
        PainelLoading.SetActive(true);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        async = SceneManager.LoadSceneAsync(level);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }

    void OnGUI()
    {
        if (async != null)
        {
            //GUI.DrawTexture(new Rect(0, 0, 100, 50), ProgressBarEmpty);
           // GUI.DrawTexture(new Rect(95, 167, 210 * async.progress, 25), ProgressBarFull);
        }
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene("Menu Principal");
    }

    public void DireitaPlayer1()
    {
        if (!p1selecionou)
        {
            Player1 = Mover(Player1, 1);
            MoldP1.AtualizaMoldura(Player1);
        }
    }

    public void EsquerdaPlayer1()
    {
        if (!p1selecionou)
        {
            Player1 = Mover(Player1, -1);
            MoldP1.AtualizaMoldura(Player1);
        }
    }

    public void ConfirmaPlayer1()
    {
        if (!p1selecionou)
        {
            p1selecionou = true;
        }
    }



    public void DireitaPlayer2()
    {
        if (!p2selecionou)
        {
            Player2 = Mover(Player2, 1);
            MoldP2.AtualizaMoldura(Player2);
        }
    }

    public void EsquerdaPlayer2()
    {
        if (!p2selecionou)
        {
            Player2 = Mover(Player2, -1);
            MoldP2.AtualizaMoldura(Player2);
        }
    }

    public void ConfirmaPlayer2()
    {
        if (!p2selecionou)
        {
            p2selecionou = true;
        }
    }



    public void DireitaPlayer3()
    {
        if (!p3selecionou)
        {
            Player3 = Mover(Player3, 1);
            MoldP3.AtualizaMoldura(Player3);
        }
    }

    public void EsquerdaPlayer3()
    {
        if (!p3selecionou)
        {
            Player3 = Mover(Player3, -1);
            MoldP3.AtualizaMoldura(Player3);
        }
    }

    public void ConfirmaPlayer3()
    {
        if (!p3selecionou)
        {
            p3selecionou = true;
        }
    }



    public void DireitaPlayer4()
    {
        if (!p4selecionou)
        {
            Player4 = Mover(Player4, 1);
            MoldP4.AtualizaMoldura(Player4);
        }
    }

    public void EsquerdaPlayer4()
    {
        if (!p4selecionou)
        {
            Player4 = Mover(Player4, -1);
            MoldP4.AtualizaMoldura(Player4);
        }
    }

    public void ConfirmaPlayer4()
    {
        if (!p4selecionou)
        {
            p4selecionou = true;
            MoldP4.AtualizaMoldura(Player4);
        }
    }

}
