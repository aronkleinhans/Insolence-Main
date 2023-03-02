using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControl : MonoBehaviour
{
    [Header ("Movement" )]
    [SerializeField] float speed = 6f;
    [SerializeField] float rotationSmoothTime = 0.2f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float dashSpeed = 18f;
    [SerializeField] float dashTime = 1f;

    [Header ("Gravity")]
    //[SerializeField] bool isGrounded = false;
    [SerializeField] float gravityMultiplier = 2f;
    [SerializeField] float groundedGravity = -0.5f;
    [SerializeField] int applyFrequency = 2;
    [SerializeField] float gravity = 9.8f;


    private CharacterController controller;
    private Camera cam;
    private float targetAngle;
    private float velocityY;
    private float currentAngle;
    private float currentAngleVelocity;
    private Vector3 rotatedMovement = Vector3.zero;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        cam.transform.position = controller.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
        HandleMovement();
        HandleGravity();
        HandleJump();
 
        //HandleLockOn();
        
    }

    
    private void HandleMovement()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;


        if (movement.magnitude > 0.1f)
        {
            targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

            currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref currentAngleVelocity, rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0, currentAngle, 0);

            rotatedMovement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            controller.Move(rotatedMovement * speed * Time.deltaTime);

        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private void HandleGravity()
    {
        if (controller.isGrounded && velocityY < 0f)
            velocityY = groundedGravity;

        for (int i = 0; i < applyFrequency; i++)
        {
            velocityY -= gravity * gravityMultiplier * Time.deltaTime;
            controller.Move(Vector3.up * velocityY * Time.deltaTime);
        }

    }

    private void HandleJump()
    {
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocityY = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }
    }

    private IEnumerator DashCoroutine()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            controller.Move(rotatedMovement * dashSpeed * Time.deltaTime);

            yield return null;
        }
    }




    private float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (Input.GetMouseButton(1))
            {
                return UnityEngine.Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            if (Input.GetMouseButton(1))
            {
                return UnityEngine.Input.GetAxis("Mouse Y");
            }
            else
            {
                return 0;
            }
        }
        return UnityEngine.Input.GetAxis(axisName);
    }


}
