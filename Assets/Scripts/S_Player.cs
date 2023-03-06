using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Player : MonoBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private Animator ac;
    [SerializeField] private Transform cam;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float MoveSpeed = 6f;

    private Vector3 Velocity;

    [SerializeField] private float TurnSmoothTime = .1f;
    private float TurnSmoothVelocity;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    private float groundDistance =.4f;


    private bool isGrounded;

    private Vector3 AdjustVelocityToSlope(Vector3 velocity) 
    {
        var ray = new Ray(cc.transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, .8f))
        {

            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if (adjustedVelocity.y < 0) 
            { 
                return adjustedVelocity;
            }
             
        }

        return velocity;
    }

    private void FixedUpdate()
    {


        // GRAVITY //

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        ac.SetBool("IsGrounded", isGrounded);
        

        if (isGrounded && Velocity.y < 0)
        {
            Velocity.y = 0;
        }

        Velocity.y -= gravity * Time.deltaTime;

        cc.Move(Velocity * Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {


        // Get Key //
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        if (direction.magnitude >= 0.1f)
        {   
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref TurnSmoothVelocity, TurnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            moveDir = AdjustVelocityToSlope(moveDir);

            cc.Move(moveDir.normalized * MoveSpeed * Time.deltaTime);

        }   //Move Character //

        ac.SetFloat("MoveSpeed", direction.magnitude);
    }
}
