using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationSpeed : GenericButtonReceiver {

    public float timeScale;
    private string bottomTextStringSlow = "times slower than real time";
    private string bottomTextStringFast = "times faster than real time";

    [SerializeField]
    private TextMesh speedText;
    [SerializeField]
    private TextMesh bottomText;

    bool slowerThanRealtime;

    private Movement_Handler playerMovement;

    private GravitySystem system;

    #region MONOBEHAVIOUR CALLBACKS
    private void Start()
    {
        if (speedText == null || bottomText == null)
            Debug.LogError("SimulationSpeed: TextMesh components for the speed and the bottom description text are required.");

        if (timeScale == 0)
        {
            Debug.LogWarning("SimulationSpeed: No time scale was provided, so the default value of 1 will be used.");
            timeScale = 1;
        }

        if (timeScale < 1)
        {
            slowerThanRealtime = true;
            timeScale = 1 / timeScale;
        }
        else
            slowerThanRealtime = false;

        playerMovement = GameObject.Find("Player").GetComponent<Movement_Handler>();
        system = GameObject.Find("System").GetComponent<GravitySystem>();
        system.timeScale = timeScale;
    }

    private void Update()
    {
        if (slowerThanRealtime && bottomText.text != bottomTextStringSlow)
            bottomText.text = bottomTextStringSlow;
        else if (!slowerThanRealtime && bottomText.text != bottomTextStringFast)
            bottomText.text = bottomTextStringFast;
    }
    #endregion

    #region GENERICBUTTONRECEIVER CALLBACKS
    public override void ReceiveButtonInput(string input)
    {
        if (input == "-")
        {
            if (timeScale > 1000000 && slowerThanRealtime)
            {
                Debug.Log("Simulation will become unstable at smaller time scales. Time cannot be made slower.");
                return;
            }

            if (slowerThanRealtime) timeScale *= 10;
            else if (timeScale == 1)
            {
                timeScale = 10;
                slowerThanRealtime = true;
            }
            else timeScale /= 10;
        }
        else if (input == "+")
        {
            if (timeScale > 1000000 && !slowerThanRealtime)
            {
                Debug.Log("Simulation will become unstable at larger time scales. Time cannot be made faster.");
                return;
            }

            if (!slowerThanRealtime) timeScale *= 10;
            else if (timeScale == 10)
            {
                timeScale = 1;
                slowerThanRealtime = false;
            }
            else timeScale /= 10;
        }
        else
        {
            Debug.LogError("SimulationSpeed: Unrecognized button message " + input);
        }

        SetTimeScale();
    }
    #endregion

    private void SetTimeScale()
    {
        if (slowerThanRealtime) system.timeScale = 1 / timeScale;
        else system.timeScale = timeScale;

        speedText.text = timeScale.ToString();
    }

}
