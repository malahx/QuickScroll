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
using System.IO;
using UnityEngine;

namespace QuickScroll {
	public class QSettings {

		public static QSettings Instance = new QSettings ();

		private string PathConfig = KSPUtil.ApplicationRootPath + "GameData/" + Quick.MOD + "/Config.txt";

		[Persistent]
		public bool StockToolBar = true;
		[Persistent]
		public bool BlizzyToolBar = true;

		[Persistent]
		internal bool EnableWheelScroll = true;
		[Persistent]
		internal bool EnableWheelShortCut = true;
		[Persistent]
		internal bool EnableKeyShortCut = true;
		[Persistent]
		internal bool EnableTWEAKPartListTooltips = false;

		[Persistent]
		internal string KeyPartListTooltipsActivate = "mouse 1";
		[Persistent]
		internal string KeyPartListTooltipsDisactivate = "mouse 0";

		[Persistent]
		internal KeyCode ModKeyFilterWheel;
		[Persistent]
		internal KeyCode ModKeyCategoryWheel;

		[Persistent]
		internal KeyCode ModKeyShortCut;

		[Persistent]
		internal KeyCode KeyFilterPrevious;
		[Persistent]
		internal KeyCode KeyFilterNext;
		[Persistent]
		internal KeyCode KeyCategoryPrevious;
		[Persistent]
		internal KeyCode KeyCategoryNext;
		[Persistent]
		internal KeyCode KeyPagePrevious;
		[Persistent]
		internal KeyCode KeyPageNext;
		[Persistent]
		internal KeyCode KeyPods;
		[Persistent]
		internal KeyCode KeyFuelTanks;
		[Persistent]
		internal KeyCode KeyEngines;
		[Persistent]
		internal KeyCode KeyCommandNControl;
		[Persistent]
		internal KeyCode KeyStructural;
		[Persistent]
		internal KeyCode KeyAerodynamics;
		[Persistent]
		internal KeyCode KeyUtility;
		[Persistent]
		internal KeyCode KeySciences;

		// GESTION DE LA CONFIGURATION
		public void Save() {
			ConfigNode _temp = ConfigNode.CreateConfigFromObject(this, new ConfigNode());
			_temp.Save(PathConfig);
			Quick.Log ("Settings Saved");
		}
		public void Load() {
			if (File.Exists (PathConfig)) {
				ConfigNode _temp = ConfigNode.Load (PathConfig);
				ConfigNode.LoadObjectFromConfig (this, _temp);
				Quick.Log ("Settings Loaded");
			} else {
				Save ();
			}
		}
	}
}