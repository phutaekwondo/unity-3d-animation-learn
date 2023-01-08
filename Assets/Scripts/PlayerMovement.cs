using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //variables
    [SerializeField] private float m_walkSpeed = 5f;
    [SerializeField] private float m_runSpeed = 10f;
    private Vector3 m_forwardDirection;
    private Vector3 m_moveDirection;

    //references
    private CharacterController m_controller;
    private Animator m_animator;
    [SerializeField] private Transform m_playerModelTransform;

    private void Start() 
    {
        m_controller = GetComponent<CharacterController>();
        m_animator = GetComponentInChildren<Animator>();
    }

    private void Update() 
    {
        UpdateForwardDirection();
        HandleInput();
        RotatePlayerOnMoveDirection();
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

    private void RotatePlayerOnMoveDirection()
    {
        // if the move direction is not zero
        if (m_moveDirection != Vector3.zero)
        {
            m_playerModelTransform.forward = m_moveDirection;
        }
    }

    private void HandleInput()
    {
        #region Update Move Direction
        // update move direction
        m_moveDirection = m_forwardDirection * Input.GetAxis("Vertical");

        // move right direction depend on the m_forwardDirection
        Vector3 moveRight = Quaternion.AngleAxis(90f, Vector3.up) * m_forwardDirection;
        Vector3 moveHorizontal = moveRight * Input.GetAxis("Horizontal");

        m_moveDirection += moveHorizontal;
        m_moveDirection.Normalize();
        #endregion

        // move the player if the move direction is not zero
        if ( m_moveDirection != Vector3.zero)
        {
            // if the player is holding shift
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // run
                Run(m_moveDirection);
            }
            else
            {
                // walk
                Walk(m_moveDirection);
            }
        }
        else
        {
            // idle
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

    private void Walk(Vector3 moveDirection)
    {
        m_controller.Move(moveDirection * m_walkSpeed * Time.deltaTime);
        SetAnimatorMoveSpeed(0.5f);
    }
    private void Run( Vector3 moveDirection)
    {
        m_controller.Move(moveDirection * m_runSpeed * Time.deltaTime);
        SetAnimatorMoveSpeed(1f);
    }
    private void Attack()
    {
        m_animator.SetTrigger("Attack");
    }
}
