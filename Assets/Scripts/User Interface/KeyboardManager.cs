using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : GenericButtonReceiver
{
    [SerializeField]
    private TextMesh textField;

    protected string text = "";

    public string defaultText;

    public void WriteCharacter(string character)
    {
        text += character;
        textField.text = text;
    }

    public void DeleteCharacter()
    {
        if (text.Length > 0)
        {
            text = text.Substring(0, text.Length - 1);
            textField.text = text;
        }
        else
        {
            textField.text = defaultText;
        }
    }

    public virtual void SubmitInput()
    {
        // Debug.Log("KeyboardManager: Resetting text...");
        text = "";
        textField.text = defaultText;
    }

    public override void ReceiveButtonInput(string input)
    {
        if (input == "Submit") SubmitInput();
        else if (input == "Delete") DeleteCharacter();
        else WriteCharacter(input);
    }
}
