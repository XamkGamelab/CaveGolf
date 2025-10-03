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
    void OnEnable()
    {
        tap = InputSystem.actions.FindAction("Click");
        UpdateReferences();
    }


    void UpdateReferences()
    {
        bMove = GameObject.FindGameObjectWithTag("Player").GetComponent<BallMovement>();
        cam = GameObject.FindFirstObjectByType<Camera>();
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
