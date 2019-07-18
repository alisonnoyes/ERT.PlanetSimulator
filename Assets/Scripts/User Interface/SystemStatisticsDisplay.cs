using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemStatisticsDisplay : MonoBehaviour
{
    private GameObject background;

    public float lineSpacing;
    public int planetsPerColumn;
    public float titleTextHeight;
    public float headerTextHeight;
    public float bodyTextHeight;

    private GameObject titleTextHolder;
    private GameObject centralBodyDisplay;
    private List<GameObject> orbitingBodyDisplays = new List<GameObject>();

    public Body centralBody;
    public List<Body> orbitingBodies;

    public GameObject centralBodyDisplayPrefab;
    public GameObject orbitingBodyDisplayPrefab;

    public float scale;

    public void AddBody(Body b)
    {
        if (!orbitingBodies.Contains(b)) orbitingBodies.Add(b);
        if (b.centralBody.GetInstanceID() == b.GetInstanceID()) AddBodyToDisplay(b, true);
        else AddBodyToDisplay(b, false);
    }

    public void RemoveCentralBody(Body b)
    {
        centralBody = null;
        Destroy(centralBodyDisplay);
    }

    public void RemoveBody(Body b)
    {
        int index = orbitingBodies.IndexOf(b);
        orbitingBodies.Remove(b);
        GameObject removed = orbitingBodyDisplays[index];
        orbitingBodyDisplays.RemoveAt(index);
        Destroy(removed);

        for (int i = 0; i < orbitingBodyDisplays.Count; i++)
        {
            GameObject remaining = orbitingBodyDisplays[i];
            remaining.GetComponent<BodyStatistics>().FormatText(
                lineSpacing, headerTextHeight / 2, bodyTextHeight / 2, planetsPerColumn, i
            );
        }
    }

    private void AddBodyToDisplay(Body b, bool isCentral)
    {
        if (isCentral)
        {
            centralBodyDisplay = Instantiate(centralBodyDisplayPrefab);
            CentralBodyStatistics stats = centralBodyDisplay.GetComponent<CentralBodyStatistics>();
            stats.centralBody = b;
            stats.scale = scale;
            centralBodyDisplay.transform.parent = gameObject.transform;

            // Move it based on spacing given by the user
            stats.SetUp();
            stats.FormatText(lineSpacing, headerTextHeight / 2, bodyTextHeight / 2);
        }
        else
        {
            GameObject newDisplay = Instantiate(orbitingBodyDisplayPrefab);
            orbitingBodyDisplays.Add(newDisplay);
            BodyStatistics stats = newDisplay.GetComponent<BodyStatistics>();
            stats.body = b;
            stats.scale = scale;
            newDisplay.transform.parent = gameObject.transform;

            // Move it based on spacing given by the user
            stats.SetUp();
            stats.FormatText(
                lineSpacing, headerTextHeight / 2, bodyTextHeight / 2, planetsPerColumn, orbitingBodyDisplays.Count - 1
            );
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (lineSpacing == 0 || planetsPerColumn == 0)
            Debug.LogError("SystemStatisticsDisplay: Must have non-zero line height and allowed planets per column.");

        if (titleTextHeight == 0 || headerTextHeight == 0 || bodyTextHeight == 0)
            Debug.LogError("SystemStatisticsDisplay: Must have non-zero text sizes.");

        if (centralBody == null)
            Debug.LogError("SystemStatisticsDisplay: Please link a central body to the script.");

        if (centralBodyDisplayPrefab == null || orbitingBodyDisplayPrefab == null)
            Debug.LogError("SystemStatisticsDisplay: Please provide prefabs to use to display statistics.");

        // Orient the title text
        titleTextHolder = transform.Find("Title").gameObject;
        titleTextHolder.GetComponent<TextMesh>().characterSize = titleTextHeight / 2;
        Vector3 titleTextPosition = titleTextHolder.transform.localPosition;
        titleTextPosition.y = 2 * titleTextHeight + lineSpacing;
        titleTextHolder.transform.localPosition = titleTextPosition;

        AddBodyToDisplay(centralBody, true);
    }
}
