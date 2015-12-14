/**
 * Ease Analytics Plugin for Unity
 * Copyright (c) 2014-2015 by Ease VR, Inc. All Rights Reserved.
 * Licensed under the terms of the Apache Public License
 * Please see the LICENSE included with this distribution for details.
 */
 
using UnityEngine;


namespace EaseAnalytics {

	[RequireComponent( typeof(Collider) )]
	public class EaseMarker : MonoBehaviour {

		public string MarkerName;
		//public string ObjectName;
		//public bool IsObject;
		//public bool Inside;
		//public bool DayTime;
		//public string CustomProp01;
		//public string CustomProp02;
		//public string CustomProp03;

		private EaseSettings _ease;
		private bool _looking;
		private Color _saveColor;

		void Awake() {
			_ease = GameObject.Find( "EaseAnalytics" ).GetComponent<EaseSettings>();
		}

		void Start() {
			if( MarkerName.Length == 0 ) {
				MarkerName = GetInstanceID().ToString();
			}
			EaseEvent.MarkerAdd( true, MarkerName, transform );
		}

		public void OnLookStart( RaycastHit hit, string name ) {
			//Dictionary< string, string > eventData;

			if( _looking ) return;
			_looking = true;

			//Debug.Log( "Ease Marker In <" + name + ">" );

			EaseEvent.MarkerEnter( true, MarkerName, transform );

			var material = GetComponent<Renderer>().material;
			_saveColor = material.color;
			if( _ease.HighlightMarkers ) {
				material.color = _ease.HighlightColor;
			}
		}

		public void OnLookEnd( RaycastHit hit, string name ) {
			if( ! _looking ) return;
			_looking = false;

			//Debug.Log( "Ease Marker Out <" + name + ">" );

			EaseEvent.MarkerEnter( false, MarkerName, transform );

			if( _ease.HighlightMarkers ) {
				GetComponent<Renderer>().material.color = _saveColor;
			}
		}

	}

}
