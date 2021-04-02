using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyRace
{
    Orc,
    Undead
}

[CreateAssetMenu(fileName = "New Enemy", menuName = "DBGTD/EnemySO")]
public class EnemySO : ScriptableObject
{
    public EnemyRace EnemyRace;
    public GameObject Prefab;
    public int Level;
    public float Health;
    public float MoveSpeed;
    public float Shell;
    public float MagicShell;
    
}
