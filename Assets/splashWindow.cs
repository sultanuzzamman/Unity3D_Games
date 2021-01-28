using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class splashWindow : MonoBehaviour {

	public float timer;
	public Slider loadingSlider;
	// Use this for initialization
	void Start () {
		timer = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
		loadingSlider.value += 1 * Time.deltaTime;
		if (loadingSlider.value == 5) {
			Application.LoadLevel("MainMenu");	
		}
	}
}
