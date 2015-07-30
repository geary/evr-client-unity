//using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EaseLook : MonoBehaviour {
	private List<GameObject> _lookedAt;

	// Use this for initialization
	void Start() {
		_lookedAt = new List<GameObject>();
	}

	// Update is called once per frame
	void Update() {
		// This is not the best implementation. Can see through objects.
		var hits = Physics.RaycastAll(
			GetComponent<Camera>().ViewportPointToRay(
				new Vector3( 0.5F, 0.5F, 0.5f )
			)
		);

		foreach( var hit in hits ) {
			if( hit.transform.gameObject.GetComponent<EaseMarker>() ) {
				AddMarker( hit.transform.gameObject );
			}
		}

		RemoveDeadMarkers( hits );
	}

	// Check for markers that should be removed
	void RemoveDeadMarkers( RaycastHit[] hits ) {
		var foundMatch = false;

		var removableGameObjects = new List<GameObject>();

		foreach( var lookObject in _lookedAt ) {
			foreach( var hit in hits ) {
				if( lookObject.GetInstanceID() == hit.transform.gameObject.GetInstanceID() ) {
					foundMatch = true;
					break;
				}
			}

			if( ! foundMatch ) {
				removableGameObjects.Add( lookObject );
			}
		}

		foreach( var removeObject in removableGameObjects ) {
			var removeMarker = removeObject.GetComponent<EaseMarker>();
			if( removeMarker ) {
				Debug.Log( "Removing marker with ID: " + removeObject.GetInstanceID() );

				removeMarker.OnLookEnd( removeMarker.MarkerName );
				_lookedAt.Remove( removeObject );
			}
		}
	}

	// Add a marker if it is not already in the list
	void AddMarker( GameObject marker ) {
		var easeMarker = marker.GetComponent<EaseMarker>();
		var runtimeId = marker.GetInstanceID();
		var shouldAdd = true;

		//Debug.Log( "Attempting to add marker with runtime ID: " + runtimeId );

		if( _lookedAt.Count == 0 ) {
			_lookedAt.Add( marker );
			easeMarker.OnLookStart( easeMarker.MarkerName );
		} else {
			foreach( var lookObject in _lookedAt ) {
				if( lookObject.GetInstanceID() == runtimeId ) {
					shouldAdd = false;
					break;
				}
			}

			if( shouldAdd ) {
				Debug.Log( "Adding marker with id: " + marker.GetInstanceID() );

				_lookedAt.Add( marker );
				easeMarker.OnLookStart( easeMarker.MarkerName );
			} else {
				//Debug.Log( "Marker already exists..." );
			}
		}
	}
}
