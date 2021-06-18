using System.Collections;
using System;
using UnityEngine;

public class MouseRotatable : MonoBehaviour
{   
    private float horzRotationSpeed = 720f, vertRotationSpeed = 720f;

    private void LookUpAndDown() {
        float horzInput = Input.GetAxis("Mouse X");
        float vertInput = Input.GetAxis("Mouse Y");
         
        if (Math.Abs(horzInput) > 0.2 || Math.Abs(vertInput) > 0.2) {
            float x = (-vertInput * vertRotationSpeed * Time.deltaTime),
                  y = (horzInput * horzRotationSpeed * Time.deltaTime);
            transform.Rotate(x, y, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        LookUpAndDown();
    }
}
