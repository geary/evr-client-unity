using UnityEngine;

public class Ease : MonoBehaviour {
	void Awake() {
		//Debug.Log( "Ease Analytics Initializing..." );
	}

	// Use this for initialization
	void Start() {
		//Debug.Log( SystemInfo.systemMemorySize );

		EaseEvent.Send( "session_start", TimeStampJson() );
	}

	// Update is called once per frame
	void Update() {
	}

	void SentUpdate() {
	}

	void OnApplicationQuit() {
		//Debug.Log( "Ease Analytics exiting..." );

		EaseEvent.Send( "session_end", TimeStampJson() );
	}

	private string TimeStampJson() {
		return string.Format(
			@"{{""uuid"":""{0}"",""timestamp"":""{1}""}}",
			SystemInfo.deviceUniqueIdentifier,
			System.DateTime.Now.ToString("yyyyMMddHHmmssffff")
		);
	}
}
