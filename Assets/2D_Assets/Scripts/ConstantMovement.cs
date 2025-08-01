using UnityEngine;

public class ConstantMovement : MonoBehaviour
{
   
    public float moveDistance = 2f;
   
    public float moveSpeed = 1f;
    public bool moveX = true;
    public bool moveY = false;
    public bool defaultDirection = true;

    
    private Vector3 startPosition;
   
    private int moveDirection = 1;

    void Start()
    {
        startPosition = transform.position;
        
        // Set initial direction based on defaultDirection setting
        moveDirection = defaultDirection ? 1 : -1;
    }

    void Update()
    {
        
        Vector3 newPosition = transform.position;
        if (moveX)
        {
            newPosition.x += moveSpeed * Time.deltaTime * moveDirection;

        
        if (newPosition.x > startPosition.x + moveDistance)
        {
            newPosition.x = startPosition.x + moveDistance;
            moveDirection = -1;
        }
        else if (newPosition.x < startPosition.x - moveDistance)
        {
            newPosition.x = startPosition.x - moveDistance;
            moveDirection = 1;
        }
        }

        if (moveY)
        {
            newPosition.y += moveSpeed * Time.deltaTime * moveDirection;

            if (newPosition.y > startPosition.y + moveDistance)
            {
                newPosition.y = startPosition.y + moveDistance;
                moveDirection = -1;
            }
            else if (newPosition.y < startPosition.y - moveDistance)
            {
                newPosition.y = startPosition.y - moveDistance;
                moveDirection = 1;
            }
        }

        transform.position = newPosition;
    }
}