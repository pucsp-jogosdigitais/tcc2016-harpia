using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AnimaBotaoScript : MonoBehaviour, ISelectHandler, IDeselectHandler// required interface when using the OnSelect method.
{
    private RectTransform ImagemBtnRect;
    private float RightSelectedValor, LeftSelectedValor, RightDeselectedValor, LeftDeselectedValor;
    public bool Selecionado = false;

    //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData)
    {
        ImagemBtnRect.offsetMin = new Vector2(LeftSelectedValor, ImagemBtnRect.offsetMin.y); //Left
        ImagemBtnRect.offsetMax = new Vector2(RightSelectedValor, ImagemBtnRect.offsetMax.y); //Right
        Selecionado = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ImagemBtnRect.offsetMin = new Vector2(LeftDeselectedValor, ImagemBtnRect.offsetMin.y); //Left
        ImagemBtnRect.offsetMax = new Vector2(RightDeselectedValor, ImagemBtnRect.offsetMax.y); //Right
        Selecionado = false;
    }

    // Use this for initialization
    void Start()
    {
        ImagemBtnRect = GetComponentInChildren<RectTransform>();
        RightSelectedValor = ImagemBtnRect.offsetMax.x + 65f;
        LeftSelectedValor = ImagemBtnRect.offsetMin.x + 65f;
        RightDeselectedValor = ImagemBtnRect.offsetMax.x - 65f;
        LeftDeselectedValor = ImagemBtnRect.offsetMin.x - 65f;
    }

    void Update()
    {
        if (Selecionado)
        {
            if (EventSystem.current.currentSelectedGameObject.gameObject != gameObject)
            {
                ImagemBtnRect.offsetMin = new Vector2(LeftDeselectedValor, ImagemBtnRect.offsetMin.y); //Left
                ImagemBtnRect.offsetMax = new Vector2(RightDeselectedValor, ImagemBtnRect.offsetMax.y); //Right
                Selecionado = false;
            }
        }
    }
}
