using UnityEngine;
using System.Collections.Generic;

public class AttackHitbox : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private HashSet<Enemy> enemiesHit = new HashSet<Enemy>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only accept collisions from the enemy's Hurtbox
        if (other.gameObject.name != "Hurtbox")
            return;

        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy != null && enemiesHit.Add(enemy))
        {
            float attackDamage = playerCombat.GetCurrentAttackDamage();
            int combo = playerCombat.GetCurrentCombo();

            enemy.Damage(attackDamage);

            if (enemy is GroundEnemy groundEnemy && (playerCombat.GetCurrentWeapon() == WeaponType.Weapon1 || playerCombat.GetCurrentWeapon() == WeaponType.Weapon3))
            {
                groundEnemy.TakeHit(transform.position);
            }

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
