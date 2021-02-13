using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MovementController : MonoBehaviour {

    // horizontal and vertical and rigidbody2D
    private float h,v;
    private Rigidbody2D rb;

    //use to rotation
    private bool right = true;

    //use to move and jump 
    private bool isJumping = false;

    public int Speed = 8;
    public int JumpForce = 18;

    // animation controller
    private Animator anim;

    //get some component
    private void Start() {
        transform.position = new Vector3Int(0, 0, 0);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Update() {
        //get horizontal axis
        h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        //get vertical axis
        v = CrossPlatformInputManager.GetAxisRaw("Vertical");

        if (h < 0f && right) Flip();
        if (h > 0f && !right) Flip();
    }
    private void FixedUpdate() {
        transform.position += new Vector3(h, 0, 0) * Time.fixedDeltaTime * Speed;
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