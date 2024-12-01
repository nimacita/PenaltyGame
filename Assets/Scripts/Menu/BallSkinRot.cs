using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSkinRot : MonoBehaviour
{

    public float rotSpeed;

    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        BallRotate();
    }

    private void BallRotate()
    {
        transform.Rotate(new Vector3(0f, 0.5f, 1f) * rotSpeed);
    }
}
