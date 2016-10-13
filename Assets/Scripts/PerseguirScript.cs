using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerseguirScript : MonoBehaviour
{

    public Transform CaminhoGroup;
    public int pointAtual;
    private Transform[] points;
    private int pointaux = 0;
    private float maxRotação = 50.0f;
    private float distDoCaminho = 0.1f;
    private List<Transform> Caminho;
    private Vector3 rotaçãoVector, aux;
    private float novaRotação;

    // Use this for initialization
    void Start()
    {
        localizarCaminho();

        foreach (Transform ponto in Caminho)
        {
            pointaux++;
            aux = transform.InverseTransformPoint(new Vector3(ponto.position.x,
                                                                    ponto.position.y,
                                                                    ponto.position.z));
            if (aux.magnitude <= 5f)
            {
                pointAtual = pointaux;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculaDistancia();
        Mover(Caminho[pointAtual], 30f);
    }

    private void Mover(Transform PosAlvo, float Velocidade)
    {
        // rotate towards the target
        transform.forward = Vector3.RotateTowards(transform.forward, PosAlvo.position - transform.position, Velocidade * Time.deltaTime / Time.timeScale, 0.0f);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, PosAlvo.position, Velocidade * Time.deltaTime / Time.timeScale);
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

    private void CalculaDistancia()
    {
        rotaçãoVector = transform.InverseTransformPoint(new Vector3(Caminho[pointAtual].position.x,
                                                                    transform.position.y,
                                                                    Caminho[pointAtual].position.z));
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
