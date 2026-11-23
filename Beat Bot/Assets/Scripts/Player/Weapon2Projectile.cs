
using UnityEngine;

public class Weapon2Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float damage = 2f;

    private Vector2 direction;

    public void Initialize(Vector2 projectileDirection)
    {
        direction = projectileDirection.normalized;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only accept collisions from the enemy's Hurtbox
        if (other.gameObject.name != "Hurtbox")
            return;

        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            enemy.Damage(damage);

            Debug.Log("Weapon 2 projectile hit enemy for " + damage + " damage.");

            Destroy(gameObject);
        }
    }
}
