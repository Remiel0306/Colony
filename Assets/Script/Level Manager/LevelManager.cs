using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("���P�����I")]
    [SerializeField] GameObject level1Door;
    [SerializeField] GameObject respawnPoint1;
    [SerializeField] GameObject respawnPoint2;

    [Header("�Τl prefab")]
    [SerializeField] GameObject bugPrefab;

    [Header("�ͦ��]�w")]
    [SerializeField] float earlySpawnInterval = 2f;   // �e15��C�j 2 ��
    [SerializeField] float lateSpawnInterval = 2f;    // 15���C�j 5 ��
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

        // �p��ɶ��ä����W�v
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
