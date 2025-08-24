using UnityEngine;
using UnityEngine.InputSystem;

public class TPPcamera : MonoBehaviour
{
    [SerializeField] private Transform target; // The player
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float distance = 5f;
    [SerializeField] private Vector2 pitchLimits = new Vector2(-30f, 60f);
    [SerializeField] private InputActionAsset inputActions;

    private InputAction lookAction;
    private float yaw;
    private float pitch;

    private void Awake()
    {
        var playerMap = inputActions.FindActionMap("Player");
        lookAction = playerMap.FindAction("Look");
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; 
       // Cursor.visible = false;
    }

    private void OnEnable() => lookAction.Enable();
    private void OnDisable() => lookAction.Disable();

    private void LateUpdate()
    {
        // Read mouse/gamepad input
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        // Apply rotation
        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        // Position camera
        transform.position = target.position - Quaternion.Euler(pitch, yaw, 0) * Vector3.forward * distance;
        transform.LookAt(target);
    }
}
