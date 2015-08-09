using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;


public class EaseLook : MonoBehaviour {

	// TODO: May want to make this a marker property?
	public bool SingleMarker = true;

	private List<GameObject> _lookedAt;
	private readonly Vector3 _forward = new Vector3( 0.5f, 0.5f, 0f );

	private string _log;

	// Use this for initialization
	void Start() {
		_lookedAt = new List<GameObject>();
	}

	// Update is called once per frame
	void Update() {
		var ray = GetComponent<Camera>().ViewportPointToRay( _forward );
		var hits = Physics.RaycastAll( ray );

		if( SingleMarker  &&  hits.Count() > 0 ) {
#if false
			hits = new[] {
				hits.Aggregate(
					( nearest, next ) => nearest.distance < next.distance ? nearest : next
				)
			};
#else
			var log = "";
			var near = hits[0];
			foreach( var hit in hits ) {
				log += hit.transform.name + ": " + hit.distance + "   ";
				if( hit.distance < near.distance )
					near = hit;
			}
			if( log != _log ) {
				_log = log;
				Debug.Log( log );
			}
			hits = new RaycastHit[] { near };
#endif
		}

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

		if( _lookedAt.All(
			lookObject => lookObject.GetInstanceID() != runtimeId
		) ) {
			//Debug.Log( "Adding marker with id: " + marker.GetInstanceID() );
			_lookedAt.Add( marker );
			easeMarker.OnLookStart( easeMarker.MarkerName );
		}
	}

	// Exit any markers we are no longer looking at
	void ExitOtherMarkers( RaycastHit[] hits ) {
		var exitedObjects = _lookedAt.Where(
			lookObject => hits.All(
				hit => lookObject.GetInstanceID() != hit.transform.gameObject.GetInstanceID()
			)
		).ToList();

		foreach( var exitedObject in exitedObjects ) {
			var exitedMarker = exitedObject.GetComponent<EaseMarker>();
			if( exitedMarker ) {
				//Debug.Log( "Removing marker with ID: " + exitedObject.GetInstanceID() );
				exitedMarker.OnLookEnd( exitedMarker.MarkerName );
				_lookedAt.Remove( exitedObject );
			}
		}
	}
}
