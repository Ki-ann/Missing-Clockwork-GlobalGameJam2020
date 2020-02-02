using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float lerpValue = 3f;

    public void LerpCamera(Vector3 pos)
    {
        pos.z = transform.position.z;
        transform.position = pos;
        transform.position = Vector3.Slerp(transform.position, pos, Time.deltaTime * lerpValue);
    }
}