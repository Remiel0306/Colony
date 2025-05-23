using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("門與重生點")]
    [SerializeField] GameObject level1Door;
    [SerializeField] GameObject respawnPoint1;
    [SerializeField] GameObject respawnPoint2;

    [Header("蟲子 prefab")]
    [SerializeField] GameObject bugPrefab;

    [Header("生成設定")]
    [SerializeField] float earlySpawnInterval = 2f;   // 前15秒每隔 2 秒
    [SerializeField] float lateSpawnInterval = 2f;    // 15秒後每隔 5 秒
    [SerializeField] float switchTime = 15f;

    public int level1BoxCounter = 0;

    private float spawnTimer = 0f;
    private float timer = 0f;
    private float currentInterval;

    void Start()
    {
        currentInterval = earlySpawnInterval;
        StartCoroutine(SpawnBugsRoutine());
    }

    void Update()
    {
        if (level1BoxCounter >= 3)
        {
            level1Door.SetActive(false);
        }

        // 計算時間並切換頻率
        timer += Time.deltaTime;
        if (timer >= switchTime)
        {
            currentInterval = lateSpawnInterval;
        }
    }

    IEnumerator SpawnBugsRoutine()
    {
        while (true)
        {
            SpawnBug(respawnPoint1.transform.position);
            SpawnBug(respawnPoint2.transform.position);
            yield return new WaitForSeconds(currentInterval);
        }
    }

    void SpawnBug(Vector2 position)
    {
        Instantiate(bugPrefab, position, Quaternion.identity);
    }
}
