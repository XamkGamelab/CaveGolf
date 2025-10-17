using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class JumpPad : MonoBehaviour
{
    private float bounce = 5f;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
        }
    }
    
}
