using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    
    [SerializeField]
    private float m_maxHealth;
    private float m_currentHealth;
    [SerializeField]
    private float m_maxStamina;
    private float m_currentStamina;
    [SerializeField]
    private float m_maxHunger;
    private float m_currentHunger;

    private Coroutine m_coroutine;

    private void Start()
    {
        m_coroutine = StartCoroutine(HungerRoutine(2f, 1f));
    }

    private IEnumerator HungerRoutine(float hungerTick,float hungerStep)
    {

        while (true)
        {

            m_currentHunger -= hungerStep;
            yield return new WaitForSeconds(hungerTick);


        }

    }

    


}
