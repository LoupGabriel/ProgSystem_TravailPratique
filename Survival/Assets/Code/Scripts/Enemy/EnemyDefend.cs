using System.Collections;
using UnityEngine;

public class EnemyDefend : EnemyState

{

    [SerializeField] private float m_defendTime = 1f;
    private float m_elapse = 0;
    private bool m_isDefend = false;
    
    public EnemyDefend(EnemyBehaviour behaviour) : base(behaviour)
    {
    }
    public override void Execute()
    {
        m_isDefend = !m_isDefend;
        m_elapse += Time.deltaTime;

        if(m_elapse > m_defendTime)
        {
            m_attachedBehavior.IsDefend(m_isDefend);
            m_elapse = 0;
            m_attachedBehavior.ChangeState(new EnemyIdle(m_attachedBehavior));
        }
    }
    
  
}
