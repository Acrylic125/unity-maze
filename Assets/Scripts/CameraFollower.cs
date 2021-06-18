using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{

    public GameObject follow;

    // Update is called once per frame
    void Update()
    {
        transform.position = follow.transform.position;
    }
}
