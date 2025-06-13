using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject comic1;
    [SerializeField] GameObject pageDownBtn;
    //[SerializeField] GameObject blackBack;
    // Start is called before the first frame update
    void Start()
    {
        comic1.SetActive(true);
        pageDownBtn.SetActive(true);
        //blackBack.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
