using System;
using UnityEditor.UI;
using UnityEngine;

public class BallMovement : MonoBehaviour
{

    [SerializeField, Range(0,100)]
    private float HitForceMultiplier = 1f;


    float standstilltime;

    [SerializeField, Range(0, 1)]
    float StandStillSpeedThreshold = .5f; //under this speed the ball is considered almost still
    [SerializeField, Range(0, 2)]
    float StandStillTimeLimit = .5f; //time spent almost still at which point the ball is MADE stationary


    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Debug.Log(rb.linearVelocity.magnitude +" " + rb.linearVelocityX+" " + rb.linearVelocityY);

        //check if the ball is moving at any considerable speed
        if (rb.linearVelocity.magnitude < StandStillSpeedThreshold  ) {
            standstilltime += Time.deltaTime; // start incresing time spent still

            //if we've been still for a bit, stop the ball on the x axis
            //separated and with a lower limit than the y axis to distinguish from momentary stillness like at the top of vertical arc
            if(standstilltime > StandStillTimeLimit / 2) rb.linearVelocityX = 0;

            if (standstilltime > StandStillTimeLimit)
            {
                rb.linearVelocityY = 0f;

                //prevent the ball from slowrolling down slight inclines
                rb.simulated = false;

            }
        }
        else
        {
            standstilltime = 0;
        }
    }
    //launches the 
    public void Launch(Vector2 pos)
    {
        //cant launch just yet
        if (rb.linearVelocity.magnitude > 0 || standstilltime < StandStillTimeLimit) return;

        rb.simulated = true;
        Vector2 launchVector = pos - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        launchVector = Vector2.ClampMagnitude(launchVector / 5, 1f);

        Debug.Log(launchVector.magnitude);
        rb.AddForce(launchVector * HitForceMultiplier ,ForceMode2D.Impulse);
    }
}
