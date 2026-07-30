using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{

    private static ProjectilePool Instance;
    private List<Projectile> m_projectilesPool;
    [SerializeField] private GameObject m_projectilePrefab;

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

        m_projectilesPool =  new List<Projectile>();
    }

  

    public Projectile GetAvailableProjectile()
    {

        //clean old reference on loading
        m_projectilesPool.RemoveAll(projectile => projectile == null);

        foreach (var p in m_projectilesPool)
        {
            if (!p.gameObject.activeInHierarchy)
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
