using UnityEngine;

public class Ease : MonoBehaviour {
	void Awake() {
		Debug.Log( "Ease Analytics Initializing..." );
	}

	// Use this for initialization
	void Start() {
		Debug.Log( SystemInfo.systemMemorySize );

		var easeEvent = new EaseEvent();
		easeEvent.Send( "session_start", TimeStampJson() );
	}

	// Update is called once per frame
	void Update() {
	}

	void SentUpdate() {
	}

	void OnApplicationQuit() {
		Debug.Log( "Session has ended..." );

		var easeEvent = new EaseEvent();
		easeEvent.Send( "session_end", TimeStampJson() );
	}

	private string TimeStampJson() {
		// TODO: use JSON serializer
		return
			"{\"uuid\": \"" +
			SystemInfo.deviceUniqueIdentifier +
			"\", \"timestamp\": \"" +
			System.DateTime.Now.ToString("yyyyMMddHHmmssffff") +
			"\"}";
	}
}
