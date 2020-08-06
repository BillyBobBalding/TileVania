using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] private float _runSpeed = 5f;
    [SerializeField] private float _jumpSpeed = 5f;
    [SerializeField] private float _climbSpeed = 5f;
    [SerializeField] private Vector2 deathKick = new Vector2(25f, 25f);

    // State
    bool isAlive = true;

    // Cached component references
    Rigidbody2D myRigidbody;  // cache
    Animator myAnimator; // cache
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeetCollider2D;
    float gravityScaleAtStart;

    // Message then methods

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        if (!isAlive) { return; }
        
        Run();
        Jump();
        ClimbLadder();
        
        FlipSprite();
        Die();
    }
    
    private void Run()
    {
        float controlThrow = Input.GetAxis("Horizontal"); // value -1 to 1

        Vector2 playerVelocity = new Vector2(controlThrow * _runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        // if not colliding with ground, allow player to jump
        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, _jumpSpeed);
            myRigidbody.velocity += jumpVelocityToAdd;
        }
    }

    private void ClimbLadder()
    {
        if (!myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimator.SetBool("isClimbing", false);
            myRigidbody.gravityScale = gravityScaleAtStart; // 1

            return;
        }

        myRigidbody.gravityScale = 0f;
        float controlThrow = Input.GetAxis("Vertical");

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, controlThrow * _climbSpeed);
        myRigidbody.velocity = climbVelocity;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);

    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(
                Mathf.Sign(myRigidbody.velocity.x), transform.localScale.y); // (x, 1f)
        }
    }

    private void Die()
    {
        //if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Spikes")))
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")) || 
            myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Spikes")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Die");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            StartCoroutine(VelocityZero());
        }
    }

    IEnumerator VelocityZero()
    {
        yield return new WaitForSeconds(1);
        myRigidbody.velocity = new Vector2(0f, 0f);
        myRigidbody.simulated = false;
    }
    
}
