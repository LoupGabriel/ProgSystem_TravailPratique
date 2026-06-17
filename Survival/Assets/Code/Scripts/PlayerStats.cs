using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    
    [SerializeField]
    private float m_maxHealth = 10f;
    private float m_currentHealth;
    [SerializeField]
    private float m_maxStamina = 10f;
    private float m_currentStamina;
    [SerializeField]
    private float m_maxHunger = 10f;
    public float m_currentHunger;


    public Action OnHungerChange;

    private Coroutine m_coroutine;

    private void Start()
    {
        m_currentHunger = m_maxHunger;
        m_currentStamina = m_maxStamina;
        m_currentHealth = m_maxHealth;
        m_coroutine = StartCoroutine(HungerRoutine(2f, 1f));
    }

    private IEnumerator HungerRoutine(float hungerTick,float hungerStep)
    {
        bool hasPlaySfx=false;
        while (true)
        {
            if(m_currentHunger == 0 && !hasPlaySfx)
            {
                SfxManager.PlaySfx("Hungry");
                SoundtrackManager.Instance.PlayMusic("Hungry");
                hasPlaySfx = true;
            }
            m_currentHunger -= hungerStep;
            OnHungerChange?.Invoke();
            m_currentHunger = Mathf.Clamp(m_currentHunger, 0, m_maxHunger);
            yield return new WaitForSeconds(hungerTick);


        }

    }
    public float GetMaxHunger()
    {
        return m_maxHunger;
    }

    
   

}
