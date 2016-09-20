using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSelecaoScript : MonoBehaviour {

    int numero;
    public GameObject Plat1, Plat2, Plat3, Plat4;

    private string level = "Pista 4 - Doceria da Violetta";
    private bool p1selecionou = false;
    private bool p2selecionou = false;
    private bool p3selecionou = false;
    private bool p4selecionou = false;
    private bool CarregarCena = false;

	// Use this for initialization
	void Start () {

        ManipulaPropriedadesScript manipulador = gameObject.AddComponent<ManipulaPropriedadesScript>();
        numero = manipulador.NumJogadores(0);

        switch (numero)
        {
            #region Caso tenha um jogador selecionando
            case 1:
                {
                    Plat1.SetActive(true);
                }
                break;
            #endregion
            #region Caso tenha dois jogadores selecionando
            case 2:
                {
                    Plat2.SetActive(true);
                }
                break;
            #endregion
            #region Caso tenha três jogadores selecionando
            case 3:
                {
                    Plat3.SetActive(true);
                }
                break;
            #endregion
            #region Caso tenha quatro jogadores selecionando
            case 4:
                {
                    Plat4.SetActive(true);
                }
                break;
            #endregion
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

        #region Se todos os jogadores selecionaram os personagens, carrega a pista
        if (p1selecionou && p2selecionou && p3selecionou && p4selecionou) //Se todos os jogadores já selecionaram os personagens
        {
            if (CarregarCena) //Se o loading já foi iniciado 
            {
                  //Pulsa a transparencia do texto de Loading pro jogador saber que o jogo não travou
            }
            else  //Se o jogador já selecionou o personagem e ainda não entrou no loading
            {
                CarregarCena = true; // Load scene é true para previnir que inicie o loading de mais de uma cena de uma vez
                StartCoroutine(CarregarNovaCena()); //Inicia uma coroutine que carrega a cena desejada
            }
        }
        #endregion

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
