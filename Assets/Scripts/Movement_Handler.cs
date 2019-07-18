using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Handler : MonoBehaviour
{
    private GameObject playerHead;
    private Rigidbody playerBody;
    private Controller_Input input;

    private Vector2 moveInput;
    private Vector2 rotateInput;
    private Controller_Input.Dpad padInput;
    private bool wasRotating;
    private bool wasMoving;

    public float movementSpeed;
    public float rotationAngle;
    public float deadZone; // Minimum magnitude of movement of the joystick to register inputs
    public GameObject entryPlatform;

    public bool smoothRotation;
    public bool smoothMotion;
    public float timeForOneRotation;

    // Start is called before the first frame update
    void Start()
    {
        playerHead = GameObject.FindGameObjectWithTag("MainCamera");
        input = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_Input>();
        wasRotating = false;
        wasMoving = false;
        playerBody = GetComponent<Rigidbody>();
    }

    private void UpdateState()
    {
        moveInput = input.GetLeftJoystickPosition();
        rotateInput = input.GetRightJoystickPosition();
        padInput = input.GetLeftDpadPosition();
    }

    private static float Angle(Vector2 vec)
    {
        if (vec.x < 0)
        {
            return 360 - (Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;
        }
    }

    Vector3 GetNewRotation(Vector3 initial, Vector3 rotation)
    {
        Vector3 final = initial + rotation;
        if (final.y > 360)
        {
            final.y -= 360;
        }

        return final;
    }

    IEnumerator SmoothRotate(Quaternion from, Quaternion to, float duration, float dt)
    {
        float elapsed = 0;

        while (elapsed < duration) {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            if (elapsed < duration / 4 || elapsed > 3 * duration / 4)
            {
                elapsed += dt / 2;
            }
            else
            {
                elapsed += dt;
            }
            yield return new WaitForSeconds(dt);
        }

        transform.rotation = to;

        yield return new WaitForSeconds(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateState();
        float dt = Time.fixedDeltaTime;

        if (smoothMotion)
        {
            // Handle movement if the player isn't moving too fast
            if (playerBody.velocity.magnitude < movementSpeed && moveInput.magnitude > deadZone)
            {
                Vector3 movementDirection = Quaternion.AngleAxis(Angle(moveInput) + playerHead.transform.rotation.eulerAngles.y, Vector3.up) * Vector3.forward;
                playerBody.MovePosition(playerBody.position + (movementDirection * movementSpeed * dt));
            }

            if (padInput == Controller_Input.Dpad.Up)
            {
                Vector3 movementDirection = Vector3.up;
                playerBody.MovePosition(playerBody.position + (movementDirection * movementSpeed * dt));
            }
            if (padInput == Controller_Input.Dpad.Down)
            {
                Vector3 movementDirection = Vector3.down;
                playerBody.MovePosition(playerBody.position + (movementDirection * movementSpeed * dt));
            }
        }
        else
        {
            if (!wasMoving)
            {
                if (moveInput.magnitude > deadZone)
                {
                    Vector3 movementDirection = Quaternion.AngleAxis(Angle(moveInput) + playerHead.transform.rotation.eulerAngles.y, Vector3.up) * Vector3.forward;
                    playerBody.MovePosition(playerBody.position + (movementDirection * movementSpeed * dt));
                    wasMoving = true;
                }
                if (padInput == Controller_Input.Dpad.Up)
                {
                    Vector3 movementDirection = Vector3.up;
                    playerBody.MovePosition(playerBody.position + (movementDirection * movementSpeed * dt));
                    wasMoving = true;
                }
                if (padInput == Controller_Input.Dpad.Down)
                {
                    Vector3 movementDirection = Vector3.down;
                    playerBody.MovePosition(playerBody.position + (movementDirection * movementSpeed * dt));
                    wasMoving = true;
                }
            }
            else if (moveInput.magnitude <= deadZone && padInput != Controller_Input.Dpad.Up && padInput != Controller_Input.Dpad.Down)
            {
                wasMoving = false;
            }
        }

        // Handle rotation
        if (smoothRotation)
        {
            if (!wasRotating && Mathf.Abs(rotateInput.x) > deadZone && Mathf.Abs(rotateInput.y) < deadZone)
            {
                Vector3 totalRotation = new Vector3(0, rotateInput.x / Mathf.Abs(rotateInput.x) * rotationAngle, 0);
                StartCoroutine(SmoothRotate(transform.rotation, Quaternion.Euler(GetNewRotation(transform.rotation.eulerAngles, totalRotation)), timeForOneRotation, dt));

                wasRotating = true;
            }
            else if (rotateInput.magnitude <= deadZone)
            {
                wasRotating = false;
            }
        }
        else
        {
            if (!wasRotating && Mathf.Abs(rotateInput.x) > deadZone && Mathf.Abs(rotateInput.y) < deadZone)
            {
                Vector3 rotation = new Vector3(0, rotateInput.x / Mathf.Abs(rotateInput.x) * rotationAngle, 0);
                transform.Rotate(rotation);
                wasRotating = true;
            }
            else if (rotateInput.magnitude <= deadZone)
            {
                wasRotating = false;
            }
        }
    }
}
