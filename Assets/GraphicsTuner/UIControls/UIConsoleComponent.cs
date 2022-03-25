using System;
using UnityEngine;

namespace Analysis.GraphicsTuner.UI {
	public interface IConsoleComponent {
		GameObject GetInst();
		void Refresh();
	}

	public abstract class UIConsoleComponent<T> : IConsoleComponent {

		protected GameObject gameObject;
		protected string title;
		protected Func<T> OnGetValue;
		protected Action<T> OnSetValue;
		public event Action<T> OnChange;

		public abstract void Refresh();

		public GameObject GetInst() {
			return this.gameObject;
		}

		public virtual void SetUI(GameObject inst) {
			this.gameObject = inst;
		}

		public UIConsoleComponent(string title, Func<T> getter, Action<T> setter) {
			this.OnGetValue = getter;
			this.OnSetValue = setter;
			this.title = title;
		}

		protected virtual void OnComponentChanged(T value) {
			this.OnChange?.Invoke(value);
		}
	}
}
