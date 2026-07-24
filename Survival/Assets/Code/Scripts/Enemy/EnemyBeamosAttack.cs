using System.Collections.Generic;
using UnityEngine;

public class EnemyBeamosAttack : EnemyBeamosState
{
    Dictionary<string, object> eventParam;

    public EnemyBeamosAttack(EnemyBeamosBehavior behaviour) : base(behaviour)
    {
    }

    public override void Execute()
    {
        //shoot a ball
      m_attachedBehavior.StartCoroutine(m_attachedBehavior.ShootingRoutine());
    }

    
}
