using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TelaInicialScript : MonoBehaviour
{
    public MovieTexture texturaVideo;

    // Use this for initialization
    void Start () {
        texturaVideo.loop = true;
        texturaVideo.Play();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.anyKey)
        {
            SceneManager.LoadScene("Menu Principal");
        }
	
	}
}
