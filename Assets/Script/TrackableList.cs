using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackableList : MonoBehaviour {
 
    // Update is called once per frame
    void Update () {
        // Get the Vuforia StateManager
        StateManager sm = TrackerManager.Instance.GetStateManager ();
 
        // Query the StateManager to retrieve the list of
        // currently 'active' trackables 
        //(i.e. the ones currently being tracked by Vuforia)
        IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours ();
 
       // Iterate through the list of active trackables
        Debug.Log ("List of trackables currently active (tracked): ");
        foreach (TrackableBehaviour tb in activeTrackables) {
            Debug.Log("Trackable: " + tb.TrackableName);
        }
    }

	public IEnumerable<TrackableBehaviour> GetActiveTrackable()
	{
		StateManager sm = TrackerManager.Instance.GetStateManager ();
        return sm.GetActiveTrackableBehaviours ();
	}
}
