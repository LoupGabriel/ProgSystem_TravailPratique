using UnityEngine;

public abstract class EnemyBeamosState
{

    protected EnemyBeamosBehavior m_attachedBehavior;


    public EnemyBeamosState(EnemyBeamosBehavior behaviour)
    {
        m_attachedBehavior = behaviour;
    }



    public abstract void Execute();

}

