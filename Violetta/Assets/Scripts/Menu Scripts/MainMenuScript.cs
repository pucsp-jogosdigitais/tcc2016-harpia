using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public GameObject PainelConfig, PainelMenu, PainelCustom;
    public Button BotInicialConfig, BotInicialMenu, BotInicialCustom;

	// Use this for initialization
	void Start () {
        PainelConfig.SetActive(false);
        PainelCustom.SetActive(false);
        PainelMenu.SetActive(true);
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(BotInicialMenu.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CustomGame()
    {
        PainelMenu.SetActive(false);
        PainelConfig.SetActive(false);
        PainelCustom.SetActive(true);
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(BotInicialCustom.gameObject);
    }

    public void CarrerGame()
    {

    }

    public void LoadCustom(int Players)
    {
       // ManipulaPropriedadesScript manipula = new ManipulaPropriedadesScript();
        ManipulaPropriedadesScript manipula = gameObject.AddComponent<ManipulaPropriedadesScript>();
        manipula.NumJogadores(Players);
        SceneManager.LoadScene("SelecaoPlayers");
    }

    public void Config()
    {
        PainelMenu.SetActive(false);
        PainelCustom.SetActive(false);
        PainelConfig.SetActive(true);
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(BotInicialConfig.gameObject);
    }

    public void Back()
    {
        PainelConfig.SetActive(false);
        PainelCustom.SetActive(false);
        PainelMenu.SetActive(true);
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(BotInicialMenu.gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
