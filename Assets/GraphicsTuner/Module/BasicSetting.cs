using Analysis.GraphicsTuner.UI;
using System;
using System.Linq;
using UnityEngine;

namespace Analysis.GraphicsTuner.Module {
	public sealed class BasicSetting : SettingModule {

		private static readonly int[] resolutions = new int[] { 480, 576, 720, 900, 1080, 1440 };
		private static readonly float[] fps = new float[] { 30f, 60f, 90f, 120f };
		private static readonly float[] shaderLODs = new float[] { 100f, 150f, 200f, 250f, 300f, 400f, 500f, 600f };

		private UIConsoleSlider _fpsComp;
		private UIConsoleDropdown _resolutionComp;
		private UIConsoleSlider _shaderLODComp;

		public event Action<float> OnFPSChange { add => this._fpsComp.OnChange += value; remove => this._fpsComp.OnChange -= value; }
		public event Action<int> OnResolutionChange { add => this._resolutionComp.OnChange += value; remove => this._resolutionComp.OnChange -= value; }
		public event Action<float> OnShaderLODChange { add => this._shaderLODComp.OnChange += value; remove => this._shaderLODComp.OnChange -= value; }

		public BasicSetting(GraphicsTuner tuner, ComponentAnchor anchor) : base(tuner, anchor) {
			this.name = "BASIC SETTINGS";
			this.SetupComponents();
		}

		private void SetupComponents() {
			this.CreateTitle(this.name);

			this._fpsComp = this.CreateSlider(
				"Max FPS",
				fps,
				() => Application.targetFrameRate,
				(v) => Application.targetFrameRate = (int)v
			);

			this._resolutionComp = this.CreateDropdown(
				"Resolution",
				resolutions.Select(x => x.ToString() + "p").ToArray(),
				() => {
					float height = Screen.height;
					int index = 0;
					if (height < resolutions[index]) {
						return index;
					}
					for (int i = 1; i < resolutions.Length; i++) {
						if (resolutions[i] >= height && resolutions[index] <= height) {
							if (resolutions[i] - height < height - resolutions[index]) {
								index = i;
							}
							break;
						}
						index = i;
					}
					return index;
				},
				(v) => {
					int height = resolutions[v];
					Utility.SetResolution(height);
				}
			);

			this._shaderLODComp = this.CreateSlider(
				"Shader LOD",
				shaderLODs,
				() => Shader.globalMaximumLOD,
				(v) => Shader.globalMaximumLOD = (int)v
			);
		}
	}
}
