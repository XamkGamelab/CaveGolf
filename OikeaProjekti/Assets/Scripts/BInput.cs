using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; //
public class BInput : MonoBehaviour
{
    InputAction tap;
    GameObject Ball;

    Vector2 pos;
    BallMovement bMove;
    Camera cam;

    public float MaxLaunchLength = 1;
    public float LaunchRampingFactor = 5;

    private Arrow arrow;

    void Start()
    {
        tap = InputSystem.actions.FindAction("Click");
        UpdateReferences();
        arrow = new Arrow();
        // Add a LineRenderer component

    }


    void UpdateReferences()
    {
        bMove = GameObject.FindGameObjectWithTag("Player").GetComponent<BallMovement>();
        cam = GameObject.FindFirstObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bMove == null ||cam ==null)
        {
            UpdateReferences();
        }

        pos = Pointer.current.position.ReadValue();
        pos = cam.ScreenToWorldPoint(pos);

        if (!bMove.IsMoving)
        {
            Vector2 launchVector = pos - bMove.position2d;
            launchVector = Vector2.ClampMagnitude(launchVector / LaunchRampingFactor, MaxLaunchLength);

            arrow.Show(launchVector, bMove.position2d, MaxLaunchLength, LaunchRampingFactor);


            if (tap.IsPressed())
            {
                //
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                //
                bMove.Launch(launchVector);
            }
        }
        else
        {
            arrow.Hide();
        }
    }
}
