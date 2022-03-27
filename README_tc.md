# Graphics Tuner

快速調用Unity圖形參數，測試遊戲性能。

## 使用方法

1. 新增到場景

    ![Create](./readme_assets/create_instance.png)

    ![Anchor](./readme_assets/switch_anchor.png)

2. 執行遊戲

    ![Run](./readme_assets/graphics_tuner.png)

## 客製化

可以根據使用情況，調整版面顯示或新增元件。

```csharp
void Start() {
    var tuner = GraphicsTuner.Instance;
    if (tuner == null) return;

    var setting = tuner.CreateCustomSettings("Custom");
    setting.CreateToggle(
        "HDR",
        () => Camera.main.allowHDR,
        (v) => Camera.main.allowHDR = v
    );
}
```

### 模組開關與位置

```csharp
public void SetActive(bool active);
public void SetAnchor(ComponentAnchor anchor);
```

```csharp
// 調整內建模組
var tuner = GraphicsTuner.Instance;
tuner.BasicSetting.SetActive(false);
tuner.TierSetting.SetActive(false);
tuner.QualitySetting.SetAnchor(ComponentAnchor.Left);
```

### 新增元件

+ UIConsoleSlider

    ```csharp
    UIConsoleSlider CreateSlider(string title, float[] values, Func<float> getter, Action<float> setter, Action<float> onChange = null);
    ```

    ![Slider](./readme_assets/slider.png)

+ UIConsoleDropdown

    ```csharp
    UIConsoleDropdown CreateDropdown(string title, string[] values, Func<int> getter, Action<int> setter, Action<int> onChange = null);
    UIConsoleDropdown CreateDropdown(string title, Type type, Func<int> getter, Action<int> setter, Action<int> onChange = null);
    ```

    ![Dropdown](./readme_assets/dropdown.png)

+ UIConsoleToggle

    ```csharp
    UIConsoleToggle CreateToggle(string title, Func<bool> getter, Action<bool> setter, Action<bool> onChange = null);
    ```

    ![Toggle](./readme_assets/toggle.png)

+ UIConsoleLabel

    ```csharp
    UIConsoleLabel CreateLabel(string title, out Action<string> setter);
    ```

    ![Label](./readme_assets/label.png)
