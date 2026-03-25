using UnityEngine;

public class FallingStalactite : MonoBehaviour
{
    private Rigidbody2D rb;
    public float destroy = 5f; // destroy timer so doesnt stay in scene

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rb.bodyType = RigidbodyType2D.Dynamic; //change from kinematic to Dynamic to enable falling
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            Destroy(gameObject, destroy);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Breakable"))
        {
            Destroy(collision.gameObject); // destroy the object the stalactite hits
            Destroy(gameObject);
        }
    }
}
