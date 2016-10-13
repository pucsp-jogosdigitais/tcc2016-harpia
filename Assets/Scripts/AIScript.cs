using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIScript : MonoBehaviour
{

    public Transform CaminhoGroup;
    public Transform NavMesh;
    public int pointAtual;

    private Transform[] points;
    private int pointaux = 0;
    private float maxRotação = 50.0f;
    private float distDoCaminho = 1f;
    private List<Transform> Caminho;
    private KartScript Kart;
    private Vector3 rotaçãoVector, aux;
    private float novaRotação;
    private string direçãoPowerUp;
    private int probSoltarPowerUp;
    private Transform PontoAtual, PontoAnterior;
    private float distancia = 0;
    private float ultimaDistancia = 0;
    private float distanciaAteAgr = 0;
    private Vector3 PontoProjetado;
    private Vector3 vectome;
    private Vector3 dir;
    private Vector3 PosAlvo;

    private float pontoInicialSensorMeio = 0.6f;
    private float pontoInicialSensorLateral = 0.3f;
    private float alcanceSensorMeio = 50;
    private float alcanceSensorLateral = 10;
    private float anguloSensorLateral = 20;
    private Vector3 auxAnguloSensor;
    private Vector3 posSensor;
    private RaycastHit hitSensor;
    
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

        PontoAtual = Caminho[0];
        PosAlvo = PontoAtual.position;
    }

    // Update is called once per frame
    void Update()
    {
        Kart.Velocimetro();
        Kart.Acelerar(1);
        mudarRotação();
        UsarPowerUps();
        Sensores();
    }

    void FixedUpdate()
    {
        NavMesh.position = this.gameObject.transform.position;
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

    private void mudarRotação()
    {
        if (PontoAnterior)
        {
            dir = PontoAnterior.position - PontoAtual.position;
            vectome = transform.position - PontoAnterior.position;
            PontoProjetado = Vector3.Project(vectome, dir);
            Debug.DrawLine(PontoAnterior.position, PontoAtual.position);
            Debug.DrawLine(transform.position, PontoProjetado + PontoAnterior.position);
            ultimaDistancia = Vector3.Distance(PontoAnterior.position, PontoProjetado + PontoAnterior.position);
            distancia = Vector3.Distance(PontoAtual.position, PontoProjetado + PontoAnterior.position);
        }
        else
        {
            distancia = new Vector3(this.transform.position.x - PontoAtual.position.x, 0, this.transform.position.y - PontoAtual.position.y).magnitude;
        }

        if (distancia < distDoCaminho)
        {
            pointAtual++;
            if (pointAtual >= Caminho.Count)
            {
                pointAtual = 0;
            }
            if (PontoAnterior)
            {
                distanciaAteAgr += Vector3.Distance(PontoAnterior.position, PontoAtual.position);
            }
            PontoAnterior = PontoAtual;
            PontoAtual = Caminho[pointAtual];
            Vector3 dir = PontoAnterior.position - PontoAtual.position;
            Vector3 vectome = transform.position - PontoAnterior.position;
            Vector3 projectedpoint = Vector3.Project(vectome, dir);
            PosAlvo = PontoAtual.position;
        }
        rotaçãoVector = transform.InverseTransformPoint(new Vector3(Caminho[pointAtual].position.x,
                                                                    transform.position.y,
                                                                    Caminho[pointAtual].position.z));
        novaRotação = maxRotação * (rotaçãoVector.x / rotaçãoVector.magnitude);
        Kart.Direção(novaRotação);
    }

    private void Sensores()
    {
        //Front Mid Sensor
        posSensor = transform.position;
        posSensor += transform.forward * pontoInicialSensorMeio;
        if (Physics.Raycast(posSensor, transform.forward, out hitSensor, alcanceSensorMeio))
        {
            Debug.DrawLine(posSensor, hitSensor.point, Color.black);
        }

        //Front Angled Right Sensor
        posSensor += transform.right * pontoInicialSensorLateral;
        auxAnguloSensor = Quaternion.AngleAxis(anguloSensorLateral, transform.up) * transform.forward;
        if (Physics.Raycast(posSensor, auxAnguloSensor, out hitSensor, alcanceSensorLateral))
        {
            Debug.DrawLine(posSensor, hitSensor.point, Color.white);
        }

        //Front Straight left Sensor
        posSensor = transform.position;
        posSensor += transform.forward * pontoInicialSensorMeio;
        posSensor -= transform.right * pontoInicialSensorLateral;
        auxAnguloSensor = Quaternion.AngleAxis(-anguloSensorLateral, transform.up) * transform.forward;
        if (Physics.Raycast(posSensor, auxAnguloSensor, out hitSensor, alcanceSensorLateral))
        {
            Debug.DrawLine(posSensor, hitSensor.point, Color.white);
        }

    }
}
