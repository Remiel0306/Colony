using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyType2 : MonoBehaviour
{
    [SerializeField] FlyEnemy flyEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flyEnemy.canDestory)
        {
            Destory();
        }
    }

    public void Destory()
    {
        Destroy(gameObject);
    }
}
