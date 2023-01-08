using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //variables
    [SerializeField] private float m_walkSpeed = 5f;
    [SerializeField] private float m_runSpeed = 10f;
    private Vector3 m_forwardDirection;

    //references
    private CharacterController m_controller;
    private Animator m_animator;

    private void Start() 
    {
        m_controller = GetComponent<CharacterController>();
        m_animator = GetComponentInChildren<Animator>();
    }

    private void Update() 
    {
        UpdateForwardDirection();
        HandleInput();
    }

    private void UpdateForwardDirection()
    {
        // get the forward direction of the camera
        m_forwardDirection = Camera.main.transform.forward;
        // set the y to 0
        m_forwardDirection.y = 0;
        // normalize the vector
        m_forwardDirection.Normalize();
    }

    private void HandleInput()
    {
        // if the vertical axis is not 0
        // move control input
        if (Input.GetAxis("Vertical") != 0)
        {
            // if shift is pressed
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Run(Input.GetAxis("Vertical"));
            }
            else
            {
                Walk(Input.GetAxis("Vertical"));
            }
        }
        else
        {
            Idle();
        }

        // attack input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    private void SetAnimatorMoveSpeed(float speed)
    {
        m_animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    private void Idle()
    {
        SetAnimatorMoveSpeed(0f);
    }

    private void Walk(float forwardWeight)
    {
        m_controller.Move(m_forwardDirection * m_walkSpeed *Time.deltaTime * forwardWeight);
        SetAnimatorMoveSpeed(0.5f);
    }

    private void Run(float forwardWeight)
    {
        m_controller.Move(m_forwardDirection * m_runSpeed *Time.deltaTime * forwardWeight);
        SetAnimatorMoveSpeed(1f);
    }

    private void Attack()
    {
        m_animator.SetTrigger("Attack");
    }
}
