using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Player")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform respawnPoint;
    [SerializeField] float respawnDelay;

    public Player player;

    [Header("Fruits Management")]
    public bool fruitsAreRandom;
    public int fruitsCollected;
    public int totalFruits;

    [Header("Checkpoints")]
    public bool canReactivate;

    [Header("Traps")]
    public GameObject arrowPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);

        }
    }

    void Start()
    {
        CollectFruitsInfo();
    }

    private void CollectFruitsInfo()
    {
        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totalFruits = allFruits.Length;
    }

    public void UpdateRespawnPosition(Transform newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player>();
    }

    public void AddFruit()
    {
        fruitsCollected++;
    }

    public bool FruitsHaveRandomLoom()
    {
        return fruitsAreRandom;
    }

    public void CreateObject(GameObject prefab, Transform target, float delay = 0)
    {
        StartCoroutine(CreateObjectCoroutine(prefab, target, delay));
    }

    IEnumerator CreateObjectCoroutine(GameObject prefab, Transform target, float delay)
    {
        Vector3 newPosition = target.position;

        yield return new WaitForSeconds(delay);

        GameObject newObject = Instantiate(prefab, newPosition, Quaternion.identity);
    }
}
