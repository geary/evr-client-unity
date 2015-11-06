using System.Collections.Generic;
using UnityEngine;

namespace EaseVR {

	public class EaseLook : MonoBehaviour {

		private EaseSettings _ease;
		private List<RaycastHit> _lookedAt;
		private readonly Vector3 _forward = new Vector3( 0.5f, 0.5f, 0f );

		private float _lastUpdateTime = 0;
		private float _fpsDeltaTime = 0;
		private float _fpsFrames = 0;

		private string _log;

		void Awake() {
			_ease = GameObject.Find( "EaseVR" ).GetComponent<EaseSettings>();
			_lookedAt = new List<RaycastHit>();
		}

		void Update() {
			UpdatePresence();
			UpdateMarkers();
		}

		private void UpdatePresence() {
			_fpsFrames++;
			_fpsDeltaTime += Time.smoothDeltaTime;
			var time = Time.time;
			if( time - _lastUpdateTime < _ease.PresenceInterval ) return;
			_lastUpdateTime = time;
			if( ! transform.hasChanged ) return;
			transform.hasChanged = false;
			EaseEvent.Presence(
				transform,
				_fpsFrames / _fpsDeltaTime,
				Profiler.usedHeapSize
			);
			if( _ease.LogPresenceEvents ) {
				var position = transform.position;
				var angles = transform.eulerAngles;
				Debug.Log(
					"EASE Presence: " +
					_fpsFrames / _fpsDeltaTime + " fps, " +
					Profiler.usedHeapSize + " bytes, " +
					"position:(" + position.x + "," + position.y + "," + position.z + "), " +
					"rotation:(" + angles.x + "," + angles.y + "," + angles.z + ")"
				);
			}
		}

		void UpdateMarkers() {
			var ray = GetComponentInParent<Camera>().ViewportPointToRay( _forward );
			var hits = Physics.RaycastAll( ray );

			if( ! _ease.SeeThrough  &&  hits.Length > 0 ) {
				var near = hits[0];
				//var log = "";
				foreach( var hit in hits ) {
					//log += "EASE hit: " + hit.transform.name + ": " + hit.distance + "   ";
					if( hit.distance < near.distance )
						near = hit;
				}
				//if( log != _log ) {
				//	_log = log;
				//	Debug.Log( log );
				//}
				hits = new RaycastHit[] { near };
			}

			ExitOtherMarkers( hits );

			foreach( var hit in hits ) {
				if( hit.transform.gameObject.GetComponent<EaseMarker>() ) {
					EnterMarker( hit );
				}
			}
		}

		// Add a marker if it is not already in the list
		void EnterMarker( RaycastHit hit ) {
			var marker = hit.transform.gameObject;
			var easeMarker = marker.GetComponent<EaseMarker>();
			var runtimeId = marker.GetInstanceID();
			var shouldAdd = true;

			foreach( var lookHit in _lookedAt ) {
				if( lookHit.transform.gameObject.GetInstanceID() == runtimeId ) {
					shouldAdd = false;
					break;
				}
			}

			if( shouldAdd ) {
				if( _ease.LogOtherEvents ) {
					Debug.Log( "EASE Enter marker " + easeMarker.MarkerName );
				}

				_lookedAt.Add( hit );
				easeMarker.OnLookStart( hit, easeMarker.MarkerName );
			} else {
				//Debug.Log( "Marker already exists..." );
			}
		}

		// Exit any markers we are no longer looking at
		void ExitOtherMarkers( RaycastHit[] hits ) {
			var exitedHits = new List<RaycastHit>();
			foreach( var lookHit in _lookedAt ) {
				var exited = true;
				foreach( var hit in hits ) {
					if(
						lookHit.transform.gameObject.GetInstanceID() ==
						hit.transform.gameObject.GetInstanceID()
					) {
						exited = false;
						break;
					}
				}
				if( exited ) {
					exitedHits.Add( lookHit );
				}
			}

			foreach( var exitedHit in exitedHits ) {
				var exitedMarker =
					exitedHit.transform.gameObject.GetComponent<EaseMarker>();
				if( exitedMarker ) {
					if( _ease.LogOtherEvents ) {
						Debug.Log( "EASE Exit marker " + exitedMarker.MarkerName );
					}
					exitedMarker.OnLookEnd( exitedHit, exitedMarker.MarkerName );
					_lookedAt.Remove( exitedHit );
				}
			}
		}
	}

}
