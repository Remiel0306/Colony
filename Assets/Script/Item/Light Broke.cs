using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightBroke : MonoBehaviour
{
    [SerializeField] Animator lightAnimator;
    [SerializeField] GameObject NormalLight;
    [SerializeField] GameObject light;
    [SerializeField] bool isGotShoot = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isGotShoot)
        {
            lightAnimator.SetBool("isBroken", true);
            light.SetActive(false);
            StartCoroutine(BrokeTheLight());
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Normal Bullet") || collider.gameObject.CompareTag("Shotgun Bullet"))
        {
            isGotShoot = true;
        }
    }

    IEnumerator BrokeTheLight()
    {
        yield return new WaitForSeconds(.4f);

        NormalLight.SetActive(false);
    }
}
