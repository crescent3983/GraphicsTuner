using Analysis.GraphicsTuner.UI;
using System;
using UnityEngine;

namespace Analysis.GraphicsTuner.Module {
	public sealed class QualitySetting : SettingModule {

		private static readonly int[] msaa = new int[] { 1, 2, 4, 8 };

		private UIConsoleDropdown _blendWeightComp;
		private UIConsoleDropdown _textureSizeComp;
		private UIConsoleDropdown _anisoFilterComp;
		private UIConsoleDropdown _multiSampleComp;

		public event Action<int> OnBlendWeightChange { add => this._blendWeightComp.OnChange += value; remove => this._blendWeightComp.OnChange -= value; }
		public event Action<int> OnTextureSizeChange { add => this._textureSizeComp.OnChange += value; remove => this._textureSizeComp.OnChange -= value; }
		public event Action<int> OnAnisoFilterChange { add => this._anisoFilterComp.OnChange += value; remove => this._anisoFilterComp.OnChange -= value; }
		public event Action<int> OnMultiSampleChange { add => this._multiSampleComp.OnChange += value; remove => this._multiSampleComp.OnChange -= value; }


		public QualitySetting(GraphicsTuner tuner, ComponentAnchor anchor) : base(tuner, anchor) {
			this.name = "QUALITY SETTINGS";
			this.SetupComponents();
		}

		private void SetupComponents() {
			this.CreateTitle(this.name);

			this._blendWeightComp = this.CreateDropdown(
				"Blend Weights",
#if UNITY_2019_1_OR_NEWER
				typeof(SkinWeights),
				() => (int)QualitySettings.skinWeights,
				(v) => QualitySettings.skinWeights = (SkinWeights)v
#else
				typeof(BlendWeights),
				() => (int)QualitySettings.blendWeights,
				(v) => QualitySettings.blendWeights = (BlendWeights)v
#endif
			);

			this._textureSizeComp = this.CreateDropdown(
				"Texture Size",
				new string[] { "Full", "Half", "Quarter", "Eighth" },
				() => QualitySettings.masterTextureLimit,
				(v) => QualitySettings.masterTextureLimit = v
			);

			this._anisoFilterComp = this.CreateDropdown(
				"Aniso Filtering",
				typeof(AnisotropicFiltering),
				() => (int)QualitySettings.anisotropicFiltering,
				(v) => QualitySettings.anisotropicFiltering = (AnisotropicFiltering)v
			);

			this._multiSampleComp = this.CreateDropdown(
				"Multi Sampling",
				new string[] { "None", "2x", "4x", "8x" },
				() => {
					for (int i = 0; i < msaa.Length; i++) {
						if (QualitySettings.antiAliasing == msaa[i]) {
							return i;
						}
					}
					return 0;
				},
				(v) => QualitySettings.antiAliasing = msaa[v]
			);
		}
	}
}
