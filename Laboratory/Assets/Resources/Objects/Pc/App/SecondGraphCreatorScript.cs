using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondGraphCreatorScript : MonoBehaviour
{
    List<float> firstXList = new List<float>();
    List<float> firstYList = new List<float>();
    List<float> secondXList = new List<float>();
    List<float> secondYList = new List<float>();
    Vector2 zero;
    double scale;
    double pixelSize = 0.835;
    double xPixels = 5;
    double yPixels = 5;
    double xFirstValue = 50;
    double yFirstValue = 0.05;
    GameObject graphField;
    GameObject dotObj;
    GameObject lineObj;
    void Start()
    {
        graphField = transform.GetChild(2).GetChild(2).gameObject;
        zero = graphField.transform.GetChild(0).transform.localPosition;
        scale = 1.06;
        dotObj = Resources.Load<GameObject>("Objects/Pc/App/Dot");
        lineObj = Resources.Load<GameObject>("Objects/Pc/App/Line");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddLineToFirstGraph(float speed, float powT)
    {
        var x = speed;
        var y = powT - 0.4f;
        firstXList.Add(x);
        firstYList.Add(y);
        var dot = Instantiate(dotObj);
        dot.GetComponent<Renderer>().material.color = Color.red;
        dot.transform.SetParent(graphField.transform, false);
        dot.transform.localPosition = new Vector3((float)(zero.x + x * pixelSize * xPixels / xFirstValue),
            (float)(zero.y + y * scale * pixelSize * yPixels / yFirstValue), -100f);
        if (firstXList.Count > 1)
        {
            var previousDotPosition = new Vector3((float)(zero.x + firstXList[firstXList.Count - 2] * pixelSize * xPixels / xFirstValue),
                (float)(zero.y + firstYList[firstYList.Count - 2] * scale * pixelSize * yPixels / yFirstValue), -100f);
            var line = Instantiate(lineObj);
            line.GetComponent<Renderer>().material.color = Color.red;
            line.transform.SetParent(graphField.transform, false);
            line.transform.localPosition = (dot.transform.localPosition + previousDotPosition) / 2;
            var angleRad = Math.Atan((dot.transform.localPosition.y - previousDotPosition.y) /
                (dot.transform.localPosition.x - previousDotPosition.x));
            var angleDeg = angleRad * Mathf.Rad2Deg;
            line.transform.localRotation = Quaternion.Euler(0, 0, 90 + (float)angleDeg);
            var inaccuracyX = (dot.transform.localPosition.x - previousDotPosition.x) / pixelSize * 0.1;
            var xScale = (float)((dot.transform.localPosition.x - previousDotPosition.x - inaccuracyX) / (pixelSize * 2));
            var inaccuracyY = (dot.transform.localPosition.y - previousDotPosition.y) / (pixelSize * scale) * 0.1;
            var yScale = (float)((dot.transform.localPosition.y - previousDotPosition.y - inaccuracyY) / (pixelSize * scale * 2));
            line.transform.localScale = new Vector3(0.5f, (float)Math.Sqrt(xScale * xScale + yScale * yScale), 0.5f);
        }
    }

    public void AddLineToSecondGraph(float speed, float powE)
    {
        var x = speed;
        var y = powE - 0.4f;
        secondXList.Add(x);
        secondYList.Add(y);
        var dot = Instantiate(dotObj);
        dot.GetComponent<Renderer>().material.color = Color.green;
        dot.transform.SetParent(graphField.transform.GetChild(1).transform, false);
        dot.transform.localPosition = new Vector3((float)(zero.x + x * pixelSize * xPixels / xFirstValue),
            (float)(zero.y + y * scale * pixelSize * yPixels / yFirstValue), -100f);
        if (secondXList.Count > 1)
        {
            var previousDotPosition = new Vector3((float)(zero.x + secondXList[secondXList.Count - 2] * pixelSize * xPixels / xFirstValue),
                (float)(zero.y + secondYList[secondYList.Count - 2] * scale * pixelSize * yPixels / yFirstValue), -100f);
            var line = Instantiate(lineObj);
            line.GetComponent<Renderer>().material.color = Color.green;
            line.transform.SetParent(graphField.transform.GetChild(1).transform, false);
            line.transform.localPosition = (dot.transform.localPosition + previousDotPosition) / 2;
            var angleRad = Math.Atan((dot.transform.localPosition.y - previousDotPosition.y) /
                (dot.transform.localPosition.x - previousDotPosition.x));
            var angleDeg = angleRad * Mathf.Rad2Deg;
            line.transform.localRotation = Quaternion.Euler(0, 0, 90 + (float)angleDeg);
            var inaccuracyX = (dot.transform.localPosition.x - previousDotPosition.x) / pixelSize * 0.1;
            var xScale = (float)((dot.transform.localPosition.x - previousDotPosition.x - inaccuracyX) / (pixelSize * 2));
            var inaccuracyY = (dot.transform.localPosition.y - previousDotPosition.y) / (pixelSize * scale) * 0.1;
            var yScale = (float)((dot.transform.localPosition.y - previousDotPosition.y - inaccuracyY) / (pixelSize * scale * 2));
            line.transform.localScale = new Vector3(0.5f, (float)Math.Sqrt(xScale * xScale + yScale * yScale), 0.5f);
        }
    }

    public void RedrawSecondGraph(List<float> newPowEs)
    {
        secondXList = new List<float>();
        secondYList = new List<float>();
        secondYList.AddRange(newPowEs);
        for (int i = 0; i < graphField.transform.GetChild(1).childCount; i++)
            Destroy(graphField.transform.GetChild(1).GetChild(i).gameObject);
        for (int i = 0; i < firstXList.Count; i++)
        {
            AddLineToSecondGraph(firstXList[i], secondYList[i]);
        }
    }

    public void AddTestLines()
    {
        List<int> firstXTestList = new List<int>();
        List<float> firstYTestList = new List<float>();
        List<int> secondXTestList = new List<int>();
        List<float> secondYTestList = new List<float>();
        for (int x = 0; x < 401; x += 50)
            firstXTestList.Add(x);
        for (float y = 0.4f; y < 1.05; y += 0.2f)
            firstYTestList.Add(y);
        for (int x = 0; x < 401; x += 50)
            secondXTestList.Add(x);
        for (float y = 1; y > 0.35f; y -= 0.05f)
            secondYTestList.Add(y);
        for (int i = 0; i < firstXTestList.Count; i++)
        {
            AddLineToFirstGraph(firstXTestList[i], firstYTestList[i]);
            AddLineToSecondGraph(secondXTestList[i], secondYTestList[i]);
        }
    }       
}
