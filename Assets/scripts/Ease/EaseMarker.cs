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

	// Use this for initialization
	void Start() {
		if( MarkerName.Length == 0 ) {
			MarkerName = GetInstanceID().ToString();
		}
	}

	// Update is called once per frame
	void Update() {
	}

	public void OnLookStart( string name ) {
		//Dictionary< string, string > eventData;

		if( _looking ) return;
		_looking = true;

		Debug.Log( "Ease Marker In <" + name + ">" );

		var easeEvent = new EaseEvent();
		easeEvent.Send( "marker_in", MarkerTimeStampJson() );

		GetComponent<Renderer>().material.color = Color.red;
	}

	public void OnLookEnd( string name ) {
		if( ! _looking ) return;
		_looking = false;

		Debug.Log( "Ease Marker Out <" + name + ">" );

		var easeEvent = new EaseEvent();
		easeEvent.Send( "marker_out", MarkerTimeStampJson() );

		GetComponent<Renderer>().material.color = Color.gray;
	}

	private string MarkerTimeStampJson() {
		return string.Format(
			@"{{""uuid"":""{0}"",""timestamp"":""{1}"",""name"":""{2}""}}",
			SystemInfo.deviceUniqueIdentifier,
			System.DateTime.Now.ToString("yyyyMMddHHmmssffff"),
			name
		);
	}
}
