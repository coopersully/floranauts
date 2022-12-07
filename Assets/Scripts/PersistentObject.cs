using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentObject : MonoBehaviour
{
    private static readonly Dictionary<string, PersistentObject> PersistentObjects = new();

    private void Start()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }
    
    private void Awake()
    {
        if (PersistentObjects.ContainsKey(gameObject.name))
        {
            Debug.Log("An existing version of " + name + " was found;" + " destroying new object.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("No existing versions of " + name + " were found; keeping conserved object.");

            PersistentObjects.Add(gameObject.name, this);
            DontDestroyOnLoad(gameObject);
        }

    }
    
    private void OnActiveSceneChanged(Scene current, Scene next)
    {
        var sceneName = SceneManager.GetActiveScene().name;
        if (!sceneName.ToLower().Contains("game")) return;

        foreach (var persistentObject in PersistentObjects)
        {
            SceneManager.MoveGameObjectToScene(persistentObject.Value.gameObject, SceneManager.GetActiveScene());
            PersistentObjects.Remove(persistentObject.Value.gameObject.name);
        }
    }

}