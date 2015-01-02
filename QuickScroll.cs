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
	public class QuickScroll : MonoBehaviour {

		public static string VERSION = "1.01";
		public static string MOD = "QuickScroll";

		private static bool isdebug = true;

		private string File_settings = KSPUtil.ApplicationRootPath + "GameData/QuickScroll/PluginData/QuickScroll/Config.txt";

		private static Rect[] ScrollRect_Simple = {
			new Rect (0, 25, 30, Screen.height), // Categories
			new Rect (40, 25, 260, Screen.height) // Parts
		};
		private static Rect[] ScrollRect_Advanced = {
			new Rect (0, 25, 30, Screen.height), // Filters
			new Rect (35, 25, 65, Screen.height), // Categories
			new Rect (70, 25, 295, Screen.height) // Parts
		};

		[Persistent]
		private string KeyFilter = "left shift";
		[Persistent]
		private string KeyCategory = "left ctrl";

		// Lister tous les filtres (ainsi que les subassemblies)
		public static PartCategorizer.Category[] Filters {
			get {
				List<PartCategorizer.Category> _filters = new List<PartCategorizer.Category>();
				_filters.AddRange(PartCategorizer.Instance.filters);
				_filters.AddRange(PartCategorizer.Instance.categories);
				return _filters.ToArray();
			}
		}

		// Indiquer quel filtre est sélectionné
		public static PartCategorizer.Category FilterSelected(out int index) {
			index = 0;
			PartCategorizer.Category[] _categories = Filters;
			foreach (PartCategorizer.Category _category in _categories) {
				if (_category.button.activeButton.State == RUIToggleButtonTyped.ButtonState.TRUE) {
					return _category;
				}
				index++;
			}
			return null;
		}

		// Lister toutes les catégories d'un filtre
		public static PartCategorizer.Category[] Categories {
			get {
				int _index;
				PartCategorizer.Category _category = FilterSelected(out _index);
				return _category.subcategories.ToArray();
			}
		}

		// Indiquer quel catégorie est sélectionnée
		public static PartCategorizer.Category CategorySelected(out int index) {
			index = 0;
			PartCategorizer.Category[] _categories = Categories;
			foreach (PartCategorizer.Category _category in _categories) {
				if (_category.button.activeButton.State == RUIToggleButtonTyped.ButtonState.TRUE) {
					return _category;
				}
				index++;
			}
			return null;
		}

		// Sélectionner la catégorie/filtre suivant
		public static PartCategorizer.Category NextCategory(PartCategorizer.Category[] categories, int index) {
			if (index >= categories.Length -1) {
				index = -1;
			}
			index++;
			return categories[index];
		}

		// Sélectionner la catégorie/filtre précédent
		public static PartCategorizer.Category PrevCategory(PartCategorizer.Category[] categories, int index) {
			if (index <= 0) {
				index = categories.Length;
			}
			index--;
			return categories[index];
		}

		// Changer de page
		private void SelectPartPage(float scroll) {
			if (scroll > 0)
				EditorPartList.Instance.PrevPage ();
			if (scroll < 0)
				EditorPartList.Instance.NextPage ();
		}

		// Changer de catégorie
		private void SelectPartCategory(float scroll) {
			int _index;
			CategorySelected (out _index);
			PartCategorizer.Category _category = (scroll > 0 ? PrevCategory (Categories, _index) : NextCategory (Categories, _index));
			RUIToggleButtonTyped _btn = _category.button.activeButton;
			_btn.SetTrue (_btn, RUIToggleButtonTyped.ClickType.FORCED, true);	
		}

		// Changer de filtre
		private void SelectPartFilter(float scroll) {
			int _index;
			FilterSelected (out _index);
			PartCategorizer.Category _category = (scroll > 0 ? PrevCategory (Filters, _index) : NextCategory (Filters, _index));
			RUIToggleButtonTyped _btn = _category.button.activeButton;
			_btn.SetTrue (_btn, RUIToggleButtonTyped.ClickType.FORCED, true);	
		}

		// Démarrage du plugin
		private void Awake() {
			Load ();
		}

		// Attendre les touches
		private void Update() {
			if (HighLogic.LoadedSceneIsEditor) {
				if (EditorLogic.fetch.editorScreen == EditorScreen.Parts) {
					if (EditorPanels.Instance.IsMouseOver ()) {
						float _scroll = Input.GetAxis ("Mouse ScrollWheel");
						if (_scroll != 0) {
							if (PartListTooltips.fetch.displayTooltip) {
								PartListTooltips.fetch.HideTooltip ();
							}
							Vector2 _mouse = Mouse.screenPos;
							if (EditorLogic.Mode == EditorLogic.EditorModes.SIMPLE) {
								if (((Input.GetKey (KeyFilter) || Input.GetKey (KeyCategory)) && ScrollRect_Simple [0].Contains (_mouse)) || (Input.GetKey (KeyFilter) && ScrollRect_Simple [1].Contains (_mouse))) {
									PartCategorizer.Instance.SetAdvancedMode ();
									SelectPartFilter (_scroll);
								} else if (ScrollRect_Simple [0].Contains (_mouse) || (Input.GetKey(KeyCategory) && ScrollRect_Simple [1].Contains (_mouse))) {
									SelectPartCategory (_scroll);
								} else if (ScrollRect_Simple [1].Contains (_mouse)) {
									SelectPartPage (_scroll);
								}
							} else {
								if (ScrollRect_Advanced [0].Contains (_mouse) || ((Input.GetKey(KeyFilter) || Input.GetKey(KeyCategory)) && ScrollRect_Advanced [1].Contains (_mouse)) || (Input.GetKey(KeyFilter) && ScrollRect_Advanced [2].Contains (_mouse))) {
									SelectPartFilter (_scroll);
								} else if (ScrollRect_Advanced [1].Contains (_mouse) || (Input.GetKey(KeyCategory) && ScrollRect_Advanced [2].Contains (_mouse))) {
									SelectPartCategory (_scroll);
								} else if (ScrollRect_Advanced [2].Contains (_mouse)) {
									SelectPartPage (_scroll);
								}
							}
						}
					}
				}
			}
		}

		// Charger la configuration
		public void Load() {
			if (File.Exists (File_settings)) {
				ConfigNode _temp = ConfigNode.Load (File_settings);
				if (_temp.HasValue ("KeyFilter") && _temp.HasValue ("KeyCategory")) {
					ConfigNode.LoadObjectFromConfig (this, _temp);
					Log("Load");
					return;
				}
			}
		}

		// Afficher les messages sur la console
		private static void Log(string String) {
			if (isdebug) {
				Debug.Log (MOD + "(" + VERSION + "): " + String);
			}
		}
	}
}