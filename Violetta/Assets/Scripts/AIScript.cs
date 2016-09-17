using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIScript : MonoBehaviour
{

    public Transform CaminhoGroup;
    public int pointAtual;
    private Transform[] points;
    private int pointaux = 0;
    private float maxRotação = 50.0f;
    private float distDoCaminho = 0.2f;
    private List<Transform> Caminho;
    private KartScript Kart;
    private Vector3 rotaçãoVector, aux;
    private float novaRotação;
    private string direçãoPowerUp;
    private int probSoltarPowerUp;

    // Use this for initialization
    void Start()
    {
        Kart = gameObject.GetComponent<KartScript>();
        Kart.Jogando = true;
        localizarCaminho();

        foreach (Transform ponto in Caminho)
        {
            pointaux++;
            aux = transform.InverseTransformPoint(new Vector3(ponto.position.x,
                                                                    ponto.position.y,
                                                                    ponto.position.z));
            if (aux.magnitude <= 0.5f)
            {
                pointAtual = pointaux;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
            Kart.Velocimetro();
            Kart.Acelerar(1);
            mandarRotação();
            UsarPowerUps();
    }

    private void UsarPowerUps()
    {
        if (Kart.powerUpTipo != 0)
        {
            if (Kart.posicao <= 1)
                direçãoPowerUp = "Trás";
            else
                direçãoPowerUp = "Frente";

            probSoltarPowerUp = Random.Range(0, 300);
            if (probSoltarPowerUp == 0)
                Kart.powerUpComum(direçãoPowerUp);
        }

        probSoltarPowerUp = Random.Range(0, 500);
        if (probSoltarPowerUp == 0)
            Kart.powerUpEspecial();
    }

    private void localizarCaminho()
    {
        points = CaminhoGroup.GetComponentsInChildren<Transform>();
        Caminho = new List<Transform>();

        for (int i = 0; i < points.Length; i++)
        {
            if (points[i] != CaminhoGroup)
            {
                Caminho.Add(points[i]);
            }
        }
    }

    private void mandarRotação()
    {
        rotaçãoVector = transform.InverseTransformPoint(new Vector3(Caminho[pointAtual].position.x,
                                                                    transform.position.y,
                                                                    Caminho[pointAtual].position.z));
        novaRotação = maxRotação * (rotaçãoVector.x / rotaçãoVector.magnitude);
        Kart.Direção(novaRotação);
        if (rotaçãoVector.magnitude <= distDoCaminho)
        {
            pointAtual++;
            if (pointAtual >= Caminho.Count)
            {
                pointAtual = 0;
            }
        }
    }
}
