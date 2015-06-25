/* 
QuickScroll
Copyright 2015 Malah

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>. 
*/

using System;
using System.Reflection;
using UnityEngine;

namespace QuickScroll {
	public partial class QuickScroll : MonoBehaviour {

		public readonly static string VERSION = Assembly.GetAssembly(typeof(QuickScroll)).GetName().Version.Major + "." + Assembly.GetAssembly(typeof(QuickScroll)).GetName().Version.Minor + Assembly.GetAssembly(typeof(QuickScroll)).GetName().Version.Build;
		public readonly static string MOD = Assembly.GetAssembly(typeof(QuickScroll)).GetName().Name;

		internal static void Log(string _string, bool debug = false) {
			if (!debug) {
				Debug.Log (MOD + "(" + VERSION + "): " + _string);
			} else {
				#if DEBUG
				Debug.Log (MOD + "(" + VERSION + "): " + _string);
				#endif
			}
		}
		internal static void Warning(string _string, bool debug = false) {
			if (!debug) {
				Debug.LogWarning (MOD + "(" + VERSION + "): " + _string);
			} else {
				#if DEBUG
				Debug.LogWarning (MOD + "(" + VERSION + "): " + _string);
				#endif
			}
		}
	}
}