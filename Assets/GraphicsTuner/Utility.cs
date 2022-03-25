using System;
using UnityEngine;

#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

namespace Analysis.GraphicsTuner {
	public static class Utility {

		public static void SetResolution(int height, bool useDeviceRatio = true) {
			SetResolution(Mathf.RoundToInt(height * GetScreenRatio(useDeviceRatio)), height);
		}

		public static void SetResolution(int width, int height) {
			if (width <= 0 || height <= 0) {
				return;
			}

#if UNITY_EDITOR
			int index = FindViewSize(width, height);
			if (index == -1) {
				AddCustomViewSize(width, height, string.Format("{0}x{1}", width, height));
				index = FindViewSize(width, height);
			}
			SetEditorViewSize(index);
#else
			FullScreenMode mode = Screen.fullScreenMode;
			if(width > Display.main.systemWidth || height > Display.main.systemHeight) {
				width = Display.main.systemWidth;
				height = Display.main.systemHeight;
			}
			Screen.SetResolution(width, height, mode);
#endif
		}

		public static float GetScreenRatio(bool useDeviceRatio = true) {
#if UNITY_EDITOR
			return GetEditorViewRatio();
#else
		if (useDeviceRatio) {
			return (float)Display.main.systemWidth / Display.main.systemHeight;
		}
		else {
			return (float)Screen.width / Screen.height;
		}
#endif
		}

#if UNITY_EDITOR
		private const int FIXED_RESOLUTION = 1;

		private static object _gameViewSizesInstance;
		private static object gameViewSizesInstance {
			get {
				if (_gameViewSizesInstance == null) {
					var gameViewSizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
					var singleType = typeof(ScriptableSingleton<>).MakeGenericType(gameViewSizesType);
					var instanceProp = singleType.GetProperty("instance");
					_gameViewSizesInstance = instanceProp.GetValue(null, null);
				}
				return _gameViewSizesInstance;
			}
		}

		private static float GetEditorViewRatio() {
			var gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
			var currentGameViewSize = gameViewType.GetProperty("currentGameViewSize", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			var gameViewWindow = EditorWindow.GetWindow(gameViewType);
			var gameViewSize = currentGameViewSize.GetValue(gameViewWindow);

			var gameViewSizeType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
			var widthProp = gameViewSizeType.GetProperty("width", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			var heightProp = gameViewSizeType.GetProperty("height", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			var gameWidth = (int)widthProp.GetValue(gameViewSize);
			var gameHeight = (int)heightProp.GetValue(gameViewSize);
			if (gameWidth <= 0 || gameHeight <= 0) {
				return 16f / 9f;
			}
			else {
				return (float)gameWidth / gameHeight;
			}
		}

		private static void SetEditorViewSize(int index) {
			var gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
			var selectedSizeIndexProp = gameViewType.GetProperty("selectedSizeIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			var updateZoomAreaAndParent = gameViewType.GetMethod("UpdateZoomAreaAndParent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			var gameViewWindow = EditorWindow.GetWindow(gameViewType);
			selectedSizeIndexProp.SetValue(gameViewWindow, index, null);
			updateZoomAreaAndParent.Invoke(gameViewWindow, null);
		}

		private static void AddCustomViewSize(int width, int height, string text) {
			var group = GetCurrentViewGroup();
			var groupType = group.GetType();
			var addCustomSize = groupType.GetMethod("AddCustomSize");
			var gameViewSizeType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
			var gameViewSizeTypeType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizeType");
			var ctor = gameViewSizeType.GetConstructor(new Type[] { gameViewSizeTypeType, typeof(int), typeof(int), typeof(string) });
			var newSize = ctor.Invoke(new object[] { FIXED_RESOLUTION, width, height, text });
			addCustomSize.Invoke(group, new object[] { newSize });
		}

		private static int FindViewSize(int width, int height, bool allowDeviation = true) {
			var group = GetCurrentViewGroup();
			var groupType = group.GetType();
			var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
			var getCustomCount = groupType.GetMethod("GetCustomCount");
			int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
			var getGameViewSize = groupType.GetMethod("GetGameViewSize");
			var gameViewSizeType = getGameViewSize.ReturnType;
			var widthProp = gameViewSizeType.GetProperty("width");
			var heightProp = gameViewSizeType.GetProperty("height");
			var sizeTypeProp = gameViewSizeType.GetProperty("sizeType");
			var indexValue = new object[1];
			for (int i = 0; i < sizesCount; i++) {
				indexValue[0] = i;
				var size = getGameViewSize.Invoke(group, indexValue);
				int sizeWidth = (int)widthProp.GetValue(size, null);
				int sizeHeight = (int)heightProp.GetValue(size, null);
				int sizeType = (int)sizeTypeProp.GetValue(size, null);
				if (sizeType == FIXED_RESOLUTION) {
					if (sizeWidth == width && sizeHeight == height) {
						return i;
					}
					else if (allowDeviation) {
						float ratio1 = (float)width / height;
						float ratio2 = (float)sizeWidth / sizeHeight;

						float widthError = Mathf.Abs(1f - (float)width / sizeWidth);
						float heightError = Mathf.Abs(1f - (float)height / sizeHeight);

						if (Mathf.Abs(ratio1 - ratio2) < 0.01 && widthError < 0.01 && heightError < 0.01) {
							return i;
						}
					}
				}
			}
			return -1;
		}

		private static object GetCurrentViewGroup() {
			var currentGroupProp = gameViewSizesInstance.GetType().GetProperty("currentGroup");
			return currentGroupProp.GetValue(gameViewSizesInstance, null);
		}
#endif
	}
}