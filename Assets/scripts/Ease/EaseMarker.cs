using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof(Collider))]

public class EaseMarker : MonoBehaviour {
	private bool debug;

	public string markerName;
	public string objectName;
	public bool   isObject;
	public bool   inside;
	public bool   daytime;
	public string customProp01;
	public string customProp02;
	public string customProp03;

	private bool looking;

	// Use this for initialization
	void Start () {
		if (this.markerName.Length == 0) {
			this.markerName = this.GetInstanceID().ToString();
		}
	}
	
	// Update is called once per frame
	void Update () {}

	public void onLookStart(string name) {
		EaseEvent easeEvent;
		Dictionary<string, string> eventData;


		if (looking == false) {
			Debug.Log ("Ease Marker In <" + name + ">");

			easeEvent = new EaseEvent();
			easeEvent.send("marker_in", "{\"uuid\": \"" + SystemInfo.deviceUniqueIdentifier + "\", \"timestamp\": \"" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + "\", \"marker_name\": \"" + name + "\"}");

			looking = true;

			this.renderer.material.color = Color.red;
		}
	}

	public void onLookEnd(string name) {
		EaseEvent easeEvent;

		if (looking == true) {
			Debug.Log ("Ease Marker Out <" + name + ">");

			easeEvent = new EaseEvent();
			easeEvent.send("marker_out", "{\"uuid\": \"" + SystemInfo.deviceUniqueIdentifier + "\", \"timestamp\": \"" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + "\", \"marker_name\": \"" + name + "\"}");

			looking = false;

			this.renderer.material.color = Color.gray;
		}
	}
}
