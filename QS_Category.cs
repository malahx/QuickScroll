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
using UnityEngine;

namespace QuickScroll {
	public class QCategory {
		// Lister tous les filtres (ainsi que les subassemblies)
		public static List<PartCategorizer.Category> Filters {
			get {
				List<PartCategorizer.Category> _filters = new List<PartCategorizer.Category>();
				_filters.AddRange(PartCategorizer.Instance.filters);
				_filters.AddRange(PartCategorizer.Instance.categories);
				return _filters;
			}
		}

		// Indiquer quel filtre est sélectionné
		public static PartCategorizer.Category CurrentFilter {
			get {
				return Filters.Find(f => f.button.activeButton.State == RUIToggleButtonTyped.ButtonState.TRUE);
			}
		}

		// Lister toutes les catégories d'un filtre
		public static List<PartCategorizer.Category> Categories {
			get {
				return CurrentFilter.subcategories;
			}
		}

		// Indiquer quel catégorie est sélectionnée
		public static PartCategorizer.Category CurrentCategory {
			get {
				return Categories.Find(f => f.button.activeButton.State == RUIToggleButtonTyped.ButtonState.TRUE);
			}
		}

		// Sélectionner le filtre ou la catégorie suivant
		public static PartCategorizer.Category NextCategory(List<PartCategorizer.Category> categories, int index) {
			if (index >= categories.Count -1) {
				index = -1;
			}
			index++;
			return categories[index];
		}

		// Sélectionner le filtre ou la catégorie précédent
		public static PartCategorizer.Category PrevCategory(List<PartCategorizer.Category> categories, int index) {
			if (index <= 0) {
				index = categories.Count;
			}
			index--;
			return categories[index];
		}

		// Changer de page
		internal static void SelectPartPage(bool dirScrolling) {
			if (dirScrolling) {
				if (!EditorPartList.Instance.prevPage.gameObject.activeSelf) {
					if (!QSettings.Instance.EnableWheelBlockTopEnd) {
						while (EditorPartList.Instance.nextPage.gameObject.activeSelf) {
							EditorPartList.Instance.NextPage ();
						}
					}
					return;
				}
				EditorPartList.Instance.PrevPage ();
			} else {
				if (!EditorPartList.Instance.nextPage.gameObject.activeSelf) {
					if (!QSettings.Instance.EnableWheelBlockTopEnd) {
						while (EditorPartList.Instance.prevPage.gameObject.activeSelf) {
							EditorPartList.Instance.PrevPage ();
						}
					}
					return;
				}
				EditorPartList.Instance.NextPage ();
			}
			QuickScroll.Warning ("SelectPartPage " + (dirScrolling ? "PrevPage" : "NextPage"), true);
			PartListTooltipsTWEAK (false);
		}

		// Changer de catégorie
		internal static void SelectPartCategory(bool dirScrolling) {
			SelectPartCategory (dirScrolling, Categories, CurrentCategory, PartCategorizer.Instance.scrollListSub.scrollList);
		}

		// Changer de filtre
		internal static void SelectPartFilter(bool dirScrolling) {
			SelectPartCategory (dirScrolling, Filters, CurrentFilter, PartCategorizer.Instance.scrollListMain.scrollList);
		}

		// Changer de filtre/catégorie
		internal static void SelectPartCategory(bool dirScrolling, List<PartCategorizer.Category> categories, PartCategorizer.Category currentCategory, UIScrollList scrollList) {
			int _index = currentCategory.button.container.Index;
			if (QSettings.Instance.EnableWheelBlockTopEnd) {
				if (dirScrolling && _index == 0) {
					return;
				}
				if (!dirScrolling && _index == categories.Count - 1) {
					return;
				}
			}
			PartCategorizer.Category _category = (dirScrolling ? PrevCategory (categories, _index) : NextCategory (categories, _index));
			RUIToggleButtonTyped _btn = _category.button.activeButton;
			_btn.SetTrue (_btn, RUIToggleButtonTyped.ClickType.FORCED, true);
			ScrollList (dirScrolling, scrollList, _category);
			QuickScroll.Warning ("SelectPartCategory " + (dirScrolling ? "Prev" : "Next"), true);
			PartListTooltipsTWEAK (false);
		}

		// Améliorer le scroll par défaut
		internal static void ScrollList(bool dirScrolling, UIScrollList scrollList, PartCategorizer.Category category) {
			int _lastposition = (scrollList.Count +2) * 34;
			if (_lastposition < scrollList.viewableArea.y) {
				return;
			}
			int _index = category.button.container.Index;
			float _position = (_index +1) * 34;
			if (_position > scrollList.viewableArea.y / 2 && _lastposition - _position > scrollList.viewableArea.y / 2) {
				scrollList.ScrollToItem (_index, 0);
				return;
			}
			if (_position < scrollList.viewableArea.y / 2) {
				scrollList.ScrollListTo (0f);
				return;
			}
			if (_lastposition - _position < scrollList.viewableArea.y / 2) {
				scrollList.ScrollListTo (1f);
				return;
			}
		}

		// Selectionner une categorie
		internal static void ForceSelectTab(int index) {
			// It doesn't work?
			//EditorPartList.Instance.ForceSelectTab (category);
			PartCategorizer.Category _category = Filters[0];
			RUIToggleButtonTyped _btn = _category.button.activeButton;
			if (_btn.State == RUIToggleButtonTyped.ButtonState.FALSE) {
				_btn.SetTrue (_btn, RUIToggleButtonTyped.ClickType.FORCED, true);
			}
			_category = Categories[index];
			_btn = _category.button.activeButton;
			if (_btn.State == RUIToggleButtonTyped.ButtonState.FALSE) {
				_btn.SetTrue (_btn, RUIToggleButtonTyped.ClickType.FORCED, true);
			}
			QuickScroll.Warning ("ForceSelectTab " + _category.button.categoryName, true);
			PartListTooltipsTWEAK (false);
		}

		// Petit tweak
		internal static void PartListTooltipsTWEAK(bool enable) {
			if (QSettings.Instance.EnableTWEAKPartListTooltips) {
				PartListTooltips.fetch.enabled = enable;
			} else {
				PartListTooltips.fetch.enabled = true;
			}
			if (!enable) {
				if (PartListTooltips.fetch.displayTooltip) {
					GameEvents.onTooltipDestroyRequested.Fire ();
					PartListTooltips.fetch.HideTooltip ();
				}
			} else {
				if (PartListTooltips.fetch.partIcon != null) {
					if (PartListTooltips.fetch.partIcon.MouseOver && !PartListTooltips.fetch.displayTooltip) {
						PartListTooltips.fetch.ShowTooltip (PartListTooltips.fetch.partIcon, PartListTooltips.fetch.partInfo);
						PartListTooltips.fetch.PinTooltip ();
					}
				}
			}
			QuickScroll.Warning ("PartListTooltipsTWEAK " + enable, true);
		}
		internal static void PartListTooltipsTWEAK() {
			if (HighLogic.LoadedSceneIsEditor) {
				if (EditorLogic.fetch.editorScreen == EditorScreen.Parts) {
					// TWEAKPartListTooltips
					if (QSettings.Instance.EnableTWEAKPartListTooltips) {
						if (Input.GetKeyDown (QSettings.Instance.KeyPartListTooltipsActivate)) {
							QCategory.PartListTooltipsTWEAK (true);
						}
						if (Input.GetKeyUp (QSettings.Instance.KeyPartListTooltipsDisactivate)) {
							QCategory.PartListTooltipsTWEAK (false);
						}
					}
				}
			}
		}
	}
}