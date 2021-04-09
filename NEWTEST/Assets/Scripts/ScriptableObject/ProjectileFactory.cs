using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum ProjectileType
{
    Target,
    Ground,
    Fly,
    Wave
}
[CreateAssetMenu(fileName = "New ProjectileFactory", menuName = "DBGTD/Factory")]
public class ProjectileFactory : ScriptableObject
{
    private Dictionary<ProjectileType, Projectile> projectileDic;
    [SerializeField]
    Projectile[] ProjectilsPrefabs;

    private bool IsInitialized => projectileDic != null;

    public void Initialize()
    {
        projectileDic = new Dictionary<ProjectileType, Projectile>();
        foreach(var projectile in ProjectilsPrefabs)
        {
            projectileDic.Add(projectile.ProjectileType, projectile);
        }
    }

    public Projectile GetProjectile(ProjectileType type=ProjectileType.Target)
    {
        Projectile instance = ObjectPool.Instance.Spawn(projectileDic[type].gameObject).GetComponent<Projectile>();
        return instance;
    }

}
