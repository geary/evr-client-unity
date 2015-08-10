using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class Ease : MonoBehaviour {

	public bool LogEvents = true;

	void Awake() {
		//Debug.Log( "Ease Analytics Initializing..." );
		EaseEvent.SessionID =
			System.DateTime.Now.ToString( "yyyyMMddHHmmssffff" );
	}

	void Start() {
		//Debug.Log( SystemInfo.systemMemorySize );
		EaseEvent.SessionBegin();
	}

	void Update() {
	}

	void SentUpdate() {
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
