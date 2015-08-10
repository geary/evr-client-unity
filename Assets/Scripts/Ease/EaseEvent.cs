using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class EaseEvent {

	public static string SessionID;

	private static bool _debug = true;
	private static string _firebaseUrl = _debug ?
		"https://resplendent-fire-8993.firebaseio.com/" :
		"https://incandescent-inferno-587.firebaseio.com/";
	private static float _posX;
	private static float _posY;
	private static float _posZ;
	private static float _rotX;
	private static float _rotY;
	private static float _rotZ;

	public static void SessionBegin() {
		var json = Ease.TimeStampJson("session_begin");
		FirebaseSend( "POST", "sessions", json );
		SendEvent( json );
	}

	public static void SessionEnd() {
		var json = Ease.TimeStampJson("session_end");
		SendEvent( json );
	}

	public static void Position( Transform transform ) {
		// TODO: This is terrible code! Isn't there an efficient library function for this?
		var posX = transform.position.x;
		var posY = transform.position.y;
		var posZ = transform.position.z;
		var rotX = transform.eulerAngles.x;
		var rotY = transform.eulerAngles.y;
		var rotZ = transform.eulerAngles.z;

		if(
			posX == _posX  &&
			posY == _posY  &&
			posZ == _posZ  &&
			rotX == _rotX  &&
			rotY == _rotY  &&
			rotZ == _rotZ
		) {
			return;
		}
		_posX = posX;
		_posY = posY;
		_posZ = posZ;
		_rotX = rotX;
		_rotY = rotY;
		_rotZ = rotZ;

		var json = PositionJson( posX, posY, posZ, rotX, rotY, rotZ );
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
			"{0}unity/{1}.json{2}",
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

	public static string PositionJson(
		float posX, float posY, float posZ,
		float rotX, float rotY, float rotZ
	) {
		return string.Format(
			@"{{""event"":""position"",""uuid"":""{0}"",""timestamp"":""{1}"",""posX"":{2},""posY"":{3},""posZ"":{4},""rotX"":{5},""rotY"":{6},""rotZ"":{7} }}",
			SystemInfo.deviceUniqueIdentifier,
			System.DateTime.Now.ToString("yyyyMMddHHmmssffff"),
			posX, posY, posZ,
			rotX, rotY, rotZ
		);
	}
}
