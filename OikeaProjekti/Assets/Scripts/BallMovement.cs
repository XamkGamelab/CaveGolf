using UnityEditor.UI;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField, Range(0,100)]
    private float HitForceMultiplier = 1f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    //launches the 
    public void Launch(Vector2 pos)
    {
        if (rb.linearVelocity.magnitude > .01f) return;

        pos = pos - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        pos = Vector2.ClampMagnitude(pos / 5, 1f);
        Debug.Log(pos.magnitude);
        rb.AddForce(pos * HitForceMultiplier ,ForceMode2D.Impulse);
    }
}
