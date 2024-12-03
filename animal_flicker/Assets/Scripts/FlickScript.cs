using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlickScript : MonoBehaviour
{
    [Range(1, 90)] public float angle;
    public float power;

    public Slider powerSlider;
    public TextMeshProUGUI powerText;

    public Slider angleSlider;
    public TextMeshProUGUI angleText;

    private Rigidbody rb;
    private bool isAdjustingPower = true;
    private bool isAdjustingAngle = false;
    private Coroutine sliderCoroutine;

    public GameManager gameManager;

    // Flag to check if the object has been launched
    public bool IsLaunched { get; private set; }

    private bool isGrounded;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("FlickScript requires a Rigidbody component!");
        }

        // Validate UI elements
        if (powerSlider == null || powerText == null)
        {
            Debug.LogError("PowerSlider or PowerText is not assigned!");
        }

        if (angleSlider == null || angleText == null)
        {
            Debug.LogError("AngleSlider or AngleText is not assigned!");
        }

        isAdjustingPower = true;
        isAdjustingAngle = false;
        IsLaunched = false;

        // Optionally reset power and angle to random values or defaults
        power = Random.Range(1.0f, 49.0f);
        powerSlider.value = power;
        UpdatePowerText();

        angle = Random.Range(1, 90);
        angleSlider.value = angle;
        UpdateAngleText();

        // Stop any running coroutines
        if (sliderCoroutine != null)
        {
            StopCoroutine(sliderCoroutine);
            sliderCoroutine = null; // Nullify the coroutine reference
        }

        // Start adjusting power again
        sliderCoroutine = StartCoroutine(AdjustSlider(powerSlider, UpdatePowerText));
    }

    void Update()
    {
        if (isAdjustingPower && Input.GetKeyDown(KeyCode.Space))
        {
            isAdjustingPower = false;

            if (sliderCoroutine != null)
            {
                StopCoroutine(sliderCoroutine);
            }

            power = powerSlider.value;

            // Switch to adjusting angle
            isAdjustingAngle = true;
            sliderCoroutine = StartCoroutine(AdjustSlider(angleSlider, UpdateAngleText));
        }
        else if (isAdjustingAngle && Input.GetKeyDown(KeyCode.Space))
        {
            isAdjustingAngle = false;

            if (sliderCoroutine != null)
            {
                StopCoroutine(sliderCoroutine);
            }

            angle = angleSlider.value;

            // Launch the object after both power and angle are set
            LaunchObject();
        }

    }

    void LaunchObject()
    {
        if (rb == null) return;

        float angleInRadians = angle * Mathf.Deg2Rad;

        Vector3 forwardDirection = transform.forward.normalized;
        Vector3 upwardDirection = transform.up.normalized;

        Vector3 launchDirection = (forwardDirection * Mathf.Cos(angleInRadians) +
                                    upwardDirection * Mathf.Sin(angleInRadians)).normalized;

        rb.velocity = launchDirection * power;

        IsLaunched = true; // Set the flag to true when the object is launched
    }

    public void ResetLaunchFlag()
    {
        IsLaunched = false; // Reset the flag when the object is reset
    }

    void UpdatePowerText()
    {
        if (powerText != null)
        {
            powerText.text = $"Power: {powerSlider.value:F1}";
        }
    }

    void UpdateAngleText()
    {
        if (angleText != null)
        {
            angleText.text = $"Angle: {angleSlider.value:F1}°";
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Update isGrounded if the object is colliding with the ground layer
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Reset isGrounded when the object leaves the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public bool CheckIfGrounded()
    {
        return isGrounded;
    }

    public void ResetFlickState()
    {
        isAdjustingPower = true;
        isAdjustingAngle = false;
        IsLaunched = false;

        // Optionally reset power and angle to random values or defaults
        power = Random.Range(1.0f, 49.0f);
        powerSlider.value = power;
        UpdatePowerText();

        angle = Random.Range(1, 90);
        angleSlider.value = angle;
        UpdateAngleText();

        // Stop any running coroutines
        if (sliderCoroutine != null)
        {
            StopCoroutine(sliderCoroutine);
            sliderCoroutine = null; // Nullify the coroutine reference
        }

        // Start adjusting power again
        sliderCoroutine = StartCoroutine(AdjustSlider(powerSlider, UpdatePowerText));
    }


    IEnumerator AdjustSlider(Slider slider, System.Action updateTextCallback)
    {
        bool increasing = true;

        while (isAdjustingPower || isAdjustingAngle)
        {
            if (increasing)
            {
                slider.value += Time.deltaTime * 50f;
                if (slider.value >= slider.maxValue)
                {
                    increasing = false;
                }
            }
            else
            {
                slider.value -= Time.deltaTime * 50f;
                if (slider.value <= slider.minValue)
                {
                    increasing = true;
                }
            }

            // Update the corresponding text as the slider value changes
            updateTextCallback?.Invoke();

            yield return null;
        }
    }
}
