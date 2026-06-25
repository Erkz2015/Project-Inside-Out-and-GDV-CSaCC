using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Walking
    public bool enableWalk = true;
    public float baseWalkSpeed = 5f;
    public float currentWalkSpeed = 5f;

    public int floorType = 1;
    public bool isplayingStepSound = false;

    public StudioEventEmitter footStepEmmiterInside;
    public StudioEventEmitter footStepEmmiterOutside;

    // Camera reference
    public Transform cameraTransform;

    private Rigidbody rb;
    public bool isJumping;
    public bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        if (!enableWalk) return;

        float h = 0f;
        float v = 0f;

        if (Keyboard.current != null)
        {
            h = (Keyboard.current.aKey.isPressed ? -1f : 0f) +
                (Keyboard.current.dKey.isPressed ? 1f : 0f);

            v = (Keyboard.current.wKey.isPressed ? 1f : 0f) +
                (Keyboard.current.sKey.isPressed ? -1f : 0f);


            bool isMoving = h != 0 || v != 0;

            if (isMoving && !isplayingStepSound)
            {
                isplayingStepSound = true;

                if (floorType == 1)
                    footStepEmmiterInside.Play();
                else if (floorType == 2)
                    footStepEmmiterOutside.Play();
            }
            else if (!isMoving && isplayingStepSound)
            {
                isplayingStepSound = false;
                footStepEmmiterInside.Stop();
                footStepEmmiterOutside.Stop();
            }
        }

        Vector3 input = new Vector3(h, 0, v);
        if (input != Vector3.zero)
            input.Normalize();

        // ---- CAMERA BASED MOVEMENT ----
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * v + camRight * h) * currentWalkSpeed;
        // -------------------------------

        Vector3 velocity = rb.linearVelocity;
        Vector3 change = moveDir - velocity;
        change.y = 0;

        rb.AddForce(change, ForceMode.VelocityChange);
    }

    public void CheckGround()
    {

        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {

            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
            isJumping = false;

        }
        else
        {

            isGrounded = false;
            isJumping = true;
        }
    }
}