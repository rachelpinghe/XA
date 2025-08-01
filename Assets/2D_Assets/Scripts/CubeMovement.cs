using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CubeMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpPower = 10f;
    
    [Header("Ground Detection")]
    public LayerMask groundLayerMask = 1; // Check default layer
    public float groundCheckDistance = 0.1f;
    public string groundTag = "ground"; // Tag that ground objects should have
    
    // Private variables
    private Rigidbody rb;
    private bool isGrounded = true;
    private float horizontalInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        // Get input
        horizontalInput = 0f;
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }
        
        // Check if grounded
        CheckGrounded();
        
        // Jump input (only when grounded) - works independently of horizontal movement
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            Jump();
        }
    }
    
    void FixedUpdate()
    {
        // Apply horizontal movement
        MoveHorizontal();
    }
    
    void MoveHorizontal()
    {
        // Calculate target velocity (remove Time.deltaTime for velocity-based movement)
        float targetVelocityX = horizontalInput * moveSpeed;
        
        // Apply movement while preserving Y velocity (for jumping/falling)
        rb.velocity = new Vector3(targetVelocityX, rb.velocity.y, rb.velocity.z);
    }
    
    void Jump()
    {
        // Set upward velocity for jump (remove Time.deltaTime - this should be instant!)
        Debug.Log("trying to jump");
        rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
    }
    
    void CheckGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.2f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
