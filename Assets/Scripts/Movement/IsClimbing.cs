using UnityEngine;

public class IsClimbing : MonoBehaviour
{
    private MovementController parent;
    void Start()
    {
        parent = transform.parent.gameObject.GetComponent<MovementController>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground") {
            parent.Climbing = 1;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground") {
            parent.Climbing = 0;
        }
    }
}
