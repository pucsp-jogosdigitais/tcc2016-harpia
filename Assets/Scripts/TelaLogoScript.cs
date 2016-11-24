using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaLogoScript : MonoBehaviour {

    private float contador;
    public MovieTexture texturaVideo;
    // Use this for initialization
    void Start () {
        texturaVideo.loop = true;
        texturaVideo.Play();
    }
	
	// Update is called once per frame
	void Update () {

        contador += Time.deltaTime / Time.timeScale;
        if (contador>=7)
        {
            SceneManager.LoadScene("Tela Inicial");
        }
    }
}
