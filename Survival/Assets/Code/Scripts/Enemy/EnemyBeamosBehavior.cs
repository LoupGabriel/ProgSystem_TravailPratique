
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeamosBehavior : MonoBehaviour
{
    private EnemyBeamosState m_currentState;
    [SerializeField] private List<Item> m_itemToDrop;
    [SerializeField] public float m_sightDistance = 100f;
    [SerializeField] public GameObject m_projectile;
    [SerializeField] public GameObject m_head;
    [SerializeField] public Transform m_shootingTip;
    [SerializeField] public float m_rotationSpeed;
    [SerializeField] private float m_shootingForce = 10f;
    [SerializeField] private float m_delayToRestart = 1f;




    private bool m_hasShoot = false;
    private PlayerStats m_player;

    private Animation m_animation;
    private void Update()
    {
        m_currentState.Execute();
    }

    private void Start()
    {
        m_animation = GetComponent<Animation>();
     
        InventorySystem.GetInstance();
        m_player = FindAnyObjectByType<PlayerStats>();
        m_currentState = new EnemyBeamosIdle(this);
        EventsManager.GetInstance().SubscribeTo(EEvents.ON_PLAYER_ATTACK, CheckforHit);
    }
    private void OnDestroy()
    {
        EventsManager.GetInstance().UnsubscribeFrom(EEvents.ON_PLAYER_ATTACK, CheckforHit);
    }
    public void ChangeState(EnemyBeamosState newState)
    {
        m_currentState = newState;
    }
    private void CheckforHit(Dictionary<string, object> parameters)
    {

        Vector3 direction = m_player.gameObject.transform.position - transform.position;

        float distance = direction.magnitude;
        if (distance <= m_player.GetDistanceToHit() )
        {
            ChangeState(new EnemyBeamosDeath(this));
        }
    }
    public void Die()
    {
        m_animation.Play();
    }
    public void DestroyEnemy()
    {
        
        Item item = m_itemToDrop[Random.Range(0, m_itemToDrop.Count)];

        Dictionary<string, object> eventParam = new Dictionary<string, object>();
        eventParam.Add("DropItem", item);

        EventsManager.GetInstance().TriggerEvents(EEvents.ON_ENEMY_DEATH, eventParam);

        Destroy(this.gameObject);
    }
    


    public IEnumerator ShootingRoutine()
    {
        if (!m_hasShoot)
        {
            SfxManager.PlaySfx("Shoot");
            Projectile projectile = ProjectilePool.GetInstance().GetAvailableProjectile();
           
            projectile.transform.SetPositionAndRotation(m_shootingTip.position, m_shootingTip.rotation);
           
            projectile.gameObject.SetActive(true);
            projectile.ResetProjectile();
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;

            Vector3 shootingDirection = m_shootingTip.forward.normalized;
            rb.AddForce(shootingDirection * m_shootingForce ,ForceMode.Impulse);

           
            m_hasShoot = true;
        }
        

        yield return new WaitForSeconds(m_delayToRestart);

        ChangeState(new EnemyBeamosIdle(this));
        m_hasShoot = false;

    }

}
