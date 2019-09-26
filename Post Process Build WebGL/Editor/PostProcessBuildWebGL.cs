using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

namespace LuviKunG.PostProcessBuild.WebGL
{
    public static class PostProcessBuildWebGL
    {
        [PostProcessBuild(0)]
        public static void OnPostProcessBuild(BuildTarget target, string targetPath)
        {
            if (target == BuildTarget.WebGL)
            {
                try
                {
                    var path = Path.Combine(targetPath, "Build/UnityLoader.js");
                    var text = File.ReadAllText(path);
                    text = text.Replace("UnityLoader.SystemInfo.mobile", "false");
                    File.WriteAllText(path, text);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }
    }
}