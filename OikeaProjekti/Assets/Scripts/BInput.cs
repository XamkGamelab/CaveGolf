using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class BInput : MonoBehaviour
{

    LineRenderer arrow;

    [HideInInspector,]
    public static BInput Instance {
        get
        {
            if (Instance != null)
            {
                return Instance;
            }
            else
            {
                Instance = new BInput();
                return Instance;
            }
        }
            
            private set; }

    InputAction tap;
    GameObject Ball;

    Vector2 ClickPos;
    BallMovement bMove;
    Camera cam;

    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        


        tap = InputSystem.actions.FindAction("Click");
    }

    // Update is called once per frame
    void Update()
    {
        //arrow = new LineRenderer();

        //ClickPos = Pointer.current.position.ReadValue();
        //ClickPos = cam.ScreenToWorldPoint(ClickPos);

        //arrow.enabled = true;
        //arrow.positionCount = 2;
        //arrow.SetPosition(0, Ball.transform.position);
        //arrow.SetPosition(1, ClickPos);


        if (bMove = null)
        {
            Debug.Log("Null");
        }


        if (tap.IsPressed())
        {

            ClickPos = Pointer.current.position.ReadValue();
            ClickPos = cam.ScreenToWorldPoint(ClickPos);
            ClickPos = cam.ScreenToWorldPoint(ClickPos); 
            bMove.Launch(ClickPos);

        }
    }

    public void ReferenceBall(BallMovement b)
    {
        bMove = b;
        cam = GameObject.FindFirstObjectByType<Camera>();
    }
}
