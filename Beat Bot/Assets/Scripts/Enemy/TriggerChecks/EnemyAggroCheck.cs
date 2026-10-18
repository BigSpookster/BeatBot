using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    public GameObject PlayerTarget {get; set;}
    private Enemy enemy;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponentInParent<Enemy>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        
        if (collision.CompareTag("Player"))
            enemy.SetAggroStatus(true);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            enemy.SetAggroStatus(false);

    }

}
