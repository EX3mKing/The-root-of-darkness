using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColapsePlatform : MonoBehaviour
{
    private SpriteRenderer spriteR;
    private BoxCollider2D boxCol;
    private Animator anim;
    [SerializeField] private float time;
    private float time_cur;

    private void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Physics2D.OverlapBox(transform.position, boxCol.size, 0f))
        {
            Debug.Log("bruh");
            anim.SetBool("shake",true);
            time_cur = time;

        }
        else
        {
            if (time_cur > 0)
            {
                time_cur -= Time.deltaTime;
            }
            else
            {
                Hide(false);
            }
        }

    }

    public void Hide(bool hid)
    {
        spriteR.enabled = !hid;
        boxCol.enabled = !hid;
    }
}
