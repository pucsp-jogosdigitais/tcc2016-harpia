using UnityEngine;
using System.Collections;

public class NumeroScript : MonoBehaviour {

    public GameObject[] Numeros;
    public int NumeroAtual;
    private int NumAnterior;

	// Use this for initialization
	void Start () {
        NumeroAtual = 0;
        NumAnterior = 0;	
	}

    // Update is called once per frame
    void Update()
    {
        if (NumAnterior != NumeroAtual)
        {
            NumAnterior = NumeroAtual;
            MudaNum();
        }
    }

    private void MudaNum()
    {
        foreach (GameObject Numero in Numeros)
        {
            if (Numero.name == NumeroAtual.ToString())
            {
                Numero.SetActive(true);
            }
            else
            {
                Numero.SetActive(false);
            }
        }
    }
}
