using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GerenciaSelecaoScript : MonoBehaviour {

    public MenuSelecaoScript ScriptSelecao;
    public GameObject VerdeP1, AmareloP1, VermelhoP1;
    public GameObject VerdeP2, AmareloP2, VermelhoP2;
    public GameObject VerdeP3, AmareloP3, VermelhoP3;
    public GameObject VerdeP4, AmareloP4, VermelhoP4;
    public List<string> Selecionados;

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        Player1();
        if (VerdeP2 != null)
            Player2();
        if (VerdeP3 != null)
            Player3();
        if (VerdeP4 != null)
            Player4();
    }



    private void Player1()
    {
        if (!ScriptSelecao.p1selecionou)
        {
            if (Selecionados.Contains((ScriptSelecao.Player1)))
            {
                VerdeP1.SetActive(false);
                AmareloP1.SetActive(true);
                VermelhoP1.SetActive(false);
                ScriptSelecao.p1PodeSelecionar = false;
            }
            else
            {
                VerdeP1.SetActive(true);
                AmareloP1.SetActive(false);
                VermelhoP1.SetActive(false);
                ScriptSelecao.p1PodeSelecionar = true;
            }        
        }
        else
        {
            VerdeP1.SetActive(false);
            AmareloP1.SetActive(false);
            VermelhoP1.SetActive(true);
            ScriptSelecao.p1PodeSelecionar = false;
        }

    }

    private void Player2()
    {
        if (!ScriptSelecao.p2selecionou)
        {
            if (Selecionados.Contains((ScriptSelecao.Player2)))
            {
                VerdeP2.SetActive(false);
                AmareloP2.SetActive(true);
                VermelhoP2.SetActive(false);
                ScriptSelecao.p2PodeSelecionar = false;
            }
            else
            {
                VerdeP2.SetActive(true);
                AmareloP2.SetActive(false);
                VermelhoP2.SetActive(false);
                ScriptSelecao.p2PodeSelecionar = true;
            }
        }
        else
        {
            VerdeP2.SetActive(false);
            AmareloP2.SetActive(false);
            VermelhoP2.SetActive(true);
            ScriptSelecao.p2PodeSelecionar = false;
        }

    }

    private void Player3()
    {
        if (!ScriptSelecao.p3selecionou)
        {
            if (Selecionados.Contains((ScriptSelecao.Player3)))
            {
                VerdeP3.SetActive(false);
                AmareloP3.SetActive(true);
                VermelhoP3.SetActive(false);
                ScriptSelecao.p3PodeSelecionar = false;
            }
            else
            {
                VerdeP3.SetActive(true);
                AmareloP3.SetActive(false);
                VermelhoP3.SetActive(false);
                ScriptSelecao.p3PodeSelecionar = true;
            }
        }
        else
        {
            VerdeP3.SetActive(false);
            AmareloP3.SetActive(false);
            VermelhoP3.SetActive(true);
            ScriptSelecao.p3PodeSelecionar = false;
        }

    }

    private void Player4()
    {
        if (!ScriptSelecao.p4selecionou)
        {
            if (Selecionados.Contains((ScriptSelecao.Player4)))
            {
                VerdeP4.SetActive(false);
                AmareloP4.SetActive(true);
                VermelhoP4.SetActive(false);
                ScriptSelecao.p4PodeSelecionar = false;
            }
            else
            {
                VerdeP4.SetActive(true);
                AmareloP4.SetActive(false);
                VermelhoP4.SetActive(false);
                ScriptSelecao.p4PodeSelecionar = true;
            }
        }
        else
        {
            VerdeP4.SetActive(false);
            AmareloP4.SetActive(false);
            VermelhoP4.SetActive(true);
            ScriptSelecao.p4PodeSelecionar = false;
        }

    }
}
