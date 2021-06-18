using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 20f,
                 acc2D = 0;
    public Camera camFollower;

    private void Orient() {
        transform.transform.rotation = camFollower.transform.rotation;
    }

    private void Move() {
        float direction2D = 0;
        bool wDown = Input.GetKey(KeyCode.W),
             sDown = Input.GetKey(KeyCode.S);
        if (!(sDown && wDown)) {
            if (wDown || sDown) {
                acc2D = 1;
                direction2D = (wDown) ? 1f : -1f;
            }
            if (direction2D != 0) {
                if (acc2D > 1f)
                    acc2D = 1f;
                float f = (direction2D * acc2D * speed * Time.deltaTime);
                Vector3 movement = new Vector3(transform.forward.x * f, 0, transform.forward.z * f);
                transform.position = transform.position + movement;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {   
        Orient();
        Move();
    }
}
