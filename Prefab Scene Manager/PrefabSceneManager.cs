using UnityEngine;
using System.Collections;

public class PrefabSceneManager : MonoBehaviour
{
    public static PrefabSceneManager Instance = null;

    [SerializeField]
    private PrefabSceneList sceneList;

    private GameObject currentScene;

    void Awake() { if (!Instance) Instance = this; }

    public void LoadScene(string sceneName)
    {
        if (currentScene != null) Destroy(currentScene);
        foreach (PrefabSceneProperty s in sceneList.list)
        {
            if (s.name == sceneName)
            {
                currentScene = Instantiate(s.scene) as GameObject;
                currentScene.name = s.scene.name;
                break;
            }
        }
    }

    void OnDestroy() { Instance = null; }
}
