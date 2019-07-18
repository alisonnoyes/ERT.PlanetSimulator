using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : GenericButtonReceiver
{
    public GravitySystem system;
    public LoadSystem load;

    public string savePath;

    private void Save()
    {
        Debug.Log("Saving system to a file folder...");
        string timestamp = DateTime.Now.ToString("MMMM.dd.yyyy.H.mm.ss");

        System.IO.Directory.CreateDirectory(savePath + timestamp);

        // Save the GravitySystem
        SystemWrapper info = new SystemWrapper(system);
        string systemJson = JsonUtility.ToJson(info);
        System.IO.File.AppendAllText(savePath + timestamp + "/GravitySystem", systemJson);

        // Save all of the bodies
        int counter = 0;
        foreach (Body b in system.orbitingBodies)
        {
            BodyWrapper wrapper = new BodyWrapper(b);
            string bodyJson = JsonUtility.ToJson(wrapper);
            System.IO.File.AppendAllText(savePath + timestamp + "/Body" + counter, bodyJson);
            counter++;
        }

        load.loadPath.Add(savePath + timestamp);
    }

    public override void ReceiveButtonInput(string input)
    {
        if (input == "Save") Save();
    }
}
