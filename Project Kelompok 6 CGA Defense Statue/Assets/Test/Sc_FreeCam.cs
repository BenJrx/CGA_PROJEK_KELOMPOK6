using UnityEngine;

public class Sc_FreeCam : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float rotationSpeed = 300.0f;
    public float distance = 10.0f;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public bool isShiftlock=false;
    public Transform thirdPersonLookAt;

    private float currentVerticalAngle;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Quaternion rotation;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)) isShiftlock = !isShiftlock;
        if(isShiftlock){
            Cursor.lockState = CursorLockMode.Locked;
            ShiftLockCamera();
        }else{
            Cursor.lockState = CursorLockMode.None;
            CameraFollowing();
        }
    }

    void CameraFollowing()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
            yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        pitch = Mathf.Clamp(pitch, -40f, 85f);
        rotation = Quaternion.Euler(pitch, yaw, 0.0f);
        CameraScrolling();
    }

    void CameraScrolling()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        distance -= scrollInput * 10f;
        distance = Mathf.Clamp(distance, 5f, 15f);

        Vector3 desiredPosition = player.position + rotation * offset.normalized * distance;

        RaycastHit hit;
        Vector3 direction = desiredPosition - player.position;

        // Adjust camera position if there's an obstacle between the camera and the player
        if (Physics.Raycast(player.position, direction.normalized, out hit, distance, whatIsGround | whatIsWall))
        {
            // Set the camera closer to the player if it collides with the ground or walls
            desiredPosition = player.position + direction.normalized * (hit.distance - 0.2f); // Adjust slightly to avoid clipping
        }

        // Ensure the camera doesn't go below the player's feet level
        desiredPosition.y = Mathf.Max(desiredPosition.y, player.position.y + 1.0f); // Adjust Y to stay above the ground

        transform.position = desiredPosition;
        transform.LookAt(player);
    }

    void ShiftLockCamera()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        distance -= scrollInput * 10f;
        distance = Mathf.Clamp(distance, 5f, 15f);

        // Rotate the player horizontally based on mouse movement
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        player.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically (up/down) without affecting the player's rotation
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        currentVerticalAngle -= mouseY;
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, -40f, 85f);

        Vector3 cameraOffset = player.position - player.forward * (distance - 2f);
        cameraOffset.y += 1f;

        // Calculate the desired position
        Vector3 desiredPosition = cameraOffset;

        // Raycast to detect obstacles
        RaycastHit hit;
        Vector3 direction = desiredPosition - player.position;
        if (Physics.Raycast(player.position, direction.normalized, out hit, distance, whatIsGround | whatIsWall))
        {
            desiredPosition = player.position + direction.normalized * (hit.distance - 0.2f); // Adjust slightly to avoid clipping
        }

        // Ensure the camera doesn't dip below the player's position
        desiredPosition.y = Mathf.Max(desiredPosition.y, player.position.y + 1.0f);

        transform.position = desiredPosition;

        // Apply the rotation and look at the target
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, player.eulerAngles.y, 0f);
        transform.rotation = rotation;
        transform.LookAt(thirdPersonLookAt.position);
    }
}