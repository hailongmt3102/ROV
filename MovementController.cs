using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MovementController : MonoBehaviour {

    // horizontal and vertical and rigidbody2D
    private float h,v;
    private Rigidbody2D rb;

    // use to rotation
    private bool right = true;

    // moving player information 
    private MovingInformation movingInfor;

    //use to move and jump 
    private bool isJumping = false;

    public int Speed = 8;
    public int JumpForce = 18;

    // animation controller
    private Animator anim;

    // use when player is going in slopping way
    public int Climbing = 0;
    [Range(0.5f, 0.8f)]
    public float smoothClimbing = 0.6f;

    private void Start() {
        //get some component
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        movingInfor = new MovingInformation(transform.position);
    }
    private void Update() {
        // get horizontal axis
        h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        //get vertical axis
        v = CrossPlatformInputManager.GetAxisRaw("Vertical");
        if (h < 0f && right) Flip();
        if (h > 0f && !right) Flip();
    }
    private void FixedUpdate() {
        transform.position += new Vector3(h, Climbing * smoothClimbing, 0) * Time.fixedDeltaTime * Speed;
        anim.SetFloat("Speed", Mathf.Abs(h));

        if (Mathf.Abs(rb.velocity.y) < 0.001f) {
            isJumping = false;
            rb.AddForce(new Vector2(0, JumpForce * v), ForceMode2D.Impulse);
        }
        if (v != 0) {
            isJumping = true;
        }
        anim.SetBool("Jumping", isJumping);
    }
    void Flip()
    {
        right = !right;
        transform.Rotate(0, 180f, 0);
    }
}