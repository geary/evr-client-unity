using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class EaseEvent {
	private const bool DEBUG = true;

	public void Send( string type, string data ) {
		Debug.Log( string.Format( "Sending Event {0}:\n{1}", type, data ) );

		var headers = new Dictionary< string, string > {
			{ "Content-Type", "application/json" },
			{ "event_type", type },
			{ "event_data", data }
		};

		var postData = "_";
		var bytes = Encoding.ASCII.GetBytes( postData.ToCharArray() );

		var url =  DEBUG ?
			"http://127.0.0.1:8080/api/v1/record_event" :
			"http://ease-solarvr.rhcloud.com/api/v1/record_event";
		var www = new WWW( url, bytes, headers );
	}
}
