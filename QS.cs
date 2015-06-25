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
	public partial class QuickScroll : MonoBehaviour {

		internal static QuickScroll Instance;
		#if GUI
		[KSPField(isPersistant = true)] internal static QBlizzyToolbar BlizzyToolbar;
		#endif

		// Initialisation des modules
		private void Awake() {
			if (Instance != null) {
				Destroy (this);
				Warning ("There's already an Instance of " + MOD);
				return;
			}
			Instance = this;
			#if GUI
			if (BlizzyToolbar == null) BlizzyToolbar = new QBlizzyToolbar ();
			QGUI.Awake ();
			QShortCuts.Awake ();
			#endif
			Warning ("Awake", true);
		}

		// Initialisation des variables
		private void Start() {
			QSettings.Instance.Load ();
			#if GUI
			BlizzyToolbar.Start ();
			#endif
			#if SHORTCUT
			QShortCuts.VerifyKey ();
			#endif
			QCategory.PartListTooltipsTWEAK (false);
			#if SCROLL
			PartCategorizer.Instance.scrollListSub.scrollList.scrollWheelFactor = 0;
			PartCategorizer.Instance.scrollListMain.scrollList.scrollWheelFactor = 0;
			#endif
			Warning ("Start", true);
		}

		#if GUI
		// Arrêter le plugin
		private void OnDestroy() {
			BlizzyToolbar.OnDestroy ();
			Warning ("OnDestroy", true);
		}
		#endif

		// Gérer les raccourcis
		private void Update() {
			#if SHORTCUT
			QShortCuts.Update ();
			#endif
			#if SCROLL
			QScroll.Update ();
			#endif
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
