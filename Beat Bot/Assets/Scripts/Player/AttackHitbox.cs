using UnityEngine;
using System.Collections.Generic;

public class AttackHitbox : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private HashSet<Enemy> enemiesHit = new HashSet<Enemy>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy != null && enemiesHit.Add(enemy))
        {
            float attackDamage = playerCombat.GetCurrentAttackDamage();
            int combo = playerCombat.GetCurrentCombo();

            enemy.Damage(attackDamage);
            Debug.Log("Attack hit enemy for " + attackDamage + " damage. Combo: " + combo);
        }
    }

    public void ResetHitEnemies()
    {
        enemiesHit.Clear();
    }

    private void Awake()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
    }
}
