using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public Vector3 windForce;
    public float maxWindStrength = 5f;

    // Randomize the wind force
    public void GenerateRandomWind()
    {
        float xForce = Random.Range(-maxWindStrength, maxWindStrength);
        float zForce = Random.Range(-maxWindStrength, maxWindStrength);

        windForce = new Vector3(xForce, 0, zForce);
        Debug.Log($"New wind generated: {windForce}");
    }

    public void ApplyWindForce(Rigidbody rb)
    {
        if (rb != null)
        {
            rb.AddForce(windForce.x * 15, windForce.y, windForce.z, ForceMode.Force);
        }
    }

    public string GetWindDescription()
    {
        float threshold = 0.1f; // Minimum noticeable wind magnitude
        if (Mathf.Abs(windForce.x) < threshold && Mathf.Abs(windForce.z) < threshold)
            return "No Wind";

        string xDirection = windForce.x > 0 ? "→ East" : windForce.x < 0 ? "← West" : "";
        string zDirection = windForce.z > 0 ? "↑ North" : windForce.z < 0 ? "↓ South" : "";

        return $"{Mathf.Abs(windForce.x):F1} mph {xDirection}\n{Mathf.Abs(windForce.z):F1} mph {zDirection}";
    }
}
