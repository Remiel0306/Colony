using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] PlayerManager playerManager;

    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = enemy.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.isDied)
        {
            enemy.StartCoroutine(enemy.Died());
        }
    }

    public void ResetBugs()
    {
        enemy.transform.position = startPosition;
    }
}
