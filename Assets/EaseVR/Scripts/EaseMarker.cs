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

	private bool _looking;

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

		GetComponent<Renderer>().material.color = Color.red;
	}

	public void OnLookEnd( RaycastHit hit, string name ) {
		if( ! _looking ) return;
		_looking = false;

		//Debug.Log( "Ease Marker Out <" + name + ">" );

		EaseEvent.MarkerEnter( false, MarkerName, transform );

		GetComponent<Renderer>().material.color = Color.gray;
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
