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

    public GameObject peeloutSmokePrefab;

    private float currentSpeed = 0f;
    private float inputVertical;
    private float inputHorizontal;
    private Rigidbody2D rb;

    private Vector2 dragStartPos;
    private Vector2 dragCurrentPos;
    private bool isDragging = false;

    private bool useTouchControls;
    private bool hasPeeledOut = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        useTouchControls = Application.isMobilePlatform;
    }

    void Update()
    {
        if (useTouchControls)
            HandleTouchInput();
        else
            HandleKeyboardInput();
    }

    void HandleKeyboardInput()
    {
        inputVertical = Input.GetAxis("Vertical");
        inputHorizontal = Input.GetAxis("Horizontal");
    }

    void HandleTouchInput()
    {
        inputVertical = 0f;
        inputHorizontal = 0f;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            dragCurrentPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                dragStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                dragCurrentPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
#endif

        if (isDragging)
        {
            Vector2 dragVector = dragCurrentPos - dragStartPos;
            dragVector = dragVector.normalized;

            inputVertical = dragVector.y;
            inputHorizontal = dragVector.x;
        }
    }

    void FixedUpdate()
    {
        if (inputVertical != 0)
        {
            if (!hasPeeledOut && Mathf.Approximately(currentSpeed, 0f))
            {
                hasPeeledOut = true;

                if (peeloutSmokePrefab != null)
                {
                    Vector3 offset = -transform.up * 0.5f;
                    Instantiate(peeloutSmokePrefab, transform.position + offset, transform.rotation);
                }
            }

            currentSpeed += inputVertical * acceleration * Time.fixedDeltaTime;
        }
        else
        {
            if (currentSpeed > 0)
                currentSpeed = Mathf.Max(currentSpeed - deceleration * Time.fixedDeltaTime, 0);
            else if (currentSpeed < 0)
                currentSpeed = Mathf.Min(currentSpeed + deceleration * Time.fixedDeltaTime, 0);

            if (Mathf.Approximately(currentSpeed, 0f))
                hasPeeledOut = false;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed / 2f, maxSpeed);
        rb.MovePosition(rb.position + (Vector2)transform.up * currentSpeed * Time.fixedDeltaTime);

        float turnMultiplier = currentSpeed >= 0 ? 1f : -1f;
        float rotationAmount = inputHorizontal * turnSpeed * Time.fixedDeltaTime * turnMultiplier;
        rb.MoveRotation(rb.rotation - rotationAmount);
    }
}
