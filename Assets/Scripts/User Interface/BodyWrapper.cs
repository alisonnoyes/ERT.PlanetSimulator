using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BodyWrapper : System.Object
{
    // Stores information about a body to be converted to and from Json

    #region BODY INFORMATION
    public string bodyName;
    public double mass;

    public double bodyRadius;

    public double[] location = new double[3];
    #endregion

    #region ORBIT INFORMATION
    public double orbitRadius;
    public double orbitSpeed;
    public Vector3 orbitDirection;
    #endregion

    public BodyWrapper(Body b)
    {
        bodyName = b.gameObject.name;
        mass = b.mass;

        bodyRadius = b.bodyRadius;

        location = b.location;

        orbitRadius = b.orbitRadius;
        orbitSpeed = b.orbitSpeed;
        orbitDirection = b.orbitDirection;
    }

}
