using System.Collections;
using UnityEditor;
using UnityEngine;

namespace EaseVR {

	public class EaseMenu : MonoBehaviour {

		private static string _easeObjName = "EaseVR";
		private static string _easeLookObjName = "EaseLook";

		[MenuItem( "Ease/Setup" )]
		public static void Setup() {
			AddSettings();
			AddLook();
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
