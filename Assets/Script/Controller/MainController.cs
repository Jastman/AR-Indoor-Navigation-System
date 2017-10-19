using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

	public static MainController instance;
	public GameObject beginPoint = null, destinationPoint = null;

	// Use this for initialization
	void Awake () {
		if (instance == null)
		{
			instance = this;
		} else if (instance != this)
		{
			Destroy (gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetBeginPoint(GameObject beginPoint)
	{
		if(beginPoint.GetComponent<MarkerData>() != null)
		{
			this.beginPoint = beginPoint;
			Debug.Log("Set Begin Point to " + beginPoint.GetComponent<MarkerData>().roomName);
		}
		
	}

	public void SetDestinationPoint(GameObject destinationPoint)
	{
		if(destinationPoint.GetComponent<MarkerData>() != null)
		{
			this.destinationPoint = destinationPoint;
			Debug.Log("Set Destination Point to " + destinationPoint.GetComponent<MarkerData>().roomName);
		}
		
	}
}
