using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SystemWrapper : System.Object
{
    #region SYSTEM PARAMETERS
    public float gravityConstant;
    public float scale;
    public float timeScale;
    public float bodyScale;
    public int timeUnit;
    public float elasticity;
    public float boundaryRadius;
    public int numBodies;
    #endregion

    #region CENTRAL BODY PARAMETERS
    public string centralBodyName;
    public double centralBodyMass;
    public double centralBodyRadius;
    #endregion

    public SystemWrapper(GravitySystem system)
    {
        gravityConstant = system.G;
        scale = system.scale;
        timeScale = system.timeScale;
        bodyScale = system.bodyScale;
        timeUnit = (int)system.timeUnits;
        elasticity = system.elasticity;
        boundaryRadius = system.gameObject.GetComponent<SystemBoundary>().radius;
        numBodies = system.orbitingBodies.Count;

        centralBodyName = system.centralBody.gameObject.name;
        centralBodyMass = system.centralBody.mass;
        centralBodyRadius = system.centralBody.bodyRadius;
    }
}
