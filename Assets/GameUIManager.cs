using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameUIManager : BritoBehavior
    {
        public static GameUIManager instance;
        public GameObject pausePanel;

        public GameObject inventoryPanel;
        private void Awake()
        {
            instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            ClosePauseMenu();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void ClosePauseMenu()
        {
            CloseInventoryPanel();
            pausePanel.SetActive(false);
            if(GameSystem.Paused)
                GameSystem.ResumeGame();
        }
        public void OpenPauseMenu()
        {
            pausePanel.SetActive(true);
            if (!GameSystem.Paused)
                GameSystem.PauseGame();
        }
        public  void TogglePauseMenu()
        {
            if (pausePanel.activeSelf) ClosePauseMenu();
            else OpenPauseMenu();
        }
        public static void DisablePausePanel() => instance.ClosePauseMenu();
        public static void EnablePausePanel() => instance.OpenPauseMenu();
        public static void TogglePause() => instance.TogglePauseMenu();
        public void OpenInventory()
        { 
            EnablePausePanel();
            inventoryPanel.SetActive(true);
        }
        public void CloseInventory()
        {
            inventoryPanel.SetActive(false);
        }
        public void ToggleInventory()
        {
            if (inventoryPanel.activeSelf) CloseInventory();
            else OpenInventory();
        }
        public static void OpenInventoryPanel() => instance.OpenInventory();
        public static void CloseInventoryPanel() => instance.CloseInventory();
        public static void ToggleInventoryPanel() => instance.ToggleInventory();
    }
}
