using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed_base = 8f;
    [SerializeField] private float speed_min = 3f;
    [SerializeField] private float speed_wall_falling = 2f;
    [SerializeField] private float speed_acceleration = 17f;
    [SerializeField] private float speed_deacceleration = 8f;
    [SerializeField] private float jump_strenght = 17.9f;
    [SerializeField] private float jump_buffer_lenght = 0.1f;
    [SerializeField] private float coyote_time_lenght = 0.1f;
    [SerializeField] private float jump_IE_wait = 0.2f;
    [SerializeField] private float jump_wall_strenght = 13;
    [SerializeField] private float jump_wall_velocity_lock_time = 0.2f;
    [SerializeField] private float jump_initial_time_lenght = 0.08f;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask layer_moving_platform;
    [SerializeField] private SpriteRenderer sprite_renderer;
    [SerializeField] private Animator animator_main;

    public Collider2D coll_celing;
    public Collider2D coll_wall_left;
    public Collider2D coll_wall_right;
    public Collider2D coll_floor;
    public Collider2D coll_base;
    public Collider2D coll_base_under;

    private bool hitting_celing;
    private bool hitting_wall_left;
    private bool hitting_wall_right;
    private bool hitting_floor;
    private bool hitting_base;
    private bool hitting_base_under;
    
    private float dirX;
    private float dirY;
    private float dirX_cur;

    private float last_y = 0f;

    [SerializeField] private float asleep_time_needed = 11f;
    private float asleep_time_cur;
    private bool asleep_bool;
    private bool asleep_countdown;

    [SerializeField] private float purify_time_lenght;
    [SerializeField] private float purify_time_cur;
    public bool purifying;

    private float coyote_time_cur;
    private float jump_buffer_cur;
    private bool jumping;
    private bool jump_initial;
    private float jump_initial_time_cur;
    private bool velocity_locked;
    private float jump_wall_velocity_lock_time_cur = 0;

    private Rigidbody2D player_rb;

    private void Awake()
    {
        player_rb = gameObject.GetComponent<Rigidbody2D>();
        asleep_time_cur = asleep_time_needed;
    }

    private void Update()
    {
        // zemi input
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");

        if (jump_wall_velocity_lock_time_cur > 0)
        {
            jump_wall_velocity_lock_time_cur -= Time.deltaTime;
        }

        if (jump_initial_time_cur > 0)
        {
            jump_initial_time_cur -= Time.deltaTime;
        }
        
        if (jump_wall_velocity_lock_time > 0) velocity_locked = true;
        
        if (purifying)
        {
            purify_time_cur -= Time.deltaTime;
            
            if (purify_time_cur <= 0)
            {
                purify_time_cur = 0;
                purifying = false;
            }
        }
        animator_main.SetBool("purify", purifying);
        animator_main.SetBool("can_move", !purifying);

        if(asleep_countdown && asleep_time_cur > 0)
        {
            asleep_time_cur -= Time.deltaTime;
        }

        jump_initial = jump_initial_time_cur > 0;
        
        animator_main.SetBool("temp_velocity_lock", jump_wall_velocity_lock_time_cur > 0);
        
        if (jump_wall_velocity_lock_time_cur > 0 || purifying) return;
        
        //collider checks
        hitting_celing = ColliderCheck(coll_celing, layer);
        hitting_wall_left = ColliderCheck(coll_wall_left, layer);
        hitting_wall_right = ColliderCheck(coll_wall_right, layer);
        hitting_floor = ColliderCheck(coll_floor, layer);
        hitting_base = ColliderCheck(coll_base, layer);
        hitting_base_under = ColliderCheck(coll_base_under, layer);
        bool jump_button_pressed = Input.GetButton("Jump");
        
        //animator
        animator_main.SetBool("grounded", hitting_base && hitting_floor);
        animator_main.SetBool("Moving", dirX != 0);
        animator_main.SetBool("initial_jump", jump_initial);
        animator_main.SetBool("on_wall", (hitting_wall_right || hitting_wall_left) && hitting_base);
        animator_main.SetBool("asleep", asleep_time_cur <= 0);
        animator_main.SetBool("base_under", hitting_base_under);

        if (dirX == 0 && hitting_base && hitting_floor)
            asleep_countdown = true;
        else
        {
            asleep_time_cur = asleep_time_needed;
            asleep_countdown = false;
        }

        
        
        // sprite flip
        if (Mathf.Abs(dirX) > 0)
        {
            if (dirX > 0) sprite_renderer.flipX = false;
            else sprite_renderer.flipX = true;
        }

        // gleda ako si na podu
        if (hitting_floor && hitting_base)
        {
            coyote_time_cur = coyote_time_lenght;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                purifying = true;
                purify_time_cur = purify_time_lenght;
                dirX = 0f;
            }

            // ubrzanje / usporenje
            if (dirX != 0)
            {
                if(dirX_cur < speed_base)
                    dirX_cur += Time.deltaTime * speed_acceleration;
                if (dirX_cur > speed_base) dirX_cur = speed_base;
            }
            else
            {
                if(dirX_cur > speed_min)
                    dirX_cur -= Time.deltaTime * speed_deacceleration;
                if (dirX_cur < speed_min) dirX_cur = speed_min;
            }
        }
        else
        {
            coyote_time_cur -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jump_buffer_cur = jump_buffer_lenght;
        }
        else
        {
            jump_buffer_cur -= Time.deltaTime;
        }

        if (coyote_time_cur > 0f && jump_buffer_cur > 0f && !jumping)
        {
            
            player_rb.velocity = new Vector2(player_rb.velocity.x, jump_strenght);
            jump_buffer_cur = 0f;
            jump_initial_time_cur = jump_initial_time_lenght;
            StartCoroutine(JumpCooldown());
        }

        if (Input.GetButtonUp("Jump") && player_rb.velocity.y > 0f)
        {
            player_rb.velocity = new Vector2(player_rb.velocity.x, player_rb.velocity.y * 0.5f);
            coyote_time_cur = 0f;
        }

        // x cord movement
        if(!ColliderCheck(coll_floor, layer_moving_platform))
            player_rb.velocity = new Vector2(dirX_cur * dirX, player_rb.velocity.y);
        
        // wall movement
        if (!hitting_floor && hitting_base && (hitting_wall_left || hitting_wall_right))
        {
            if (dirX > 0 && hitting_wall_right)
            {
                player_rb.velocity = new Vector2(0f, -speed_wall_falling);
                if (Input.GetButton("Jump"))
                {
                    player_rb.AddForce(new Vector2(-jump_wall_strenght, jump_wall_strenght* 2.3f), ForceMode2D.Impulse);
                    jump_wall_velocity_lock_time_cur = jump_wall_velocity_lock_time;
                    sprite_renderer.flipX = true;
                }
            }
            if (dirX < 0 && hitting_wall_left)
            {
                player_rb.velocity = new Vector2(0f, -speed_wall_falling);
                if (Input.GetButton("Jump"))
                {
                    player_rb.AddForce(new Vector2(jump_wall_strenght, jump_wall_strenght* 2.3f), ForceMode2D.Impulse);
                    jump_wall_velocity_lock_time_cur = jump_wall_velocity_lock_time;
                    sprite_renderer.flipX = false;
                }
            }
        }
    }
    
    private bool ColliderCheck(Collider2D col, LayerMask lm)
    {
        return Physics2D.OverlapBox(col.bounds.center, col.bounds.size, 0, lm);
    }

    private IEnumerator JumpCooldown()
    {
        jumping = true;
        yield return new WaitForSeconds(jump_IE_wait);
        jumping = false;
    }

    private void FixedUpdate()
    {
        animator_main.SetBool("falling",  last_y > transform.position.y);
        last_y = transform.position.y;
    }
}
