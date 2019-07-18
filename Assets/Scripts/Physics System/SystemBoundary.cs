using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemBoundary : MonoBehaviour
{
    public float radius;

    private SphereCollider sphere;
    private GravitySystem system;
    private SystemStatisticsDisplay stats;

    public GameObject timerPrefab;

    private float scale;

    #region COLLIDER METHODS
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SystemBody" || other.tag == "ThrowableBody")
        {
            GameObject newPlanet = other.gameObject;
            newPlanet.transform.parent = transform;
            newPlanet.GetComponent<Body>().centralBody = system.centralBody;
            newPlanet.GetComponent<Body>().scale = scale;
            newPlanet.GetComponent<Body>().timeScale = system.timeScale;

            GameObject newTimer = Instantiate(timerPrefab, system.transform);
            newTimer.name = newPlanet.name + " Timer";
            newTimer.transform.position = new Vector3(radius / 4, 0, 0);
            newTimer.transform.localScale = new Vector3(radius / 2, radius, radius);

            newPlanet.GetComponent<Body>().timer = newTimer.GetComponent<OrbitTimer>();

            // Use the current radius, velocity, etc. instead of setting the ones that may be in the prefab if it is thrown
            if (other.tag == "ThrowableBody") newPlanet.GetComponent<Body>().SetUp(false);
            else newPlanet.GetComponent<Body>().SetUp(true);

            other.tag = "SystemBody";

            // Wait for the new Body to be ready and in the correct position before adding it to the system
            while (!newPlanet.GetComponent<Body>().IsReady()) { }

            system.AddBody(newPlanet.GetComponent<Body>());
            stats.AddBody(newPlanet.GetComponent<Body>());
        }
        else if (other.tag == "SystemCentralBody")
        {
            GameObject newCentralBody = other.gameObject;
            newCentralBody.transform.parent = transform;
            newCentralBody.transform.position = Vector3.zero;

            Body newBody = newCentralBody.GetComponent<Body>();
            newBody.centralBody = newBody;
            newBody.scale = scale;
            newBody.SetUp(true);

            stats.AddBody(newBody);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SystemBody" || other.tag == "ThrowableBody")
        {
            Debug.Log(other.name + " has exited the system boundary. It will now be destroyed.");
            system.RemoveBody(other.gameObject.GetComponent<Body>());
            stats.RemoveBody(other.gameObject.GetComponent<Body>());
            Destroy(other.gameObject.GetComponent<Body>().timer.gameObject);
            Destroy(other.gameObject);
        }
    }
    #endregion

    // Start is called before the first frame update
    public void Awake()
    {
        sphere = GetComponent<SphereCollider>();
        if (sphere == null)
            Debug.LogError("SystemBoundary: No SphereCollider component was found, but this component is required.");

        if (radius != 0)
            sphere.radius = radius;
        else
        {
            Debug.LogWarning("SystemBoundary: No radius was provided for the boundary, so the sphere collider will keep its default radius.");
            radius = sphere.radius;
        }

        system = gameObject.GetComponent<GravitySystem>();
        if (system == null)
            Debug.LogError("SystemBoundary: GameObject needs to have a GravitySystem component.");
        scale = system.scale;

        stats = GameObject.Find("System Statistics").GetComponent<SystemStatisticsDisplay>();
        if (stats == null)
            Debug.LogError("SystemBoundary: No SystemStatisticsDisplay was found in the scene.");
        stats.scale = system.scale;
    }
}
