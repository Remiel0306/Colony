using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] Transform Player;
    
    PlayerControl playerCrontrol;

    private Vector3 offset = new Vector3(.5f, .8f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        playerCrontrol = Player.GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        // CamFollowOgj face left will becom z -10, it's not finish !!!!

        if (playerCrontrol.facingRight)
        {
            transform.position = Player.position + offset;
        }
        else
        {
            Vector3 flippedOffset = new Vector3(-offset.x, offset.y, offset.z);
            transform.position = Player.position + flippedOffset;
        }
    }
}
