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

    private LineRenderer lineRenderer;

    void OnEnable()
    {
        tap = InputSystem.actions.FindAction("Click");
        UpdateReferences();
        // Add a LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set the material
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        lineRenderer.endColor = Color.red;
        lineRenderer.startColor = Color.green;

        // Set the width
        lineRenderer.startWidth = 0.2f;
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
        pos = Pointer.current.position.ReadValue();
        pos = cam.ScreenToWorldPoint(pos);

        if (bMove != null)
        {
            lineRenderer.SetPosition(0, bMove.position);

            lineRenderer.SetPosition(1, pos);

        }


        if (tap.IsPressed())
        {
           pos = Pointer.current.position.ReadValue();
            pos = cam.ScreenToWorldPoint(pos); 
            bMove.Launch(pos);
            //Debug.Log(pos);
        }
    }



}
