using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [Header("Input System")]
    [SerializeField]
    private float maxMagitudeStrength;
    [SerializeField]
    private float powerLevel;
    private Vector3 mouseFirstPos = Vector3.zero;
    private Vector3 mousePosition = Vector3.zero;
    private Vector3 dir = Vector3.zero;
    private bool mouseFlag = false;
    private bool thrown = false;

    [SerializeField]
    private float decelerationMultiplier = 1;

    private float lastStoppedMotionTime;
    private Vector3 lastPos;
    [SerializeField]
    private float rotationMultiplier = 1f;
    float bouncefirst = 0;

    [Header("References")]
    [SerializeField]
    private LineRenderer line;

    [SerializeField]
    private CameraFollow follow;
    [SerializeField]
    private GameObject glowPS;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private VolumeProfile postProfile;
    [SerializeField]
    private MainMenu menu;
    [SerializeField]
    private GameObject bounce;

    private bool Cheat = false;

    // Start is called before the first frame update
    void Start()
    {
        lastStoppedMotionTime = Time.time;
        GetComponent<Animator>().enabled = true;
        glowPS.SetActive(true);
        menu.TotalTime = Time.time;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            Cheat = !Cheat;
        }
        if (Cheat)
        {
            var pos = transform.position;
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, pos.z);

        }
        //if still moving

        if (rb.velocity.magnitude > 0.05f)
        {
            //Reset timer since still moving
            lastStoppedMotionTime = 0.5f;
            glowPS.SetActive(false);
        }
        else if ((rb.velocity.magnitude > 0 && rb.velocity.magnitude < 0.05) || lastStoppedMotionTime > 0f)
        {
            //if timer runs out tp player and allow for next throw
            if (lastStoppedMotionTime <= 0f)
            {
                rb.velocity = Vector3.zero;
                thrown = false;
                glowPS.SetActive(true);

                Debug.Log("throw ready");
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

        follow.LerpCamera(transform.position);

        //Input System
        if (thrown)
        {
            glowPS.SetActive(false);
            if ((rb.velocity.magnitude >= 0 && rb.velocity.magnitude < 0.05) || lastStoppedMotionTime > 0f)
            {
                //if timer runs out tp player and allow for next throw
                if (lastStoppedMotionTime <= 0f)
                {
                    rb.velocity = Vector3.zero;
                    thrown = false;
                    glowPS.SetActive(true);
                    Debug.Log("throw ready");
                }
            }
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            mouseFlag = true;
            line.enabled = true;
            mouseFirstPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseFirstPos.z = 1.0f;
            line.SetPosition(0, transform.position);
            rb.velocity = Vector3.zero;
        }
        if (Input.GetMouseButtonUp(0) && mouseFlag)
        {
            mouseFlag = false;
            mouseFirstPos = Vector3.zero;
            line.enabled = false;
            rb.AddForceAtPosition(dir * powerLevel * 200f, transform.position);
            thrown = true;
            mouseFirstPos = Vector3.zero;
            glowPS.SetActive(false);
            menu.TotalJumps++;
            StartCoroutine(FlyEffect());
        }

        if (mouseFlag)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 1.0f;
            dir = Vector3.Normalize(mouseFirstPos - mousePosition);
            dir.z = 1.0f;
            FindBallPower((mouseFirstPos - mousePosition).magnitude);
            line.SetPosition(1, transform.position + dir * powerLevel);
        }
    }
    IEnumerator FlyEffect()
    {
        float time = 0f;
        float aberationValue = 0.8f;
        while (time < 2f)
        {
            SetAbbaration(Mathf.Lerp(0.05f, aberationValue, time));
            time += Time.deltaTime;
            yield return null;
        }

        SetAbbaration(0.5f);
        time = 0f;
        aberationValue = 0.05f;
        while (time < 2f)
        {
            SetAbbaration(Mathf.Lerp(0.8f, aberationValue, time));
            time += Time.deltaTime;
            yield return null;
        }
        SetAbbaration(0.05f);

    }

    public void SetAbbaration(float x)
    {
        //ChromaticAberration aber;
        //if (postProfile.TryGet<ChromaticAberration>(out aber))
        //{
        //    aber.intensity.value = x;
        //}
    }

    private void Awake()
    {
        SetAbbaration(0.05f);
    }
    public void StopAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
    private void FindBallPower(float magnitude)
    {
        powerLevel = Mathf.Clamp(Mathf.Clamp(magnitude, 0, maxMagitudeStrength) * Mathf.Log10(6), 0, 100);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bounce");
        if (bouncefirst > 2)
        {
            var go = Instantiate(bounce, transform.position, Quaternion.identity);
            Destroy(go, 2f);
        }
        else
        {
            bouncefirst++;
        }
    }
}
