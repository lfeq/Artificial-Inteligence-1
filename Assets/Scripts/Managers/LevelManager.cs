using Cinemachine;
using UnityEngine;

/// <summary>
/// LevelManager class manages the overall game level, including player respawn, game over conditions,
/// and transitioning between levels.
/// </summary>
public class LevelManager : MonoBehaviour {

    #region Public variables

    public static LevelManager instance;

    #endregion Public variables

    #region Serializable variables

    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private Vector2 spawnPosition;
    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private AudioClip victorySound;

    #endregion Serializable variables

    #region Private variables

    private bool m_isShowingYouDiedScreen;
    private int towerCount = 4;
    private AudioSource audioSource;

    #endregion Private variables

    #region Unity functions

    private void Awake() {
        if (FindObjectOfType<LevelManager>() != null &&
            FindObjectOfType<LevelManager>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        m_isShowingYouDiedScreen = false;
        gameOverCanvasGroup.alpha = 0;
        if (PlayerManager.instance == null) {
            Instantiate(player);
        }
    }

    private void Start() {
        GameManager.s_instance.changeGameSate(GameState.Playing);
        PlayerManager.instance.transform.position = spawnPosition;
        PlayerManager.instance.changePlayerState(PlayerState.Idle);
        PlayerController.instance.enabled = true;
        virtualCamera.Follow = PlayerManager.instance.transform;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        showingGameOverScreen();
    }

    #endregion Unity functions

    #region Public functions

    /// <summary>
    /// Called when a tower is destroyed in the level.
    /// </summary>
    public void towerDestroyed() {
        towerCount--;
        if (towerCount == 0) {
            showGameOverScreen();
        }
    }

    /// <summary>
    /// Displays the game over screen when function is called.
    /// </summary>
    public void showGameOverScreen() {
        gameOverCanvasGroup.interactable = true;
        m_isShowingYouDiedScreen = true;
        audioSource.clip = victorySound;
        audioSource.Play();
    }

    /// <summary>
    /// Restarts the current level.
    /// </summary>
    public void restartLevel() {
        GameManager.s_instance.changeGameSate(GameState.RestartLevel);
    }

    /// <summary>
    /// Returns to the main menu.
    /// </summary>
    public void returnToMenu() {
        GameManager.s_instance.changeGameSate(GameState.LoadMainMenu);
    }

    #endregion Public functions

    #region Private functions

    /// <summary>
    /// Gradually increases the alpha value of the game over screen.
    /// </summary>
    private void showingGameOverScreen() {
        if (!m_isShowingYouDiedScreen) {
            return;
        }
        gameOverCanvasGroup.alpha += Time.deltaTime;
    }

    #endregion Private functions
}