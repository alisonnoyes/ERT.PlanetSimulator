using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ThrowingPlanetSelector : MonoBehaviour
{
    [SerializeField]
    private List<Throwable> planets = new List<Throwable>();

    private Throwable attachedPlanet = null;

    private void DestroyAll()
    {
        foreach (Throwable planet in planets)
        {
            Destroy(planet.gameObject);
        }

        planets.Clear();
    }

    private void Update()
    {
        foreach (Throwable planet in planets)
        {
            if (planet.IsAttachedToHand())
            {
                attachedPlanet = planet;
                break;
            }
        }

        if (attachedPlanet != null && planets.Count > 0)
        {
            planets.Remove(attachedPlanet);
            attachedPlanet.transform.parent = null;
            attachedPlanet.tag = "Untagged";
            attachedPlanet.tag = "ThrowableBody";
            DestroyAll();
        }
        else if (attachedPlanet != null)
        {
            if (!attachedPlanet.IsAttachedToHand())
            {
                attachedPlanet.gameObject.GetComponent<Interactable>().enabled = false;
                attachedPlanet.gameObject.GetComponent<Throwable>().enabled = false;
                attachedPlanet.gameObject.GetComponent<VelocityEstimator>().enabled = false;

                GameObject.Find("System").GetComponent<SystemBoundary>().OnTriggerEnter(attachedPlanet.GetComponent<Collider>());
                Destroy(this.gameObject);
            }
        }
    }
}
