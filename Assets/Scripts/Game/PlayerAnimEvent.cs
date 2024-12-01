using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    public PlayerController controller;

    public void KickedEvent()
    {
        controller.Kicked();
    }
}
