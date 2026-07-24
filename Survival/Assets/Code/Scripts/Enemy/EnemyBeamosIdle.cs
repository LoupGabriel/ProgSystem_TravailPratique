using UnityEngine;

public class EnemyBeamosIdle : EnemyBeamosState
{
    private bool m_animationStarted = false;
    public EnemyBeamosIdle(EnemyBeamosBehavior behaviour) : base(behaviour)
    {
    }

    public override void Execute()
    {
        HandleMovement();
        HandleLaser();
    }


    private void HandleMovement()
    {
        float speed = m_attachedBehavior.m_rotationSpeed;

        m_attachedBehavior.m_head.transform.Rotate(Vector3.up *speed * Time.deltaTime);
    }

    private void HandleLaser()
    {
        RaycastHit hit;

        Debug.DrawLine(m_attachedBehavior.m_shootingTip.position, m_attachedBehavior.m_shootingTip.position
            + (m_attachedBehavior.m_shootingTip.forward * m_attachedBehavior.m_sightDistance));

        if (Physics.Raycast(m_attachedBehavior.m_shootingTip.position, m_attachedBehavior.m_shootingTip.forward,out hit,m_attachedBehavior.m_sightDistance))
        {

            if (hit.collider.CompareTag("Player"))
            {
                
                m_attachedBehavior.ChangeState(new EnemyBeamosAttack(m_attachedBehavior));
                m_animationStarted = false;
            }
            
        }
        
       
    }
}

