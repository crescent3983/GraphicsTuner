using Analysis.GraphicsTuner.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Analysis.GraphicsTuner.Module;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Analysis.GraphicsTuner {

	public enum ComponentAnchor {
		Left,
		Right,
	}

	public class GraphicsTuner : MonoBehaviour {

		[SerializeField] private Button switchBtn;
		[SerializeField] private GameObject content;
		[SerializeField] private Transform leftPanel;
		[SerializeField] private Transform rightPanel;

		[SerializeField] private GameObject titleRes;
		[SerializeField] private GameObject toggleRes;
		[SerializeField] private GameObject dropdownRes;
		[SerializeField] private GameObject sliderRes;
		[SerializeField] private GameObject labelRes;

		public bool isActive => this.content.activeSelf;

		public List<SettingModule> Modules { get; private set; }
		public BasicSetting BasicSetting { get; private set; }
		public TierSetting TierSetting { get; private set; }
		public QualitySetting QualitySetting { get; private set; }

		private static GraphicsTuner _instance;
		public static GraphicsTuner Instance {
			get {
				if (_instance == null) {
					_instance = FindObjectOfType<GraphicsTuner>();
				}
				return _instance;
			}
		}

#if UNITY_EDITOR
		[MenuItem("GameObject/Create Other/Graphics Tuner", false)]
		private static void CreateInstance() {
			if(Instance == null) {
				var prefabPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("\"Graphics Tuner\" t:prefab")[0]);
				var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
				Debug.Log(prefab);
				PrefabUtility.InstantiatePrefab(prefab);
			}
		}
#endif

		#region MonoBehaviour Hooks
		private void Awake() {
			_instance = this;

			this.switchBtn.onClick.AddListener(() =>
			{
				this.content.SetActive(!this.content.activeSelf);
				var rect = this.switchBtn.GetComponent<RectTransform>();
				rect.localScale = new Vector3(rect.localScale.x, -rect.localScale.y, rect.localScale.z);
			});

			if (FindObjectOfType<EventSystem>() == null) {
				var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
				DontDestroyOnLoad(eventSystem);
			}

			this.BasicSetting = new BasicSetting(this, ComponentAnchor.Left);
			this.TierSetting = new TierSetting(this, ComponentAnchor.Left);
			this.QualitySetting = new QualitySetting(this, ComponentAnchor.Left);

			this.Modules = new List<SettingModule>();
			this.Modules.Add(this.BasicSetting);
			this.Modules.Add(this.TierSetting);
			this.Modules.Add(this.QualitySetting);
		}

		private void Start() {
			DontDestroyOnLoad(transform.root.gameObject);
		}
		#endregion

		#region API
		public CustomSetting CreateCustomSettings(string title, ComponentAnchor anchor = ComponentAnchor.Right) {
			var setting = new CustomSetting(this, anchor, title);
			this.Modules.Add(setting);
			return setting;
		}

		public void DestroySetting(SettingModule setting) {
			setting.Destroy();
			this.Modules.Remove(setting);
		}

		public void SetComponentAnchor(IConsoleComponent comp, ComponentAnchor anchor) {
			var obj = comp.GetInst();
			if (obj) {
				obj.transform.SetParent(
					anchor == ComponentAnchor.Left ? this.leftPanel : this.rightPanel,
					false
				);
			}
		}

		public void RefreshUI() {
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.leftPanel.GetComponent<RectTransform>());
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.rightPanel.GetComponent<RectTransform>());
		}
		#endregion

		#region Component Creation
		internal UIConsoleSlider CreateSlider(string title, float[] values, Func<float> getter, Action<float> setter) {
			UIConsoleSlider slider = new UIConsoleSlider(title, values, getter, setter);
			GameObject sliderObj = GameObject.Instantiate(this.sliderRes);
			slider.SetUI(sliderObj);
			return slider;
		}

		internal UIConsoleDropdown CreateDropdown(string title, string[] values, Func<int> getter, Action<int> setter) {
			UIConsoleDropdown dropdown = new UIConsoleDropdown(title, values, getter, setter);
			GameObject dropdownObj = GameObject.Instantiate(this.dropdownRes);
			dropdown.SetUI(dropdownObj);
			return dropdown;
		}

		internal UIConsoleDropdown CreateDropdown(string title, Type type, Func<int> getter, Action<int> setter) {
			UIConsoleDropdown dropdown = new UIConsoleDropdown(title, type, getter, setter);
			GameObject dropdownObj = GameObject.Instantiate(this.dropdownRes);
			dropdown.SetUI(dropdownObj);
			return dropdown;
		}

		internal UIConsoleToggle CreateToggle(string title, Func<bool> getter, Action<bool> setter) {
			UIConsoleToggle toggle = new UIConsoleToggle(title, getter, setter);
			GameObject toggleObj = GameObject.Instantiate(this.toggleRes);
			toggle.SetUI(toggleObj);
			return toggle;
		}

		internal UIConsoleLabel CreateLabel(string title) {
			UIConsoleLabel label = new UIConsoleLabel(title);
			GameObject labelObj = GameObject.Instantiate(this.labelRes);
			label.SetUI(labelObj);
			return label;
		}

		internal UIConsoleTitle CreateTitle(string name) {
			UIConsoleTitle title = new UIConsoleTitle(name);
			GameObject titleObj = GameObject.Instantiate(this.titleRes);
			title.SetUI(titleObj);
			return title;
		}
		#endregion
	}
}
