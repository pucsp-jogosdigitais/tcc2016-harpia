using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AnimaBotaoScript : MonoBehaviour // required interface when using the OnSelect method.
{
    private RectTransform ImagemBtnRect;
    private float RightSelectedValor, LeftSelectedValor, RightDeselectedValor, LeftDeselectedValor;

    // Use this for initialization
    void Start()
    {
        ImagemBtnRect = GetComponentInChildren<RectTransform>();
        RightSelectedValor = ImagemBtnRect.offsetMax.x + 65f;
        LeftSelectedValor = ImagemBtnRect.offsetMin.x + 65f;
        RightDeselectedValor = ImagemBtnRect.offsetMax.x;
        LeftDeselectedValor = ImagemBtnRect.offsetMin.x;
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject.gameObject != gameObject)
        {
            ImagemBtnRect.offsetMin = new Vector2(LeftDeselectedValor, ImagemBtnRect.offsetMin.y); //Left
            ImagemBtnRect.offsetMax = new Vector2(RightDeselectedValor, ImagemBtnRect.offsetMax.y); //Right
        }
        else
        {
            ImagemBtnRect.offsetMin = new Vector2(LeftSelectedValor, ImagemBtnRect.offsetMin.y); //Left
            ImagemBtnRect.offsetMax = new Vector2(RightSelectedValor, ImagemBtnRect.offsetMax.y); //Right
        }

    }
}
