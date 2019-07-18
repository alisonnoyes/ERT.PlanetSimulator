using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyButton : GenericButton
{
    public Body body;

    protected override void SendToReceiver()
    {
        // Debug.Log("BodyButton: Sending info...");
        receiver.ReceiveButtonInput(gameObject.name + " " + body.gameObject.name);
    }
}
