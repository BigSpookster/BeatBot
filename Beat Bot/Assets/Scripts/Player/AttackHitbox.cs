using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 3.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            enemy.Damage(damage);
            Debug.Log("Hit " + enemy.name + " for " + damage + " damage!");
        }
    }
}
