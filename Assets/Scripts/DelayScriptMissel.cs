using UnityEngine;
using System.Collections;

public class DelayScriptMissel : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.2f);
        gameObject.tag = "Missel";

    }
}
