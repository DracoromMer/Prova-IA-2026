using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ShamanScript : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    [SerializeField] float crouchSpeed = 1f;

    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] float sliderMaxTimer = 1f;
    [SerializeField] float slideSpeed = 5f;

    [SerializeField] float attackMaxTimer = 1f;

    [SerializeField] float attackCooldownTimer = 5f;

    [SerializeField] float specialMaxTimer = 1f;

    [SerializeField] float deathDuration = 1f;

    [SerializeField] float damageDuration = 1f;
    private Vector2 input;

    private Vector2 lastMoveDirection;

    private bool isSliding;

    private float deathTimer;

    private float damageTimer;

    private float attackCooldown;
    private float dashX;
    private float dashY;
   

    private float slidertime;

    private bool isCrouching = false;

    private bool isRolling;

    private bool isDamage;

    private bool isDying;

    private float attackTime;

    private float specialTime;

    private bool isAttacking;

    private bool isSpecial;

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
        if ((isSliding == false) && (isAttacking == false) && (isCrouching == false))
        {
        rb.linearVelocity = input * speed;
        }
        else if ((isSliding == false) && (isAttacking == false) && (isCrouching == true))
        {
        rb.linearVelocity = input * crouchSpeed;
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
        //Special
        if (Input.GetKeyDown(KeyCode.F) && !isSpecial)
        {
            Special();
        }

        if (isSpecial)
        {
            specialTime += Time.fixedDeltaTime;

            if (specialTime > specialMaxTimer)
            {
                isSpecial = false;
                specialTime = 0f;
            }
        }

        //Crouch
        if ((Input.GetKeyDown(KeyCode.C)) && (input.magnitude == 0) && (isCrouching == false))
        {
            isCrouching = true;
        }
        else if ((Input.GetKeyDown(KeyCode.C)) && (input.magnitude == 0) && (isCrouching == true))
        {
            isCrouching = false;
        }

        //Dano
        if (Input.GetKeyDown(KeyCode.Space) && !isDamage)
        {
            Damage();
        }

        if (isDamage)
        {
            damageTimer += Time.fixedDeltaTime;

            if (damageTimer > damageDuration)
            {
                isDamage = false;
                damageTimer = 0f;
            }
        }

        //Morte
        if (Input.GetKeyDown(KeyCode.R) && !isDying)
        {
            Death();
        }

        if (isDying)
        {
            deathTimer += Time.fixedDeltaTime;

            if (deathTimer > deathDuration)
            {
                deathTimer = 0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
       
    }

    void Special()
    {
        isSpecial = true;
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

    void Death()
    {
        isDying = true;
    }

    void Damage()
    {
        isDamage = true;
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
        animator.SetBool("IsCrouching", isCrouching);
        animator.SetBool("IsSpecial", isSpecial);
        animator.SetBool("IsDamage", isDamage);
        animator.SetBool("IsDead", isDying);
    }
}
