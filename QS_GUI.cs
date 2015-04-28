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

	public class QGUI {

		internal static bool WindowSettings = false;
		internal static Rect RectSettings = new Rect();

		internal static void Awake() {
			RectSettings = new Rect (0, 0, 615, 214);
			RefreshRect ();
		}

		internal static void RefreshRect() {
			RectSettings.x = (Screen.width - RectSettings.width) / 2;
			RectSettings.y = (Screen.height - RectSettings.height) / 2;
		}

		private static void Lock(bool activate, ControlTypes Ctrl = ControlTypes.None) {
			if (HighLogic.LoadedSceneIsEditor) {
				if (activate) {
					EditorLogic.fetch.Lock (true, true, true, "Lock" + Quick.MOD);
				} else {
					EditorLogic.fetch.Unlock ("Lock" + Quick.MOD);
				}
			}
			if (!activate) {
				InputLockManager.RemoveControlLock ("Lock" + Quick.MOD);
			}
		}

		public static void Settings() {
			SettingsSwitch ();
			if (!WindowSettings) {
				QCategory.PartListTooltipsTWEAK (false);
				QuickScroll.StockToolbar.Reset ();
				QuickScroll.BlizzyToolbar.Reset ();
				QSettings.Instance.Save ();
			}
		}

		internal static void SettingsSwitch() {
			WindowSettings = !WindowSettings;
			QuickScroll.StockToolbar.Set (WindowSettings);
			Lock (WindowSettings);
		}

		internal static void OnGUI() {
			if (WindowSettings) {
				GUI.skin = HighLogic.Skin;
				RefreshRect ();
				RectSettings = GUILayout.Window (1545145, RectSettings, DrawSettings, Quick.MOD + " " + Quick.VERSION, GUILayout.Width (RectSettings.width), GUILayout.ExpandHeight (true));
			}
		}
		// Panneau de configuration
		private static void DrawSettings(int id) {
			int _rect = 214;
			GUILayout.BeginVertical ();

			GUILayout.BeginHorizontal ();
			GUILayout.Box ("Options", GUILayout.Height (30));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			GUILayout.BeginHorizontal ();
			QSettings.Instance.EnableWheelScroll = GUILayout.Toggle (QSettings.Instance.EnableWheelScroll, "Enable Wheel Scroll", GUILayout.Width (300));
			QSettings.Instance.EnableWheelShortCut = GUILayout.Toggle (QSettings.Instance.EnableWheelShortCut, "Enable Shortcut with Wheel Scroll", GUILayout.Width (300));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			GUILayout.BeginHorizontal ();
			QSettings.Instance.EnableKeyShortCut = GUILayout.Toggle (QSettings.Instance.EnableKeyShortCut, "Enable Keyboard Shortcut", GUILayout.Width (300));
			QSettings.Instance.EnableTWEAKPartListTooltips = GUILayout.Toggle (QSettings.Instance.EnableTWEAKPartListTooltips, "Enable the tweak for PartListTooltips", GUILayout.Width (300));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			GUILayout.BeginHorizontal ();
			QSettings.Instance.StockToolBar = GUILayout.Toggle (QSettings.Instance.StockToolBar, "Use the Stock ToolBar", GUILayout.Width (300));
			if (QBlizzyToolbar.isAvailable) {
				QSettings.Instance.BlizzyToolBar = GUILayout.Toggle (QSettings.Instance.BlizzyToolBar, "Use the Blizzy ToolBar", GUILayout.Width (300));
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			if (QSettings.Instance.EnableWheelScroll && QSettings.Instance.EnableWheelShortCut) {
				_rect += 68;

				GUILayout.BeginHorizontal ();
				GUILayout.Box ("Modifier Keyboard for Scrolling", GUILayout.Height (30));
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.ModKeyFilterWheel);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.ModKeyCategoryWheel);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

			}
			if (QSettings.Instance.EnableKeyShortCut) {
				_rect += 307;

				GUILayout.BeginHorizontal ();
				GUILayout.Box ("Keyboard ShortCuts", GUILayout.Height (30));
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.ModKeyShortCut);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.FilterPrevious);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.FilterNext);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.CategoryPrevious);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.CategoryNext);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.PagePrevious);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.PageNext);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.Pods);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.FuelTanks);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.Engines);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.CommandNControl);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.Structural);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.Aerodynamics);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.Utility);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.Sciences);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

			}
			if (GUILayout.Button ("Close", GUILayout.Height (30))) {
				QShortCuts.VerifyKey ();
				Settings ();
			}
			GUILayout.BeginHorizontal ();
			GUILayout.Space (5);
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
			RectSettings.height = _rect;
		}
	}
}