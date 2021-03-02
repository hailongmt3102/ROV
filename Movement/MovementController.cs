using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class MovementController : MonoBehaviour {

    public int Speed = 7;
    public int JumpForce = 16;
    // use when player is going in slopping way
    public LayerMask whatIsGround;
    public Transform SlopePointCheck;
    public Transform GroundCheck;
    [Range(.1f, .3f)]
    public float CheckRadius = .2f;
    [Range(0f, 0.2f)]
    public float MovementSmoothing = 0.05f;

    public int Climbing = 0;
    [Range(0.5f, 1f)]
    public float DegreeRate = 0.8f;

    // Score when moving
    public Text Score;

    // when the player dead.
    public GameObject DiePanel;

    // when player complete this level
    public GameObject WinPanel;

    //--------------------------------------------------------------------------------------------------------------------------//
    // horizontal and vertical and rigidbody2D
    private float h, v;
    private Rigidbody2D rb;

    // check the player is standing on the ground 
    private bool IsClimbing = false;
    // check the player is moving in the slopping way
    private bool IsGrounded = false;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;

    // use to rotation
    private bool FacingRight = true;

    // moving player information 
    private MovingInformation movingInfor;

    // animation controller
    private Animator anim;

    private bool Endgame = false;
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
        if (h < 0f && FacingRight) Flip();
        if (h > 0f && !FacingRight) Flip();

        if (transform.position.y < -2) {
            Dying();
        }
    }
    private void FixedUpdate() {
        // check the player is grounded
        if (Physics2D.OverlapCircle(GroundCheck.position, CheckRadius, whatIsGround))
        {
            IsGrounded = true;
        }
        // Check if the player is moving in the sloping ground
        if (Physics2D.OverlapCircle(SlopePointCheck.position, CheckRadius, whatIsGround)) {
            IsClimbing = true;
        }
        else {
            IsClimbing = false; 
        }
        // caculate the target velocity.
        if (IsClimbing)
        {
            targetVelocity = new Vector2(h, h * DegreeRate) * Speed;
        }
        else
        {
            targetVelocity = new Vector2(h * Speed, rb.velocity.y);
        }
        // smoothing when using it
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, MovementSmoothing);
        // the player is standing on the ground and wants to jump
        if (v != 0 && IsGrounded)
        {
            IsGrounded = false;
            rb.AddForce(new Vector2(0, JumpForce * v), ForceMode2D.Impulse);
        }
        // animation paramater
        anim.SetFloat("Speed", Mathf.Abs(h));
        anim.SetBool("Jumping", !IsGrounded);
    }
    void Flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0, 180f, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            // add score
            movingInfor.addCoin();
            Destroy(collision.collider.gameObject);
            Score.text = (int.Parse(Score.text) + 1).ToString();
        }
        else
        if (collision.collider.tag == "SavePoint")
        {
            // save the player position
            movingInfor.SavePosition = transform.position;
        }
        else 
        if (collision.collider.tag == "EndPoint" && !Endgame) {
            // complete level
            transform.gameObject.SetActive(false);
            int CoinNumber = GameObject.Find("Level").GetComponent<GenarateTileMap>().CoinNumber;

            int star = 1;
            if (movingInfor.Coin > (CoinNumber / 2)) star++;
            if (movingInfor.Coin > (CoinNumber / 3) * 2) star++;
            WinPanel.GetComponent<WinController>().victory(movingInfor.Coin, star);
            Endgame = true;
        }
    }
    // when player is die
    private void Dying() {
        transform.gameObject.SetActive(false);
        DiePanel.SetActive(true);
        Invoke("Rivive", 3);
    }
    // the player will be rivived in save position
    private void Rivive() {
        transform.gameObject.SetActive(true);
        transform.position = movingInfor.SavePosition;
        DiePanel.SetActive(false);
    }
}