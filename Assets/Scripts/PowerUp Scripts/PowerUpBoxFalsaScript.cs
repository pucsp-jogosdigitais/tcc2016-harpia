using UnityEngine;
using System.Collections;

public class PowerUpBoxFalsaScript : MonoBehaviour
{

    public Transform eixoX, eixoY, eixoZ;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {       
        eixoX.Rotate(new Vector3(1, 0, 0), 30f * Time.deltaTime);
        eixoY.Rotate(new Vector3(0, 1, 0), 50f * Time.deltaTime);
        eixoZ.Rotate(new Vector3(0, 0, 1), 40f * Time.deltaTime);

    }

    void OnTriggerEnter(Collider Objeto)
    {
        
    }
}
