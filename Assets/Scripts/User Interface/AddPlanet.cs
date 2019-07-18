using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlanet : GenericButtonReceiver
{
    public GameObject system;
    public GameObject systemStats;

    private GravitySystem gravSystem;
    private SystemStatisticsDisplay stats;

    public GameObject planetPrefab;
    public GameObject keyboardPrefab;
    public GameObject nameKeyboardPrefab;
    public GameObject inputButtonPrefab;
    public GameObject throwButtonPrefab;
    public GameObject throwingPlanetPrefab;

    private string newPlanetName;
    private List<double> newPlanetParams = new List<double>();

    private GameObject inputButton;
    private GameObject throwButton;

    private float uiDistFromPlayer = 1;

    #region MONOBEHAVIOUR CALLBACKS
    private void Start()
    {
        if (system == null || system.GetComponent<GravitySystem>() == null)
            Debug.LogError("AddPlanet: Please attach a GameObject with a GravitySystem component.");
        else
            gravSystem = system.GetComponent<GravitySystem>();

        if (systemStats == null || systemStats.GetComponent<SystemStatisticsDisplay>() == null)
            Debug.LogWarning("AddPlanet: No SystemStatisticsDisplay component detected. Statistics for this body will not be shown.");
        else
            stats = systemStats.GetComponent<SystemStatisticsDisplay>();
    }
    #endregion

    private void AddPlanetInput()
    {
        GameObject keyboard = Instantiate(nameKeyboardPrefab, GameObject.FindGameObjectWithTag("Player").transform);
        keyboard.GetComponent<AddPlanetKeyboardManager>().receiver = gameObject.GetComponent<AddPlanet>();
        keyboard.transform.localPosition = new Vector3(0, 1f, 0.77f);
        keyboard.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void AddPlanetThrowing()
    {
        GameObject planet = Instantiate(throwingPlanetPrefab);
        GameObject playerCam = GameObject.FindGameObjectWithTag("MainCamera");

        // Planet should be near the player but not moving with the player's motion so it can be grabbed
        planet.transform.position = playerCam.transform.position + uiDistFromPlayer * playerCam.transform.forward;
        planet.transform.rotation = playerCam.transform.rotation;
    }

    public override void ReceiveButtonInput(string input)
    {
        if (input == "Add Input Planet(Clone)") AddPlanetInput();
        else if (input == "Add Throwing Planet(Clone)") AddPlanetThrowing();
        else if (input == "Add Planet")
        {
            // Ask the user if they would like to add the planet by plugging in parameters or throwing one in
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            inputButton = Instantiate(inputButtonPrefab, player.transform);
            inputButton.transform.localPosition = new Vector3(-0.22f, 0.86f, 0.381f);
            inputButton.transform.localRotation = Quaternion.Euler(new Vector3(0, -41.757f, 0));
            inputButton.GetComponent<GenericButton>().receiver = gameObject.GetComponent<AddPlanet>();

            throwButton = Instantiate(throwButtonPrefab, player.transform);
            throwButton.transform.localPosition = new Vector3(-0.047f, 0.86f, 0.466f);
            throwButton.transform.localRotation = Quaternion.Euler(new Vector3(0, -13.471f, 0));
            throwButton.GetComponent<GenericButton>().receiver = gameObject.GetComponent<AddPlanet>();

            return;
        }
        else Debug.LogWarning("AddPlanet: Unrecognized button message: " + input + ".");

        Destroy(inputButton);
        Destroy(throwButton);
    }

    public void ReceiveKeyboardInput(double input)
    {
        newPlanetParams.Add(input);
        TryToAddNewPlanet();
    }

    public void ReceiveKeyboardInput(string input)
    {
        newPlanetName = input;

        // Start up the keyboard to read inputs about the new body
        GameObject keyboard = Instantiate(keyboardPrefab, GameObject.FindGameObjectWithTag("Player").transform);
        keyboard.GetComponent<AddPlanetKeyboardManager>().receiver = gameObject.GetComponent<AddPlanet>();
        keyboard.GetComponent<AddPlanetKeyboardManager>().index = 1;
        keyboard.GetComponent<AddPlanetKeyboardManager>().Start();
        keyboard.transform.localPosition = new Vector3(0, 1f, 0.77f);
    }

    private void TryToAddNewPlanet()
    {
        if (newPlanetParams.Count < 4) return;

        Debug.Log("Creating new planet...");

        GameObject newPlanet = Instantiate(planetPrefab, system.transform);
        newPlanet.transform.localPosition = new Vector3(3, 0, 0);
        newPlanet.name = newPlanetName;
        Body newBody = newPlanet.GetComponent<Body>();
        newBody.mass = newPlanetParams[0];
        newBody.orbitRadius = newPlanetParams[1];
        newBody.orbitSpeed = newPlanetParams[2];
        newBody.bodyRadius = newPlanetParams[3];

        newBody.tag = "SystemBody";

        newPlanetParams = new List<double>();
    }
}
