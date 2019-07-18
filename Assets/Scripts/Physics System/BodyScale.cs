using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyScale : GenericButtonReceiver
{
    public float bodyScale;
    private string bottomTextStringSmall = "times smaller relative to distance";
    private string bottomTextStringLarge = "times larger relative to distance";

    [SerializeField]
    private TextMesh scaleText;
    [SerializeField]
    private TextMesh bottomText;

    bool smaller;

    private Movement_Handler playerMovement;

    private GravitySystem system;

    #region MONOBEHAVIOUR CALLBACKS
    private void Start()
    {
        if (scaleText == null || bottomText == null)
            Debug.LogError("SimulationSpeed: TextMesh components for the speed and the bottom description text are required.");

        if (bodyScale == 0)
        {
            Debug.LogWarning("SimulationSpeed: No time scale was provided, so the default value of 1 will be used.");
            bodyScale = 1;
        }

        if (bodyScale < 1)
        {
            smaller = true;
            bodyScale = 1 / bodyScale;
        }
        else
            smaller = false;

        playerMovement = GameObject.Find("Player").GetComponent<Movement_Handler>();
        system = GameObject.Find("System").GetComponent<GravitySystem>();
        system.bodyScale = bodyScale;
    }

    private void Update()
    {
        if (smaller && bottomText.text != bottomTextStringSmall)
            bottomText.text = bottomTextStringSmall;
        else if (!smaller && bottomText.text != bottomTextStringLarge)
            bottomText.text = bottomTextStringLarge;
    }
    #endregion

    #region GENERICBUTTONRECEIVER CALLBACKS
    public override void ReceiveButtonInput(string input)
    {
        if (input == "-")
        {
            if (smaller) bodyScale += 5;
            else if (bodyScale == 1)
            {
                bodyScale = 5;
                smaller = true;
            }
            else if (bodyScale == 5) bodyScale = 1;
            else bodyScale -= 5;
        }
        else if (input == "+")
        {
            if (!smaller && bodyScale != 1) bodyScale += 5;
            else if (!smaller) bodyScale = 5;
            else if (bodyScale == 5)
            {
                bodyScale = 1;
                smaller = false;
            }
            else bodyScale -= 5;
        }
        else
        {
            Debug.LogError("SimulationSpeed: Unrecognized button message " + input);
        }

        SetScale();
    }
    #endregion

    private void SetScale()
    {
        if (smaller) system.bodyScale = 1 / bodyScale;
        else system.bodyScale = bodyScale;

        scaleText.text = bodyScale.ToString();

        system.ScaleBodies();
    }
}
