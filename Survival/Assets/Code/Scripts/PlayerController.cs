using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset m_actionAsset;
    private Animator m_animator;

    [Header("Movement Stats")]
    [SerializeField] private float m_playerSpeed;
    [SerializeField] private float m_playerTurnSpeed = 180f;

    private InputAction m_moveAction;

    private Vector2 m_moveAmount;
    private Vector3 m_input;

    private Rigidbody m_rb;
    private void Start()
    {
        m_moveAction = m_actionAsset.FindAction("Move");
        m_rb = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_moveAction.Enable();
    }

    private void OnDisable()
    {
        m_moveAction.Disable();
    }



    private void Update()
    {
        HandleInput();

    }
    private void FixedUpdate()
    {
        Movement();
        LookDirection();
    }
    private void HandleInput()
    {
        m_moveAmount = m_moveAction.ReadValue<Vector2>();
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

            transform.rotation = Quaternion.RotateTowards(transform.rotation,rotation, m_playerTurnSpeed);
        }
    }
    private void Movement()
    {

        

        //move the player to the expect position 
        m_rb.MovePosition(transform.position +(transform.forward * m_input.magnitude)*m_playerSpeed* Time.deltaTime);
     

        //set animator data
        m_animator.SetFloat("currentSpeed",Mathf.Abs(m_rb.linearVelocity.z));
    }


}
