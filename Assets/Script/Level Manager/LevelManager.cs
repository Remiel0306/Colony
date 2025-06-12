using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [Header("門與重生點")]
    [SerializeField] GameObject level1Door;
    [SerializeField] GameObject respawnPoint1;
    [SerializeField] GameObject respawnPoint2;
    [SerializeField] Animator doorOpen;

    [Header("蟲子 prefab")]
    [SerializeField] GameObject bugPrefab;

    [Header("生成設定")]
    [SerializeField] float earlySpawnInterval = 3f;   // 前15秒每隔 2 秒
    [SerializeField] float lateSpawnInterval = 3f;    // 15秒後每隔 5 秒
    [SerializeField] float switchTime = 15f;

    public int level1BoxCounter = 0;
    public bool canSpawn = true;

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
            doorOpen.SetBool("canOpen", true);
            StartCoroutine(DoorOpen());
        }

        // 計算時間並切換頻率
        timer += Time.deltaTime;
        if (timer >= switchTime)
        {
            currentInterval = lateSpawnInterval;
        }

        if (playerManager.isPlayerDead)
        {
            DeleteAllStupidBoomBugs();
        }
    }

    IEnumerator SpawnBugsRoutine()
    {
        while (true)
        {
            // 如果箱子數達到 3，就停止生成
            if (level1BoxCounter >= 3)
                yield break; // 結束協程

            SpawnBug(respawnPoint1.transform.position);
            SpawnBug(respawnPoint2.transform.position);
            yield return new WaitForSeconds(currentInterval);
        }
    }

    void SpawnBug(Vector2 position)
    {
        Instantiate(bugPrefab, position, Quaternion.identity);
    }

    void DeleteAllStupidBoomBugs()
    {
        // 尋找場景中所有的 GameObject
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("StupidBoomBug"))
        {
            Destroy(go);
        }
    }

    IEnumerator DoorOpen()
    {
        yield return new WaitForSeconds(1f);
        level1Door.SetActive(false);
        doorOpen.SetBool("canOpen", false);
    }
}
