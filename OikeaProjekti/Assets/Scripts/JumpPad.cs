using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class JumpPad : MonoBehaviour
{
    private float bounce = 10f;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.1f, 0.1f),1f) * bounce, ForceMode2D.Impulse);
        }
    }
    
}
