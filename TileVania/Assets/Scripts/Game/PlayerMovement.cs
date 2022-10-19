using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] float deathImpulse = 1f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] float timeBetweenAttacks = 2f;



    private Vector2 moveInput;
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;
    private CapsuleCollider2D myBodyCollider;
    private BoxCollider2D myFeetCollider;
    private CinemachineImpulseSource myImpulseSource;
    private float gravityScaleAtStart;
    private bool isAlive = true;
    private float timeSinceLastAttack = Mathf.Infinity;

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myImpulseSource = GetComponent<CinemachineImpulseSource>();
        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    void Update()
    {
        if (!isAlive) return;
        timeSinceLastAttack += Time.deltaTime;
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        myAnimator.SetBool("isRunning", PlayerHasHorizontalSpeed());
    }

    void OnMove(InputValue value) 
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return; 

        if (value.isPressed)
        {
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) return;
        if (timeSinceLastAttack > timeBetweenAttacks)
        {
            ShootArrow();
            timeSinceLastAttack = 0f;
        }
    }

    private void ShootArrow()
    {
        myAnimator.SetBool("isShooting", true);
        Invoke(nameof(InstantiateArrow), timeBetweenAttacks);
    }

    private void InstantiateArrow()
    {
        if (transform.localScale.x > Mathf.Epsilon)
        {
            Instantiate(bullet, bulletSpawn.position, transform.rotation);
        }
        else
        {
            Instantiate(bullet, bulletSpawn.position, transform.rotation * Quaternion.Euler(0f,180f,0f));
        }
        myAnimator.SetBool("isShooting", false);
    }

    private void FlipSprite()
    {
        if (PlayerHasHorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private bool PlayerHasHorizontalSpeed()
    {
        return Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
    }

    private bool PlayerHasVerticalSpeed()
    {
        return Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
    }

    private void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidBody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myAnimator.SetBool("isClimbing", PlayerHasVerticalSpeed());
        myRigidBody.gravityScale = 0f;
    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidBody.velocity = deathKick;
            myImpulseSource.GenerateImpulse(deathImpulse);
            Invoke(nameof(RemovePhysics), 1f);
            GameSession gameSession = FindObjectOfType<GameSession>();
            gameSession.ProcessPlayerDeath();
        }
    }

    private void RemovePhysics()
    {
        myRigidBody.simulated = false;
    }
}
