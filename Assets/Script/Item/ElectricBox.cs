using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBox : MonoBehaviour
{
    [SerializeField] ElectricBoxManager electricBoxManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject boxSwitchOpen;
    [SerializeField] GameObject boxOpenLight;
    [SerializeField] GameObject boxBoom;
    [SerializeField] GameObject[] boxPowerLight;
    [SerializeField] int maxPower = 9;
    [SerializeField] public int currentPower;
    [SerializeField] bool shotgunHit = false;

    bool shotgunAdded = false;
    bool finishCharge = false;
    // Start is called before the first frame update
    void Start()
    {
        boxSwitchOpen.SetActive(false);
        boxOpenLight.SetActive(false);
        boxBoom.SetActive(false);

        for(int i = 0; i < boxPowerLight.Length; i++)
        {
            boxPowerLight[i].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (electricBoxManager.openBox)
        {
            boxSwitchOpen.SetActive(true);
            boxOpenLight.SetActive(true);
        }
        else
        {
            boxSwitchOpen.SetActive(false);
            boxOpenLight.SetActive(false);
        }

        if (currentPower >= maxPower)
        {
            //good
        }

        if (shotgunHit)
        {

        }

        if (currentPower >= 3)
        {
            boxPowerLight[0].SetActive(true);
        }
        if (currentPower >= 6)
        {
            boxPowerLight[1].SetActive(true);
        }
        if (currentPower >= 9)
        {
            boxPowerLight[2].SetActive(true);

            if (!finishCharge)
            {
                levelManager.level1BoxCounter++;
                finishCharge = true;
            }
        }
        if(currentPower <= 0)
        {
            for (int i = 0; i < boxPowerLight.Length; i++)
            {
                boxPowerLight[i].SetActive(false);
            }
        }
    }

    public void AddPower(int amount)
    {
        currentPower = Mathf.Min(currentPower + amount, maxPower);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Normal Bullet"))
        {
            currentPower++;
        }

        if (collision.gameObject.CompareTag("Shotgun Bullet"))
        {
            if (electricBoxManager.openBox && !shotgunAdded && currentPower < 9)
            {
                AddPower(6);
                shotgunAdded = true;
                boxBoom.SetActive(true);
                StartCoroutine(StopBoom());
            }
        }
    }

    IEnumerator StopBoom()
    {
        yield return new WaitForSeconds(.2f);

        boxBoom.SetActive(false);
        shotgunAdded = false;
    }
}
