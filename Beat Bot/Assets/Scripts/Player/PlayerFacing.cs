
using UnityEngine;

public class PlayerFacing : MonoBehaviour
{
    public Sprite frontSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        spriteRenderer.sprite = frontSprite;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal < 0)
        {
            spriteRenderer.sprite = leftSprite;
        }
        else if (horizontal > 0)
        {
            spriteRenderer.sprite = rightSprite;
        }
        else
        {
            spriteRenderer.sprite = frontSprite;
        }
    }
}
