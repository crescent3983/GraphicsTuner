using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Analysis.GraphicsTuner.UI {
	public class UIConsoleSlider : UIConsoleComponent<float> {

		public const string TITLE_NAME = "Title";
		public const string SLIDER_TEXT_NAME = "Slider Value";
		public const string SLIDER_NAME = "Slider";

		private float _minValue = 0f;
		private float _maxValue = 1f;
		private float[] _values;
		private float[] _normalizedValues;
		private bool _paddingZero = false;
		private Text _titleLabel;
		private Text _sliderLabel;
		private Slider _slider;

		#region Constructor
		public UIConsoleSlider(string title, float minValue, float maxValue, Func<float> getter, Action<float> setter) : base(title, getter, setter) {
			if(this._maxValue > this._minValue) {
				this._minValue = minValue;
				this._maxValue = maxValue;
			}
		}

		public UIConsoleSlider(string title, float[] values, Func<float> getter, Action<float> setter) : base(title, getter, setter) {
			if(values != null && values.Length > 0) {
				List<float> tmp = new List<float>(values);
				tmp.Sort();
				if(tmp[0] > 0) {
					this._paddingZero = true;
					tmp.Insert(0, 0f);
				}
				this._values = tmp.ToArray();
				this._normalizedValues = new float[this._values.Length];
				float total = this._values[this._values.Length - 1] - this._values[0];
				for (int i = 0; i < this._normalizedValues.Length; i++) {
					this._normalizedValues[i] = (this._values[i] - this._values[0]) / total;
				}
			}
		}
		#endregion

		#region Component Implementation
		public override void SetUI(GameObject instObj) {
			base.SetUI(instObj);

			this.BindUIRelation(instObj);

			this._slider.onValueChanged.AddListener(this.OnComponentChanged);
			this._titleLabel.text = this.title;
			this.Refresh();
		}

		public override void Refresh() {
			this._slider.value = this.ActualValueToSliderValue(this.OnGetValue?.Invoke() ?? 0f);
		}

		protected override void OnComponentChanged(float value) {
			if (this._values != null) {
				float v = this.FindNearestValue(value, this._normalizedValues);
				if (v == 0f && this._paddingZero) {
					v = this._normalizedValues[1];
				}
				this._slider.value = v;
			}
			float actualValue = this.SliderValueToActualValue(this._slider.value);
			this.OnSetValue?.Invoke(actualValue);
			this._sliderLabel.text = actualValue.ToString("n2");
			base.OnComponentChanged(value);
		}
		#endregion

		#region Internal Methods
		private void BindUIRelation(GameObject instObj) {
			this._titleLabel = instObj.transform.Find(TITLE_NAME).GetComponent<Text>();
			this._sliderLabel = instObj.transform.Find(SLIDER_TEXT_NAME).GetComponent<Text>();
			this._slider = instObj.transform.Find(SLIDER_NAME).GetComponent<Slider>();
		}

		private float SliderValueToActualValue(float value) {
			float upperValue, lowerValue;
			if (this._values != null) {
				upperValue = this._values[this._values.Length - 1];
				lowerValue = this._values[0];
			}
			else {
				upperValue = this._maxValue;
				lowerValue = this._minValue;
			}
			return value * (upperValue - lowerValue) + lowerValue;
		}

		private float ActualValueToSliderValue(float value) {
			float clampValue, upperValue, lowerValue;
			if (this._values != null) {
				upperValue = this._values[this._values.Length - 1];
				lowerValue = this._values[0];
				clampValue = FindNearestValue(value, this._values);
			}
			else {
				upperValue = this._maxValue;
				lowerValue = this._minValue;
				clampValue = Mathf.Clamp(value, this._minValue, this._maxValue);
			}
			return (clampValue - lowerValue) / (upperValue - lowerValue);
		}

		private float FindNearestValue(float value, float[] values) {
			float nearestValue = values[0];
			if(value < nearestValue) {
				return nearestValue;
			}
			for (int i = 1; i < values.Length; i++) {
				float a = values[i] - value;
				float b = value - nearestValue;
				if (a >= 0 && b >= 0) {
					if (a < b) {
						nearestValue = values[i];
					}
					break;
				}
				nearestValue = values[i];
			}
			return nearestValue;
		}
		#endregion
	}
}
