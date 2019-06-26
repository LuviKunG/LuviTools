using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Use this class for replace Thai Unicode characters.
/// </summary>
[ExecuteInEditMode, DisallowMultipleComponent, RequireComponent(typeof(TextMeshProUGUI)), AddComponentMenu("LuviKunG/Thai Character Replacer (Text Mesh Pro)")]
public class ThaiCharacterReplacerTMP : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text = default;
    [TextArea(1, 3)]
    public string message;

    private void Update()
    {
        if (text == null) return;
        if (string.IsNullOrEmpty(message))
        {
            text.text = "";
        }
        else
        {
            text.text = ThaiFontAdjuster.Adjust(message);
        }
    }

#if UNITY_EDITOR
    private void Reset()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();
    }
#endif
}
