using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class BInput : MonoBehaviour
{
    InputAction tap;
    GameObject Ball;

    Vector2 pos;
    BallMovement bMove;
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tap = InputSystem.actions.FindAction("Click");
        Ball = GameObject.FindGameObjectWithTag("Player");
        bMove = Ball.GetComponent<BallMovement>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tap.IsPressed())
        {
           pos = Pointer.current.position.ReadValue();
            pos = cam.ScreenToWorldPoint(pos); 
            bMove.Launch(pos);
            //Debug.Log(pos);
        }
    }

}
