using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EaseLook : MonoBehaviour {
	private List<GameObject> lookedAt;

	// Use this for initialization
	void Start () {
		lookedAt = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		// This is not the best implementation. Can see through objects.
		RaycastHit[] hits = Physics.RaycastAll(GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5F, 0.5F, 0.5f)));

		for (int i = 0; i < hits.Length; i++) {
			if (hits[i].transform.gameObject.GetComponent<EaseMarker>()) {
				addMarker(hits[i].transform.gameObject);
			}
		}


		removeDeadMarkers(hits);
	}

	// This will run on every update to check for markers that should be removed
	void removeDeadMarkers(RaycastHit[] hits) {
		bool             foundMatch = false;
		List<GameObject> removableGameObjects;

		removableGameObjects = new List<GameObject>();

		foreach (GameObject gameObject in lookedAt) {
			for (int i = 0; i < hits.Length; i++) {
				if (gameObject.GetInstanceID() == hits[i].transform.gameObject.GetInstanceID()) {
					foundMatch = true;
					break;
				}
			}

			if (!foundMatch) {
				removableGameObjects.Add(gameObject);
			}
		}

		foreach (GameObject gameObjectToRemove in removableGameObjects) {
			if (gameObjectToRemove.GetComponent<EaseMarker>()) {
				Debug.Log("Removing marker with ID: " + gameObjectToRemove.GetInstanceID());

				gameObjectToRemove.GetComponent<EaseMarker>().onLookEnd(gameObjectToRemove.GetComponent<EaseMarker>().markerName);
				lookedAt.Remove(gameObjectToRemove);
			}
		}
	}

	// This will add a marker after it is determined that the marker is not already in the list
	void addMarker(GameObject marker) {
		int  runtimeId = marker.GetInstanceID();
		bool shouldAdd = true;

		Debug.Log("Attempting to add marker with runtime ID: " + runtimeId);

		if (lookedAt.Count == 0) {
			lookedAt.Add(marker);
			marker.GetComponent<EaseMarker>().onLookStart(marker.GetComponent<EaseMarker>().markerName);
		} else {
			foreach (GameObject gameObject in lookedAt) {
				if (gameObject.GetInstanceID() == runtimeId) {
					shouldAdd = false;
					break;
				}
			}

			if (shouldAdd == true) {
				Debug.Log("Adding marker with id: " + marker.GetInstanceID());

				lookedAt.Add(marker);
				marker.GetComponent<EaseMarker>().onLookStart(marker.GetComponent<EaseMarker>().markerName);
			} else {
				Debug.Log("Marker already exists...");
			}
		}
	}
}
