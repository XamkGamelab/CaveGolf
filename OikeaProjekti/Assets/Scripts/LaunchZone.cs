using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LaunchZone : MonoBehaviour
{
    private float bounce = 10f;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direction = collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity.normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * bounce, ForceMode2D.Impulse);
        }
    }
    
}

