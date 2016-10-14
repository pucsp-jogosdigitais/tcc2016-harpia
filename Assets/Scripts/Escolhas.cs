using UnityEngine;
using System.Collections;

public class Escolhas : MonoBehaviour {

    public static int Numjogadores;

    public static string player1;
    public static string player2;
    public static string player3;
    public static string player4;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int GetNumJogadores()
    {
        return Numjogadores;
    }

    public void SetNumJogadores(int num)
    {
        Numjogadores = num;
    }

    public string GetPlayer(int Num)
    {
        if (Num == 1)
            return player1;
        else if (Num == 2)
            return player2;
        else if (Num == 3)
            return player3;
        else 
            return player4;
    }

    public void SetPlayer1(string Personagem)
    {
        player1 = Personagem;
    }

    public void SetPlayer2(string Personagem)
    {
        player2 = Personagem;
    }

    public void SetPlayer3(string Personagem)
    {
        player3 = Personagem;
    }

    public void SetPlayer4(string Personagem)
    {
        player4 = Personagem;
    }
}
