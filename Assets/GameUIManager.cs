using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameUIManager : BritoBehavior
    {
        public static GameUIManager instance;
        public GameObject pausePanel;
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
            CloseInventory();
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

        public static void OpenItemPanel()
        { 
            EnablePausePanel();
            UIInventoryMenu.DisplayItemPage();
        }
        public static void OpenEquipmentPanel()
        {
            EnablePausePanel();
            UIInventoryMenu.DisplayEquipmentPage();
        }
        public static void CloseInventory()
        {
            UIInventoryMenu.Close();
        }
    }
}
