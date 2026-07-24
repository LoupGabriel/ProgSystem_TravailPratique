using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_damageAmount = 1f;
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
}
