using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class collectable : MonoBehaviour
{
    [SerializeField] private float collect_radius;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerMovement pm;
    [SerializeField] private Animator animator;
    private manager mng;

    private void Start()
    {
        mng = GameObject.Find("manager").GetComponent<manager>();
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= collect_radius && pm.purifying)
        {
            animator.SetTrigger("collect");
        }
    }

    public void Collected()
    {
        mng.OnCollect();
    }
}
