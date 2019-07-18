using UnityEngine;

public class Body : MonoBehaviour
{
    #region BODY INFORMATION
    public double mass;
    public Body centralBody;
    private TextMesh label;

    public double bodyRadius;
    private float scaledBodyRadius;

    public double[] location = new double[3];
    #endregion

    #region ORBIT INFORMATION
    public double orbitRadius;
    private float scaledRadius;

    public double orbitSpeed;
    public Vector3 orbitDirection;

    [System.NonSerialized]
    public OrbitTimer timer;
    [System.NonSerialized]
    public double period;
    [System.NonSerialized]
    public double perihelion;
    private double calcPeri;
    [System.NonSerialized]
    public double aphelion;
    private double calcAph;
    public double eccentricity;
    #endregion

    #region PHYSICS INFORMATION
    [System.NonSerialized]
    public double forceMagnitude;
    [System.NonSerialized]
    public double[] velocity = new double[3];
    private double[] force = new double[3];
    private double[] impulse = new double[3];

    [System.NonSerialized]
    public float scale;
    [System.NonSerialized]
    public float timeScale;
    [System.NonSerialized]
    public float bodyScale = 1;
    #endregion

    private bool ready;
    public bool IsReady() { return ready; }

    private bool IsCentralBody()
    {
        return centralBody.gameObject.name == gameObject.name;
    }

    #region MUTATOR METHODS
    public void SetMass(double mass)
    {
        this.mass = mass;
    }

    public void SetRadius(double radius)
    {
        if (IsCentralBody())
            return;

        orbitRadius = radius;
        scaledRadius = (float)(radius / scale);

        Vector3 currLocation = transform.position;
        Vector3 center = centralBody.transform.position;
        Vector3 direction = (currLocation - center).normalized;

        Vector3 newLocation = center + direction * scaledRadius;
        transform.position = newLocation;
    }

    public void SetSpeed(double speed)
    {
        if (IsCentralBody())
            return;

        orbitSpeed = speed;
        double[] newVelocity = { speed * orbitDirection.x, speed * orbitDirection.y, speed * orbitDirection.z };
        velocity = newVelocity;
    }

    public void AddForce(double[] force)
    {
        if (!ready) return;

        this.force[0] += force[0];
        this.force[1] += force[1];
        this.force[2] += force[2];
    }

    public void AddImpulse(double[] impulse)
    {
        if (!ready) return;

        this.impulse[0] += impulse[0];
        this.impulse[1] += impulse[1];
        this.impulse[2] += impulse[2];
    }

    public void SetBodyRadius(double radius)
    {
        bodyRadius = radius;
        scaledBodyRadius = (float)(radius / scale * bodyScale);

        gameObject.transform.localScale = new Vector3(scaledBodyRadius * 2, scaledBodyRadius * 2, scaledBodyRadius * 2);

        label = transform.Find("Planet Label").GetComponent<TextMesh>();
        if (label == null)
            Debug.LogWarning("Body: There is no TextMesh in the children of " + this.name + ", so no label will display.");
        else
        {
            float labelScale = 0.25f / scaledBodyRadius;
            label.transform.localPosition = new Vector3(0, labelScale / 2, 0);
            label.transform.localScale = new Vector3(labelScale, labelScale, labelScale);
        }
    }

    public void SetName(string name)
    {
        Debug.Log("Setting the name of " + this.name + " to " + name);
        this.name = name;
        transform.Find("Planet Label").GetComponent<TextMesh>().text = name;
    }
    #endregion

    public void SetUp(bool usePrefabVals)
    {
        if (centralBody == null) Debug.LogError("Body.cs: Please provide a central body for " + name + "'s system.");

        if (mass == 0) Debug.LogError("Body.cs: Please provide a non-zero mass for " + name + ".");

        if (orbitRadius == 0 && !IsCentralBody()) Debug.LogWarning("Body.cs: Provided orbit radius for " + name + " is 0.");

        if (bodyRadius == 0) Debug.LogError("Body.cs: Please provide a non-zero body radius for " + name + ".");

        // Make sure this is a direction vector
        orbitDirection = orbitDirection.normalized;

        label = transform.Find("Planet Label").GetComponent<TextMesh>();
        if (label == null)
            Debug.LogWarning("Body: There is no TextMesh in the children of " + this.name + ", so no label will display.");
        else
        {
            float labelScale = 0.25f / (float)(bodyRadius / scale);
            label.transform.localPosition = new Vector3(0, labelScale / 2, 0);
            label.transform.localScale = new Vector3(labelScale, labelScale, labelScale);
            label.text = this.name;
        }
        
        if (usePrefabVals)
        {
            if (location[0] != 0 || location[1] != 0 || location[2] != 0)
            {
                Vector3 norm = DoubleVectorHelper.ToVector3(DoubleVectorHelper.Normalized(location));
                float mag = (float)(DoubleVectorHelper.Magnitude(location) / scale);
                gameObject.transform.position = norm * mag;
            }

            SetMass(mass);
            SetRadius(orbitRadius);
            SetSpeed(orbitSpeed);
            SetBodyRadius(bodyRadius);
        }
        else
        {
            SetMass(mass);
            SetBodyRadius(bodyRadius);

            Vector3 scaledVelocity = gameObject.GetComponent<Rigidbody>().velocity;
            velocity[0] = scaledVelocity.x * scale / timeScale;
            velocity[1] = scaledVelocity.y * scale / timeScale;
            velocity[2] = scaledVelocity.z * scale / timeScale;

            Vector3 worldPos = gameObject.transform.position;
            Vector3 norm = worldPos.normalized;
            double mag = worldPos.magnitude * scale;
            location[0] = norm.x * mag;
            location[1] = norm.y * mag;
            location[2] = norm.z * mag;

            UpdateStatistics();
        }

        if (!IsCentralBody() && timer != null) timer.body = gameObject.GetComponent<Body>();

        ready = true;
    }

    // Determines the statistics to be displayed based on how the GameObject is moving
    private void UpdateStatistics()
    {
        if (IsCentralBody()) return; // Central body state currently does not change

        scaledRadius = Vector3.Distance(transform.position, centralBody.transform.position);
        orbitRadius = scaledRadius * scale;
        // Debug.Log("Velocity of " + name + " at update is " + velocity[0] + ", " + velocity[1] + ", " + velocity[2]);
        orbitDirection = DoubleVectorHelper.ToVector3(DoubleVectorHelper.Normalized(velocity));
        
        orbitSpeed = DoubleVectorHelper.Magnitude(velocity);

        Vector3 direction = (transform.position - centralBody.transform.position).normalized;
        double[] newLocation = { direction.x * orbitRadius, direction.y * orbitRadius, direction.z * orbitRadius };
        location = newLocation;

        double oldTime = period;
        if (timer != null) period = timer.orbitTime * timeScale;
        if (period != oldTime)
        {
            aphelion = calcAph;
            perihelion = calcPeri;

            calcAph = -Mathf.Infinity;
            calcPeri = Mathf.Infinity;
        }

        // Perihelion is the closest point of orbit
        if (orbitRadius < calcPeri) calcPeri = orbitRadius;
        // Aphelion is the farthest point of orbit
        if (orbitRadius > calcAph) calcAph = orbitRadius;

        eccentricity = (aphelion - perihelion) / (aphelion + perihelion);

        forceMagnitude = DoubleVectorHelper.Magnitude(force);
    }

    // Updates the values of the body's velocity and location after the given timestep
    private void TickBody(float dt)
    {
        double[] initVelocity = velocity;

        // Handle impulses
        double[] momentum = { mass * velocity[0], mass * velocity[1], mass * velocity[2] };
        momentum[0] += impulse[0];
        momentum[1] += impulse[1];
        momentum[2] += impulse[2];

        velocity[0] = momentum[0] / mass;
        velocity[1] = momentum[1] / mass;
        velocity[2] = momentum[2] / mass;

        // Handle forces
        velocity[0] += force[0] * dt / mass;
        velocity[1] += force[1] * dt / mass;
        velocity[2] += force[2] * dt / mass;

        // Use the average velocity instead of the new one
        double[] averageVelocity = { (initVelocity[0] + velocity[0]) / 2,
            (initVelocity[1] + velocity[1]) / 2,
            (initVelocity[2] + velocity[2]) / 2 };

        // Use velocity to apply change in position
        location[0] += averageVelocity[0] * dt;
        location[1] += averageVelocity[1] * dt;
        location[2] += averageVelocity[2] * dt;

        Vector3 norm = DoubleVectorHelper.ToVector3(DoubleVectorHelper.Normalized(location));
        float mag = (float)(DoubleVectorHelper.Magnitude(location) / scale);
        gameObject.transform.position = norm * mag;

        force[0] = 0;
        force[1] = 0;
        force[2] = 0;

        impulse[0] = 0;
        force[1] = 0;
        force[2] = 0;
    }

    private void Update()
    {
        if (!ready) return;
        UpdateStatistics();

        if (!IsCentralBody())
        {
            float dt = Time.deltaTime;
            TickBody(dt * timeScale);
        }
    }
}
