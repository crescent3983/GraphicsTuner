using Analysis.GraphicsTuner.Module;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Analysis.GraphicsTuner {
	[CustomEditor(typeof(GraphicsTuner))]
	public class GraphicsTunerInspector : Editor {

		private const int PADDING = 30;
		private const int TOGGLE_WIDTH = 100;

		public override void OnInspectorGUI() {
			this.DrawSwitchAnchor();
			this.DrawModuleSetting();
		}

		private void DrawSwitchAnchor() {
			var switchBtn = (Button)serializedObject.FindProperty("switchBtn").objectReferenceValue;
			var rect = switchBtn.GetComponent<RectTransform>();

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.LabelField("Anchor", EditorStyles.toolbarButton);
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			// Top Left
			if (GUILayout.Toggle(rect.anchorMin == Vector2.up && rect.anchorMax == Vector2.up, "Top-Left", EditorStyles.toolbarButton, GUILayout.Width(TOGGLE_WIDTH))) {
				rect.anchorMin = Vector2.up;
				rect.anchorMax = Vector2.up;
				rect.anchoredPosition = new Vector3(PADDING, -PADDING);
			}
			// Top Right
			if (GUILayout.Toggle(rect.anchorMin == Vector2.one && rect.anchorMax == Vector2.one, "Top-Right", EditorStyles.toolbarButton, GUILayout.Width(TOGGLE_WIDTH))) {
				rect.anchorMin = Vector2.one;
				rect.anchorMax = Vector2.one;
				rect.anchoredPosition = new Vector3(-PADDING, -PADDING);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			// Bottom Left
			if (GUILayout.Toggle(rect.anchorMin == Vector2.zero && rect.anchorMax == Vector2.zero, "Bottom-Left", EditorStyles.toolbarButton, GUILayout.Width(TOGGLE_WIDTH))) {
				rect.anchorMin = Vector2.zero;
				rect.anchorMax = Vector2.zero;
				rect.anchoredPosition = new Vector3(PADDING, PADDING);
			}
			// Bottom Right
			if (GUILayout.Toggle(rect.anchorMin == Vector2.right && rect.anchorMax == Vector2.right, "Bottom-Right", EditorStyles.toolbarButton, GUILayout.Width(TOGGLE_WIDTH))) {
				rect.anchorMin = Vector2.right;
				rect.anchorMax = Vector2.right;
				rect.anchoredPosition = new Vector3(-PADDING, PADDING);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();
		}

		private void DrawModuleSetting() {
			var tuner = (GraphicsTuner)this.target;
			if (tuner.Modules != null) {
				for (int i = 0; i < tuner.Modules.Count; i++) {
					this.DrawModuleSetting(tuner.Modules[i]);
				}
			}
		}

		private void DrawModuleSetting(SettingModule module) {
			if (module == null) return;

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.LabelField(module.name, EditorStyles.toolbarButton);
			EditorGUILayout.Space();

			var active = EditorGUILayout.Toggle("Enable", module.isActive);
			if (active != module.isActive) {
				module.SetActive(active);
			}

			var anchor = (ComponentAnchor)EditorGUILayout.EnumPopup("Anchor", module.anchor);
			if (anchor != module.anchor) {
				module.SetAnchor(anchor);
			}

			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();
		}
	}
}
