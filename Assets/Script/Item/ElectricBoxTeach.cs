using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBoxTeach : MonoBehaviour
{
    [SerializeField] ElectricBoxManager electricBoxManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject level1EnterDoor;
    [SerializeField] GameObject boxSwitchOpen;
    [SerializeField] GameObject boxOpenLight;
    [SerializeField] GameObject boxBoom;
    [SerializeField] GameObject[] boxPowerLight;
    [SerializeField] GameObject teachText;
    [SerializeField] int maxPower = 9;
    [SerializeField] public int currentPower = 0;
    [SerializeField] bool shotgunHit = false;

    bool shotgunAdded = false;
    bool finishCharge = false;
    bool openText = false;
    // Start is called before the first frame update
    void Start()
    {
        boxSwitchOpen.SetActive(false);
        boxOpenLight.SetActive(false);
        boxBoom.SetActive(false);
        level1EnterDoor.SetActive(true);
        teachText.SetActive(false);

        for (int i = 0; i < boxPowerLight.Length; i++)
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

        if(Input.GetKeyDown(KeyCode.E))
        {
            openText = !openText;
        }
        if (openText)
        {
            teachText.SetActive(true);
        }
        else
        {
            teachText.SetActive(false);
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
                //levelManager.level1BoxCounter++;
                finishCharge = true;
                level1EnterDoor.SetActive(false);
            }
        }
        if(currentPower <= 0)
        {
            for (int i = 0; i < boxPowerLight.Length; i++)
            {
                boxPowerLight[i].SetActive(false);
            }
        }
        if(currentPower >= maxPower)
        {
            teachText.SetActive(false);
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
