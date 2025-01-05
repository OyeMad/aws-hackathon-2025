using Terresquall;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneControls : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float smoothFactor = 0.5f;
    
    private Vector2 moveInput;
    private Vector3 smoothedMovement;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float v = VirtualJoystick.GetAxis("Vertical");
        float h = VirtualJoystick.GetAxis("Horizontal");

        moveInput = new Vector2( h , v );
    }

    private void FixedUpdate()
    {
        // Calculate movement direction
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        
        // Smooth the movement
        smoothedMovement = Vector3.Lerp(smoothedMovement, movement, smoothFactor);
        
        // Apply movement
        rb.linearVelocity = smoothedMovement;
    }
}
