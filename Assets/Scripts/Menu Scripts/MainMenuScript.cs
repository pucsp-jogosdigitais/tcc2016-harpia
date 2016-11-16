using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public GameObject PainelConfig, PainelMenu, PainelCustom, PainelCreditos, PainelControles;
    public Button BotInicialConfig, BotInicialMenu, BotInicialCustom, BotInicialCreditos, BotInicialControles;
    public MovieTexture texturaVideo;

	// Use this for initialization
	void Start () {
        PainelConfig.SetActive(false);
        PainelCustom.SetActive(false);
        PainelControles.SetActive(false);
        PainelCreditos.SetActive(false);
        PainelMenu.SetActive(true);
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(BotInicialMenu.gameObject);
        texturaVideo.loop = true;
        texturaVideo.Play();
	}

    public void CustomGame()
    {
        PainelMenu.SetActive(false);
        PainelConfig.SetActive(false);
        PainelControles.SetActive(false);
        PainelCreditos.SetActive(false);
        PainelCustom.SetActive(true);
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(BotInicialCustom.gameObject);
    }

    public void LoadCustom(int Players)
    {
        Escolhas.Numjogadores = Players;
        SceneManager.LoadScene("SelecaoPlayers");
    }

    public void Config()
    {
        PainelMenu.SetActive(false);
        PainelCustom.SetActive(false);
        PainelControles.SetActive(false);
        PainelCreditos.SetActive(false);
        PainelConfig.SetActive(true);
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(BotInicialConfig.gameObject);
    }

    public void Controles()
    {
        PainelMenu.SetActive(false);
        PainelCustom.SetActive(false);
        PainelConfig.SetActive(false);
        PainelControles.SetActive(true);
        PainelCreditos.SetActive(false);
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(BotInicialControles.gameObject);
    }

    public void Creditos()
    {
        PainelMenu.SetActive(false);
        PainelCustom.SetActive(false);
        PainelConfig.SetActive(false);
        PainelControles.SetActive(false);
        PainelCreditos.SetActive(true);
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(BotInicialCreditos.gameObject);
    }

    public void Back()
    {
        PainelConfig.SetActive(false);
        PainelCustom.SetActive(false);
        PainelControles.SetActive(false);
        PainelCreditos.SetActive(false);
        PainelMenu.SetActive(true);
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(BotInicialMenu.gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
