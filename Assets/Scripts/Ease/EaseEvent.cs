using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;


public class EaseEvent {

	public static string SessionID;

	private static bool _debug = true;
	private static string _apiUrl = _debug ?
		"http://mikebook:8080/v1" :
		"http://api.easevr.com/v1";

	private static readonly DateTime JavaScriptEpoch =
		new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );

	public static void SessionBegin() {
#if false
		var form = new WWWForm();
		form.AddField( "api_key", "TODO" );
		form.AddField( "sid", SessionID );
		form.AddField( "ts", System.DateTime.Now.ToString("yyyyMMddHHmmssffff") );
		form.AddField( "udid", SystemInfo.deviceUniqueIdentifier );
		form.AddField( "hmd", "TODO" );
		form.AddField( "ver", "TODO" );
		form.AddField( "os", "TODO" );
		form.AddField( "os_ver", "TODO" );
		form.AddField( "mem", 99999 );
		form.AddField( "proc", "TODO" );
		ApiPost( "session_start", form );
#else
		ApiPost( "session_start", string.Format(
			@"udid={0}&hmd={1}&ver={2}&os={3}&os_ver={4}&mem={5}&proc={6}",
			SystemInfo.deviceUniqueIdentifier,
			"TODO",
			"TODO",
			"TODO",
			"TODO",
			"9999",
			"TODO"
		));
#endif
	}

	public static void SessionEnd() {
		ApiPost( "session_end", "" );
		System.Threading.Thread.Sleep( 1000 );  // TODO: TEMP HACK
	}

	public static void MarkerRegister( bool register, string name, Transform transform ) {
		ApiPost( register ? "marker_register" : "marker_deregister", string.Format(
			@"name={0}&pos={1}",
			name,
			GetXYZ( transform.position )
		));
	}

	// TODO: This is basically the same as MarkerRegister right now, but may change
	public static void MarkerEnter( bool enter, string name, Transform transform ) {
		ApiPost( enter ? "marker_enter" : "marker_exit", string.Format(
			@"name={0}&pos={1}",
			name,
			GetXYZ( transform.position )
		));
	}

	public static void Presence( Transform transform ) {
		ApiPost( "presence", string.Format(
			@"pos={0}&rot={1}&fps={2}&m_use={3}",
			GetXYZ( transform.position ),
			GetXYZ( transform.eulerAngles ),
			0,  // TODO
			0  // TODO
		));
	}

	//public static void SendEvent( string json ) {
	//	if( SessionID == "" ) {
	//		Debug.LogError( "SendEvent with no SessionID" );
	//		return;
	//	}

	//	Debug.Log( "POST/" + SessionID + ": " + json );
	//	FirebaseSend( "POST", "events/" + SessionID, json );
	//}

#if false
	private static void ApiPost(
		string endpoint,
		WWWForm form
	) {
		var url = _apiUrl + "GUID_TODO" + "/" + endpoint;
		
		var www = new WWW( url, form );
		StartCoroutine
		yield return www;
		var text = www.text;
		Debug.Log( text );
	}
#else
	private static void ApiPost(
		string endpoint,
		string parameters
	) {
		var postData = string.Format(
			@"api_key={0}&sid={1}&ts={2}",
			"mike_TODO",
			SessionID,
			TimeStamp()
		);

		if( parameters != "" ) {
			postData += "&" + parameters;
		}

		var url = string.Format(
			@"{0}/client/{1}/{2}",
			_apiUrl,
			"GUID_TODO",
			endpoint
		);
		
		var www = new WWW( url, Encoding.UTF8.GetBytes(postData) );
		//StartCoroutine
		//yield return www;
		//var text = www.text;
		//Debug.Log( text );
	}
#endif

	private static IEnumerator ApiPostRoutine(
		string endpoint,
		WWWForm form
	) {
		var url = _apiUrl + "GUID_TODO" + "/" + endpoint;
		var www = new WWW( url, form );
		yield return www;
		var text = www.text;
		Debug.Log( text );
	}

	//public static string PositionJson(
	//	float posX, float posY, float posZ,
	//	float rotX, float rotY, float rotZ
	//) {
	//	return string.Format(
	//		@"{{""event"":""position"",""uuid"":""{0}"",""timestamp"":""{1}"",""posX"":{2},""posY"":{3},""posZ"":{4},""rotX"":{5},""rotY"":{6},""rotZ"":{7} }}",
	//		SystemInfo.deviceUniqueIdentifier,
	//		System.DateTime.Now.ToString("yyyyMMddHHmmssffff"),
	//		posX, posY, posZ,
	//		rotX, rotY, rotZ
	//	);
	//}

	private static string TimeStamp() {
		return DateTime.UtcNow
			.Subtract(JavaScriptEpoch).TotalMilliseconds
			.ToString( "F0", CultureInfo.InvariantCulture );
	}

	private static string GetXYZ( Vector3 vec ) {
		return string.Format(
			@"{{""x"":{0},""y"":{0},""z"":{0}}}",
			vec.x,
			vec.y,
			vec.z
		);
	}
}
