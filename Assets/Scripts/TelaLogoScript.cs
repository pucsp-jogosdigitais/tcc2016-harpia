using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaLogoScript : MonoBehaviour {

    private float contador;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        contador += Time.deltaTime / Time.timeScale;
        if (contador>=5)
        {
            SceneManager.LoadScene("Tela Inicial");
        }
    }
}
