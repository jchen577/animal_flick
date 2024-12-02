using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlickScript : MonoBehaviour
{
    // Public variables for angle (in degrees) and power
    [Range(0, 90)] public float angle = 45f; // Angle of launch, 0 to 90 degrees
    public float power = 10f;                // Launch power or force magnitude

    // Reference to the Rigidbody component
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component on the same GameObject
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("CannonLauncher requires a Rigidbody component!");
        }
    }

    void Update()
    {
        // Launch the object when the space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchObject();
        }
    }

    void LaunchObject()
    {
        if (rb == null) return;

        // Convert angle to radians
        float angleInRadians = angle * Mathf.Deg2Rad;

        // Calculate the direction vector based on the angle
        // Use transform.forward for the forward direction and adjust with the angle
        Vector3 forwardDirection = transform.forward.normalized;
        Vector3 upwardDirection = transform.up.normalized;

        Vector3 launchDirection = (forwardDirection * Mathf.Cos(angleInRadians) +
                                   upwardDirection * Mathf.Sin(angleInRadians)).normalized;

        // Apply force to the Rigidbody in the calculated direction
        rb.velocity = launchDirection * power;
    }
}
