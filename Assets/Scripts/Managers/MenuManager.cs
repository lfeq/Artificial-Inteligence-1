using UnityEngine;

/// <summary>
/// MenuManager class manages the main menu functionalities, including starting the game and exiting.
/// </summary>
public class MenuManager : MonoBehaviour {

    #region Unity functions

    private void Start() {
        GameManager.s_instance.changeGameSate(GameState.MainMenu);
    }

    #endregion Unity functions

    #region Public functions

    /// <summary>
    /// Initiates the start of the game.
    /// </summary>
    public void startGame() {
        GameManager.s_instance.setNewLevelName("Game Scene");
        GameManager.s_instance.changeGameSate(GameState.LoadLevel);
    }

    /// <summary>
    /// Exits the game.
    /// </summary>
    public void exitGame() {
        GameManager.s_instance.changeGameSate(GameState.QuitGame);
    }

    #endregion Public functions
}