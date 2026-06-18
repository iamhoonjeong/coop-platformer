using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static event Action OnPlayerRespawn;
    public static PlayerManager instance;

    [Header("Player")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform respawnPoint;
    [SerializeField] float respawnDelay;
    public Player player;

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
        if (respawnPoint == null) respawnPoint = FindFirstObjectByType<StartPoint>().transform;
        if (player == null) player = FindFirstObjectByType<Player>();
    }

    public void RespawnPlayer()
    {
        DifficultyManager difficultyManager = DifficultyManager.instance;
        if (difficultyManager && difficultyManager.difficulty == DifficultyType.Hard) return;

        StartCoroutine(RespawnCoroutine());
        OnPlayerRespawn?.Invoke();
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player>();
    }

    public void UpdateRespawnPosition(Transform newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }
}
