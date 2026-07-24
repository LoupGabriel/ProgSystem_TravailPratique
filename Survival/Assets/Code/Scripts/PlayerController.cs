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
    private bool m_canAttack = true;

    private bool m_noStamina = false;

    private InputAction m_moveAction;
    private InputAction m_meleeAction;

    private InputAction m_inventory;
    private Vector2 m_moveAmount;
    private bool m_isAttacking;
    private bool m_inventoryPress;
    private Vector3 m_input;
    private bool m_isDead = false;
    private Rigidbody m_rb;

    
    private void Start()
    {
        m_moveAction = m_actionAsset.FindAction("Move");
        m_meleeAction = m_actionAsset.FindAction("Attack");
        m_inventory = m_actionAsset.FindAction("Inventory");

        
        m_rb = GetComponent<Rigidbody>();
       
        m_animator = GetComponent<Animator>();
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_PLAYER_DEAD, TriggerDead);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_NOT_ENOUGHT_STAMINA, SetCantAttack);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_ENOUGHT_STAMINA, SetCanAttack);
    }

    private void OnDestroy()
    {
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_PLAYER_DEAD, TriggerDead);
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_NOT_ENOUGHT_STAMINA, SetCantAttack);
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_ENOUGHT_STAMINA, SetCanAttack);
    }



    private void Update()
    {
        m_inventoryPress = m_inventory.WasPressedThisFrame();
        if (m_inventoryPress)
        {
            Dictionary<string, object> eventParam = new Dictionary<string, object>();
            eventParam.Add("toggle", m_inventoryPress);
          
          
            EventsManager.GetInstance().TriggerEvents(EEvents.ON_INVENTORY_TOGGLE, eventParam);
            
        }

        if (PauseController.m_isGamePaused) return;
        if (m_isDead) return;
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

        m_rb.MovePosition(
       transform.position +
       (transform.forward * m_input.magnitude) *
       m_playerSpeed *
       Time.deltaTime);

        bool isMoving = m_input.magnitude > 0.01f;

        if (isMoving)
        {
            m_animator.SetTrigger("Run");
        }
        else
        {
            m_animator.SetTrigger("Idle");
        }

    }


    private void HandleAttack()
    {
        
        if (m_isAttacking && m_canAttack )
        {
            Dictionary<string, object> eventParam = new Dictionary<string, object>();
            eventParam.Add("AttackStamina", m_attackStamina);
            eventParam.Add("Attack", m_attackDamage);
            m_animator.SetTrigger("Attack");
            EventsManager.GetInstance().TriggerEvents(EEvents.ON_PLAYER_ATTACK, eventParam);
            SfxManager.PlaySfx("PlayerAttack");
        }


    }


    private void TriggerDead(Dictionary<string, object> parameters)
    {
        if ((bool)parameters["hungerDeath"])
        {
            m_animator.SetTrigger("Hunger");
        }
        else
        {
            m_animator.SetTrigger("Dead");
        }
       
        m_isDead = true;


    }
    public void SetCanAttack(Dictionary<string, object> param)
    {
        m_canAttack = (bool)param["EnoughtStamina"];

    }
    public void SetCantAttack(Dictionary<string, object> param)
    {
        m_canAttack = !(bool)param["NotEnoughtStamina"];

    }


    public void PlaySound(string soundToPlay)
    {
        SfxManager.PlaySfx(soundToPlay, gameObject.transform.position);
    }


    
   
}

   
