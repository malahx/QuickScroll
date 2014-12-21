/* 
QuickScroll
Copyright 2014 Malah

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
using System.Collections.Generic;
using UnityEngine;

namespace QuickScroll {
	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public class QuickScroll : MonoBehaviour {
		public static string VERSION = "0.10";
		public static string MOD = "QuickScroll";
		private static Rect[] ScrollRect_Simple = {
			new Rect (0, 25, 30, Screen.height), // Categories
			new Rect (40, 25, 260, Screen.height) // Parts
		};
		private static Rect[] ScrollRect_Advanced = {
			new Rect (0, 25, 30, Screen.height), // Filters
			new Rect (35, 25, 65, Screen.height), // Categories
			new Rect (70, 25, 295, Screen.height) // Parts
		};

		private void SelectPartPage(float scroll) {
			if (scroll > 0)
				EditorPartList.Instance.PrevPage ();
			if (scroll < 0)
				EditorPartList.Instance.NextPage ();
		}

		private void Update() {
			if (HighLogic.LoadedSceneIsEditor) {
				if (EditorLogic.fetch.editorScreen == EditorScreen.Parts) {
					if (EditorPanels.Instance.IsMouseOver ()) {
						float _scroll = Input.GetAxis ("Mouse ScrollWheel");
						if (_scroll != 0) {
							Vector2 _mouse = Mouse.screenPos;
							if (EditorLogic.Mode == EditorLogic.EditorModes.SIMPLE) {
								if (ScrollRect_Simple [1].Contains (_mouse))
									SelectPartPage (_scroll);
							} else {
								if (ScrollRect_Advanced [2].Contains (_mouse))
									SelectPartPage (_scroll);
							}
						}
					}
				}
			}
		}
	}
}