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

	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public class QuickScroll : Quick {

		internal static QuickScroll Instance;
		#if GUI
		internal static QBlizzyToolbar BlizzyToolbar;
		internal static QStockToolbar StockToolbar;
		#endif

		// Initialisation des modules
		private void Awake() {
			Instance = this;
			#if GUI
			BlizzyToolbar = new QBlizzyToolbar ();
			StockToolbar = new QStockToolbar ();
			GameEvents.onGUIApplicationLauncherDestroyed.Add (StockToolbar.AppLauncherDestroyed);
			GameEvents.onGameSceneLoadRequested.Add (StockToolbar.AppLauncherDestroyed);
			QGUI.Awake ();
			#endif
			QShortCuts.Awake ();
		}

		// Initialisation des variables
		private void Start() {
			QSettings.Instance.Load ();
			#if GUI
			BlizzyToolbar.Start ();
			StartCoroutine (StockToolbar.AppLauncherReady ());
			#endif
			QShortCuts.VerifyKey ();
			if (HighLogic.LoadedSceneIsEditor) {
				QCategory.PartListTooltipsTWEAK (false);
			}
		}

		#if GUI
		// Arrêter le plugin
		private void OnDestroy() {
			BlizzyToolbar.OnDestroy ();
			GameEvents.onGUIApplicationLauncherDestroyed.Remove (StockToolbar.AppLauncherDestroyed);
			GameEvents.onGameSceneLoadRequested.Remove (StockToolbar.AppLauncherDestroyed);
		}
		#endif

		// Gérer les raccourcis
		private void Update() {
			QShortCuts.Update ();
		}

		private void LateUpdate() {
			QCategory.PartListTooltipsTWEAK();
		}

		#if GUI
		// Gérer l'interface
		private void OnGUI() {
			GUI.skin = HighLogic.Skin;
			QShortCuts.OnGUI ();
			QGUI.OnGUI ();
		}
		#endif
	}
}