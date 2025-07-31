using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D rb;
    private Collider2D capsuleCollider;

    private Vector2 velocity;
    private float inputAxis;

    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    
    [Header("Movement Bounds")]
    public float leftBound = -8f;   // Adjust these values in the inspector
    public float rightBound = 250f;   // Adjust these values in the inspector
    public bool useAutoBounds = false; // Toggle to use automatic camera-based bounds
    
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);
    public bool falling => velocity.y < 0f && !grounded;

    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        Debug.Log("[Mario] PlayerMovement enabled - resetting physics state");
        rb.isKinematic = false;
        capsuleCollider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void OnDisable()
    {
        Debug.Log("[Mario] PlayerMovement disabled - stopping physics");
        rb.isKinematic = true;
        capsuleCollider.enabled = false;
        velocity = Vector2.zero;
        inputAxis = 0f;
        jumping = false;
    }

    private void Update()
    {
        // Debug logs every 30 frames (about twice per second)
        if (Time.frameCount % 30 == 0)
        {
            Debug.Log($"[Mario] Frame {Time.frameCount}: Position: {transform.position}, Velocity: {velocity}, Grounded: {grounded}, InputAxis: {inputAxis}");
        }

        HorizontalMovement();

        grounded = rb.Raycast(Vector2.down);
        
        // Log grounding changes
        if (Time.frameCount % 30 == 0)
        {
            Debug.Log($"[Mario] Grounded check: {grounded}, Rigidbody position: {rb.position}");
        }

        // Restore jumping logic
        if (grounded) {
            GroundedMovement();
        }

        // Restore gravity for jumping
        ApplyGravity();
    }

    private void FixedUpdate()
    {
        // Debug log before movement
        Vector2 oldPosition = rb.position;
        
        // Move mario based on his velocity (both horizontal and vertical now)
        Vector2 position = rb.position;
        position.x += velocity.x * Time.fixedDeltaTime;
        position.y += velocity.y * Time.fixedDeltaTime; // Restore vertical movement for jumping

        // Choose bounds calculation method
        float leftBoundary, rightBoundary;
        
        if (useAutoBounds)
        {
            // AUTOMATIC: Use camera-based bounds calculation
            Camera currentCamera = mainCamera;
            if (currentCamera == null)
            {
                currentCamera = Camera.main;
            }
            
            // Calculate world bounds based on camera's orthographic size
            float cameraHeight = currentCamera.orthographicSize;
            float cameraWidth = cameraHeight * currentCamera.aspect;
            
            leftBoundary = currentCamera.transform.position.x - cameraWidth + 0.5f;
            rightBoundary = currentCamera.transform.position.x + cameraWidth - 0.5f;
        }
        else
        {
            // MANUAL: Use inspector-set bounds (recommended)
            leftBoundary = leftBound;
            rightBoundary = rightBound;
        }
        
        float clampedX = Mathf.Clamp(position.x, leftBoundary, rightBoundary);
        
        // Debug clamping with clearer info
        if (Mathf.Abs(position.x - clampedX) > 0.01f && Time.frameCount % 30 == 0)
        {
            Debug.Log($"[Mario] Position clamped from {position.x:F2} to {clampedX:F2}. Bounds: {leftBoundary:F2} to {rightBoundary:F2} (Method: {(useAutoBounds ? "AUTO" : "MANUAL")})");
        }
        
        position.x = clampedX;
        
        // Debug movement
        if (Vector2.Distance(oldPosition, position) > 0.1f && Time.frameCount % 30 == 0)
        {
            Debug.Log($"[Mario] Moving from {oldPosition} to {position}, velocity: {velocity}, deltaTime: {Time.fixedDeltaTime}");
        }

        rb.MovePosition(position);
    }

    private void HorizontalMovement()
    {
        // Get input
        float oldInputAxis = inputAxis;
        inputAxis = Input.GetAxis("Horizontal");
        
        // Debug input changes
        if (Mathf.Abs(oldInputAxis - inputAxis) > 0.1f)
        {
            Debug.Log($"[Mario] Input changed from {oldInputAxis} to {inputAxis}");
        }
        
        // Store old velocity for debugging
        float oldVelocityX = velocity.x;
        
        // Accelerate / decelerate
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);

        // Debug velocity changes
        if (Mathf.Abs(oldVelocityX - velocity.x) > 0.1f && Time.frameCount % 15 == 0)
        {
            Debug.Log($"[Mario] Velocity.x changed from {oldVelocityX} to {velocity.x}, target: {inputAxis * moveSpeed}");
        }

        // Removed wall detection - no wall collision needed
        // bool hitWall = rb.Raycast(Vector2.right * velocity.x);
        // if (hitWall) {
        //     Debug.Log($"[Mario] Hit wall! Stopping horizontal movement. Was moving at velocity.x: {velocity.x}");
        //     velocity.x = 0f;
        // }

        // Flip sprite to face direction
        if (velocity.x > 0f) {
            transform.eulerAngles = Vector3.zero;
        } else if (velocity.x < 0f) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void GroundedMovement()
    {
        // Store old values for debugging  
        float oldVelocityY = velocity.y;
        bool wasJumping = jumping;
        
        // Prevent gravity from infinitly building up
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        // Perform jump
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log($"[Mario] Jump input detected! Setting velocity.y to {jumpForce}");
            velocity.y = jumpForce;
            jumping = true;
        }
        
        // Debug jumping state changes
        if (wasJumping != jumping || Mathf.Abs(oldVelocityY - velocity.y) > 0.1f)
        {
            Debug.Log($"[Mario] Grounded movement - Y velocity: {oldVelocityY} -> {velocity.y}, Jumping: {wasJumping} -> {jumping}");
        }
    }

    private void ApplyGravity()
    {
        // Store old velocity for debugging
        float oldVelocityY = velocity.y;
        
        // Check if falling
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        // Apply gravity and terminal velocity
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
        
        // Debug gravity application
        if (Mathf.Abs(oldVelocityY - velocity.y) > 0.1f && Time.frameCount % 30 == 0)
        {
            Debug.Log($"[Mario] Gravity applied - Y velocity: {oldVelocityY} -> {velocity.y}, falling: {falling}, multiplier: {multiplier}, gravity: {gravity}");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Bounce off enemy head
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            // Stop vertical movement if mario bonks his head
            if (transform.DotTest(collision.transform, Vector2.up)) {
                velocity.y = 0f;
            }
        }
    }
}
