using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroWait : MonoBehaviour
{
    [SerializeField] private PlayerMovement pm;
    [SerializeField] private Animator pa;
    [SerializeField] private float time;
    private float time_cur;

    private void Awake()
    {
        pa.Play("wake_up");
        pa.SetBool("can_move", false);
    }

    private void Update()
    {
        time_cur += Time.deltaTime;
        if (time_cur >= time)
        {
            pm.enabled = true;
            pa.SetBool("can_move", true);
            Destroy(gameObject);
        }
    }
}