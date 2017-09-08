using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasResolutionScript : MonoBehaviour {

	private float actionBarHeight;
	public Image actionBarImage;
	private RectTransform actionBar;

	// Use this for initialization
	void Start () {
		Screen.fullScreen = false;
		actionBarHeight = DPinPixel(48);

		actionBar = actionBarImage.gameObject.GetComponent<RectTransform>();
		actionBar.sizeDelta = new Vector2(Screen.currentResolution.width, actionBarHeight);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/* get dp value return in pixel */
	private float DPinPixel(int dp)
	{
		return dp * (Screen.dpi/160f);
	}
}
