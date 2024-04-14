
using Game;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 

public enum SceneIndex
{
    DEFAULT,
    SCENE_A, SCENE_B, SCENE_C
}

public class SceneController : BritoBehavior
{
    public List<SceneIndex> loadedScenes;

    public void UnloadScene(SceneIndex index)
    {
        if (!loadedScenes.Contains(index))
            throw new System.Exception($"{index} is not even loaded!? Cannot unload!");
        loadedScenes.Remove(index);
        SceneManager.UnloadSceneAsync((int)index);
    }

    public void LoadScene(SceneIndex index)
    {
        if (loadedScenes.Contains(index))
            throw new System.Exception($"{index} already exists!? Can't load that!");
        loadedScenes.Add(index);
        SceneManager.LoadSceneAsync((int)index);
    }
}
