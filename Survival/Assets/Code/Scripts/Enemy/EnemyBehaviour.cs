using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private EnemyState m_currentState;


    private PlayerStats m_player;
    [SerializeField] private float m_speed = 3f;
    [SerializeField] public float m_distanceToAttack = 1f;
    [SerializeField] private float m_attackDamage = 1f;
    private bool m_isDefending = false;


    private void Start()
    {
        m_player = FindAnyObjectByType<PlayerStats>();
        m_currentState = new EnemyIdle(this);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_PLAYER_ATTACK, CheckforHit);
    }
    public void ChangeState(EnemyState newState)
    {
        m_currentState = newState;
    }

    private void Update()
    {
        m_currentState.Execute();
    }

    private void OnDestroy()
    {
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_PLAYER_ATTACK, CheckforHit);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeState(new EnemyChase(this));
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeState(new EnemyIdle(this));
        }
    }

    public PlayerStats GetPlayer()
    {
        return m_player;
    }
    public float GetSpeed()
    {
        return m_speed;
    }
    public float GetAttackDamage()
    {
        return m_attackDamage;
    }
    public void IsDefend(bool enemyDefend)
    {
        m_isDefending = enemyDefend;
    }


    private void CheckforHit(Dictionary<string, object> parameters)
    {

        Vector3 direction = m_player.gameObject.transform.position - transform.position;

        float distance = direction.magnitude;
        if(distance <= m_player.GetDistanceToHit() && !m_isDefending)
        {
            ChangeState(new EnemyDead(this));   
        }
    }


    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }






}
