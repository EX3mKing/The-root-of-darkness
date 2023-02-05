using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private float damageAmount;
    [SerializeField] private GameObject playerObject;
    private HealthToOpRatio _healthToOpRatio;
    private void Start()
    {
        _healthToOpRatio = playerObject.GetComponent<HealthToOpRatio>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _healthToOpRatio.DMGCall(damageAmount);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.parent.parent.CompareTag("Player"))
        {
            _healthToOpRatio.DMGCall(damageAmount);
        }
    }
}
