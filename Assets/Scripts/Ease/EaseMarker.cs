using UnityEngine;


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

	private bool _looking;

	void Start() {
		if( MarkerName.Length == 0 ) {
			MarkerName = GetInstanceID().ToString();
		}
		EaseEvent.SendEvent( MarkerTimeStampJson("add_marker") );
	}

	void Update() {
	}

	public void OnLookStart( string name ) {
		//Dictionary< string, string > eventData;

		if( _looking ) return;
		_looking = true;

		//Debug.Log( "Ease Marker In <" + name + ">" );

		EaseEvent.SendEvent( MarkerTimeStampJson("marker_in") );

		GetComponent<Renderer>().material.color = Color.red;
	}

	public void OnLookEnd( string name ) {
		if( ! _looking ) return;
		_looking = false;

		//Debug.Log( "Ease Marker Out <" + name + ">" );

		EaseEvent.SendEvent( MarkerTimeStampJson("marker_out") );

		GetComponent<Renderer>().material.color = Color.gray;
	}

	private string MarkerTimeStampJson( string eventName ) {
		return string.Format(
			@"{{""event"":""{0}"",""uuid"":""{1}"",""timestamp"":""{2}"",""name"":""{3}""}}",
			eventName,
			SystemInfo.deviceUniqueIdentifier,
			System.DateTime.Now.ToString("yyyyMMddHHmmssffff"),
			name
		);
	}
}
