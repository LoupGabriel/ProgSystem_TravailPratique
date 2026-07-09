using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    [SerializeField] private float m_chanceOfDefend = 0.35f;

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
        float random = Random.Range(0, 1);
        if(random <= m_chanceOfDefend)
        {
            m_attachedBehavior.IsDefend(false);
            EventsManager.GetInstance().TriggerEvents(EEvents.ON_ENEMY_ATTACK, eventParam);
        }
        else
        {
            m_attachedBehavior.ChangeState(new EnemyDefend(m_attachedBehavior));
        }
        
    }
}
