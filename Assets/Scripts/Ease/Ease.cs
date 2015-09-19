using System.Collections;
using UnityEngine;

public class Ease : MonoBehaviour {

	public float PushInterval = 2.0f;
	public bool LogEvents = true;

	void Awake() {
		//Debug.Log( "Ease Analytics Initializing..." );
		LogSystemInfo();
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

	private static void LogSystemInfo() {
		Debug.Log( "deviceModel: " + SystemInfo.deviceModel );
		Debug.Log( "deviceName: " + SystemInfo.deviceName );
		Debug.Log( "deviceType: " + SystemInfo.deviceType );
		Debug.Log( "deviceUniqueIdentifier: " + SystemInfo.deviceUniqueIdentifier );
		Debug.Log( "graphicsDeviceID: " + SystemInfo.graphicsDeviceID );
		Debug.Log( "graphicsDeviceName: " + SystemInfo.graphicsDeviceName );
		Debug.Log( "graphicsDeviceType: " + SystemInfo.graphicsDeviceType );
		Debug.Log( "graphicsDeviceVendor: " + SystemInfo.graphicsDeviceVendor );
		Debug.Log( "graphicsDeviceVendorID: " + SystemInfo.graphicsDeviceVendorID );
		Debug.Log( "graphicsDeviceVersion: " + SystemInfo.graphicsDeviceVersion );
		Debug.Log( "graphicsMemorySize: " + SystemInfo.graphicsMemorySize );
		Debug.Log( "graphicsMultiThreaded: " + SystemInfo.graphicsMultiThreaded );
		Debug.Log( "graphicsShaderLevel: " + SystemInfo.graphicsShaderLevel );
		Debug.Log( "operatingSystem: " + SystemInfo.operatingSystem );
		Debug.Log( "processorCount: " + SystemInfo.processorCount );
		Debug.Log( "processorType: " + SystemInfo.processorType );
		Debug.Log( "systemMemorySize: " + SystemInfo.systemMemorySize );
	}
}
