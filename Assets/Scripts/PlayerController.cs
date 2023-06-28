using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] Camera camera;
    public Rigidbody2D rb;
    public Vector2 velocity;
    private float inputAxis;
    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    [SerializeField] public AnimationController playerAnim;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);

    private void Update()
    {
        HorizontalMovement();
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 0.55f, LayerMask.GetMask("Ground"));
        RaycastHit2D hit1 = Physics2D.Raycast(rb.position, Vector2.down, 0.55f, LayerMask.GetMask("Obtacles"));
        grounded = (hit.collider != null && hit.rigidbody != rb) || (hit1.collider != null && hit1.rigidbody != rb);
        //grounded = rb.Raycast(Vector2.down);
        if(grounded)
            GroundedMovement();
        
        ApplyGravity();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rb.position, rb.position + Vector2.down);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(rb.position, rb.position + _shootingDir);
    }
    private void FixedUpdate()
    {
        Vector2 pos = rb.position;
        pos += velocity * Time.deltaTime;

        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        pos.x = Mathf.Clamp(pos.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);
        rb.MovePosition(pos);
    }

    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);
        if (rb.Raycast(Vector2.right * velocity.x)) velocity.x = 0;
        if (velocity.x > 0)
            transform.eulerAngles = Vector3.zero;
        else if (velocity.x < 0)
            transform.eulerAngles = Vector3.up * 180;
    }
    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0;
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }
    }
    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !Input.GetKey(KeyCode.Space);
        float multiplier = falling ? 2f : 1f;
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if(transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
            }
            
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // bounce off enemy head
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }
    }

}
