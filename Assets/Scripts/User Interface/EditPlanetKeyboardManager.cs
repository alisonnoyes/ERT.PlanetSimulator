using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPlanetKeyboardManager : KeyboardManager
{
    public EditPlanet receiver;
    public string toEdit;
    public Body body;

    public override void SubmitInput()
    {
        if (toEdit == "name")
            receiver.ReceiveLetterKeyboardInput(body, toEdit, text);
        else
            receiver.ReceiveNumberKeyboardInput(body, toEdit, double.Parse(text, System.Globalization.NumberStyles.Float));

        Destroy(gameObject);
    }

    public void SetUp()
    {
        defaultText = "Enter a new " + toEdit + "...";
        base.SubmitInput();
    }
}
