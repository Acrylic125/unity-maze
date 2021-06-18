using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 20f,
                 acc2D = 0,
                 rotateSpeed = 30f;

    private void Move() {
        float direction2D = 0;
        bool wDown = Input.GetKey(KeyCode.W),
             sDown = Input.GetKey(KeyCode.S);
        if (!(sDown && wDown)) {
            if (wDown || sDown) {
                acc2D += (0.45f * Time.deltaTime);
                acc2D += 0.45f;
            }
            if (acc2D > 1f)
                acc2D = 1f;
            if (acc2D > 0)
                transform.Translate(transform.forward * (direction2D * acc2D * speed * Time.deltaTime));
        }
    }

    // Update is called once per frame
    void Update()
    {   
        Move();
    }
}
