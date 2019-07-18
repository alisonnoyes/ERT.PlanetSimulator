using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Body))]
public class BodyCollision : MonoBehaviour
{
    private Collider c;
    private Body b;

    [System.NonSerialized]
    public float scale;
    [System.NonSerialized]
    public double handMass;
    [System.NonSerialized]
    public double elasticity;

    public List<int> layersToCollide = new List<int>();

    private double GetImpulseMagnitude(Body other, Vector3 axis)
    {
        double[] dAxis = DoubleVectorHelper.FloatToDoubleVector(axis);

        double thisSpeed = DoubleVectorHelper.Dot(b.velocity, dAxis);
        double otherSpeed = DoubleVectorHelper.Dot(other.velocity, dAxis);
        
        double reducedMass = (b.mass * other.mass) / (b.mass + other.mass);
        double impulseMag = reducedMass * (1 + elasticity) * (otherSpeed - thisSpeed);

        return impulseMag;
    }
    
    private double GetImpulseMagnitude(GameObject hand, Vector3 axis)
    {
        double[] dAxis = DoubleVectorHelper.FloatToDoubleVector(axis);

        double thisSpeed = DoubleVectorHelper.Dot(b.velocity, dAxis);

        Rigidbody handBody = hand.gameObject.GetComponent<Rigidbody>();
        double[] scaledHandVelocity = { handBody.velocity.x * scale, handBody.velocity.y * scale, handBody.velocity.z * scale };
        double handSpeed = DoubleVectorHelper.Dot(scaledHandVelocity, dAxis);

        double reducedMass = (b.mass * handMass) / (b.mass + handMass);
        double impulseMag = reducedMass * (1 + elasticity) * (handSpeed - thisSpeed);

        return impulseMag;
    }

    private void ApplyImpulse(Vector3 axis, double mag)
    {
        double[] impulse = { axis.x * mag, axis.y * mag, axis.z * mag };
        b.AddImpulse(impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (layersToCollide.Contains(other.gameObject.layer))
        {
            double magnitude;
            Vector3 axis = (gameObject.transform.position - other.gameObject.transform.position).normalized;

            Body otherBody = other.gameObject.GetComponent<Body>();
            // Collision is between two bodies
            if (otherBody != null)
            {
                magnitude = GetImpulseMagnitude(otherBody, axis);
            }
            // Collision is between a body and a hand
            else if (other.tag == "LeftHand" || other.tag == "RightHand")
            {
                magnitude = GetImpulseMagnitude(other.gameObject, axis);
            }
            else return;

            ApplyImpulse(axis, magnitude);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        c = GetComponent<Collider>();
        b = GetComponent<Body>();

        if (c == null || !c.isTrigger)
            Debug.LogError("Collision: GameObject must have a trigger collider component.");

        if (layersToCollide.Count == 0)
        {
            Debug.LogWarning("Collision: No layers were provided to collide with. Defaulting to all layers.");
            layersToCollide.Add(0);
            layersToCollide.Add(1);
            layersToCollide.Add(2);
            layersToCollide.Add(3);
            layersToCollide.Add(4);
            layersToCollide.Add(5);
            layersToCollide.Add(6);
            layersToCollide.Add(7);
            layersToCollide.Add(8);
            layersToCollide.Add(9);
        }
    }
}
