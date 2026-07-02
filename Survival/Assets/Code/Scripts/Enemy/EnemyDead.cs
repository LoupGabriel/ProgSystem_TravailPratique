using System.Collections;
using UnityEngine;
using System;
public class EnemyDead : EnemyState
{
    private bool m_deadAnimStart = false;
    public EnemyDead(EnemyBehaviour behaviour) : base(behaviour)
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
        m_attachedBehavior.gameObject.GetComponent<Animator>().SetTrigger("Dead");
        m_deadAnimStart = true;  
    }
}

