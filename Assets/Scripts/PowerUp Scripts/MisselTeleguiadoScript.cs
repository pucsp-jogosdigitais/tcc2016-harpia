using UnityEngine;
using System.Collections;

public class MisselTeleguiadoScript : MonoBehaviour {

    public float velocidade = 25f;
    Rigidbody MisselRigidbody;
    GameObject KartAlvo;

    // Use this for initialization
    void Start ()
    {
        MisselRigidbody = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update () {

        if (KartAlvo != null)
        {
            transform.LookAt(KartAlvo.transform);
            //MisselRigidbody.velocity = transform.forward * velocidade;
        }
        MisselRigidbody.velocity = transform.forward * velocidade;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Unttaged")
        Destroy(this.gameObject);
    }

    public void defineAlvo(GameObject alvo)
    {
        KartAlvo = alvo;
    }
}
