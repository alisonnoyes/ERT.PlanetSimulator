using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericButton : MonoBehaviour
{
    public GenericButtonReceiver receiver;

    public Material selectedMaterial;
    private Material unselectedMaterial;

    private bool selected;

    private bool ready = false; // Want to make sure the user can't just hold down the trigger and accidentally click the button

    private Controller_Input input;

    // Start is called before the first frame update
    public void Start()
    {
        if (receiver == null) Debug.LogError("GenericButton: Button must have a receiver to pass input to.");

        unselectedMaterial = gameObject.GetComponent<Renderer>().material;
        if (selectedMaterial == null)
        {
            Debug.LogWarning("GenericButton: Selected material was not set, so the original material will not change when selected.");
            unselectedMaterial = selectedMaterial;
        }

        input = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller_Input>();

        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Pointer")
        {
            selected = true;
            gameObject.GetComponent<Renderer>().material = selectedMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Pointer")
        {
            selected = false;
            gameObject.GetComponent<Renderer>().material = unselectedMaterial;
        }
    }

    protected virtual void SendToReceiver()
    {
        receiver.ReceiveButtonInput(gameObject.name);
    }

    public void Update()
    {
        if (selected && input.IsTriggerDown() && ready)
        {
            ready = false;
            SendToReceiver();
        }
        else if (!input.IsTriggerDown())
            ready = true;
    }
}
