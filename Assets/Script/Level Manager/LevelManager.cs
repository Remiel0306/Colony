using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [Header("���P�����I")]
    [SerializeField] GameObject level1Door;
    [SerializeField] GameObject respawnPoint1;
    [SerializeField] GameObject respawnPoint2;
    [SerializeField] Animator doorOpen;

    [Header("�Τl prefab")]
    [SerializeField] GameObject bugPrefab;

    [Header("�ͦ��]�w")]
    [SerializeField] float earlySpawnInterval = 3f;   // �e15��C�j 2 ��
    [SerializeField] float lateSpawnInterval = 3f;    // 15���C�j 5 ��
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

        // �p��ɶ��ä����W�v
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
            // �p�G�c�l�ƹF�� 3�A�N����ͦ�
            if (level1BoxCounter >= 3)
                yield break; // ������{

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
        // �M��������Ҧ��� GameObject
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
