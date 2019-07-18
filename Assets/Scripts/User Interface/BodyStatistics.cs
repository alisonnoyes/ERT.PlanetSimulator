using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyStatistics : MonoBehaviour
{
    public Body body;

    private TextMesh title;
    private GameObject editTitle;
    private TextMesh mass;
    private GameObject editMass;
    private TextMesh bodyRadius;
    private GameObject editBodyRadius;
    private TextMesh radius;
    private GameObject editRadius;
    private TextMesh speed;
    private GameObject editSpeed;
    private TextMesh gravitationalForce;
    private TextMesh period;
    private TextMesh perihelion;
    private TextMesh aphelion;
    private TextMesh eccentricity;

    private Color goodColor;
    [SerializeField] [Tooltip("Text will display as this color if the statistics are inaccurate (ie. period statistic when less than one revolution has been completed)")]
    private Color badColor;

    private bool ready = false;

    private const int transformScale = 6;
    private const float xShiftMagnitude = 7;
    private Vector3 leftShift = new Vector3(-1f, 0, 0);

    public float scale;

    public void FormatText(float lineSpacing, float headerTextSize, float bodyTextSize, int maxColCount, float index)
    {
        float totalHeight = transformScale * (lineSpacing * 5 + headerTextSize * 2 + bodyTextSize * 6);
        float heightToStart = totalHeight * (index % maxColCount) + 4 * transformScale * (headerTextSize + lineSpacing) + 4 * lineSpacing * transformScale * (index % maxColCount);
        float xShift = xShiftMagnitude * Mathf.Floor(index / maxColCount);

        title.characterSize = headerTextSize / 2;
        mass.characterSize = bodyTextSize / 2;
        bodyRadius.characterSize = bodyTextSize / 2;
        radius.characterSize = bodyTextSize / 2;
        speed.characterSize = bodyTextSize / 2;
        gravitationalForce.characterSize = bodyTextSize / 2;
        period.characterSize = bodyTextSize / 2;
        perihelion.characterSize = bodyTextSize / 2;
        aphelion.characterSize = bodyTextSize / 2;
        eccentricity.characterSize = bodyTextSize / 2;

        Vector3 adjustedTitlePos = new Vector3(xShift, -heightToStart, 0);
        title.gameObject.transform.localPosition = adjustedTitlePos;
        editTitle.gameObject.transform.localPosition = leftShift;

        Vector3 adjustedBodyPos = Vector3.zero;
        adjustedBodyPos.y -= headerTextSize + lineSpacing + bodyTextSize;
        mass.gameObject.transform.localPosition = adjustedBodyPos;
        editMass.gameObject.transform.localPosition = leftShift;

        adjustedBodyPos.y -= lineSpacing + 2 * bodyTextSize;
        bodyRadius.gameObject.transform.localPosition = adjustedBodyPos;
        editBodyRadius.gameObject.transform.localPosition = leftShift;

        adjustedBodyPos.y -= lineSpacing + 2 * bodyTextSize;
        radius.gameObject.transform.localPosition = adjustedBodyPos;
        editRadius.gameObject.transform.localPosition = leftShift;

        adjustedBodyPos.y -= lineSpacing + 2 * bodyTextSize;
        speed.gameObject.transform.localPosition = adjustedBodyPos;
        editSpeed.gameObject.transform.localPosition = leftShift;

        adjustedBodyPos.y -= lineSpacing + 2 * bodyTextSize;
        gravitationalForce.gameObject.transform.localPosition = adjustedBodyPos;

        adjustedBodyPos.y -= lineSpacing + 2 * bodyTextSize;
        period.gameObject.transform.localPosition = adjustedBodyPos;

        adjustedBodyPos.y -= lineSpacing + 2 * bodyTextSize;
        perihelion.gameObject.transform.localPosition = adjustedBodyPos;

        adjustedBodyPos.y -= lineSpacing + 2 * bodyTextSize;
        aphelion.gameObject.transform.localPosition = adjustedBodyPos;

        adjustedBodyPos.y -= lineSpacing + 2 * bodyTextSize;
        eccentricity.gameObject.transform.localPosition = adjustedBodyPos;
    }

    public void SetUp()
    {
        if (body == null) Debug.LogError("BodyStatistics: Please link the script to a Body object.");

        EditPlanet editor = GameObject.Find("System/SystemEditor").GetComponent<EditPlanet>();

        title = GetComponent<TextMesh>();
        editTitle = title.transform.Find("Edit Title").gameObject;
        BodyButton editTitleButton = editTitle.GetComponent<BodyButton>();
        editTitleButton.receiver = editor;
        editTitleButton.body = body;
        editTitleButton.Start();

        mass = transform.Find("Mass").GetComponent<TextMesh>();
        editMass = mass.transform.Find("Edit Mass").gameObject;
        BodyButton editMassButton = editMass.GetComponent<BodyButton>();
        editMassButton.receiver = editor;
        editMassButton.body = body;
        editMassButton.Start();

        bodyRadius = transform.Find("Body Radius").GetComponent<TextMesh>();
        editBodyRadius = bodyRadius.transform.Find("Edit Body Radius").gameObject;
        BodyButton editBodyRadiusButton = editBodyRadius.GetComponent<BodyButton>();
        editBodyRadiusButton.receiver = editor;
        editBodyRadiusButton.body = body;
        editBodyRadiusButton.Start();

        radius = transform.Find("Orbit Radius").GetComponent<TextMesh>();
        editRadius = radius.transform.Find("Edit Radius").gameObject;
        BodyButton editRadiusButton = editRadius.GetComponent<BodyButton>();
        editRadiusButton.receiver = editor;
        editRadiusButton.body = body;
        editRadiusButton.Start();

        speed = transform.Find("Orbit Speed").GetComponent<TextMesh>();
        editSpeed = speed.transform.Find("Edit Speed").gameObject;
        BodyButton editSpeedButton = editSpeed.GetComponent<BodyButton>();
        editSpeedButton.receiver = editor;
        editSpeedButton.body = body;
        editSpeedButton.Start();

        gravitationalForce = transform.Find("Gravitational Force").GetComponent<TextMesh>();
        period = transform.Find("Orbit Period").GetComponent<TextMesh>();
        perihelion = transform.Find("Perihelion").GetComponent<TextMesh>();
        aphelion = transform.Find("Aphelion").GetComponent<TextMesh>();
        eccentricity = transform.Find("Eccentricity").GetComponent<TextMesh>();

        if (title == null || mass == null || radius == null || speed == null || gravitationalForce == null 
            || period == null || perihelion == null || aphelion == null || eccentricity == null)
            Debug.LogError("BodyStatistics: The required TextMesh components are not present.");

        title.text = body.gameObject.name;

        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);

        goodColor = period.color;
        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready) return;

        if (title.text != body.name) title.text = body.name;

        string prevMassString = mass.text;
        string newMassString = "Mass: " + body.mass.ToString("G3") + " kg";
        if (prevMassString != newMassString) mass.text = newMassString;

        string prevBodyRadString = bodyRadius.text;
        string newBodyRadString = "Body radius: " + body.bodyRadius.ToString("G3") + " m";
        if (prevBodyRadString != newBodyRadString) bodyRadius.text = newBodyRadString;

        string prevRadString = radius.text;
        string newRadString = "Orbit radius: " + body.orbitRadius.ToString("G3") + " m";
        if (prevRadString != newRadString) radius.text = newRadString;

        string prevSpeedString = speed.text;
        string newSpeedString = "Speed: " + body.orbitSpeed.ToString("G3") + " m/s";
        if (prevSpeedString != newSpeedString) speed.text = newSpeedString;

        string prevForceString = gravitationalForce.text;
        string newForceString = "Gravitational force: " + body.forceMagnitude.ToString("G3") + " N";
        if (prevForceString != newForceString) gravitationalForce.text = newForceString;

        string prevPeriodString = period.text;
        string newPeriodString = "Period: " + body.period.ToString("G3");
        if (body.timer.unit == OrbitTimer.TimeUnit.Seconds) newPeriodString += " s";
        else if (body.timer.unit == OrbitTimer.TimeUnit.Minutes) newPeriodString += " mins";
        else if (body.timer.unit == OrbitTimer.TimeUnit.Hours) newPeriodString += " hours";
        else if (body.timer.unit == OrbitTimer.TimeUnit.Days) newPeriodString += " days";
        else newPeriodString += " years";
        if (prevPeriodString != newPeriodString) period.text = newPeriodString;

        string prevPeriString = perihelion.text;
        string newPeriString = "Perihelion: " + body.perihelion.ToString("G3") + " m";
        if (prevPeriString != newPeriString) perihelion.text = newPeriString;

        string prevAphString = aphelion.text;
        string newAphString = "Aphelion: " + body.aphelion.ToString("G3") + " m";
        if (prevAphString != newAphString) aphelion.text = newAphString;

        string prevEccString = eccentricity.text;
        string newEccString = "Eccentricity: " + body.eccentricity.ToString("G3");
        if (prevEccString != newEccString) eccentricity.text = newEccString;

        if (!body.timer.fullRevolution && period.color != badColor)
        {
            period.color = badColor;
            perihelion.color = badColor;
            aphelion.color = badColor;
            eccentricity.color = badColor;
        }
        else if (body.timer.fullRevolution && period.color == badColor)
        {
            period.color = goodColor;
            perihelion.color = goodColor;
            aphelion.color = goodColor;
            eccentricity.color = goodColor;
        }
    }
}
