using UnityEngine;

public abstract class EnemyState 
{

    protected EnemyBehaviour m_attachedBehavior;

    public EnemyState(EnemyBehaviour behaviour)
    {
        m_attachedBehavior = behaviour;
    }



    public abstract void Execute();
    
}
