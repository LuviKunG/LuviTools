using System;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace LuviKunG
{
    public class CaptureScreenshot : MonoBehaviour
    {
        private const string DATETIME_FORMAT_FILENAME = "yyyyMMddHHmmss";
        private const string FILE_FORMAT_EXTENSION = ".png";

        [SerializeField]
        private KeyCode m_key = KeyCode.F2;
        [SerializeField]
        private string m_fileName = "capture";
        [SerializeField, IntPopup(new string[] { "x1", "x2", "x3", "x4" }, new int[] { 1, 2, 3, 4 })]
        private int m_superSize = 1;

        private void LateUpdate()
        {
            if (Input.GetKeyDown(m_key))
            {
                string path = GetFileNameWithDate(m_fileName, FILE_FORMAT_EXTENSION);
                ScreenCapture.CaptureScreenshot(path, m_superSize);
                Debug.Log($"Screenshot captured '{path}'");
            }
        }

        private string GetFileNameWithDate(string fileName, string extension)
        {
            StringBuilder s = new StringBuilder();
            s.Append(fileName);
            s.Append('_');
            s.Append(DateTime.Now.ToString(DATETIME_FORMAT_FILENAME, CultureInfo.InvariantCulture));
            s.Append(extension);
            return s.ToString();
        }
    }
}