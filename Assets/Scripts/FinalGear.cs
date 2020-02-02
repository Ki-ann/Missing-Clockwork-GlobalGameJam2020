using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FinalGear : MonoBehaviour
{
    [SerializeField]
    private GearTurn[] gears;
    [SerializeField]
    private Transform gearParent;
    [SerializeField]
    private PlayableDirector timeline;
    // Start is called before the first frame update
    void Start()
    {
        gears = gearParent.GetComponentsInChildren<GearTurn>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            MainMenu menu = FindObjectOfType<MainMenu>();
            menu.TotalTime = Time.time - menu.TotalTime;
            menu.timetext.text =menu.TotalTime.ToString("F3") + "s";
            menu.jumptext.text = menu.TotalJumps.ToString();

            other.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            timeline.Play();
        }
    }

    private void EndLevel()
    {
       
        
        //timeline.Play();
        foreach (GearTurn g in gears)
        {
            g.StartTurning();
        }
    }
}
