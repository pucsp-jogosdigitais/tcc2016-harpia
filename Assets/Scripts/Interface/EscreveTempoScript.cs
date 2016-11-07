using UnityEngine;
using System.Collections;

public class EscreveTempoScript : MonoBehaviour {

    public NumeroScript MilisegUnidade, MilisegDezena, MilisegCentena, 
                        SegundoUnidade, SegundoDezena, 
                        MinutoUnidade, MinutoDezena;
    private int Minutos, Segundos, Milisegundos;
    private int NumMilisegUnidade, NumMilisegDezena, NumMilisegCentena, 
               NumSegundoUnidade, NumSegundoDezena, 
               NumMinutoUnidade, NumMinutoDezena;
    private int aux;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetTempos(int minutos, int segundos, int milisegundos)
    {
        Minutos = minutos;
        Segundos = segundos;
        Milisegundos = milisegundos;
        calculaNumeros();
    }

    private void calculaNumeros()
    { 
        NumSegundoUnidade = Segundos % 10;
        NumSegundoDezena = Segundos / 10;
        NumMinutoUnidade = Minutos % 10;
        NumMinutoDezena = Minutos / 10;
        NumMilisegCentena = Milisegundos / 100;
        aux = Milisegundos % 100;
        NumMilisegDezena = aux / 10;
        NumMilisegUnidade = aux % 10;
        InsereNum();
    }

    private void InsereNum()
    {
        MilisegUnidade.NumeroAtual = NumMilisegUnidade;
        MilisegDezena.NumeroAtual = NumMinutoDezena;
        MilisegCentena.NumeroAtual = NumMilisegCentena;
        SegundoUnidade.NumeroAtual = NumSegundoUnidade;
        SegundoDezena.NumeroAtual = NumSegundoDezena;
        MinutoUnidade.NumeroAtual = NumMinutoUnidade;
        MinutoDezena.NumeroAtual = NumMinutoDezena;
    }
}
