/**
 * Ease Analytics Plugin for Unity
 * Copyright (c) 2014-2015 by Ease VR, Inc. All Rights Reserved.
 * Licensed under the terms of the Apache Public License
 * Please see the LICENSE included with this distribution for details.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.VR;

namespace EaseAnalytics {

	public class EaseEvent {

		public static string SessionID;

		private static EaseSettings _ease;

		private static bool _debug = false;
		private static string _apiUrl = _debug ?
			"http://localhost:8080/v1" :
			"http://api.easevr.com/v1";

		private static readonly DateTime JavaScriptEpoch =
			new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );

		private static readonly double JavaScriptStartTimeMilliseconds =
			Math.Floor(
				DateTime.UtcNow.Subtract( JavaScriptEpoch ).TotalMilliseconds
			);

		private static List<string> _events = new List<string>();

		public static void SessionStart() {
			AddEvent( "ST",
				DeTab( SystemInfo.deviceUniqueIdentifier ),
				VRDevice.family,
				VRDevice.model,
				DeTab( SystemInfo.operatingSystem ),
				DeTab( SystemInfo.processorType ),
				SystemInfo.processorCount.ToString(),
				SystemInfo.systemMemorySize.ToString(),
				DeTab( SystemInfo.graphicsDeviceName ),
				SystemInfo.graphicsMemorySize.ToString(),
				DeTab( SystemInfo.graphicsDeviceVersion )
			);
			PostEvents();
		}

		public static void SessionEnd() {
			AddEvent( "SE" );
			PostEvents();
			System.Threading.Thread.Sleep( 1000 );  // TODO: TEMP HACK
		}

		public static void MarkerAdd( bool add, string name, Transform transform ) {
			AddEvent( add ? "MA" : "MR",
				DeTab( name ),
				PosStr( transform.position.x ),
				PosStr( transform.position.y ),
				PosStr( transform.position.z )
			);
		}

		// TODO: This is basically the same as MarkerAdd right now, but may change
		public static void MarkerEnter( bool enter, string name, Transform transform ) {
			AddEvent( enter ? "ME" : "MX",
				DeTab( name ),
				PosStr( transform.position.x ),
				PosStr( transform.position.y ),
				PosStr( transform.position.z )
			);
		}

		public static void Presence(
			Transform transform,
			float framesPerSecond,
			uint memoryUsed
		) {
			// #TODO: Calling this event is not ccurrently publically supported and requests will be blocked. fred@easevr.com
			// AddEvent( "P",
			// 	PosStr( transform.position.x ),
			// 	PosStr( transform.position.y ),
			// 	PosStr( transform.position.z ),
			// 	PosStr( transform.eulerAngles.x ),
			// 	PosStr( transform.eulerAngles.y ),
			// 	PosStr( transform.eulerAngles.z ),
			// 	framesPerSecond.ToString( "F1" ),
			// 	memoryUsed.ToString()
			// );
		}

		private static void AddEvent( string type, params string[] values ) {
			_events.Add( string.Format(
				"{0}\t{1}\t{2}",
				type,
				TimeStamp(),
				string.Join( "\t", values )
			));
		}

		public static void PostEvents() {
			if( _ease == null ) {
				_ease = GameObject.Find( "EaseAnalytics" ).GetComponent<EaseSettings>();
			}

			if( _events.Count == 0 ) return;

			var payload = string.Format(
				"H\t {0}\t{1}\t{2}\t{3}\n{4}",
				JavaScriptStartTimeMilliseconds,
				"0",
				_ease.ApiKey,
				SessionID,
				string.Join( "\n", _events.ToArray() )
			);

			_events.Clear();

			var url = string.Format(
				@"{0}/client/{1}/events",
				_apiUrl,
				_ease.ExperienceID
			);
		
			var www = new WWW( url, Encoding.UTF8.GetBytes(payload) );
			//StartCoroutine
			//yield return www;
			//var text = www.text;
			//Debug.Log( text );
		}

		private static double TimeStamp() {
			var ms = DateTime.UtcNow
				.Subtract( JavaScriptEpoch )
				.TotalMilliseconds;
		
			return Math.Floor( ms - JavaScriptStartTimeMilliseconds );
		}

		private static string DeTab( string str ) {
			return str.Replace( "\t", " " );
		}

		private static string PosStr( float x ) {
			return x.ToString();  // TODO: precision
		}
	}

}
