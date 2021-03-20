using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Turret",menuName ="DBGTD/TurretSO")]
public class TurretSO : ScriptableObject
{
    public GameObject turretPrefab;
    public float attackDamage;
    public float attackRange;
    public float attackSpeed;



}
