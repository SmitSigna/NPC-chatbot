using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class characterMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float rotationSpeed = 10f; // rotation smoothness
    [SerializeField] private InputActionAsset inputActions;

    private CharacterController controller;
    private InputAction moveAction;
    private InputAction jumpAction;


    private Vector3 velocity;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;

    private Animator anim;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        // Get Move and Jump from Player map
        var playerMap = inputActions.FindActionMap("Player");
        moveAction = playerMap.FindAction("Move");
        jumpAction = playerMap.FindAction("Jump");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Movement
        Vector3 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        //Animations
        anim.SetFloat("Speed", move.magnitude);
        anim.SetBool("isJumping", !isGrounded);


        if (move.magnitude >= 0.1f)
        {
            // Calculate target angle relative to camera
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            targetAngle += Camera.main.transform.eulerAngles.y;

            // Smoothly rotate player
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move in that direction
            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }


        //Ground check
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // stick to ground
        }

        //Jump
        if (jumpAction.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
