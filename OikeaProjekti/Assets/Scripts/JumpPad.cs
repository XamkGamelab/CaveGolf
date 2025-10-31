using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class JumpPad : MonoBehaviour
{
    private float bounce = 10f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * bounce, ForceMode2D.Impulse);
        }
    }
    
}
