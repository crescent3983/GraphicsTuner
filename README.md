# Graphics Tuner

[中文版說明](./README_tc.md)

Quick tuning graphics settings of Unity.

## Usage

1. Add to Scene

    ![Create](https://drive.google.com/uc?export=view&id=1-RWssutdqQGh07oAmxPpye9QzmPho7hI)

    ![Anchor](https://drive.google.com/uc?export=view&id=1-UlA229H6S5AUxtuUQTsvsj0CV3MvtBa)

2. Play

    ![Run](https://drive.google.com/uc?export=view&id=1-948SZMqx7qUB6GhPghFAb-IDD7WH1xl)

## Customization

You can change the layout or add new components depend on your need.

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

### Module Layout

```csharp
public void SetActive(bool active);
public void SetAnchor(ComponentAnchor anchor);
```

```csharp
// Change setting of built-in modules
var tuner = GraphicsTuner.Instance;
tuner.BasicSetting.SetActive(false);
tuner.TierSetting.SetActive(false);
tuner.QualitySetting.SetAnchor(ComponentAnchor.Left);
```

### Components

+ UIConsoleSlider

    ```csharp
    UIConsoleSlider CreateSlider(string title, float[] values, Func<float> getter, Action<float> setter, Action<float> onChange = null);
    ```

    ![Slider](https://drive.google.com/uc?export=view&id=1-YOO3ARN8NoR1iEgNpYLdtxG25Y_ZvlR)

+ UIConsoleDropdown

    ```csharp
    UIConsoleDropdown CreateDropdown(string title, string[] values, Func<int> getter, Action<int> setter, Action<int> onChange = null);
    UIConsoleDropdown CreateDropdown(string title, Type type, Func<int> getter, Action<int> setter, Action<int> onChange = null);
    ```

    ![Dropdown](https://drive.google.com/uc?export=view&id=1-_NNuh-oPGgdcZLfaaCpfLt1NRUuyHBh)

+ UIConsoleToggle

    ```csharp
    UIConsoleToggle CreateToggle(string title, Func<bool> getter, Action<bool> setter, Action<bool> onChange = null);
    ```

    ![Toggle](https://drive.google.com/uc?export=view&id=1-b4Nl9Xh_6mUXsJ1SjsQ6YDuQ20smzJ2)

+ UIConsoleLabel

    ```csharp
    UIConsoleLabel CreateLabel(string title, out Action<string> setter);
    ```

    ![Label](https://drive.google.com/uc?export=view&id=1-gbkinp6k4Bd_6d4Q9pJ84CQiGMjfxFI)
