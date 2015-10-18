using UnityEngine;


namespace EaseVR {

	[RequireComponent( typeof(Collider) )]
	public class EaseMarker : MonoBehaviour {

		//private bool _debug;

		public string MarkerName;
		public string ObjectName;
		public bool IsObject;
		public bool Inside;
		public bool DayTime;
		//public string CustomProp01;
		//public string CustomProp02;
		//public string CustomProp03;

		private EaseSettings _ease;
		private bool _looking;
		private Color _saveColor;

		void Awake() {
			_ease = GameObject.Find( "EaseVR" ).GetComponent<EaseSettings>();
		}

		void Start() {
			if( MarkerName.Length == 0 ) {
				MarkerName = GetInstanceID().ToString();
			}
			EaseEvent.MarkerAdd( true, MarkerName, transform );
		}

		public void OnLookStart( RaycastHit hit, string name ) {
			//Dictionary< string, string > eventData;

			if( _looking ) return;
			_looking = true;

			//Debug.Log( "Ease Marker In <" + name + ">" );

			EaseEvent.MarkerEnter( true, MarkerName, transform );

			var material = GetComponent<Renderer>().material;
			_saveColor = material.color;
			if( _ease.HighlightMarkers ) {
				material.color = _ease.HighlightColor;
			}
		}

		public void OnLookEnd( RaycastHit hit, string name ) {
			if( ! _looking ) return;
			_looking = false;

			//Debug.Log( "Ease Marker Out <" + name + ">" );

			EaseEvent.MarkerEnter( false, MarkerName, transform );

			if( _ease.HighlightMarkers ) {
				GetComponent<Renderer>().material.color = _saveColor;
			}
		}

		private string MarkerTimeStampJson( string eventName, float distance ) {
			return string.Format(
				@"{{""event"":""{0}"",""uuid"":""{1}"",""timestamp"":""{2}"",""name"":""{3}"",""distance"":{4}}}",
				eventName,
				SystemInfo.deviceUniqueIdentifier,
				System.DateTime.Now.ToString("yyyyMMddHHmmssffff"),
				name,
				distance
			);
		}
	}

}
