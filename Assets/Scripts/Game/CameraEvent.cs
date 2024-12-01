using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEvent : MonoBehaviour
{

    [Header("Main Settings")]
    public GameController gameController;

    void Start()
    {
        
    }

    //метод перезагрузки поля
    public void RestartFieldEvent()
    {
        gameController.RestartField();
    }
}
