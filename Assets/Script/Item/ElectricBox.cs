using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBox : MonoBehaviour
{
    [SerializeField] ElectricBoxManager electricBoxManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] Level2SceondBoxSystem levelSecond;
    [SerializeField] GameObject boxSwitchOpen;
    [SerializeField] GameObject boxOpenLight;
    [SerializeField] GameObject boxBoom;
    [SerializeField] GameObject[] boxPowerLight;
    [SerializeField] int maxPower = 9;
    [SerializeField] public int currentPower;
    [SerializeField] bool shotgunHit = false;

    bool shotgunAdded = false;
    bool finishCharge = false;

    void Start()
    {
        boxSwitchOpen.SetActive(false);
        boxOpenLight.SetActive(false);
        boxBoom.SetActive(false);

        for (int i = 0; i < boxPowerLight.Length; i++)
        {
            boxPowerLight[i].SetActive(false);
        }
    }

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

        // ✅ 判斷電量達標並只執行一次
        if (currentPower >= maxPower && !finishCharge)
        {
            finishCharge = true;
            boxPowerLight[2].SetActive(true);
            levelManager.level1BoxCounter++;
            levelSecond.boxCounter++;
            electricBoxManager.openBox = false;
        }

        if (currentPower >= 3)
        {
            boxPowerLight[0].SetActive(true);
        }
        if (currentPower >= 6)
        {
            boxPowerLight[1].SetActive(true);
        }
        if (currentPower <= 0)
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
        if (electricBoxManager.openBox && collision.gameObject.CompareTag("Normal Bullet"))
        {
            currentPower++;
        }

        if (collision.gameObject.CompareTag("Shotgun Bullet"))
        {
            if (electricBoxManager.openBox && !shotgunAdded && currentPower < maxPower)
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
        yield return new WaitForSeconds(0.2f);
        boxBoom.SetActive(false);
        shotgunAdded = false;
    }
}
