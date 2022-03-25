using Analysis.GraphicsTuner.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Analysis.GraphicsTuner.Module {
	public abstract class SettingModule {

		public bool isActive { get; private set; }
		public ComponentAnchor anchor { get; private set; }
		public string name { get; protected set; }

		private GraphicsTuner _tuner;
		private List<IConsoleComponent> _components;

		public SettingModule(GraphicsTuner tuner, ComponentAnchor anchor) {
			this._components = new List<IConsoleComponent>();
			this._tuner = tuner;
			this.anchor = anchor;
			this.isActive = true;
		}

		public void Destroy() {
			this._tuner = null;
			if(this._components != null) {
				for (int i = 0; i < this._components.Count; i++) {
					GameObject.Destroy(this._components[i].GetInst());
				}
				this._components = null;
			}
			this._tuner.RefreshUI();
		}

		public void SetActive(bool active) {
			if (active != this.isActive) {
				this.isActive = active;
				for(int i = 0; i < this._components.Count; i++) {
					this._components[i].GetInst().SetActive(this.isActive);
				}
				this._tuner.RefreshUI();
			}
		}

		public void SetAnchor(ComponentAnchor anchor) {
			if (anchor != this.anchor) {
				this.anchor = anchor;
				for (int i = 0; i < this._components.Count; i++) {
					this._tuner.SetComponentAnchor(this._components[i], this.anchor);
				}
				this._tuner.RefreshUI();
			}
		}

		public UIConsoleSlider CreateSlider(string title, float[] values, Func<float> getter, Action<float> setter, Action<float> onChange = null) {
			var slider = this._tuner.CreateSlider(title, values, getter, setter);
			if(onChange != null) slider.OnChange += onChange;
			this._tuner.SetComponentAnchor(slider, this.anchor);
			this._components.Add(slider);
			return slider;
		}

		public UIConsoleDropdown CreateDropdown(string title, string[] values, Func<int> getter, Action<int> setter, Action<int> onChange = null) {
			var dropdown = this._tuner.CreateDropdown(title, values, getter, setter);
			if (onChange != null) dropdown.OnChange += onChange;
			this._tuner.SetComponentAnchor(dropdown, this.anchor);
			this._components.Add(dropdown);
			return dropdown;
		}

		public UIConsoleDropdown CreateDropdown(string title, Type type, Func<int> getter, Action<int> setter, Action<int> onChange = null) {
			var dropdown = this._tuner.CreateDropdown(title, type, getter, setter);
			if (onChange != null) dropdown.OnChange += onChange;
			this._tuner.SetComponentAnchor(dropdown, this.anchor);
			this._components.Add(dropdown);
			return dropdown;
		}

		public UIConsoleToggle CreateToggle(string title, Func<bool> getter, Action<bool> setter, Action<bool> onChange = null) {
			var toggle = this._tuner.CreateToggle(title, getter, setter);
			if (onChange != null) toggle.OnChange += onChange;
			this._tuner.SetComponentAnchor(toggle, this.anchor);
			this._components.Add(toggle);
			return toggle;
		}

		public UIConsoleLabel CreateLabel(string title, out Action<string> setter) {
			var label = this._tuner.CreateLabel(title);
			setter = label.SetText;
			this._tuner.SetComponentAnchor(label, this.anchor);
			this._components.Add(label);
			return label;
		}

		public UIConsoleTitle CreateTitle(string name) {
			var title = this._tuner.CreateTitle(name);
			this._tuner.SetComponentAnchor(title, this.anchor);
			this._components.Add(title);
			return title;
		}
	}
}
