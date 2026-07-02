using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyState
{


    private float m_elapse = 0;
    Dictionary<string, object> eventParam;
    public EnemyAttack(EnemyBehaviour behaviour) : base(behaviour)
    {
       eventParam = new Dictionary<string, object>();
      
        eventParam.Add("AttackDamage",m_attachedBehavior.GetAttackDamage());
    }
    public override void Execute()
    {
       
        
        m_elapse += Time.deltaTime;

        if (m_elapse >= 0.5f)
        {
            TryAttack();
            m_elapse = 0;
        }


    }



    private void TryAttack()
    {
        EventsManager.GetInstance().TriggerEvents(EEvents.ON_ENEMY_ATTACK, eventParam);
    }
}
