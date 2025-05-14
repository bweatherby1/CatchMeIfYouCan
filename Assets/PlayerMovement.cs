using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 10f;
    public float deceleration = 8f;
    public float maxSpeed = 8f;
    public float turnSpeed = 200f;

    private float currentSpeed = 0f;
    private float inputVertical;
    private float inputHorizontal;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        inputVertical = Input.GetAxis("Vertical");
        inputHorizontal = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
{
    if (inputVertical != 0)
    {
        currentSpeed += inputVertical * acceleration * Time.fixedDeltaTime;
    }
    else
    {
        if (currentSpeed > 0)
            currentSpeed = Mathf.Max(currentSpeed - deceleration * Time.fixedDeltaTime, 0);
        else if (currentSpeed < 0)
            currentSpeed = Mathf.Min(currentSpeed + deceleration * Time.fixedDeltaTime, 0);
    }

    currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed / 2f, maxSpeed);

    rb.MovePosition(rb.position + (Vector2)transform.up * currentSpeed * Time.fixedDeltaTime);

    float turnMultiplier = currentSpeed >= 0 ? 1f : -1f;
    float rotationAmount = inputHorizontal * turnSpeed * Time.fixedDeltaTime * turnMultiplier;
    rb.MoveRotation(rb.rotation - rotationAmount);
}

}
