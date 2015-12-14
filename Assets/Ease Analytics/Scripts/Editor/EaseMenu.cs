/**
 * Ease Analytics Plugin for Unity
 * Copyright (c) 2014-2015 by Ease VR, Inc. All Rights Reserved.
 * Licensed under the terms of the Apache Public License
 * Please see the LICENSE included with this distribution for details.
 */
 
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EaseAnalytics {

	public class EaseMenu : MonoBehaviour {

		private static string _easeObjName = "EaseAnalytics";
		private static string _easeLookObjName = "EaseLook";

		[MenuItem( "Ease Analytics/Setup", false, 1 )]
		public static void Setup() {
			AddSettings();
			AddLook();
		}

		[MenuItem( "Ease Analytics/Build Ease Analytics Unity Package", false, 51 )]
		public static void BuildUnityPackage() {
			Directory.CreateDirectory( "Build" );
			AssetDatabase.ExportPackage(
				"Assets/EaseAnalytics",
				"Build/EaseAnalytics.unitypackage",
				ExportPackageOptions.Recurse
			);
		}

		private static void AddSettings() {
			var objEase = GameObject.Find( _easeObjName );
			if( objEase != null ) {
				GameObject.DestroyImmediate( objEase );
			}
			objEase = new GameObject( _easeObjName );
			objEase.AddComponent<EaseSettings>();
			Selection.activeGameObject = objEase;
		}

		private static void AddLook() {
			var camera = Camera.main;
			if( camera == null ) {
				return;  // TODO;
			}
			var objCamera = camera.gameObject;
			if( objCamera == null ) {
				return;  // TODO
			}
			var objEaseLook = GameObject.Find( _easeLookObjName );
			if( objEaseLook ) {
				GameObject.DestroyImmediate( objEaseLook );
			}
			objEaseLook = new GameObject( _easeLookObjName );
			objEaseLook.AddComponent<EaseLook>();
			objEaseLook.transform.parent = objCamera.transform;
		}

	}

}
