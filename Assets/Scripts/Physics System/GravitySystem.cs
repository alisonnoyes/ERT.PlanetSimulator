using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GravitySystem : MonoBehaviour
{
    public Body centralBody;
    public List<Body> orbitingBodies;

    public float G = 1;

    [Tooltip("Real world meters / scale = Unity meters")]
    public float scale;
    public float timeScale;
    public float bodyScale;
    public OrbitTimer.TimeUnit timeUnits;
    public float elasticity;
    [Tooltip("For calculating the impulse caused by collisions")]
    public double handMass;

    public void ClearAll()
    {
        Debug.Log("Clearing the system...");
        SystemStatisticsDisplay stats = GameObject.Find("System Statistics").GetComponent<SystemStatisticsDisplay>();

        if (centralBody != null)
        {
            stats.RemoveCentralBody(centralBody);
            Destroy(centralBody.gameObject);
            centralBody = null;
        }

        foreach (Body b in orbitingBodies)
        {
            stats.RemoveBody(b);
            Destroy(b.timer.gameObject);
            Destroy(b.gameObject);
        }

        orbitingBodies.Clear();
    }

    public void AddBody(Body b)
    {
        orbitingBodies.Add(b);
        b.timeScale = timeScale;
        b.gameObject.layer = 0;

        BodyCollision collision = b.gameObject.GetComponent<BodyCollision>();
        if (collision == null)
        {
            Debug.Log("Body " + b.name + " does not have a BodyCollision component, so it will not register any collisions.");
            return;
        }

        collision.elasticity = elasticity;
        collision.handMass = handMass;
        collision.scale = scale;
        collision.layersToCollide.Clear();
        // Once it enters the system, the body should be able to collide with hands or any other Body
        collision.layersToCollide.Add(0);
        collision.layersToCollide.Add(1);
        collision.layersToCollide.Add(2);
        collision.layersToCollide.Add(3);
        collision.layersToCollide.Add(4);
        collision.layersToCollide.Add(5);
        collision.layersToCollide.Add(6);
        collision.layersToCollide.Add(7);
        collision.layersToCollide.Add(8);
        collision.layersToCollide.Add(9);
    }

    public void RemoveBody(Body b)
    {
        orbitingBodies.Remove(b);
    }

    private double[] GetForceOn(Body b, Body central)
    {
        double distance = b.orbitRadius;
        double forceMagnitude = G * b.mass * central.mass / Math.Pow(distance, 2);
        Vector3 forceDirection = (central.transform.position - b.transform.position).normalized;

        double[] force = { forceDirection.x * forceMagnitude, forceDirection.y * forceMagnitude, forceDirection.z * forceMagnitude };
        return force;
    }

    private void ApplyForces()
    {
        foreach (Body b in orbitingBodies)
        {
            if (b == null) continue;

            double[] force = GetForceOn(b, centralBody);
            b.AddForce(force);
            if (b.timeScale != timeScale)
            {
                b.timer.collisionCount = 0;
                b.timeScale = timeScale;
            }
            b.timer.unit = timeUnits;
        }
    }

    public void ScaleBodies()
    {
        centralBody.bodyScale = bodyScale;
        centralBody.SetBodyRadius(centralBody.bodyRadius);

        foreach (Body b in orbitingBodies)
        {
            if (b == null) continue;

            b.bodyScale = bodyScale;
            b.SetBodyRadius(b.bodyRadius);
        }
    }

    #region MONOBEHAVIOUR CALLBACKS
    // Start is called before the first frame update
    void Start()
    {
        if (centralBody == null)
        {
            Debug.LogError("GravitySystem: Please provide a central body.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ApplyForces();
    }
    #endregion
}
