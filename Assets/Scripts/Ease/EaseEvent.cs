using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class EaseEvent {

	private static bool _debug = true;
	private static string _firebaseUrl = _debug ?
		"https://resplendent-fire-8993.firebaseio.com/" :
		"";

	public static string SessionID;

	public static void SessionBegin() {
		var json = Ease.TimeStampJson("session_begin");
		FirebaseSend( "POST", "sessions", json );
		SendEvent( json );
	}

	public static void SessionEnd() {
		var json = Ease.TimeStampJson("session_end");
		SendEvent( json );
	}

	public static void SendEvent( string json ) {
		if( SessionID == "" ) {
			Debug.LogError( "SendEvent with no SessionID" );
			return;
		}

		Debug.Log( "POST/" + SessionID + ": " + json );
		FirebaseSend( "POST", "events/" + SessionID, json );
	}

	private static void FirebaseSend(
		string method,
		string path,
		string json
	) {
		var headers = new Dictionary<string, string> {
			{ "X-HTTP-Method-Override", method }
		};

		var url = string.Format(
			"{0}{1}.json{2}",
			_firebaseUrl,
			path,
			"?print=silent"
		);

		Send( url, headers, json );
	}

	private static void Send(
		string url,
		Dictionary< string, string > headers,
		string data
	) {
		var www = new WWW( url, Encoding.UTF8.GetBytes(data), headers );
	}
}
