using UnityEngine;

public class UILookAt : MonoBehaviour {

    #region Private variables

    private Camera mainCamera;

    #endregion Private variables

    #region Unity functions

    private void Start() {
        mainCamera = Camera.main;
    }

    private void LateUpdate() {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }

    #endregion Unity functions
}