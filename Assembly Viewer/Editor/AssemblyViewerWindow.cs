using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

public class AssemblyViewerWindow : EditorWindow
{
    private class AssemblyProperty
    {
        public Assembly assembly;
        public bool isShowSourceFiles;

        private readonly GUIContent GUI_NAME;

        public AssemblyProperty()
        {
            isShowSourceFiles = false;
        }

        public AssemblyProperty(Assembly assembly) : this()
        {
            this.assembly = assembly;
            GUI_NAME = new GUIContent(assembly.name);
        }

        public void Render()
        {
            if (GUILayout.Button(GUI_NAME, EditorStyles.toolbarDropDown))
            {
                ToggleView();
            }
            if (isShowSourceFiles)
            {
                using (var verticalScope = new EditorGUILayout.VerticalScope())
                {
                    foreach (string sourceFile in assembly.sourceFiles)
                    {
                        EditorGUILayout.LabelField(sourceFile, EditorStyles.miniLabel);
                    }
                }
            }
        }

        private void ToggleView()
        {
            isShowSourceFiles = !isShowSourceFiles;
        }
    }

    private const string WINDOW_TITLE = "Assembly Viewer";
    private const string EDITOR_PREFS_TYPE = "LuviKunG.AssemblyViewer.Type";

    private AssembliesType type = AssembliesType.Player;
    private List<AssemblyProperty> assemblyPropertyList = new List<AssemblyProperty>();
    private Vector2 scrollPositionViewAssemblies = Vector2.zero;

    public static AssemblyViewerWindow Open()
    {
        AssemblyViewerWindow window = GetWindow<AssemblyViewerWindow>(false, WINDOW_TITLE, true);
        return window;
    }

    private void UpdateAssembliesList()
    {
        assemblyPropertyList ??= new List<AssemblyProperty>();
        assemblyPropertyList.Clear();
        var assemblies = CompilationPipeline.GetAssemblies(type);
        foreach (var assembly in assemblies)
        {
            assemblyPropertyList.Add(new AssemblyProperty(assembly));
        }
    }

    private void OnEnable()
    {
        type = (AssembliesType)EditorPrefs.GetInt(EDITOR_PREFS_TYPE);
        UpdateAssembliesList();
    }

    private void OnGUI()
    {
        using (var toolBarScope = new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            using (var changeCheckScope = new EditorGUI.ChangeCheckScope())
            {
                type = (AssembliesType)EditorGUILayout.EnumPopup(type, EditorStyles.toolbarPopup);
                if (changeCheckScope.changed)
                {
                    EditorPrefs.SetInt(EDITOR_PREFS_TYPE, (int)type);
                    UpdateAssembliesList();
                }
            }
            GUILayout.FlexibleSpace();
        }
        using (var scrollScope = new EditorGUILayout.ScrollViewScope(scrollPositionViewAssemblies))
        {
            scrollPositionViewAssemblies = scrollScope.scrollPosition;
            using (var verticalScope = new EditorGUILayout.VerticalScope())
            {
                foreach (var property in assemblyPropertyList)
                {
                    property.Render();
                }
            }
        }
    }
}
