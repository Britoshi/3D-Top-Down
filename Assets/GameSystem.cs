using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

    public static bool Paused { internal set; get; }



    private static float previousTimeScale;

    private void Awake()
    {
        Instance = this;
        Paused = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void PauseGame()
    {
        Paused = true;
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        GameUIManager.EnablePausePanel();
        //pauseMenu.SetActive(true);
    }

    public static void ResumeGame()
    {
        Paused = false;
        Time.timeScale = previousTimeScale;
        if (Time.timeScale < 0.01f) Time.timeScale = 1;
        GameUIManager.DisablePausePanel();
        //pauseMenu.SetActive(false);
    }
}
