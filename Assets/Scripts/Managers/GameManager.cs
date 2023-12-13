using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager class manages the overall game state and transitions between different scenes.
/// </summary>
public class GameManager : MonoBehaviour {

    #region Public variables

    public static GameManager s_instance;

    #endregion Public variables

    #region Private variables

    private GameState m_gameState;
    private string m_newLevel;

    #endregion Private variables

    #region Unity functions

    private void Awake() {
        if (s_instance != null && s_instance != this) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        s_instance = this;
        m_gameState = GameState.None;
    }

    #endregion Unity functions

    #region Public functions

    /// <summary>
    /// Changes the current game state and performs relevant actions.
    /// </summary>
    /// <param name="t_newState">The new game state to transition to.</param>
    public void changeGameSate(GameState t_newState) {
        if (m_gameState == t_newState) {
            return;
        }
        m_gameState = t_newState;
        switch (m_gameState) {
            case GameState.None:
                break;
            case GameState.LoadMainMenu:
                loadMenu();
                break;
            case GameState.MainMenu:
                break;
            case GameState.LoadLevel:
                loadLevel();
                break;
            case GameState.Playing:
                break;
            case GameState.RestartLevel:
                restartLevel();
                break;
            case GameState.GameOver:
                break;
            case GameState.Credits:
                break;
            case GameState.QuitGame:
                quitGame();
                break;
            default:
                throw new UnityException("Invalid Game State");
        }
    }

    /// <summary>
    /// Changes the game state using a string representation (for editor use).
    /// </summary>
    /// <param name="t_newState">String representation of the new game state.</param>
    public void changeGameStateInEditor(string t_newState) {
        changeGameSate((GameState)System.Enum.Parse(typeof(GameState), t_newState));
    }

    /// <summary>
    /// Gets the current game state.
    /// </summary>
    /// <returns>The current game state.</returns>
    public GameState getGameState() {
        return m_gameState;
    }

    /// <summary>
    /// Sets the name of the new level to be loaded.
    /// </summary>
    /// <param name="t_newLevel">The name of the new level.</param>
    public void setNewLevelName(string t_newLevel) {
        m_newLevel = t_newLevel;
    }

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void loadMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    #endregion Public functions

    #region Private functions

    /// <summary>
    /// Restarts the current level.
    /// </summary>
    private void restartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Loads the specified level.
    /// </summary>
    private void loadLevel() {
        SceneManager.LoadScene(m_newLevel);
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    private void quitGame() {
        Application.Quit();
    }

    #endregion Private functions
}

public enum GameState {
    None,
    LoadMainMenu,
    MainMenu,
    LoadLevel,
    Playing,
    RestartLevel,
    GameOver,
    Credits,
    QuitGame,
}