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

		// Initialisation des modules
		private void Awake() {
			QToolbar.Awake ();
			QShortCuts.Awake ();
			QGUI.Awake ();
		}

		// Initialisation des variables
		private void Start() {
			if (HighLogic.LoadedSceneIsGame) {
				QSettings.Instance.Load ();
				QToolbar.Instance.Start ();
				QShortCuts.VerifyKey ();
				if (HighLogic.LoadedSceneIsEditor) {
					QCategory.PartListTooltipsTWEAK (false);
				}
			}
		}

		// Arrêter le plugin
		private void OnDestroy() {
			if (HighLogic.LoadedSceneIsGame) {
				QToolbar.Instance.OnDestroy ();
			}
		}

		// Gérer les raccourcis
		private void Update() {
			QShortCuts.Update ();
		}

		private void LateUpdate() {
			QCategory.PartListTooltipsTWEAK();
		}

		// Gérer l'interface
		private void OnGUI() {
			GUI.skin = HighLogic.Skin;
			QShortCuts.OnGUI ();
			QGUI.OnGUI ();
		}
	}
}