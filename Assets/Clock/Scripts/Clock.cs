using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour
{
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //
    //  Simple Clock Script / Andre "AEG" B�rger / VIS-Games 2012
    //
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------

    //-- set start time 00:00
    public int minutes = 0;
    public int hour = 0;

    //-- time speed factor
    public float clockSpeed = 1.0f;     // 1.0f = realtime, < 1.0f = slower, > 1.0f = faster

    //-- internal vars
    int seconds;
    float msecs;
    GameObject pointerSeconds;
    GameObject pointerMinutes;
    GameObject pointerHours;
    bool move;
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        pointerSeconds = transform.Find("rotation_axis_pointer_seconds").gameObject;
        pointerMinutes = transform.Find("rotation_axis_pointer_minutes").gameObject;
        pointerHours = transform.Find("rotation_axis_pointer_hour").gameObject;

        hour = System.DateTime.Now.Hour;
        minutes = System.DateTime.Now.Minute;
        //msecs = System.DateTime.Now.Millisecond;
        seconds = System.DateTime.Now.Second;
        StartCoroutine(RandomMovement());
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        //-- calculate time
        msecs += Time.deltaTime * clockSpeed;
        if (msecs >= 1.0f)
        {
            msecs -= 1.0f;
            seconds++;
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
                if (minutes > 60)
                {
                    minutes = 0;
                    hour++;
                    if (hour >= 24)
                        hour = 0;
                }
            }
        }


        if (move)
        {
            //-- calculate pointer angles
            float rotationSeconds = (360.0f / 60.0f) * seconds;
            float rotationMinutes = (360.0f / 60.0f) * minutes;
            float rotationHours = ((360.0f / 12.0f) * hour) + ((360.0f / (60.0f * 12.0f)) * minutes);

            //-- draw pointers
            pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationSeconds);
            pointerMinutes.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationMinutes);
            pointerHours.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationHours);
        }
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------

    IEnumerator RandomMovement()
    {
        float time = 0f, x = 12200f, y = 8200f, z = 4200f;
        while (time < 2.0f)
        {
            x = Mathf.Lerp(x, 0, Time.deltaTime);
            y = Mathf.Lerp(y, 0, Time.deltaTime);
            z = Mathf.Lerp(z, 0, Time.deltaTime);
            pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, x);
            pointerMinutes.transform.localEulerAngles = new Vector3(0.0f, 0.0f, y);
            pointerHours.transform.localEulerAngles = new Vector3(0.0f, 0.0f, z);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0f;


        while (time < 3.0f)
        {
            //-- calculate pointer angles
            float rotationSeconds = (360.0f / 60.0f) * seconds;
            float rotationMinutes = (360.0f / 60.0f) * minutes;
            float rotationHours = ((360.0f / 12.0f) * hour) + ((360.0f / (60.0f * 12.0f)) * minutes);

            pointerSeconds.transform.rotation = Quaternion.Slerp(pointerSeconds.transform.rotation, Quaternion.Euler(0, 0, rotationSeconds), Time.deltaTime);
            pointerMinutes.transform.rotation = Quaternion.Slerp(pointerMinutes.transform.rotation, Quaternion.Euler(0, 0, rotationMinutes), Time.deltaTime);
            pointerHours.transform.rotation = Quaternion.Slerp(pointerHours.transform.rotation, Quaternion.Euler(0, 0, rotationHours), Time.deltaTime);
            // x = Mathf.Lerp(pointerSeconds.transform.localEulerAngles.z, rotationSeconds, 3 * Time.deltaTime);
            // y = Mathf.Lerp(pointerMinutes.transform.localEulerAngles.z, rotationMinutes, 3) * Time.deltaTime;
            // z = Mathf.Lerp(pointerHours.transform.localEulerAngles.z, rotationHours, 3 * Time.deltaTime);

            // pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, x);
            // pointerMinutes.transform.localEulerAngles = new Vector3(0.0f, 0.0f, y);
            // pointerHours.transform.localEulerAngles = new Vector3(0.0f, 0.0f, z);
            time += Time.deltaTime;
            yield return null;
        }
        move = true;
    }
}
