using UnityEngine;

public class PlayerMovement
{
    private Camera mainCamera;
    private Rigidbody2D rb;
    private Collider2D capsuleCollider;

    private Vector2 velocity;
    private float inputAxis;

    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);
    public bool falling => velocity.y < 0f && !grounded;

    public bool isEnabled = true;

    public PlayerMovement(Rigidbody2D rigidbody, Collider2D collider)
    {
        rb = rigidbody;
        capsuleCollider = collider;
        velocity = Vector2.zero;
    }

    public void Enable()
    {
        isEnabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        capsuleCollider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }

    public void Disable()
    {
        isEnabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        capsuleCollider.enabled = false;
        velocity = Vector2.zero;
        jumping = false;
    }

    public void UpdateMovement()
    {
        if (!isEnabled) return;

        HorizontalMovement();

        grounded = rb.Raycast(Vector2.down);

        if (grounded)
        {
            GroundedMovement();
        }

        ApplyGravity();
    }

    public void FixedUpdateMovement()
    {
        if (!isEnabled) return;

        Vector2 position = rb.position;
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = mainCamera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rb.MovePosition(position);
    }

    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);

        if (rb.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0f;
        }

        if (velocity.x > 0f)
        {
            rb.transform.eulerAngles = Vector3.zero;
        }
        else if (velocity.x < 0f)
        {
            rb.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }
    }

    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isEnabled) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (rb.transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if (rb.transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
            }
        }
    }
}
