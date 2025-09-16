using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    // Follow player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    [Header("Level Bounds")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    private float camHalfWidth;

    private void Start()
    {
        // calculate camera half width (important for mobile screen sizes!)
        Camera cam = GetComponent<Camera>();
        float camHalfHeight = cam.orthographicSize;
        camHalfWidth = cam.aspect * camHalfHeight;
    }

    private void Update()
    {
        // Desired camera position following the player
        float targetX = player.position.x + lookAhead;

        // Clamp so camera stops at walls
        targetX = Mathf.Clamp(targetX, minX + camHalfWidth, maxX - camHalfWidth);

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        // Smooth look ahead
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
