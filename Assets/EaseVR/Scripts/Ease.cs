using System.Collections;
using UnityEngine;

namespace EaseVR {

public class Ease : MonoBehaviour {

	public string ApiKey = "Your-API-Key";
	public string AppGuid = "Your-App-GUID";
	public float PushInterval = 2.0f;
	public bool LogEvents = true;
	public float UpdateInterval = 0.1f;
	public bool SeeThrough = false;  // TODO: Make this a marker property?

	void Awake() {
		//Debug.Log( "Ease Analytics Initializing..." );
		EaseEvent.SessionID = PushID.Generate();
	}

	void Start() {
		//Debug.Log( SystemInfo.systemMemorySize );
		EaseEvent.SessionStart();
		StartCoroutine( PushEvents() );
	}

	IEnumerator PushEvents() {
		for(;;) {
			yield return new WaitForSeconds( PushInterval );
			EaseEvent.PostEvents();
		}
	}

	void Update() {
	}

	void OnApplicationQuit() {
		//Debug.Log( "Ease Analytics exiting..." );
		EaseEvent.SessionEnd();
	}

	public static string TimeStampJson( string eventName ) {
		return string.Format(
			@"{{""event"":""{0}"",""uuid"":""{1}"",""timestamp"":""{2}""}}",
			eventName,
			SystemInfo.deviceUniqueIdentifier,
			System.DateTime.Now.ToString("yyyyMMddHHmmssffff")
		);
	}

}

}
