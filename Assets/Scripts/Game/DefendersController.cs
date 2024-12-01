using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendersController : MonoBehaviour
{

    [Header("Main Components")]
    public GameObject[] defenders;
    public Transform downZ, upZ;

    void Start()
    {
        
    }

    public void SetDefenders(int counts)
    {
        DisableAllDefenders();

        if (counts < defenders.Length + 1) 
        {
            //если количество нужных защитников меньше чем общее их количетво, то случайно включаем
            for (int i = 0;i<counts;i++)
            {
                EnableRandomDefenders();
            }
        }
        else
        {
            //иначе включаем всех
            EnableAllDefenders();
        }
    }

    //включаем выбранного защитника
    private void ActivateDefender(int ind)
    {
        //случайная позиция по Z
        float randZ = Random.Range(downZ.position.z, upZ.transform.position.z);

        Vector3 newPos = defenders[ind].transform.position;
        newPos.z = randZ;
        defenders[ind].transform.position = newPos;

        defenders[ind].SetActive(true);
    }

    private void EnableRandomDefenders()
    {
        int defInd = Random.Range(0, defenders.Length);
        if (!defenders[defInd].activeSelf)
        {
            ActivateDefender(defInd);
            return;
        }
        else
        {
            //пробуем нового
            EnableRandomDefenders();
        }
    }

    //выключаем всех защитников
    private void DisableAllDefenders() 
    {
        foreach (GameObject def in defenders)
        {
            def.SetActive(false);
        }
    }

    //включаем всех защитников
    private void EnableAllDefenders()
    {
        for (int i = 0; i < defenders.Length; i++) 
        {
            ActivateDefender(i);
        }
    }

    public int DefendersLength()
    {
        return defenders.Length;
    }
}
