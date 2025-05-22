using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject level1Door;

    public int level1BoxCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (level1BoxCounter >= 3)
        {
            level1Door.SetActive(false);
        }
    }
}
