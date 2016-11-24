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
		ImagemBtnRect.offsetMin.x = ImagemBtnRect.offsetMin.x + 65f; //Left
		ImagemBtnRect.offsetMax.x = ImagemBtnRect.offsetMax.x + 65f; //Right
	}

	public void OnDeselect (BaseEventData eventData) 
	{
		Debug.Log (this.gameObject.name + " was Deselected");
		ImagemBtnRect.offsetMin.x = ImagemBtnRect.offsetMin.x - 65f; //Left
		ImagemBtnRect.offsetMax.x = ImagemBtnRect.offsetMax.x - 65f; //Right
	}

	// Use this for initialization
	void Start () {
		ImagemBtnRect = GetComponentInChildren<RectTransform> ();
	
	}
}
