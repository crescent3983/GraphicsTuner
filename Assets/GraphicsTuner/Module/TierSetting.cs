using Analysis.GraphicsTuner.UI;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Analysis.GraphicsTuner.Module {
	public sealed class TierSetting : SettingModule {

		private UIConsoleDropdown _graphicsTierComp;
		public event Action<int> OnGraphicsTierChange { add => this._graphicsTierComp.OnChange += value; remove => this._graphicsTierComp.OnChange -= value; }

		public TierSetting(GraphicsTuner tuner, ComponentAnchor anchor) : base(tuner, anchor) {
			this.name = "GRAPHICS TIER SETTINGS";
			this.SetupComponents();
		}

		private void SetupComponents() {
			this.CreateTitle(this.name);

			this._graphicsTierComp = this.CreateDropdown(
				"Graphics Tier",
				typeof(GraphicsTier),
				() => (int)Graphics.activeTier,
				(v) => Graphics.activeTier = (GraphicsTier)v
			);
		}
	}
}
