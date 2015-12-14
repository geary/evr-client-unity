/**
 * Ease Analytics Plugin for Unity
 * Copyright (c) 2014-2015 by Ease VR, Inc. All Rights Reserved.
 * Licensed under the terms of the Apache Public License
 * Please see the LICENSE included with this distribution for details.
 */
 
// PushID.cs
// Based on generate-pushid.js:
// https://gist.github.com/mikelehen/3596a30bd69384624c11
//
// Fancy ID generator that creates 20-character string identifiers with the following properties:
//
// 1. They're based on timestamp so that they sort *after* any existing ids.
// 2. They contain 72-bits of random data after the timestamp so that IDs won't collide with other clients' IDs.
// 3. They sort *lexicographically* (so the timestamp is converted to characters that will sort properly).
// 4. They're monotonically increasing.  Even if you generate more than one in the same timestamp, the
//    latter ones will sort after the former ones.  We do this by using the previous random bits
//    but "incrementing" them by 1 (only in the case of a timestamp collision).

using System;
using System.Globalization;

namespace EaseAnalytics {

	static class PushID {

		// Modeled after base64 web-safe chars, but ordered by ASCII.
		private const string PUSH_CHARS =
			"-0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz";

		// Timestamp of last push, used to prevent local collisions if you push twice in one ms.
		private static double lastPushTime = 0;

		// We generate 72-bits of randomness which get turned into 12 characters and appended to the
		// timestamp to prevent collisions with other clients.  We store the last characters we
		// generated because in the event of a collision, we'll use those same characters except
		// "incremented" by one.
		private static int[] lastRandChars = new int[12];

		// The beginning of time for Unix or JavaScript
		private static readonly DateTime Epoch =
			new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );
	
		// Random number generator
		private static Random random = new Random();

		public static string Generate() {
		
			var now = JavaScriptTime();
			var duplicateTime = ( now == lastPushTime );
			lastPushTime = now;

			var timeStampChars = new char[8];
			for( var i = 7;  i >= 0;  i-- ) {
				timeStampChars[i] = PUSH_CHARS[ (int)( now % 64 ) ];
				// NOTE: Can't use << here because javascript will convert to int and lose the upper bits.
				now = Math.Floor( now / 64 );
			}
			if( now != 0 ) throw new Exception( "We should have converted the entire timestamp." );

			var id = new string( timeStampChars );

			if( ! duplicateTime ) {
				for( var i = 0;  i < 12;  i++ ) {
					lastRandChars[i] = random.Next( 64 );
				}
			} else {
				// If the timestamp hasn't changed since last push, use the same random number, except incremented by 1.
				var i = 11;
				for( ;  i >= 0  &&  lastRandChars[i] == 63;  i-- ) {
					lastRandChars[i] = 0;
				}
				lastRandChars[i]++;
			}
			for( var i = 0;  i < 12;  i++ ) {
				id += PUSH_CHARS[ lastRandChars[i] ];
			}
			if( id.Length != 20 ) throw new Exception( "Length should be 20." );

			return id;
		}

		static double JavaScriptTime() {
			return DateTime.UtcNow.Subtract(Epoch).TotalMilliseconds;
		}
	}

}
