using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ProjectileFactory", menuName = "DBGTD/Factory")]
public class ProjectileFactory : ScriptableObject
{
    private Dictionary<ProjectileType, Projectile> projectileDic;
    [SerializeField]
    Projectile[] ProjectilsPrefabs;

    private bool IsInitialized => projectileDic != null;

    private void Initialize()
    {
        if (IsInitialized)
            return;
        projectileDic = new Dictionary<ProjectileType, Projectile>();
        foreach(var projectile in ProjectilsPrefabs)
        {
            projectileDic.Add(projectile.ProjectileType, projectile);
        }
    }

    public Projectile GetProjectile(ProjectileType type=ProjectileType.Target)
    {
        Initialize();
        Projectile instance = ObjectPool.Instance.Spawn(projectileDic[type].gameObject).GetComponent<Projectile>();
        return instance;
    }

}
