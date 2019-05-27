using UnityEngine;
using UnityEngine.UI;

public class TextSlider : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private string format;

    public void OnSliderValueReceived(float value) { text.text = string.IsNullOrEmpty(format) ? value.ToString() : value.ToString(format); }
}