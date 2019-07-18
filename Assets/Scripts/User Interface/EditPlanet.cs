using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPlanet : GenericButtonReceiver
{
    public GameObject keyboardPrefab;
    public GameObject letterKeyboardPrefab;
    public float scale;

    public override void ReceiveButtonInput(string input)
    {
        GameObject body = null;
        string fieldToEdit = "";
        string bodyName = "";

        if (input.Substring(0, 9) == "Edit Mass")
        {
            bodyName = input.Substring(10);
            fieldToEdit = "mass";
        }
        else if (input.Substring(0, 10) == "Edit Title")
        {
            bodyName = input.Substring(11);
            fieldToEdit = "name";
        }
        else if (input.Substring(0, 10) == "Edit Speed")
        {
            bodyName = input.Substring(11);
            fieldToEdit = "speed";
        }
        else if (input.Substring(0, 11) == "Edit Radius")
        {
            bodyName = input.Substring(12);
            fieldToEdit = "radius";
        }
        else if (input.Substring(0, 16) == "Edit Body Radius")
        {
            bodyName = input.Substring(17);
            fieldToEdit = "bodyradius";
        }
        else
        {
            Debug.LogError("EditPlanet: Unrecognized button message " + input);
            return;
        }

        body = GameObject.Find(bodyName);
        if (body == null)
        {
            Debug.LogError("EditPlanet: Passed Body " + bodyName + " was not found.");
            return;
        }

        GameObject keyboard = null;
        if (fieldToEdit == "name")
            keyboard = Instantiate(letterKeyboardPrefab, GameObject.FindGameObjectWithTag("Player").transform);
        else
            keyboard = Instantiate(keyboardPrefab, GameObject.FindGameObjectWithTag("Player").transform);

        keyboard.transform.localPosition = new Vector3(0, 1f, 0.77f);
        keyboard.transform.localRotation = Quaternion.Euler(Vector3.zero);

        EditPlanetKeyboardManager manager = keyboard.GetComponent<EditPlanetKeyboardManager>();
        manager.receiver = gameObject.GetComponent<EditPlanet>();
        manager.toEdit = fieldToEdit;
        manager.body = body.GetComponent<Body>();
        manager.SetUp();
    }

    public void ReceiveNumberKeyboardInput(Body body, string editing, double input)
    {
        if (editing == "mass")
            body.SetMass(input);
        else if (editing == "radius")
            body.SetRadius(input);
        else if (editing == "speed")
            body.SetSpeed(input);
        else if (editing == "bodyradius")
            body.SetBodyRadius(input);
        else
            Debug.LogError("EditPlanet: Unrecognized editing field passed to ReceiveNumberKeyboardInput, " + editing + ".");
    }

    public void ReceiveLetterKeyboardInput(Body body, string editing, string input)
    {
        if (editing == "name")
            body.SetName(input);
        else
            Debug.LogError("EditPlanet: Unrecognized editing field passed to ReceiveLetterKeyboardInput, " + editing + ".");
    }

    private void Start()
    {
        scale = transform.parent.gameObject.GetComponent<GravitySystem>().scale;
    }
}
