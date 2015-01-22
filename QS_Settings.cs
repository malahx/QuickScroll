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
	public class Settings : QS {

		public static Settings Instance = new Settings ();

		private string File_settings = KSPUtil.ApplicationRootPath + "GameData/" + MOD + "/Config.txt";

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
		internal string ModKeyFilterWheel = "left shift";
		[Persistent]
		internal string ModKeyCategoryWheel = "left ctrl";

		[Persistent]
		internal string ModKeyShortCut = "right ctrl";

		[Persistent]
		internal string KeyFilterPrevious = "page up";
		[Persistent]
		internal string KeyFilterNext = "page down";
		[Persistent]
		internal string KeyCategoryPrevious = "up";
		[Persistent]
		internal string KeyCategoryNext = "down";
		[Persistent]
		internal string KeyPagePrevious = "left";
		[Persistent]
		internal string KeyPageNext = "right";
		[Persistent]
		internal string KeyPods = "[1]";
		[Persistent]
		internal string KeyFuelTanks = "[2]";
		[Persistent]
		internal string KeyEngines = "[3]";
		[Persistent]
		internal string KeyCommandNControl = "[4]";
		[Persistent]
		internal string KeyStructural = "[5]";
		[Persistent]
		internal string KeyAerodynamics = "[6]";
		[Persistent]
		internal string KeyUtility = "[7]";
		[Persistent]
		internal string KeySciences = "[8]";

		// Charger la configuration
		public void Load() {
			if (File.Exists (File_settings)) {
				ConfigNode _temp = ConfigNode.Load (File_settings);
				ConfigNode.LoadObjectFromConfig (this, _temp);
				Log("Load");
				return;
			}
		}
	}
}