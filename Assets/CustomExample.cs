using Analysis.GraphicsTuner;
using UnityEngine;

public class CustomExample : MonoBehaviour {
    private void Start() {
        var tuner = GraphicsTuner.Instance;
        if (tuner == null) return;

        var setting = tuner.CreateCustomSettings("Custom");
        setting.CreateToggle(
            "HDR",
            () => Camera.main.allowHDR,
            (v) => Camera.main.allowHDR = v
        );
    }
}
