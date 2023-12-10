using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour {
    private bool isShooting;

    private void Update() {
        if (isShooting) {
            print("Het");
        }
    }

    public void OnShoot(InputAction.CallbackContext context) {
        if (context.performed) {
            isShooting = true;
            PlayerManager.instance.changePlayerState(PlayerState.Shooting);
        } else if (context.canceled) {
            isShooting = false;
        }
    }
}