//using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EaseLook : MonoBehaviour {
	private List<GameObject> _lookedAt;
	private readonly Vector3 _forward = new Vector3( 0.5f, 0.5f, 0f );

	// Use this for initialization
	void Start() {
		_lookedAt = new List<GameObject>();
	}

	// Update is called once per frame
	void Update() {
		// This is not the best implementation. Can see through objects.
		var ray = GetComponent<Camera>().ViewportPointToRay( _forward );
		var hits = Physics.RaycastAll( ray );

		ExitOtherMarkers( hits );

		foreach( var hit in hits ) {
			if( hit.transform.gameObject.GetComponent<EaseMarker>() ) {
				EnterMarker( hit.transform.gameObject );
			}
		}
	}

	// Add a marker if it is not already in the list
	void EnterMarker( GameObject marker ) {
		var easeMarker = marker.GetComponent<EaseMarker>();
		var runtimeId = marker.GetInstanceID();
		var shouldAdd = true;

		//Debug.Log( "Attempting to add marker with runtime ID: " + runtimeId );

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

	// Exit any markers we are no longer looking at
	void ExitOtherMarkers( RaycastHit[] hits ) {
		var exitedObjects = new List<GameObject>();
		foreach( var lookObject in _lookedAt ) {
			var exit = true;
			foreach( var hit in hits ) {
				if( lookObject.GetInstanceID() == hit.transform.gameObject.GetInstanceID() ) {
					exit = false;
					break;
				}
			}
			if( exit ) {
				exitedObjects.Add( lookObject );
			}
		}

		foreach( var exitedObject in exitedObjects ) {
			var exitedMarker = exitedObject.GetComponent<EaseMarker>();
			if( exitedMarker ) {
				Debug.Log( "Removing marker with ID: " + exitedObject.GetInstanceID() );
				exitedMarker.OnLookEnd( exitedMarker.MarkerName );
				_lookedAt.Remove( exitedObject );
			}
		}
	}
}
