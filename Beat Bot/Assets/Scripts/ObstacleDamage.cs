using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    // Damage InanimateObject inflicts
    public float damage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().TakeDamage(damage);
        }
    }
}
