using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpHeight = 5f;
    BoxCollider2D boxCollider;
    Rigidbody2D rbody;
    Animator myAnimator;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 20f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;


    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movementVector = new Vector2(horizontalInput * movementSpeed * 100 * Time.deltaTime, rbody.velocity.y);   
        rbody.velocity = movementVector;
        Run(); 
        Jump();   
    }

    void Run()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRun", playerHasHorizontalSpeed);
    }

    void Jump()
    {
        bool playerHasVerticalSpeed = Mathf.Abs(rbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isJump", playerHasVerticalSpeed);
    }

    void Update()
    {
        if (isDashing)
        {
           return;
        } 
        if (!boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
           return;
        }
        if (Input.GetButtonDown("Jump"))
        {
           rbody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
           StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rbody.gravityScale;
        rbody.gravityScale = 0f;
        rbody.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rbody.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }


 
}
