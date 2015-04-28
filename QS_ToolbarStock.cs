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
using System.Collections;
using UnityEngine;

namespace QuickScroll {
	//[KSPAddon(KSPAddon.Startup.EveryScene, false)]
	public class QStockToolbar : MonoBehaviour {
	
		internal static bool Enabled {
			get {
				return QSettings.Instance.StockToolBar;
			}
		}

		private ApplicationLauncher.AppScenes AppScenes = ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB;
		private static string TexturePath = Quick.MOD + "/Textures/StockToolBar";

		private void OnClick() { 
			QGUI.Settings ();
		}
			
		private Texture2D GetTexture {
			get {
				return GameDatabase.Instance.GetTexture(TexturePath, false);
			}
		}

		private ApplicationLauncherButton appLauncherButton;

		internal static bool isAvailable {
			get {
				return ApplicationLauncher.Instance != null;
			}
		}

		/*internal static QStockToolbar Instance {
			get;
			private set;
		}

		internal void Awake() {
			Instance = this;
			GameEvents.onGUIApplicationLauncherDestroyed.Add (AppLauncherDestroyed);
			GameEvents.onGameSceneLoadRequested.Add (AppLauncherDestroyed);
		}

		internal void Start() {
			if (!HighLogic.LoadedSceneIsGame) {
				return;
			}
			StartCoroutine (AppLauncherReady ());
		}*/

		internal IEnumerator AppLauncherReady() {
			if (!Enabled || !HighLogic.LoadedSceneIsGame) {
				yield break;
			}
			while (!isAvailable) {
				yield return 0;
			}
			while (!ApplicationLauncher.Ready) {
				yield return 0;
			}
			Init ();
			//yield return new WaitForEndOfFrame ();
			//Refresh ();
		}

		internal void AppLauncherDestroyed(GameScenes gameScenes) {
			AppLauncherDestroyed ();
		}

		internal void AppLauncherDestroyed() {
			Destroy ();
		}

		/*internal void OnDestroy() {
			GameEvents.onGUIApplicationLauncherDestroyed.Remove (AppLauncherDestroyed);
			GameEvents.onGameSceneLoadRequested.Remove (AppLauncherDestroyed);
		}*/

		private void Init() {
			if (!isAvailable) {
				return;
			}
			if (appLauncherButton == null) {
				appLauncherButton = ApplicationLauncher.Instance.AddModApplication (OnClick, OnClick, null, null, null, null, AppScenes, GetTexture);
			}
		}

		private void Destroy() {
			if (!isAvailable) {
				return;
			}
			if (appLauncherButton != null) {
				ApplicationLauncher.Instance.RemoveModApplication (appLauncherButton);
				appLauncherButton = null;
			}
		}

		internal void Set(bool SetTrue, bool force = false) {
			if (!isAvailable) {
				return;
			}
			if (appLauncherButton != null) {
				if (SetTrue) {
					if (appLauncherButton.State == RUIToggleButton.ButtonState.FALSE) {
						appLauncherButton.SetTrue (force);
					}
				} else {
					if (appLauncherButton.State == RUIToggleButton.ButtonState.TRUE) {
						appLauncherButton.SetFalse (force);
					}
				}
			}
		}

		/*internal void Refresh() {
			if (appLauncherButton != null) {
				if (QGUI.WindowSettings && appLauncherButton.State == RUIToggleButton.ButtonState.FALSE) {
					appLauncherButton.SetTrue (false);
				}
				if (!QGUI.WindowSettings && appLauncherButton.State == RUIToggleButton.ButtonState.TRUE) {
					appLauncherButton.SetFalse (false);
				}
				//appLauncherButton.SetTexture (GetTexture);
			}
		}*/

		internal void Reset() {
			if (appLauncherButton != null) {
				Set (false);
				if (!Enabled) {
					Destroy ();
				}
			}
			if (Enabled) {
				Init ();
			}
		}
	}
}