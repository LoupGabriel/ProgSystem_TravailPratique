using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{

    private static ProjectilePool Instance;


    public static ProjectilePool GetInstance()
    {
        return Instance;

    }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private List<Projectile> m_projectilesPool = new List<Projectile>();
    [SerializeField] private GameObject m_projectilePrefab;

    public Projectile GetAvailableProjectile()
    {
        foreach (var p in m_projectilesPool)
        {
            if (!p.isActiveAndEnabled)
            {
                return p;
            }
        }

        GameObject newProjectile = Instantiate(m_projectilePrefab);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();

        m_projectilesPool.Add(projectileComponent);
        return projectileComponent;
    }
}
