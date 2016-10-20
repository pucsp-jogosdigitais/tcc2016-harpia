using UnityEngine;
using System.Collections;

public class DesaparecerScript : MonoBehaviour {

    void Start()
    {
        StartCoroutine(WaitAndDestroy());
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);

    }
}
