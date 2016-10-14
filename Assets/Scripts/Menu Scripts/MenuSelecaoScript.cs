using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSelecaoScript : MonoBehaviour {

    #region Variáveis
    int numero;
    public GameObject Plat1, Plat2, Plat3, Plat4;
    private Object InstanciaPlat1, InstanciaPlat2, InstanciaPlat3, InstanciaPlat4;
    public GameObject Moldura1, Moldura2, Moldura3, Moldura4;
    private string level = "Pista 4 - Doceria da Violetta";
    private string Player1, Player2, Player3, Player4;
    private bool p1selecionou, p2selecionou, p3selecionou, p4selecionou;
    private bool CarregarCena;
    public GameObject KartAyah, KartVioletta, KartJeshi, KartMomoto;
    #endregion

    void Start()
    {
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
                    Plat1.SetActive(true);
                    p2selecionou = true;
                    p3selecionou = true;
                    p4selecionou = true;
                    Player1 = "Violetta";
                    Player2 = "";
                    Player3 = "";
                    Player4 = "";
                    Moldura1.SetActive(true);
                }
                break;
            #endregion
            #region Caso tenha dois jogadores selecionando
            case 2:
                {
                    Plat2.SetActive(true);
                    p3selecionou = true;
                    p4selecionou = true;
                    Player1 = "Violetta";
                    Player2 = "Ayah";
                    Player3 = "";
                    Player4 = "";
                    Moldura1.SetActive(true);
                    Moldura2.SetActive(true);
                }
                break;
            #endregion
            #region Caso tenha três jogadores selecionando
            case 3:
                {
                    Plat3.SetActive(true);
                    p4selecionou = true;
                    Player1 = "Violetta";
                    Player2 = "Ayah";
                    Player3 = "Momoto";
                    Player4 = "";
                    Moldura1.SetActive(true);
                    Moldura2.SetActive(true);
                    Moldura3.SetActive(true);
                }
                break;
            #endregion
            #region Caso tenha quatro jogadores selecionando
            case 4:
                {
                    Plat4.SetActive(true);
                    Player1 = "Violetta";
                    Player2 = "Ayah";
                    Player3 = "Momoto";
                    Player4 = "Jeshi";
                    Moldura1.SetActive(true);
                    Moldura2.SetActive(true);
                    Moldura3.SetActive(true);
                    Moldura4.SetActive(true);
                }
                break;
                #endregion
        }
        InvokeRepeating("myUpdate", 0, 0.3f);
    }

    void Update()
    {
        if (Input.GetButtonDown("Confirmar/PowerUp Comum Player1"))
            p1selecionou = true;

        if (Input.GetButtonDown("Confirmar/PowerUp Comum Player2"))
            p2selecionou = true;

        if (Input.GetButtonDown("Confirmar/PowerUp Comum Player3"))
            p3selecionou = true;

        if (Input.GetButtonDown("Confirmar/PowerUp Comum Player4"))
            p4selecionou = true;
    }
	
	void myUpdate ()
    {
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
        else
        {
            #region Selecao do Player1
            if (!p1selecionou)
            {
                Player1 = Mover(Player1, Input.GetAxis("Volante Player1"));
                AtualizaMoldura(Player1, Moldura1);
                //Destroy(InstanciaPlat1);
                //InstanciaPlat1 = AtualizaModelos(Player1, Plat1);
                if (Input.GetButtonDown("Confirmar/PowerUp Comum Player1"))
                    p1selecionou = true;
            }
            #endregion
            #region Selecao do Player2
            if (!p2selecionou)
            {
                Player2 = Mover(Player2, Input.GetAxis("Volante Player2"));
                AtualizaMoldura(Player2, Moldura2);
                if (Input.GetButtonDown("Confirmar/PowerUp Comum Player2"))
                    p2selecionou = true;
            }
            #endregion
            #region Selecao do Player3
            if (!p3selecionou)
            {
                Player3 = Mover(Player3, Input.GetAxis("Volante Player3"));
                AtualizaMoldura(Player3, Moldura3);
                if (Input.GetButtonDown("Confirmar/PowerUp Comum Player3"))
                    p3selecionou = true;
            }
            #endregion
            #region Selecao do Player4
            if (!p4selecionou)
            {
                Player4 = Mover(Player4, Input.GetAxis("Volante Player4"));
                AtualizaMoldura(Player4, Moldura4);
                if (Input.GetButtonDown("Confirmar/PowerUp Comum Player4"))
                    p4selecionou = true;
            }
            #endregion
        }
    }

    private string Mover (string Atual, float Direcao)
    {
        print(Direcao);
        if (Direcao <= 0.5f && Direcao >= -0.5f)
            return Atual;

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

    private void AtualizaMoldura (string Personagem, GameObject Moldura)
    {
        Moldura.transform.parent = GameObject.Find(Personagem).transform;
        RectTransform Rect = Moldura.GetComponent<RectTransform>();
        Rect.offsetMin = new Vector2(0, 0);
        Rect.offsetMax = new Vector2(0, 0);
        Rect.sizeDelta = new Vector2(0, 0);
    }

    private Object AtualizaModelos (string Personagem, GameObject Plataforma)
    {
        if (Personagem == "Ayah")
            return Instantiate(KartAyah, Plataforma.transform.position, Plataforma.transform.rotation);
        if (Personagem == "Violetta")
            return Instantiate(KartVioletta, Plataforma.transform.position, Plataforma.transform.rotation);
        if (Personagem == "Jeshi")
            return Instantiate(KartJeshi, Plataforma.transform.position, Plataforma.transform.rotation);
        if (Personagem == "Momoto")
            return Instantiate(KartMomoto, Plataforma.transform.position, Plataforma.transform.rotation);
        return null;
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
        yield return new WaitForSeconds(5);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(level);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }



}
