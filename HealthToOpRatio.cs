using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthToOpRatio : MonoBehaviour
{
    public SpriteRenderer distort;
    private float distortAmount;
    [SerializeField] private float health = 100;
    private DamageDealer damage;
    private float instaDamageTimer;
    public float instaDamageTimerCap;
    private float healTimer;
    [SerializeField] private float healTimerCap;
    [SerializeField] private float heal_speed;
    [SerializeField] private GameObject damage_marker;
    [SerializeField] private SpriteRenderer damage_marker_sr;
    [SerializeField] private float time_before_healing_lenght;
    [SerializeField] private float invincible_lenght;
    private float invincible_cur;
    private float time_before_healing_cur;
    void Update()
    {
       
        distortAmount = 2.5f / health;
        
        if (health > 99f)
        {
            distort.enabled = false;
        }
        else
        {
            distort.enabled = true;
        }
        
        distort.color = new Color(0.8f,0f,0.3f,distortAmount);
        damage_marker_sr.color = new Color(damage_marker_sr.color.r, damage_marker_sr.color.g, damage_marker_sr.color.b, 100/(100 - health));

        if (time_before_healing_cur < time_before_healing_lenght)
            time_before_healing_cur += Time.deltaTime;

        if (invincible_cur > 0)
            invincible_cur -= Time.deltaTime;
        
        
        if (health < 100)
        {
            if (time_before_healing_cur >= time_before_healing_lenght)
            {
                health += Time.deltaTime * heal_speed;
            }
            
            damage_marker.SetActive(true);
        }
        if(health >= 100)
        {
            damage_marker.SetActive(false);
            health = 100;
        }
    }

    public void DMGCall(float dmg)
    {
        if (invincible_cur <= 0)
        {
            if(health < dmg) Die(); 
            invincible_cur = invincible_lenght;
            time_before_healing_cur = time_before_healing_lenght;
            health -= dmg;
        }
    }

    private void Die()
    {
        Debug.Log("diesd");
        SceneManager.LoadScene("Death");
    }
}
