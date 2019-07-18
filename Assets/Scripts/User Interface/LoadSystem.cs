using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadSystem : GenericButtonReceiver
{
    public List<string> loadPath;
    public GravitySystem system;

    public GameObject bodyPrefab;

    private void Load(int index)
    {
        if (index >= loadPath.Count)
        {
            Debug.LogError("LoadSystem: Provided index is out of bounds.");
            return;
        }

        system.ClearAll();

        string folderPath = loadPath[index];

        StreamReader systemReader = new StreamReader(folderPath + "/GravitySystem");
        SystemWrapper systemWrapper = JsonUtility.FromJson<SystemWrapper>(systemReader.ReadToEnd());

        system.gameObject.GetComponent<SystemBoundary>().radius = systemWrapper.boundaryRadius;

        system.G = systemWrapper.gravityConstant;
        system.scale = systemWrapper.scale;
        system.timeScale = systemWrapper.timeScale;
        system.bodyScale = systemWrapper.bodyScale;
        system.timeUnits = (OrbitTimer.TimeUnit)systemWrapper.timeUnit;
        system.elasticity = systemWrapper.elasticity;

        system.gameObject.GetComponent<SystemBoundary>().Awake();

        GameObject centralBody = Instantiate(bodyPrefab, system.gameObject.transform);
        centralBody.tag = "SystemCentralBody";
        centralBody.gameObject.name = systemWrapper.centralBodyName;
        Body central = centralBody.GetComponent<Body>();
        central.mass = systemWrapper.centralBodyMass;
        central.bodyRadius = systemWrapper.centralBodyRadius;

        system.centralBody = central;

        for (int i = 0; i < systemWrapper.numBodies; i++)
        {
            StreamReader bodyReader = new StreamReader(folderPath + "/Body" + i);
            BodyWrapper bodyWrapper = JsonUtility.FromJson<BodyWrapper>(bodyReader.ReadToEnd());

            GameObject body = Instantiate(bodyPrefab, system.gameObject.transform);
            body.transform.localPosition = new Vector3(systemWrapper.boundaryRadius / 2, 0, 0);

            body.name = bodyWrapper.bodyName;
            Body b = body.GetComponent<Body>();
            b.mass = bodyWrapper.mass;
            b.centralBody = central;
            b.bodyRadius = bodyWrapper.bodyRadius;
            b.location = bodyWrapper.location;
            b.scale = systemWrapper.scale;

            b.orbitRadius = bodyWrapper.orbitRadius;
            b.orbitSpeed = bodyWrapper.orbitSpeed;
            b.orbitDirection = bodyWrapper.orbitDirection;

            //b.SetUp(true);

            body.tag = "SystemBody";
            //system.gameObject.GetComponent<SystemBoundary>().OnTriggerEnter(body.GetComponent<Collider>());
        }
    }

    public override void ReceiveButtonInput(string input)
    {
        if (input == "Load") Load(0); // IN THE FUTURE THIS INDEX SHOULD BE SELECTABLE
    }
}
