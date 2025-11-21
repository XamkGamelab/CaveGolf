using System;
using UnityEngine;

public class BallMovement : MonoBehaviour
{


    Vector2 startupPosition;

    public bool IsMoving = false;
    public float MaximumLaunchDistance;
    public Vector3 position
    {
        get { return rb.position; }
    }
    public Vector2 position2d
    {
        get { return new Vector2(rb.position.x, rb.position.y); }
    }


    [SerializeField, Range(0,100)]
    private float HitForceMultiplier = 1f;


    float standstilltime;

    [SerializeField, Range(0, 1)]
    float StandStillSpeedThreshold = .5f; //under this speed the ball is considered almost still
    [SerializeField, Range(0, 2)]
    float StandStillTimeLimit = .5f; //time spent almost still at which point the ball is MADE stationary


    [Header("Water Physics")]
    [SerializeField]
    bool WaterPhysics = false;
    [SerializeField]
    float WaterGravityScale = .55f;
    [SerializeField]
    float WaterHitForceMultiplier = 9;




    Rigidbody2D rb;
    void Start()
    {

        startupPosition = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        if (WaterPhysics)
        {
            rb.gravityScale = WaterGravityScale;
            HitForceMultiplier = WaterHitForceMultiplier;
        }



    }

    void Update()
    {
        //Debug.Log(rb.linearVelocity.magnitude +" " + rb.linearVelocityX+" " + rb.linearVelocityY);

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
                IsMoving = false;

            }
        }
        else
        {
            standstilltime = 0;
        }
    }


    public void resetPosition()
    {
        gameObject.transform.position = startupPosition;
        rb.linearVelocity = new Vector2(0, 0);
        rb.angularVelocity = 0;
    }

    public void Launch(Vector2 _LaunchVector)
    {
        //cant launch just yet
        if (rb.linearVelocity.magnitude > 0 || standstilltime < StandStillTimeLimit) return;
        Score.Add(1);
        rb.simulated = true;


        //Debug.Log(_LaunchVector.magnitude);
        rb.AddForce(_LaunchVector * HitForceMultiplier ,ForceMode2D.Impulse);
        IsMoving = true;
    }
}
