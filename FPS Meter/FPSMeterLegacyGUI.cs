using System.Text;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("LuviKunG/Benchmark/FPS Meter (Legacy GUI)")]
public class FPSMeterLegacyGUI : MonoBehaviour
{
    private static readonly Rect RECT_POSITION = new Rect(8.0f, 8.0f, 128.0f, 32.0f);

    [Header("Colors")]
    [SerializeField]
    private Color colorGood = new Color(0, 1, 0, 1);
    [SerializeField]
    private Color colorWarn = new Color(1, 1, 0, 1);
    [SerializeField]
    private Color colorBad = new Color(1, 0, 0, 1);

    private float currentFPS;

    private void OnGUI()
    {
        currentFPS = 1f / Time.deltaTime;
        if (currentFPS < 15.0f)
            GUI.color = colorBad;
        else if (currentFPS < 30.0f)
            GUI.color = colorWarn;
        else
            GUI.color = colorGood;
        GUI.Label(RECT_POSITION, currentFPS.ToString("N2"));
    }
}
