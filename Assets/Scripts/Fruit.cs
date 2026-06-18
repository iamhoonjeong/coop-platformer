using UnityEngine;

public enum FruitType { Apple, Banana, Cherry, Kiwi, Melon, Orange, Pineapple, Strawberry }

public class Fruit : MonoBehaviour
{
    [SerializeField] FruitType fruitType;
    [SerializeField] GameObject pickupVFX;

    GameManager gameManager;
    protected Animator anim;
    protected SpriteRenderer sr;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        gameManager = GameManager.instance;
        SetRandomLookIfNeeded();
    }

    void SetRandomLookIfNeeded()
    {
        if (!gameManager.FruitsHaveRandomLoom())
        {
            UpdateFruitVisuals();
            return;
        }

        int randomIndex = Random.Range(0, 8);
        anim.SetFloat("fruitIndex", randomIndex);
    }

    void UpdateFruitVisuals()
    {
        anim.SetFloat("fruitIndex", (int)fruitType);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            gameManager.AddFruit();
            AudioManager.instance.PlaySFX(8);
            Destroy(gameObject);

            GameObject newFx = Instantiate(pickupVFX, transform.position, Quaternion.identity);
        }
    }
}
