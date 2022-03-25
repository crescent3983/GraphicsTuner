using UnityEngine;
using UnityEngine.UI;

namespace Analysis.GraphicsTuner.UI {
	public class UIConsoleLabel : IConsoleComponent {

		public const string TITLE_NAME = "Title";
		public const string LABEL_NAME = "Label";

		protected GameObject gameObject;
		protected string title;
		private Text _titleLabel;
		private Text _label;

		public UIConsoleLabel(string title) {
			this.title = title;
		}

		public GameObject GetInst() {
			return this.gameObject;
		}

		public void SetUI(GameObject instObj) {
			this.gameObject = instObj;
			this.BindUIRelation(instObj);
			this._titleLabel.text = this.title;
			this.Refresh();
		}

		public void Refresh() {

		}

		public void SetText(string text) {
			this._label.text = text;
		}

		private void BindUIRelation(GameObject instObj) {
			this._titleLabel = instObj.transform.Find(TITLE_NAME).GetComponent<Text>();
			this._label = instObj.transform.Find(LABEL_NAME).GetComponent<Text>();
		}
	}
}
