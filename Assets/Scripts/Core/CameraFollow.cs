
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;   // drag your Player here
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

    [Header("Level Bounds")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private void LateUpdate()
    {
        if (target == null) return;

        // follow player
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // stop camera from leaving level
        float camX = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        float camY = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        transform.position = new Vector3(camX, camY, transform.position.z);
    }
}
