using UnityEngine;
using UnityEngine.UI;
using System.Text;

[RequireComponent(typeof(Text)), DisallowMultipleComponent, AddComponentMenu("LuviKunG/Benchmark/FPS Meter")]
public class FPSMeter : MonoBehaviour
{
    [Header("Components")]
    public Text text;
    [Header("Colors")]
    public Color colorGood = new Color(0, 1, 0, 1);
    public Color colorWarn = new Color(1, 1, 0, 1);
    public Color colorBad = new Color(1, 0, 0, 1);

    StringBuilder sb = new StringBuilder();
    float currentFPS;

    void Update()
    {
        currentFPS = 1f / Time.deltaTime;
        if (currentFPS < 15.0f)
            text.color = colorBad;
        else if (currentFPS < 30.0f)
            text.color = colorWarn;
        else
            text.color = colorGood;
        sb.Clear();
        sb.Append("FPS: ");
        sb.Append(currentFPS);
        text.text = sb.ToString();
    }

#if UNITY_EDITOR
    private void Reset()
    {
        if (text == null)
        {
            text = GetComponent<Text>();
            if (text != null)
                text.text = "FPS: ?";
        }
    }
#endif
}
