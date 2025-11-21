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
    bool isPaused = false;

    AudioManager audioManager;


    public void Pause()
    {
        isPaused = true;
        arrow.Hide();
    }
    public void Resume()
    {
        isPaused = false;
    }

    void Start()
    {
        tap = InputSystem.actions.FindAction("Click");
        UpdateReferences();
        arrow = new Arrow();
        GameObject tempAudioManagerObject = GameObject.FindWithTag("Audio");
        if (tempAudioManagerObject != null)
        {
            audioManager = tempAudioManagerObject.GetComponent<AudioManager>();
        }
        else
        {
            Debug.LogError("AUDIO MANAGER NOT FOUND");
        }




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

        if (!bMove.IsMoving && !isPaused)
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

                audioManager?.PlaySFX(audioManager.throwBall);
            }
        }
        else
        {
            arrow.Hide();
        }
    }
}
