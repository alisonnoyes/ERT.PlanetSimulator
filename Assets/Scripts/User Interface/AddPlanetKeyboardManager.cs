using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPlanetKeyboardManager : KeyboardManager
{
    
    private string[] defaultTextArray = new string[5];
    [NonSerialized]
    public int index = 0;
    [NonSerialized]
    public AddPlanet receiver;

    public override void SubmitInput()
    {
        if (index == 0)
        {
            receiver.ReceiveKeyboardInput(text);
            Destroy(gameObject);
            return;
        }

        else receiver.ReceiveKeyboardInput(double.Parse(text, System.Globalization.NumberStyles.Float));

        index++;
        if (index < defaultTextArray.Length)
        {
            defaultText = defaultTextArray[index];
            base.SubmitInput();
        }
        else {
            Destroy(gameObject);
        }
    }

    #region MONOBEHAVIOUR_METHODS
    // Start is called before the first frame update
    public void Start()
    {
        defaultTextArray[0] = "Enter name of new planet...";
        defaultTextArray[1] = "Enter mass of new planet...";
        defaultTextArray[2] = "Enter radius of new planet's orbit...";
        defaultTextArray[3] = "Enter speed of new planet...";
        defaultTextArray[4] = "Enter radius of new planet's body...";

        defaultText = defaultTextArray[index];
        base.SubmitInput();
    }
    #endregion



}
