using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    private Animator m_animator;
    private PlayerState m_playerState;

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

    public PlayerState getPlayerState() {
        return m_playerState;
    }

    private void resetAnimatorParameters() {
        foreach (AnimatorControllerParameter parameter in m_animator.parameters) {
            if (parameter.type == AnimatorControllerParameterType.Bool) {
                m_animator.SetBool(parameter.name, false);
            }
        }
    }
}

public enum PlayerState {
    None,
    Idle,
    Running,
    Dead,
    Shooting,
}