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

    public float MaxLaunchLength = 1;
    public float LaunchRampingFactor = 5;

    private LineRenderer lineRenderer;

    void Start()
    {
        tap = InputSystem.actions.FindAction("Click");
        UpdateReferences();

        // Add a LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.sortingOrder = 1000;
        // Set the material
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        lineRenderer.endColor = Color.red;
        lineRenderer.startColor = Color.green;

        // Set the width
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.2f;

        // Set the number of vertices
        lineRenderer.positionCount = 2;

        // Set the positions of the vertices
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        
        lineRenderer.SetPosition(1, new Vector3(1, 1, 0));
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
            lineRenderer.endColor = Color.Lerp(Color.green, Color.red, launchVector.magnitude / MaxLaunchLength);
            lineRenderer.endWidth = Mathf.Lerp(.2f, .5f, launchVector.magnitude / MaxLaunchLength);

            lineRenderer.SetPosition(0, bMove.position2d);
            lineRenderer.SetPosition(1, bMove.position2d + launchVector*LaunchRampingFactor);

            if (tap.IsPressed())
            {
                bMove.Launch(launchVector);
            }


        }
        else
        {
            lineRenderer.SetPosition(0, Vector3.one*10000000000);
            lineRenderer.SetPosition(1, Vector3.one * 10000000000);
        }




    }



}
