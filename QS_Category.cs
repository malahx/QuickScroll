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
	public class Cat : QS {
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
		internal static void SelectPartPage(float scroll) {
			if (scroll > 0)
				EditorPartList.Instance.PrevPage ();
			if (scroll < 0)
				EditorPartList.Instance.NextPage ();
			PartListTooltipsTWEAK (false);
		}

		// Changer de catégorie
		internal static void SelectPartCategory(float scroll) {
			int _index;
			CategorySelected (out _index);
			PartCategorizer.Category _category = (scroll > 0 ? PrevCategory (Categories, _index) : NextCategory (Categories, _index));
			RUIToggleButtonTyped _btn = _category.button.activeButton;
			_btn.SetTrue (_btn, RUIToggleButtonTyped.ClickType.FORCED, true);
			PartListTooltipsTWEAK (false);
		}

		// Changer de filtre
		internal static void SelectPartFilter(float scroll) {
			int _index;
			FilterSelected (out _index);
			PartCategorizer.Category _category = (scroll > 0 ? PrevCategory (Filters, _index) : NextCategory (Filters, _index));
			RUIToggleButtonTyped _btn = _category.button.activeButton;
			_btn.SetTrue (_btn, RUIToggleButtonTyped.ClickType.FORCED, true);	
			PartListTooltipsTWEAK (false);
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
			PartListTooltipsTWEAK (false);
		}

		// Petit tweak
		internal static void PartListTooltipsTWEAK(bool enable) {
			if (Settings.Instance.EnableTWEAKPartListTooltips) {
				PartListTooltips.fetch.enabled = enable;
			}
			if (PartListTooltips.fetch.enabled && PartListTooltips.fetch.displayTooltip) {
				PartListTooltips.fetch.HideTooltip ();
			}
		}
	}
}