using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageController1 : MonoBehaviour
{
    [SerializeField] Comic3 comic3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PageDown()
    {
        comic3.pageCounter++;
    }
}
