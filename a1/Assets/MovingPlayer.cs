using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController: MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 20.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] float jumpHeight = 3f;
    // [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool lockCursor = true;

    float cameraPitch = 0.0f;
    CharacterController controller = null;
    Vector3 velocity;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode. Locked;
            Cursor.visible = false;
        }
    }
    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }
    void UpdateMouseLook()
    {
        Vector2 currentMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        
        //2-rotate camera up and down around x-ais // and remmber that camera is inside the player, so we need to rotate it localy
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        
        //1-rotating the player body around Y-axis
        //Vector3.up = 0, 1, 0
        transform. Rotate (Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement ()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (controller.isGrounded)
            velocity.y = 0.0f;
        
        velocity.y += gravity * Time.deltaTime;

        if (controller.isGrounded && Input.GetButtonDown("Jump")) {
            velocity.y = Mathf.Sqrt (jumpHeight * -2 * gravity);
        }
        //red axis of the transform in the world space // blue //green
        Vector3 move = (transform.right * x) + (transform.forward * z) +(Vector3.up * velocity.y);

        controller.Move(move * Time.deltaTime* walkSpeed);
    }
}