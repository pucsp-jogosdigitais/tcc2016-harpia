using UnityEngine;
using System.Collections;

public class AutodestruirAncoraScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
