using UnityEngine;

public class AttackHitbox : MonoBehaviour
{


    private PlayerCombat playerCombat;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            float attackDamage = playerCombat.GetCurrentAttackDamage();
            int combo = playerCombat.GetCurrentCombo();


            enemy.Damage(attackDamage);
            Debug.Log("Attack hit enemy for " + attackDamage + " damage. Combo: " + combo);
            

        }
    }

    private void Awake()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
    }
}
