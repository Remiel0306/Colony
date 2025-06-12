using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2SceondBoxSystem : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] Animator doorAnime;
    public int boxCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(boxCounter >= 4)
        {
            doorAnime.SetBool("canOpen", true);
            StartCoroutine(DoorOpen());
        }
    }

    IEnumerator DoorOpen()
    {
        yield return new WaitForSeconds(1f);

        door.SetActive(false);
    }
}
