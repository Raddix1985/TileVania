using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator anim;
    CapsuleCollider2D capsuleCollider2D;

    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float jumpForce = 10.0f;
    [SerializeField] float climbSpeed = 5.0f;

    float gravityScaleAtStart;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = rb2d.gravityScale;
    }

    
    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) {return;}

        if (value.isPressed)
        {
            rb2d.velocity += new Vector2(0f, jumpForce);
        }
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;
        
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        anim.SetBool("isRunning", playerHasHorizontalSpeed);
        
        
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
        }
        
    }

    void ClimbLadder()
    {
        if (!capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            rb2d.gravityScale = gravityScaleAtStart;
            anim.SetBool("isClimbing", false);
            return; 
        }

        Vector2 climbVelocity = new Vector2(rb2d.velocity.x, moveInput.y * climbSpeed);
        rb2d.velocity = climbVelocity;
        rb2d.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(rb2d.velocity.y) > Mathf.Epsilon;
        anim.SetBool("isClimbing", playerHasVerticalSpeed);
    }
}
