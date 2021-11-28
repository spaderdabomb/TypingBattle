using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonOverride : MonoBehaviour
{
    MainMenuController mainMenuController;
    GameObject buttonGO;
    Button button;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainMenuController = Core.FindGameObjectByNameAndTag("MainMenuController", "Menu Item").GetComponent<MainMenuController>();
        }

        buttonGO = gameObject;
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(delegate { ButtonClicked(buttonGO); });
    }

    void ButtonClicked(GameObject buttonGO)
    {
        if (buttonGO.name == "SettingsButton")
        {
            mainMenuController.ChangeMenu("Settings");
        }
        else if (buttonGO.name == "AchievementsButton")
        {
            mainMenuController.ChangeMenu("Achievements");
        }
        else if (buttonGO.name == "HighscoresButton")
        {
            mainMenuController.ChangeMenu("Highscores");
        }
        else if (buttonGO.name == "PlayButton")
        {
            SceneManager.LoadScene(sceneName: "Lobby");
        }
        else if (buttonGO.name == "BackToMainMenuButton")
        {
            mainMenuController.ChangeMenu("Main");
        }
        else if (buttonGO.name == "GameSceneMainMenu")
        {
            SceneManager.LoadScene(sceneName: "MainMenu");
        }
        else if (buttonGO.name == "GameSceneLobby")
        {

        }
    }
}
