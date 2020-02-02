using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private float decelerationMultiplier = 1;

    public delegate void teleportEvent(Vector3 pos);
    public event teleportEvent tp;
    private float lastStoppedMotionTime;
    private Vector3 lastPos;
    [SerializeField]
    private float rotationMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        lastStoppedMotionTime = Time.time;
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log((lastPos - transform.position).magnitude);


        // if ((lastPos - transform.position).magnitude == 0)
        // {
        //     rb.velocity = Vector3.zero;
        //     tp(transform.position);
        // }

        //if still moving

        if (rb.velocity.magnitude > 0.05f)
        {
            //Reset timer since still moving
            lastStoppedMotionTime = 0.5f;
        }
        else if ((rb.velocity.magnitude > 0 && rb.velocity.magnitude < 0.05) || lastStoppedMotionTime > 0f)
        {
            //if timer runs out tp player and allow for next throw
            if (lastStoppedMotionTime <= 0f)
            {
                rb.velocity = Vector3.zero;
                tp(transform.position);
                Debug.Log("tp");
            }
        }
        else
        {
            // rb.velocity = Vector3.zero;
            // tp(transform.position);
            lastStoppedMotionTime = 0f;
        }
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * decelerationMultiplier);
        Vector3 rbVelocity = rb.velocity;
        Vector3 correctedAxes = new Vector3(rbVelocity.z, 0, -rbVelocity.x) * rotationMultiplier;
        transform.Rotate(correctedAxes, Space.World);
        lastStoppedMotionTime -= 1.0f * Time.deltaTime;
    }

    public void StopAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
}
