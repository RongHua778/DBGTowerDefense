using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField]private Turret turret;

    private float attackRange;

    public void SetAttackRange(float range)
    {
        attackRange = range;
        this.GetComponent<CircleCollider2D>().radius = range;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!turret.TurretLanded)
            return;
        if (other.CompareTag("Enemy"))
        {
            Enemy newEnemy = other.GetComponent<Enemy>();
            turret._enemies.Add(newEnemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!turret.TurretLanded)
            return;
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (turret._enemies.Contains(enemy))
            {
                turret._enemies.Remove(enemy);
            }
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
