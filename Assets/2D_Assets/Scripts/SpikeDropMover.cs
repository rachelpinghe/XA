using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDropMover : MonoBehaviour
{
    public float dropDistance = 2f;           // 要移动的总距离
    public float dropSpeed = 2f;              // 每秒移动多少距离
    public bool FallRotating = false;         // 启用物理落下和旋转
    
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool shouldDrop = false;
    private Rigidbody rb;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.down * dropDistance;
        rb = GetComponent<Rigidbody>();
        
        // Disable physics initially if using FallRotating mode
        if (FallRotating && rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (shouldDrop)
        {
            if (FallRotating && rb != null)
            {
                // Use physics for natural falling and rotation - do nothing here as physics handles it
            }
            else
            {
                // Use manual movement (original behavior)
                transform.position = Vector3.MoveTowards(transform.position, targetPos, dropSpeed * Time.deltaTime);
            }
        }
    }

    public void StartDrop()
    {
        shouldDrop = true;
        if (GetComponent<Rigidbody>() != null)
            GetComponent<Rigidbody>().useGravity = true;
        
        if (FallRotating && rb != null)
        {
            // Enable physics for natural falling and rotation
            rb.isKinematic = false;
            rb.useGravity = true;

            // Add a small random torque for natural rotation
            Vector3 randomTorque = new Vector3(
                Random.Range(-2f, 2f),
                Random.Range(-2f, 2f),
                Random.Range(-2f, 2f)
            );
            rb.AddTorque(randomTorque, ForceMode.Impulse);

            // Optionally add a small downward force to start the fall
            rb.AddForce(Vector3.down * dropSpeed, ForceMode.Impulse);
        }
    }
}