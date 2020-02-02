using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTurn : MonoBehaviour
{
    [SerializeField]
    private float rotationRate;
    private float currRotationRate = 0;
    [SerializeField]

    private float rotationLerpSpeed;
    [SerializeField]
    private bool activated = false;
    [SerializeField]
    private Material goldGearMat;


    // Update is called once per frame
    void Update()
    {
        if (activated)
            currRotationRate = Mathf.Clamp(currRotationRate + rotationLerpSpeed * Time.deltaTime, 0, rotationRate);
        transform.Rotate(Vector3.forward * currRotationRate);
    }

    public void StartTurning()
    {
        activated = true;
        GetComponent<Renderer>().sharedMaterial = goldGearMat;
    }
}
