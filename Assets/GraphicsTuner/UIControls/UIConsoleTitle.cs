using UnityEngine;
using UnityEngine.UI;

namespace Analysis.GraphicsTuner.UI {
	public class UIConsoleTitle : IConsoleComponent {

		protected GameObject gameObject;
		protected string title;
		private Text _titleLabel;

		public UIConsoleTitle(string title) {
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

		private void BindUIRelation(GameObject instObj) {
			this._titleLabel = instObj.GetComponent<Text>();
		}
	}
}
