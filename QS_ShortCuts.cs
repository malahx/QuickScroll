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
using UnityEngine;

namespace QuickScroll {
	public class QShortCuts : QKey {

		private static Rect[] ScrollRect_Simple = {
			new Rect (0, 25, 30, Screen.height), // Categories
			new Rect (40, 25, 260, Screen.height) // Parts
		};
		private static Rect[] ScrollRect_Advanced = {
			new Rect (0, 25, 30, Screen.height), // Filters
			new Rect (35, 25, 65, Screen.height), // Categories
			new Rect (70, 25, 295, Screen.height) // Parts
		};

		internal static void Awake() {
			if (HighLogic.LoadedSceneIsGame) {
				RectSetKey = new Rect ((Screen.width - 300)/2, (Screen.height - 100)/2, 300, 100);
				string[] _keys = Enum.GetNames (typeof(Key));
				int _length = _keys.Length;
				for (int _key = 1; _key < _length; _key++) {
					Key _GetKey = (Key)_key;
					SetCurrentKey (_GetKey, DefaultKey (_GetKey));
				}
			}
		}

		internal static void Update() {
			if (HighLogic.LoadedSceneIsEditor) {
				if (SetKey != Key.None) {
					KeyCode _key = GetKeyPressed ();
					if (_key != KeyCode.None) {
						SetCurrentKey (SetKey, _key);
						QSettings.Instance.Save ();
						SetKey = Key.None;
						QGUI.WindowSettings = true;
					}
					return;
				}
				if (EditorLogic.fetch.editorScreen == EditorScreen.Parts) {
		
					// Wheel Scroll
					if (QSettings.Instance.EnableWheelScroll) {
						if (EditorPanels.Instance.IsMouseOver ()) {
							float _scroll = Input.GetAxis ("Mouse ScrollWheel");
							if (_scroll != 0) {
								Vector2 _mouse = Mouse.screenPos;
								bool _ModKeyFilterWheel = false;
								bool _ModKeyCategoryWheel = false;
								if (QSettings.Instance.EnableWheelShortCut) {
									_ModKeyFilterWheel = Input.GetKey (QSettings.Instance.ModKeyFilterWheel);
									_ModKeyCategoryWheel = Input.GetKey (QSettings.Instance.ModKeyCategoryWheel);
								}
								bool _ModKeyWheel = _ModKeyFilterWheel || _ModKeyCategoryWheel;
								if (EditorLogic.Mode == EditorLogic.EditorModes.SIMPLE) {
									if ((_ModKeyWheel && ScrollRect_Simple [0].Contains (_mouse)) || (_ModKeyFilterWheel && ScrollRect_Simple [1].Contains (_mouse))) {
										PartCategorizer.Instance.SetAdvancedMode ();
										QCategory.SelectPartFilter (_scroll);
									} else if (ScrollRect_Simple [0].Contains (_mouse) || (_ModKeyCategoryWheel && ScrollRect_Simple [1].Contains (_mouse))) {
										QCategory.SelectPartCategory (_scroll);
									} else if (ScrollRect_Simple [1].Contains (_mouse)) {
										QCategory.SelectPartPage (_scroll);
									}
								} else {
									if (ScrollRect_Advanced [0].Contains (_mouse) || (_ModKeyWheel && ScrollRect_Advanced [1].Contains (_mouse)) || (_ModKeyFilterWheel && ScrollRect_Advanced [2].Contains (_mouse))) {
										QCategory.SelectPartFilter (_scroll);
									} else if (ScrollRect_Advanced [1].Contains (_mouse) || (_ModKeyCategoryWheel && ScrollRect_Advanced [2].Contains (_mouse))) {
										QCategory.SelectPartCategory (_scroll);
									} else if (ScrollRect_Advanced [2].Contains (_mouse)) {
										QCategory.SelectPartPage (_scroll);
									}
								}
							}
						}
					}

					// Keyboard shortcut
					if (QSettings.Instance.EnableKeyShortCut) {
						bool _ModKey = (QSettings.Instance.ModKeyShortCut == KeyCode.None ? true : Input.GetKey (QSettings.Instance.ModKeyShortCut));
						if (_ModKey) {
							if (Input.GetKeyDown (QSettings.Instance.KeyFilterPrevious)) {
								PartCategorizer.Instance.SetAdvancedMode ();
								QCategory.SelectPartFilter (1);
							}
							if (Input.GetKeyDown (QSettings.Instance.KeyFilterNext)) {
								PartCategorizer.Instance.SetAdvancedMode ();
								QCategory.SelectPartFilter (-1);
							}
							if (Input.GetKeyDown (QSettings.Instance.KeyCategoryPrevious)) {
								QCategory.SelectPartCategory (1);
							}
							if (Input.GetKeyDown (QSettings.Instance.KeyCategoryNext)) {
								QCategory.SelectPartCategory (-1);
							}
							if (Input.GetKeyDown (QSettings.Instance.KeyPagePrevious)) {
								QCategory.SelectPartPage (1);
							}
							if (Input.GetKeyDown (QSettings.Instance.KeyPageNext)) {
								QCategory.SelectPartPage (-1);
							}
							if (Input.GetKeyDown (QSettings.Instance.KeyPods)) {
								QCategory.ForceSelectTab (0);
							}
							if (Input.GetKeyDown (QSettings.Instance.KeyFuelTanks)) {
								QCategory.ForceSelectTab (1);
							}
							if (Input.GetKeyDown (QSettings.Instance.KeyEngines)) {
								QCategory.ForceSelectTab (2);
							}
							if (Input.GetKeyDown (QSettings.Instance.KeyCommandNControl)) {
								QCategory.ForceSelectTab (3);
							}
							if (Input.GetKeyDown (QSettings.Instance.KeyStructural)) {
								QCategory.ForceSelectTab (4);
							}
							if (Input.GetKeyDown (QSettings.Instance.KeyAerodynamics)) {
								QCategory.ForceSelectTab (5);
							}
							if (Input.GetKeyDown (QSettings.Instance.KeyUtility)) {
								QCategory.ForceSelectTab (6);
							}
							if (Input.GetKeyDown (QSettings.Instance.KeySciences)) {
								QCategory.ForceSelectTab (7);
							}
						}
					}
				}
			}
		}

		internal static void OnGUI() {
			if (HighLogic.LoadedSceneIsEditor) {
				if (SetKey != Key.None) {
					RectSetKey = GUILayout.Window (1545146, RectSetKey, DrawSetKey, string.Format ("Set Key: {0}", GetText (SetKey)), GUILayout.Width (RectSetKey.width), GUILayout.ExpandHeight (true));
				}
			}
		}
	}
}