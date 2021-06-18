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

    private int GenerateMoveMask() {
        int maskDown = 0x0;
        if (Input.GetKey(KeyCode.W))
            maskDown |= 0x01;
        if (Input.GetKey(KeyCode.S))
            maskDown |= 0x02;
        if (Input.GetKey(KeyCode.A))
            maskDown |= 0x04;
        if (Input.GetKey(KeyCode.D))
            maskDown |= 0x08;
        if (Input.GetKey(KeyCode.Space))
            maskDown |= 0x10;
        if (Input.GetKey(KeyCode.LeftShift))
            maskDown |= 0x20;
        return maskDown;
    }

    private void Move() {
        float direction2D = 0;
        int maskDown = GenerateMoveMask();
        if (maskDown != 0x0) {
            Vector3 xzFacing = new Vector3(transform.forward.x, 0, transform.forward.z);
            Vector3 movement = new Vector3();
            if ((maskDown & 0x03) != 0x03) {
                if ((maskDown & 0x01) == 0x01 || (maskDown & 0x02) == 0x02) {
                    direction2D = ((maskDown & 0x01) == 0x01) ? 1f : -1f;
                    float f = (direction2D * speed * Time.deltaTime);
                    movement.x += xzFacing.x * f;
                    movement.z += xzFacing.z * f;
                }
            }
            if ((maskDown & 0xC) != 0xC) {
                // Normalized, multiply by rotational matrix (AC 90)
                Vector3 perpAxis = new Vector3(-xzFacing.z, 0, xzFacing.x);
                if ((maskDown & 0x04) == 0x04 || (maskDown & 0x08) == 0x08) {
                    direction2D = ((maskDown & 0x04) == 0x04) ? 1f : -1f;
                    float f = (direction2D * speed * Time.deltaTime);
                    movement.x += perpAxis.x * f;
                    movement.z += perpAxis.z * f;
                }
            }
            if ((maskDown & 0x30) != 0x30) {
                // Normalized, multiply by rotational matrix (AC 90)
                Vector3 perpAxis = new Vector3(-xzFacing.z, 0, xzFacing.x);
                if ((maskDown & 0x10) == 0x10 || (maskDown & 0x20) == 0x20) {
                    direction2D = ((maskDown & 0x10) == 0x10) ? 1f : -1f;
                    float f = (direction2D * speed * Time.deltaTime);
                    movement.y += f;
                }
            }
            transform.position = transform.position + movement;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        Orient();
        Move();
    }
}
