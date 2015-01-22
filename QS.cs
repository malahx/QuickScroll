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
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace QuickScroll {

	public class QS : MonoBehaviour {

		public readonly static string VERSION = "1.10";
		public readonly static string MOD = "QuickScroll";
		private static bool isdebug = true;

		// Afficher les messages sur la console
		internal static void Log(string String) {
			if (isdebug) {
				Debug.Log (MOD + "(" + VERSION + "): " + String);
			}
		}
	}

	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public class QuickScroll : QS {

		private static Rect[] ScrollRect_Simple = {
			new Rect (0, 25, 30, Screen.height), // Categories
			new Rect (40, 25, 260, Screen.height) // Parts
		};
		private static Rect[] ScrollRect_Advanced = {
			new Rect (0, 25, 30, Screen.height), // Filters
			new Rect (35, 25, 65, Screen.height), // Categories
			new Rect (70, 25, 295, Screen.height) // Parts
		};

		// Démarrage du plugin
		private void Awake() {
			Settings.Instance.Load ();
			if (HighLogic.LoadedSceneIsEditor) {
				Cat.PartListTooltipsTWEAK (false);
			}
		}

		// Attendre les touches
		private void Update() {
			if (HighLogic.LoadedSceneIsEditor) {
				if (EditorLogic.fetch.editorScreen == EditorScreen.Parts) {

					// Wheel Scroll
					if (Settings.Instance.EnableWheelScroll) {
						if (EditorPanels.Instance.IsMouseOver ()) {
							float _scroll = Input.GetAxis ("Mouse ScrollWheel");
							if (_scroll != 0) {
								Vector2 _mouse = Mouse.screenPos;
								bool _ModKeyFilterWheel = false;
								bool _ModKeyCategoryWheel = false;
								if (Settings.Instance.EnableWheelShortCut) {
									_ModKeyFilterWheel = Input.GetKey (Settings.Instance.ModKeyFilterWheel);
									_ModKeyCategoryWheel = Input.GetKey (Settings.Instance.ModKeyCategoryWheel);
								}
								bool _ModKeyWheel = _ModKeyFilterWheel || _ModKeyCategoryWheel;
								if (EditorLogic.Mode == EditorLogic.EditorModes.SIMPLE) {
									if ((_ModKeyWheel && ScrollRect_Simple [0].Contains (_mouse)) || (_ModKeyFilterWheel && ScrollRect_Simple [1].Contains (_mouse))) {
										PartCategorizer.Instance.SetAdvancedMode ();
										Cat.SelectPartFilter (_scroll);
									} else if (ScrollRect_Simple [0].Contains (_mouse) || (_ModKeyCategoryWheel && ScrollRect_Simple [1].Contains (_mouse))) {
										Cat.SelectPartCategory (_scroll);
									} else if (ScrollRect_Simple [1].Contains (_mouse)) {
										Cat.SelectPartPage (_scroll);
									}
								} else {
									if (ScrollRect_Advanced [0].Contains (_mouse) || (_ModKeyWheel && ScrollRect_Advanced [1].Contains (_mouse)) || (_ModKeyFilterWheel && ScrollRect_Advanced [2].Contains (_mouse))) {
										Cat.SelectPartFilter (_scroll);
									} else if (ScrollRect_Advanced [1].Contains (_mouse) || (_ModKeyCategoryWheel && ScrollRect_Advanced [2].Contains (_mouse))) {
										Cat.SelectPartCategory (_scroll);
									} else if (ScrollRect_Advanced [2].Contains (_mouse)) {
										Cat.SelectPartPage (_scroll);
									}
								}
							}
						}
					}

					// Keyboard shortcut
					if (Settings.Instance.EnableKeyShortCut) {
						bool _ModKey = (Settings.Instance.ModKeyShortCut == "None" ? true : Input.GetKey (Settings.Instance.ModKeyShortCut));
						if (_ModKey) {
							if (Input.GetKeyUp (Settings.Instance.KeyFilterPrevious)) {
								PartCategorizer.Instance.SetAdvancedMode ();
								Cat.SelectPartFilter (1);
							}
							if (Input.GetKeyUp (Settings.Instance.KeyFilterNext)) {
								PartCategorizer.Instance.SetAdvancedMode ();
								Cat.SelectPartFilter (-1);
							}
							if (Input.GetKeyUp (Settings.Instance.KeyCategoryPrevious)) {
								Cat.SelectPartCategory (1);
							}
							if (Input.GetKeyUp (Settings.Instance.KeyCategoryNext)) {
								Cat.SelectPartCategory (-1);
							}
							if (Input.GetKeyUp (Settings.Instance.KeyPagePrevious)) {
								Cat.SelectPartPage (1);
							}
							if (Input.GetKeyUp (Settings.Instance.KeyPageNext)) {
								Cat.SelectPartPage (-1);
							}
							if (Input.GetKeyUp (Settings.Instance.KeyPods)) {
								Cat.ForceSelectTab (0);
							}
							if (Input.GetKeyUp (Settings.Instance.KeyFuelTanks)) {
								Cat.ForceSelectTab (1);
							}
							if (Input.GetKeyUp (Settings.Instance.KeyEngines)) {
								Cat.ForceSelectTab (2);
							}
							if (Input.GetKeyUp (Settings.Instance.KeyCommandNControl)) {
								Cat.ForceSelectTab (3);
							}
							if (Input.GetKeyUp (Settings.Instance.KeyStructural)) {
								Cat.ForceSelectTab (4);
							}
							if (Input.GetKeyUp (Settings.Instance.KeyAerodynamics)) {
								Cat.ForceSelectTab (5);
							}
							if (Input.GetKeyUp (Settings.Instance.KeyUtility)) {
								Cat.ForceSelectTab (6);
							}
							if (Input.GetKeyUp (Settings.Instance.KeySciences)) {
								Cat.ForceSelectTab (7);
							}
						}
					}

					// TWEAKPartListTooltips
					if (Settings.Instance.EnableTWEAKPartListTooltips) {
						if (Input.GetKeyUp (Settings.Instance.KeyPartListTooltipsActivate)) {
							PartListTooltips.fetch.enabled = true;
						}
						if (Input.GetKeyUp (Settings.Instance.KeyPartListTooltipsDisactivate)) {
							Cat.PartListTooltipsTWEAK(false);
						}
					}
				}
			}
		}
	}
}