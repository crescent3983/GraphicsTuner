using System;
using UnityEngine;
using UnityEngine.UI;

namespace Analysis.GraphicsTuner.UI {
	public class UIConsoleToggle : UIConsoleComponent<bool> {

		public const string TITLE_NAME = "Title";
		public const string TOGGLE_NAME = "Toggle";

		private Text _titleLabel;
		private Toggle _toggle;

		#region Constructor
		public UIConsoleToggle(string title, Func<bool> getter, Action<bool> setter) : base(title, getter, setter) {}
		#endregion

		#region Component Implementation
		public override void SetUI(GameObject instObj) {
			base.SetUI(instObj);

			this.BindUIRelation(instObj);
			this._titleLabel.text = this.title;

			this._toggle.onValueChanged.AddListener(this.OnComponentChanged);
			this.Refresh();
		}

		public override void Refresh() {
			this._toggle.isOn = this.OnGetValue?.Invoke() ?? false;
		}

		protected override void OnComponentChanged(bool value) {
			this.OnSetValue?.Invoke(value);
			base.OnComponentChanged(value);
		}
		#endregion

		#region Internal Methods
		private void BindUIRelation(GameObject instObj) {
			this._titleLabel = instObj.transform.Find(TITLE_NAME).GetComponent<Text>();
			this._toggle = instObj.transform.Find(TOGGLE_NAME).GetComponent<Toggle>();
		}
		#endregion
	}
}
