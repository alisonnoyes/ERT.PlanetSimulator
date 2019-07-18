using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitTimer : MonoBehaviour
{
    [System.NonSerialized]
    public Body body;
    [System.NonSerialized]
    public float orbitTime = 0;
    
    private float timeCounter = 0;

    public bool fullRevolution;
    public int collisionCount = 0;

    public TimeUnit unit = TimeUnit.Seconds;
    public enum TimeUnit
    {
        Seconds,
        Minutes,
        Hours,
        Days,
        Years
    };

    #region COLLIDER METHODS
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() == body.gameObject.GetInstanceID())
        {
            collisionCount++;
            if (collisionCount >= 2) fullRevolution = true;

            float timeInSeconds = timeCounter;

            if (unit == TimeUnit.Seconds) orbitTime = timeInSeconds;
            else if (unit == TimeUnit.Minutes) orbitTime = timeInSeconds / 60;
            else if (unit == TimeUnit.Hours) orbitTime = timeInSeconds / 60 / 60;
            else if (unit == TimeUnit.Days) orbitTime = timeInSeconds / 60 / 60 / 24;
            else orbitTime = timeInSeconds / 60 / 60 / 24 / 365;

            timeCounter = 0;
        }
    }
    #endregion

    #region MONOBEHAVIOUR CALLBACKS
    // Start is called before the first frame update
    void Start()
    {
        if (body == null) Debug.LogError("OrbitTimer: No Body instance to time the orbit of.");
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;
    }
    #endregion
}
