using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    //[SerializeField] private Button optionsButton;

    private void Start()
    {
        if(!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        DisableMenuButtons();
        // Create a new game - which will initialize our game data
        DataPersistenceManager.instance.NewGame();

        // Load the gameplay scene - which will in turn save the game because
        // OnSceneUnloaded() in the DataPersistenceManager
        string startingScene = DataPersistenceManager.instance.GetCheckpointScene();

        DataPersistenceManager.instance.PrepareForDataLoad();

        SceneManager.LoadSceneAsync(startingScene);
    }

    public void OnContinuedGameClicked()
    {
        DisableMenuButtons();
        // Load the next scene - which will in turn load the game because of
        // OnSceneUnloaded() in the DataPersistenceManager
        string savedScene = DataPersistenceManager.instance.GetCheckpointScene();

        DataPersistenceManager.instance.PrepareForDataLoad();

        SceneManager.LoadSceneAsync(savedScene);
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
        //optionButton.interactable = false;
    }
}
