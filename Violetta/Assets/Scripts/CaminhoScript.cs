using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaminhoScript : MonoBehaviour
{

    Color rayColor = Color.white;
    List<Transform> Caminho;
    Transform[] Points;

    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        Points = transform.GetComponentsInChildren<Transform>();
        Caminho = new List<Transform>();

        foreach (Transform point in Points) //Adiciona todos os pontos na lista de caminho
        {
            if (point != transform)
                Caminho.Add(point);
        }

        for (int i = 0; i < Caminho.Count; i++)
        {
            Vector3 pos = Caminho[i].position;
            Gizmos.DrawWireSphere(pos, 0.3f);
            if (i > 0)
            {
                Vector3 prev = Caminho[i - 1].position;
                Gizmos.DrawLine(prev, pos);
            }
        }

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
