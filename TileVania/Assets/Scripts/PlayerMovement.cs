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
    private Vector2 moveInput;
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Run();
        FlipSprite();
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        myAnimator.SetBool("isRunning", PlayerHasHorizontalSpeed());
    }

    void OnMove(InputValue value) 
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
        }
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
}
