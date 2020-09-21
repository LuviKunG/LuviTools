using LuviKunG;
using TMPro;
using UnityEngine;

namespace LuviKunG
{
    [AddComponentMenu("UI/Preprocess Thai Text (TMPro)")]
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PreprocessThaiTMPro : MonoBehaviour, ITextPreprocessor
    {
        public TextMeshProUGUI component;
        public bool enableLexTo;
        public bool enableThaiFontAdjuster;

        private LexTo lexto;

        private void Awake()
        {
            lexto = lexto ?? LexTo.Instance;
        }

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            component.textPreprocessor = this;
        }

        public string PreprocessText(string text)
        {
            string str = text;
            if (enableLexTo)
                str = LexTo.Instance.ParseToString(str);
            if (enableThaiFontAdjuster)
                str = ThaiFontAdjuster.Adjust(str);
            return str;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (component == null)
                component = GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}