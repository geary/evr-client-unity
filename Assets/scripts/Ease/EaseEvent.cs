using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class EaseEvent {
	private bool DEBUG = false;

	public void send(string type, string data) {
		Debug.Log("Sending Events: " + type);

		string ourPostData = "_";
		
		var headers = new Dictionary<string, string>();

		headers.Add("Content-Type", "application/json");
		
		byte[] pData = Encoding.ASCII.GetBytes(ourPostData.ToCharArray());

		headers.Add("event_type", type);
		headers.Add("event_data", data);

		WWW www = new WWW((DEBUG) ? "http://127.0.0.1:8080/api/v1/record_event" : "http://ease-solarvr.rhcloud.com/api/v1/record_event", pData, headers);
	}
}
