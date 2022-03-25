using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Analysis.GraphicsTuner.UI {
	public class UIConsoleDropdown : UIConsoleComponent<int> {

		public const string TITLE_NAME = "Title";
		public const string DROPDOWN_NAME = "Dropdown";

		private string[] _values;
		private Type enumType = null;
		private Text _titleLabel;
		private Dropdown _popupList;

		#region Constructor
		public UIConsoleDropdown(string title, string[] values, Func<int> getter, Action<int> setter) : base(title, getter, setter) {
			this._values = values != null ? values : new string[0];
		}

		public UIConsoleDropdown(string title, Type type, Func<int> getter, Action<int> setter) : base(title, getter, setter) {
			if(type.IsEnum) {
				enumType = type;
				this._values = Enum.GetNames(type);
			}
			else {
				this._values = new string[0];
			}
		}
		#endregion

		#region Component Implementation
		public override void SetUI(GameObject instObj) {
			base.SetUI(instObj);

			this.BindUIRelation(instObj);

			this._popupList.ClearOptions();

			var options = new List<Dropdown.OptionData>();
			for(int i = 0; i < this._values.Length; i++) {
				options.Add(new Dropdown.OptionData(this._values[i]));
			}
			this._popupList.AddOptions(options);
			this._popupList.onValueChanged.AddListener(this.OnComponentChanged);

			this._titleLabel.text = this.title;
			this.Refresh();
		}

		public override void Refresh() {
			this._popupList.value = this.EnumValueToIndex(this.OnGetValue?.Invoke() ?? 0);
		}

		protected override void OnComponentChanged(int value) {
			this.OnSetValue?.Invoke(this.IndexToEnumValue(value));
			base.OnComponentChanged(value);
		}
		#endregion

		#region Internal Methods
		private void BindUIRelation(GameObject instObj) {
			this._titleLabel = instObj.transform.Find(TITLE_NAME).GetComponent<Text>();
			this._popupList = instObj.transform.Find(DROPDOWN_NAME).GetComponent<Dropdown>();

			this._titleLabel = instObj.GetComponentInChildren<Text>();
			this._popupList = instObj.GetComponentInChildren<Dropdown>();
		}

		private int IndexToEnumValue(int index) {
			if(enumType != null) {
				return (int)Convert.ChangeType(Enum.GetValues(enumType).GetValue(index), typeof(int));
			}
			return index;
		}

		private int EnumValueToIndex(int value) {
			if (enumType != null) {
				string name = Enum.GetName(enumType, value);
				if (string.IsNullOrEmpty(name)) {
					return 0;
				}
				for (int i = 0; i < this._values.Length; i++) {
					if(name.Equals(this._values[i], StringComparison.Ordinal)) {
						return i;
					}
				}
			}
			return value;
		}
		#endregion
	}
}
