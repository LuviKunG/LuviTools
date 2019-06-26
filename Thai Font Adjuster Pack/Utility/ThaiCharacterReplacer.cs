using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Use this class for replace Thai Unicode characters.
/// </summary>
[ExecuteInEditMode, DisallowMultipleComponent, RequireComponent(typeof(Text)), AddComponentMenu("LuviKunG/Thai Character Replacer")]
public class ThaiCharacterReplacer : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [TextArea]
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
            text = GetComponent<Text>();
    }
#endif
}
