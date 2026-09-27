using System;
using Unity.VisualScripting;
using UnityEngine;

public class ShamanScript : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] float sliderMaxTimer = 1f;
    [SerializeField] float slideSpeed = 5f;

    [SerializeField] float attackMaxTimer = 1f;

    [SerializeField] float attackCooldownTimer = 5f;
    private Vector2 input;

    private Vector2 lastMoveDirection;

    private bool isSliding;

    private float attackCooldown;
    private float dashX;
    private float dashY;
   

    private float slidertime;

    private float attackTime;

    private bool isAttacking;

    private bool hasAttackedOnce;

    private bool hasAttackedTwice;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Comandos();
        Animar();
        
    }

    private void FixedUpdate()
    {
        // Velocidade
        if ((isSliding == false) && (isAttacking == false))
        {
        rb.linearVelocity = input * speed;
        }
    }

    void Comandos()
    {
        // Armazenar direção
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if((moveX == 0 && moveY == 0) && (input.x != 0 || input.y != 0))
        {
            lastMoveDirection = input;
        }
        // Mover
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        //Slide
        if ((Input.GetKey(KeyCode.E)) && (input.magnitude !=0))
        {
           Slide();
        }
        if (isSliding == true)
        {
        slidertime += Time.fixedDeltaTime;

             if (slidertime > sliderMaxTimer)
        {
            isSliding = false;
            slidertime = 0f;
        }
        }

        //Attack
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
                Attack();
        }

        if (isAttacking)
        {
            attackTime += Time.deltaTime;
            if (attackTime > attackMaxTimer)
            {
                isAttacking = false;
                attackTime = 0f;
                if (!hasAttackedOnce && !hasAttackedTwice)
                {
                    hasAttackedOnce = true;
                }
                else if (hasAttackedOnce)
                {
                    hasAttackedOnce = false;
                    hasAttackedTwice = true;
                }
                else if (hasAttackedTwice)
                {
                    ResetCombo();
                }
            }
        }


        if (hasAttackedOnce || hasAttackedTwice)
        {
            attackCooldown += Time.deltaTime;
            if (attackCooldown > attackCooldownTimer)
            {
                ResetCombo();
            }
        }
       
    }

    void Slide()
    {
        isSliding = true;
        Vector2 dashDirection = input;
        dashX = dashDirection.x;
        dashY = dashDirection.y;
        rb.linearVelocity = slideSpeed * dashDirection;
        
    }

    void ResetCombo()
    {
        hasAttackedOnce = false;
        hasAttackedTwice = false;
        attackCooldown = 0f;
        
    }

    void Attack()
    {
        isAttacking = true;
    }


    void Animar()
    {
        animator.SetFloat("MoveX", input.x);
        animator.SetFloat("MoveY", input.y);
        animator.SetFloat("MovePotency", input.magnitude);
        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
        animator.SetBool("IsSliding", isSliding);
        animator.SetFloat("DashX", dashX);
        animator.SetFloat("DashY", dashY);
        animator.SetBool("IsAttacking", isAttacking);
        animator.SetBool("Attack3", hasAttackedTwice);
        animator.SetBool("Attack2", hasAttackedOnce);
    }
}
