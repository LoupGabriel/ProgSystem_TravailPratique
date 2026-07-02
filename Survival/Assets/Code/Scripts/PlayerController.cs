using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset m_actionAsset;
    private Animator m_animator;

    [Header("Movement Stats")]
    [SerializeField] private float m_playerSpeed;
    [SerializeField] private float m_playerTurnSpeed = 180f;




    [Header("Attack Stats")]
    [SerializeField] private int m_attackDamage;
    [SerializeField] private int m_attackStamina;


    private InputAction m_moveAction;
    private InputAction m_meleeAction;
    private Vector2 m_moveAmount;
    private bool m_isAttacking;
    private Vector3 m_input;

    private Rigidbody m_rb;
    private void Start()
    {
        m_moveAction = m_actionAsset.FindAction("Move");
        m_meleeAction = m_actionAsset.FindAction("Attack");
        m_rb = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
    }





    private void Update()
    {
        if (PauseController.m_isGamePaused) return;
        HandleInput();
        HandleAttack();
    }
    private void FixedUpdate()
    {
        if (PauseController.m_isGamePaused) return;

        Movement();

        LookDirection();
    }
    private void HandleInput()
    {
        m_moveAmount = m_moveAction.ReadValue<Vector2>();
        m_isAttacking = m_meleeAction.WasPressedThisFrame();

        m_input = new Vector3(m_moveAmount.x, 0, m_moveAmount.y);
    }


    private void LookDirection()
    {

        if (m_input != Vector3.zero)
        {
            //calculated the input direction
            Vector3 direction = (transform.position + m_input.ToIso()) - transform.position;
            //look at the input direction
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, m_playerTurnSpeed);
        }
    }
    private void Movement()
    {



        //move the player to the expect position 
        m_rb.MovePosition(transform.position + (transform.forward * m_input.magnitude) * m_playerSpeed * Time.deltaTime);


        //set animator data
        m_animator.SetFloat("currentSpeed", Mathf.Abs(m_rb.linearVelocity.z));
    }


    private void HandleAttack()
    {
        if (m_isAttacking)
        {
            Dictionary<string, object> eventParam = new Dictionary<string, object>();
            eventParam.Add("AttackStamina", m_attackStamina);
            eventParam.Add("Attack", m_attackDamage);
            m_animator.SetTrigger("Attack");
            EventsManager.GetInstance().TriggerEvents(EEvents.ON_PLAYER_ATTACK, eventParam);
        }


    }
}

   
