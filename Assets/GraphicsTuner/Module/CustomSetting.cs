namespace Analysis.GraphicsTuner.Module {
	public sealed class CustomSetting : SettingModule {

		public CustomSetting(GraphicsTuner tuner, ComponentAnchor anchor, string title) : base(tuner, anchor) {
			this.name = title;
			this.CreateTitle(title);
		}

	}
}
