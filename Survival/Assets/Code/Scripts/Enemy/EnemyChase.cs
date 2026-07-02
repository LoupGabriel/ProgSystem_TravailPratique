using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyChase : EnemyState
{

    private Vector3 direction = Vector3.zero;

    public EnemyChase(EnemyBehaviour behaviour) : base(behaviour)
    {
    }
    public override void Execute()
    {
        direction = m_attachedBehavior.GetPlayer().transform.position - m_attachedBehavior.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

        m_attachedBehavior.transform.rotation = Quaternion.RotateTowards(m_attachedBehavior.transform.rotation, rotation, 180);


        m_attachedBehavior.transform.Translate(direction * m_attachedBehavior.GetSpeed() * Time.deltaTime);

        if (direction.magnitude < m_attachedBehavior.m_distanceToAttack)
        {
            StartAttack();
        }
    }



    private void StartAttack()
    {
        m_attachedBehavior.ChangeState(new EnemyAttack(m_attachedBehavior));
    }



}
