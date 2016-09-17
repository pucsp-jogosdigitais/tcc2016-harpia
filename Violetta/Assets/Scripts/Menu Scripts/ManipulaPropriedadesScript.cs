using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class ManipulaPropriedadesScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public string Jogador(int num)
    {
        BinaryFormatter bf = new BinaryFormatter(); //Responsavel pela serializaçao
        FileStream file = File.Open(Application.persistentDataPath + "/Dados", FileMode.Open);
        Propriedades prop = (Propriedades)bf.Deserialize(file);
        file.Close(); //fecha o arquivo
        if (num == 1)
            return prop.Player1;
        else if (num == 2)
            return prop.Player2;
        else if (num == 3)
            return prop.Player3;
        else
            return prop.Player4;
    }

    public int NumJogadores(int num)
    {
        BinaryFormatter bf = new BinaryFormatter(); //Responsavel pela serializaçao

        if (num == 0) //Carregar
        {
            FileStream file = File.Open(Application.persistentDataPath + "/Dados", FileMode.Open);
            Propriedades prop = (Propriedades)bf.Deserialize(file);
            file.Close(); //fecha o arquivo
            return prop.numJogadores;
        }
        else
        {
            FileStream file = File.Create(Application.persistentDataPath + "/Dados"); //arquivo que contera os dados
            Propriedades prop = new Propriedades(); //Obj que contem os dados a serem salvos
            prop.numJogadores = num;
            bf.Serialize(file, prop); //serializa os dados no arquivo
            file.Close(); //fecha o arquivo
            return 0;
        }

        teste();
    }

    private void teste()
    {

        //Teste
        BinaryFormatter bf = new BinaryFormatter(); //Responsavel pela serializaçao
        FileStream file = File.Create(Application.persistentDataPath + "/Dados"); //arquivo que contera os dados
        Propriedades prop = new Propriedades(); //Obj que contem os dados a serem salvos
        prop.Player1 = "Violetta";
        prop.Player2 = "Jeshi";
        prop.Player3 = "Ayah";
        prop.Player4 = "Momoto";
        bf.Serialize(file, prop); //serializa os dados no arquivo
        file.Close(); //fecha o arquivo
    }

    public void SalvarEscolhaPersonagem(int jogador, string personagem)
    {
        BinaryFormatter bf = new BinaryFormatter(); //Responsavel pela serializaçao
        FileStream file = File.Create(Application.persistentDataPath + "/Dados"); //arquivo que contera os dados
        Propriedades prop = new Propriedades(); //Obj que contem os dados a serem salvos
        switch (jogador) //Salva o nome do personagem escolhido pelo jogador de acordo com seu número
        {
            #region Player 1
            case 1:
                {
                    prop.Player1 = personagem;
                }
                break;
            #endregion
            #region Player 2
            case 2:
                {
                    prop.Player2 = personagem;
                }
                break;
            #endregion
            #region Player 3
            case 3:
                {
                    prop.Player3 = personagem;
                }
                break;
            #endregion
            #region Player 4
            case 4:
                {
                    prop.Player4 = personagem;
                }
                break;
            #endregion
        }
        bf.Serialize(file, prop); //serializa os dados no arquivo
        file.Close(); //fecha o arquivo
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter(); //Responsavel pela serializaçao
        FileStream file = File.Create(Application.persistentDataPath + "/Dados"); //arquivo que contera os dados
        Propriedades prop = new Propriedades(); //Obj que contem os dados a serem salvos
        // prop.numJogadores = numJogadores;
        bf.Serialize(file, prop); //serializa os dados no arquivo
        file.Close(); //fecha o arquivo
    }

    public void Load()
    {
       /* if (File.Exists(Application.persistentDataPath + "/Dados"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Dados", FileMode.Open);
          //  Propriedades prop = (Propriedades)bf.Deserialize(file);
            file.Close();
           // numJogadores = prop.numJogadores;
        }*/
    }
}

[Serializable]
public class Propriedades
{
    public int numJogadores;
    public string Player1;
    public string Player2;
    public string Player3;
    public string Player4;
}
