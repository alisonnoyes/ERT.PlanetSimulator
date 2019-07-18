using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralBodyStatistics : MonoBehaviour
{
    public Body centralBody;

    private TextMesh title;
    private GameObject editTitle;
    private TextMesh mass;
    private GameObject editMass;
    private TextMesh bodyRadius;
    private GameObject editBodyRadius;

    private bool ready = false;
    private Vector3 leftShift = new Vector3(-1f, 0, 0);

    public float scale;

    public void FormatText(float lineSpacing, float headerTextSize, float bodyTextSize)
    {
        title.characterSize = headerTextSize / 2;
        mass.characterSize = bodyTextSize / 2;
        bodyRadius.characterSize = bodyTextSize / 2;

        title.gameObject.transform.localPosition = new Vector3(0, -headerTextSize - lineSpacing, 0);
        editTitle.gameObject.transform.localPosition = leftShift;

        Vector3 adjustedPosition = title.gameObject.transform.localPosition;
        adjustedPosition.y = -(headerTextSize + lineSpacing + bodyTextSize / 2);
        mass.gameObject.transform.localPosition = adjustedPosition;
        editMass.gameObject.transform.localPosition = leftShift;

        adjustedPosition.y -= lineSpacing + 2 * bodyTextSize;
        bodyRadius.gameObject.transform.localPosition = adjustedPosition;
        editBodyRadius.gameObject.transform.localPosition = leftShift;
    }

    public void SetUp()
    {
        if (centralBody == null)
            Debug.LogError("CentralBodyStatistics: Please link the script to a Body object.");

        EditPlanet editor = GameObject.Find("System/SystemEditor").GetComponent<EditPlanet>();

        title = GetComponent<TextMesh>();
        editTitle = title.transform.Find("Edit Title").gameObject;
        BodyButton editTitleButton = editTitle.GetComponent<BodyButton>();
        editTitleButton.receiver = editor;
        editTitleButton.body = centralBody;
        editTitleButton.Start();

        mass = transform.Find("Mass").gameObject.GetComponent<TextMesh>();
        editMass = mass.transform.Find("Edit Mass").gameObject;
        BodyButton editMassButton = editMass.GetComponent<BodyButton>();
        editMassButton.receiver = editor;
        editMassButton.body = centralBody;
        editMassButton.Start();

        bodyRadius = transform.Find("Body Radius").gameObject.GetComponent<TextMesh>();
        editBodyRadius = bodyRadius.transform.Find("Edit Body Radius").gameObject;
        BodyButton editBodyRadiusButton = editBodyRadius.GetComponent<BodyButton>();
        editBodyRadiusButton.receiver = editor;
        editBodyRadiusButton.body = centralBody;
        editBodyRadiusButton.Start();

        if (title == null || mass == null || bodyRadius == null)
            Debug.LogError("CentralBodyStatistics: A TextMesh for the title, mass, and body radius are required.");

        title.text = centralBody.gameObject.name;

        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);

        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready) return;

        if (title.text != centralBody.name) title.text = centralBody.name;

        string prevText = mass.text;
        string newText = "Mass: " + centralBody.mass.ToString("G3") + " kg";

        string prevRadText = bodyRadius.text;
        string newRadText = "Body radius: " + centralBody.bodyRadius.ToString("G3") + " m";

        // Only set the text if the value has changed
        if (newText != prevText) mass.text = newText;
        if (newRadText != prevRadText) bodyRadius.text = newRadText;
    }
}
