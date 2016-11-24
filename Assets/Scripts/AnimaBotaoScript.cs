using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AnimaBotaoScript : MonoBehaviour, ISelectHandler, IDeselectHandler// required interface when using the OnSelect method.
{
	private RectTransform ImagemBtnRect;

	//Do this when the selectable UI object is selected.
	public void OnSelect (BaseEventData eventData) 
	{
		Debug.Log (this.gameObject.name + " was selected");
        ImagemBtnRect.offsetMin = new Vector2(ImagemBtnRect.offsetMin.x + 65f, ImagemBtnRect.offsetMin.y);
        ImagemBtnRect.offsetMax = new Vector2(ImagemBtnRect.offsetMax.x + 65f, ImagemBtnRect.offsetMax.y);
        //ImagemBtnRect.offsetMin.x = ImagemBtnRect.offsetMin.x + 65f; //Left
        //ImagemBtnRect.offsetMax.x = ImagemBtnRect.offsetMax.x + 65f; //Right
    }

	public void OnDeselect (BaseEventData eventData) 
	{
		Debug.Log (this.gameObject.name + " was Deselected");
        ImagemBtnRect.offsetMin = new Vector2(ImagemBtnRect.offsetMin.x - 65f, ImagemBtnRect.offsetMin.y);
        ImagemBtnRect.offsetMax = new Vector2(ImagemBtnRect.offsetMax.x - 65f, ImagemBtnRect.offsetMax.y);
        //ImagemBtnRect.offsetMin.x = ImagemBtnRect.offsetMin.x - 65f; //Left
		//ImagemBtnRect.offsetMax.x = ImagemBtnRect.offsetMax.x - 65f; //Right
	}

	// Use this for initialization
	void Start () {
		ImagemBtnRect = GetComponentInChildren<RectTransform> ();
	
	}
}
