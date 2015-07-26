using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;

public class Ease : MonoBehaviour {
	void Awake() {
		Debug.Log( "Ease Analytics Initializing..." );
	}

	// Use this for initialization
	void Start() {
		Debug.Log( SystemInfo.systemMemorySize );

		EaseEvent easeEvent = new EaseEvent();
		easeEvent.send( "session_start", "{\"uuid\": \"" + SystemInfo.deviceUniqueIdentifier + "\", \"timestamp\": \"" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + "\"}" );
	}

	// Update is called once per frame
	void Update() {
	}

	void sentUpdate() {
	}

	void OnApplicationQuit() {
		Debug.Log( "Session has ended..." );

		EaseEvent easeEvent = new EaseEvent();
		easeEvent.send( "session_end", "{\"uuid\": \"" + SystemInfo.deviceUniqueIdentifier + "\", \"timestamp\": \"" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + "\"}" );
	}
}
