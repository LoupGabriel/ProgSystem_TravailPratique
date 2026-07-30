using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_damageAmount = 1f;
    [SerializeField] private float m_lifetime = 2f;

    private Coroutine m_Coroutine;
    private void Start()
    {
        m_Coroutine=  StartCoroutine(lifetimeRoutine());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Dictionary<string, object> eventParam = new Dictionary<string, object>();
            eventParam.Add("AttackDamage", m_damageAmount);
            EventsManager.GetInstance().TriggerEvents(EEvents.ON_PROJECTILE_HIT, eventParam);
            gameObject.SetActive(false);
        }
    }

    private IEnumerator lifetimeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_lifetime);
            gameObject.SetActive(false);
        }
    }

    public void ResetProjectile()
    {
        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
        }
       m_Coroutine = StartCoroutine(lifetimeRoutine());
    }

}
