using UnityEngine;

/// <summary>
/// PlayerManager class manages the player character's state and animation transitions.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerManager : MonoBehaviour {

    #region Public variables

    public static PlayerManager instance;

    #endregion Public variables

    #region Private variables

    private Animator m_animator;
    private PlayerState m_playerState;

    #endregion Private variables

    #region Unity functions

    private void Awake() {
        m_animator = GetComponent<Animator>();
        if (FindObjectOfType<PlayerManager>() != null &&
            FindObjectOfType<PlayerManager>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        m_playerState = PlayerState.None;
    }

    #endregion Unity functions

    #region Public functions

    /// <summary>
    /// Changes the player state and triggers corresponding animations.
    /// </summary>
    /// <param name="t_newSate">The new player state.</param>
    public void changePlayerState(PlayerState t_newSate) {
        if (m_playerState == t_newSate) {
            return;
        }
        resetAnimatorParameters();
        m_playerState = t_newSate;
        switch (m_playerState) {
            case PlayerState.None:
                break;
            case PlayerState.Idle:
                m_animator.SetBool("isIdeling", true);
                break;
            case PlayerState.Running:
                m_animator.SetBool("isRunning", true);
                break;
            case PlayerState.Shooting:
                m_animator.SetTrigger("shoot");
                break;
            case PlayerState.Dead:
                break;
        }
    }

    /// <summary>
    /// Gets the current player state.
    /// </summary>
    /// <returns>The current player state.</returns>
    public PlayerState getPlayerState() {
        return m_playerState;
    }

    #endregion Public functions

    #region Private functions

    /// <summary>
    /// Resets all boolean parameters of the animator to false.
    /// </summary>
    private void resetAnimatorParameters() {
        foreach (AnimatorControllerParameter parameter in m_animator.parameters) {
            if (parameter.type == AnimatorControllerParameterType.Bool) {
                m_animator.SetBool(parameter.name, false);
            }
        }
    }

    #endregion Private functions
}

public enum PlayerState {
    None,
    Idle,
    Running,
    Dead,
    Shooting,
}