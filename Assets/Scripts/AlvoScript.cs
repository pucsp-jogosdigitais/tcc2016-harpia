using UnityEngine;
using System.Collections;

public class AlvoScript : MonoBehaviour {

	public Camera KartCam;
	private float ScaleMax, ScaleMin;
	private bool aumentando;

	// Use this for initialization
	void Start () {
		ScaleMax = this.transform.localScale.x;
		ScaleMin = ScaleMax * 0.6f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (2 * transform.position - KartCam.transform.position);	
		if (this.transform.localScale.x >= ScaleMax)
			aumentando = false;

		if (this.transform.localScale.x < ScaleMin)
			aumentando = true;

		if (aumentando) {
			this.transform.localScale = new Vector3 (
				this.transform.localScale.x + Time.deltaTime/Time.timeScale, 
				this.transform.localScale.y + Time.deltaTime/Time.timeScale, 
				this.transform.localScale.z + Time.deltaTime/Time.timeScale);
		} else {
			this.transform.localScale = new Vector3 (
				this.transform.localScale.x - Time.deltaTime/Time.timeScale, 
				this.transform.localScale.y - Time.deltaTime/Time.timeScale, 
				this.transform.localScale.z - Time.deltaTime/Time.timeScale);
		}
	}
}
