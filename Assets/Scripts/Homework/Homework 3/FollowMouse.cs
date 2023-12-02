using UnityEngine;

//https://medium.com/daily-unity/unity3d-place-or-move-an-object-on-click-bb4fb68c66e9
public class FollowMouse : MonoBehaviour {

    private void Update() {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float rayDistance;
        if (plane.Raycast(ray, out rayDistance)) {
            Vector3 targetPosition = ray.GetPoint(rayDistance);
            transform.position = targetPosition;
        }
    }
}