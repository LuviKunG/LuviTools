using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.ProjectWindowCallback;

namespace UnityEditor.ProjectWindowCallback
{
    internal class CreateCacheBehaviourAssetAction : EndNameEditAction
    {
        public override void Action(int instanceId, string path, string source)
        {
            string className = Path.GetFileNameWithoutExtension(path);
            source = source.Replace("#CLASSNAME#", className);
            File.WriteAllText(path, source);
            AssetDatabase.ImportAsset(path);
            Object o = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            ProjectWindowUtil.ShowCreatedAsset(o);
        }
    }
}

namespace LuviKunG
{
	public class CacheBehaviourScriptCreator
    {
		const string basicSource =
@"using UnityEngine;
using LuviKunG;

public class #CLASSNAME# : CacheBehaviour
{
    
}";
		const string advanceSource =
@"using UnityEngine;
using LuviKunG;
using System;
using System.Collections;
using System.Collections.Generic;

public class #CLASSNAME# : CacheBehaviour
{
    void Awake()
    {
        
    }
    
    IEnumerable Start()
    {
        yield break;
    }
    
    void Update()
    {
        
    }
}";
        const string instanceSource =
@"using UnityEngine;
using LuviKunG;

public class #CLASSNAME# : CacheBehaviourInstance<#CLASSNAME#>
{
    
}";
        private static Texture2D ScriptIcon { get { return EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D; } }
        [MenuItem("Assets/Create/LuviKunG/Basic CacheBehaviour", false, 1)]
        public static void CreateBasicScript()
        {
            string path = GetNewScriptPath("NewBasicCacheBehaviour");
            CreateScript(path, basicSource);
        }
        [MenuItem("Assets/Create/LuviKunG/Advance CacheBehaviour", false, 2)]
        public static void CreateAdvanceScript()
        {
            string path = GetNewScriptPath("NewAdvanceCacheBehaviour");
            CreateScript(path, advanceSource);
        }
        [MenuItem("Assets/Create/LuviKunG/Instance CacheBehaviour", false, 3)]
        public static void CreateInstanceScript()
        {
            string path = GetNewScriptPath("NewCacheBehaviourInstance");
            CreateScript(path, instanceSource);
        }
        private static string GetNewScriptPath(string scriptName)
        {
            string path = "Assets";
            if (Selection.activeObject != null)
            {
                path = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (!AssetDatabase.IsValidFolder(path))
                    path = Path.GetDirectoryName(path);
            }
            return Path.Combine(path, string.Concat(scriptName, ".cs"));
        }
		private static void CreateScript( string path, string source )
		{
            CreateCacheBehaviourAssetAction createScriptAssetAction = ScriptableObject.CreateInstance<CreateCacheBehaviourAssetAction>();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, createScriptAssetAction, path, ScriptIcon, source);
		}
	}
}
