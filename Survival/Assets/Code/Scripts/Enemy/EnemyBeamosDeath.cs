using UnityEditor;
using UnityEngine;

public class EnemyBeamosDeath : EnemyBeamosState
{
    private bool m_deadAnimStart = false;
    public EnemyBeamosDeath(EnemyBeamosBehavior behaviour) : base(behaviour)
    {
    }

    public override void Execute()
    {
        if (!m_deadAnimStart)
        {
            SetDeadAnimation();
        }
    }


    private void SetDeadAnimation()
    {
        m_attachedBehavior.Die();
        m_attachedBehavior.DestroyEnemy();
        m_deadAnimStart = true;
    }
}
