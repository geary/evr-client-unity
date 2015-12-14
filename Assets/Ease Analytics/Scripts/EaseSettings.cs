/**
 * Ease Analytics Plugin for Unity
 * Copyright (c) 2014-2015 by Ease VR, Inc. All Rights Reserved.
 * Licensed under the terms of the Apache Public License
 * Please see the LICENSE included with this distribution for details.
 */
 
using System.Collections;
using UnityEngine;

namespace EaseAnalytics {

	public class EaseSettings : MonoBehaviour {

		[Header( "Authentication" )]

		[Tooltip( "Your API key from the Ease Analytics Dashboard" )]
		public string ApiKey = "Your-API-Key";
		[Tooltip( "This experience's ID from the Ease Analytics Dashboard" )]
		public string ExperienceID = "Your-Experience-ID";

		[Header( "Timing" )]

		[Tooltip( "How often to record Presence (seconds)" )]
		public float PresenceInterval = 0.1f;
		[Tooltip( "How often to push data to the Ease Analytics server (seconds)" )]
		public float PushInterval = 2.0f;

		[Header( "Markers" )]

		[Tooltip( "Check to select all markers in the line of sight,\nuncheck to select only the nearest marker" )]
		public bool SeeThrough = false;  // TODO: Make this a marker property?

		[Header( "Debug" )]

		[Tooltip( "Log Ease Analytics events to the console?" )]
		public bool LogPresenceEvents = false;
		public bool LogOtherEvents = false;
		[Tooltip( "Highlight active markers for debugging?" )]
		public bool HighlightMarkers = false;
		[Tooltip( "Active marker highlight color\n(if Highlight Markers is checked)" )]
		[ColorUsage( false )]
		public Color HighlightColor = Color.red;

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

	}

}
