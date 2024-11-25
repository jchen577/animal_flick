using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickControl : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float stopVelocity = .05f;
    [SerializeField] private float shotPower = 150;
    private bool isIdle;
    private bool isAiming;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        isAiming = false;
        lineRenderer.enabled = false;
    }
    private void DrawLine(Vector3 mousePoint)
    {
        Vector3[] positions = {
            transform.position,
            mousePoint
        };
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
    }

    private Vector3 CastMouseClickRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane - Camera.main.nearClipPlane
        );
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        );
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        return worldMousePosFar - worldMousePosNear;
    }

    private void OnMouseDown()
    {
        if (isIdle)
        {
            isAiming = true;
        }
    }

    private void ProcessAim()
    {
        if (!isAiming || !isIdle)
        {
            return;
        }
        Vector3 mousePoint = CastMouseClickRay();

        DrawLine(mousePoint);

        if (Input.GetMouseButtonUp(0))
        {
            Shoot(mousePoint);
        }
    }

    private void Shoot(Vector3 mousePoint)
    {
        isAiming = false;
        lineRenderer.enabled = false;

        Vector3 direction = (mousePoint - transform.position).normalized;
        rb.AddForce(direction * shotPower);
    }

    private void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isIdle = true;
    }

    private void Update()
    {
        if (rb.velocity.magnitude < stopVelocity)
        {
            Stop();
        }
        ProcessAim();
    }
}
