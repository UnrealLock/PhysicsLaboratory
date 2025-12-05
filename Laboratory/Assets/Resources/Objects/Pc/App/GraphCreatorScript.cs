using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraphCreatorScript : MonoBehaviour
{
    List<double> testXList = new List<double>() { 0, 0.17, 0.25, 0.3, 0.34, 0.38, 0.4, 0.41, 0.42, 0.44, 0.46, 0.48, 0.51, 0.54, 0.56, 0.58, 0.62, 0.64, 0.66, 0.68 };
    List<double> testYList = new List<double>() { 182, 170, 155, 140, 124, 110, 100, 95, 90, 85, 80, 70, 58, 50, 38, 30, 25, 20, 15, 10 };
    List<double> xList = new List<double>();
    List<double> yList = new List<double>();
    Vector2 zero;
    double scale;
    double pixelSize = 0.871;
    double xPixels = 10;
    double yPixels = 4;
    double xFirstValue = 0.1;
    double yFirstValue = 20;
    GameObject graphField;
    GameObject dotObj;
    GameObject lineObj;
    void Start()
    {
        graphField = transform.GetChild(1).GetChild(7).gameObject;
        zero = graphField.transform.GetChild(0).transform.localPosition;
        scale = graphField.GetComponent<RectTransform>().rect.size.x / graphField.GetComponent<RectTransform>().rect.size.y + 0.01;
        dotObj = Resources.Load<GameObject>("Objects/Pc/App/Dot");
        lineObj = Resources.Load<GameObject>("Objects/Pc/App/Line");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddLineToGraph(float voltage, float amperage)
    {
        var x = System.Math.Sqrt(voltage);
        var y = amperage;
        xList.Add(x);
        yList.Add(y);
        var dot = Instantiate(dotObj);
        dot.transform.SetParent(graphField.transform, false);
        dot.transform.localPosition = new Vector3((float)(zero.x + x * pixelSize * xPixels / xFirstValue),
<<<<<<< HEAD
            (float)(zero.y + y * scale * pixelSize * yPixels / yFirstValue), -0.5f);
        if (xList.Count > 1)
        {
            var previousDotPosition = new Vector3((float)(zero.x + xList[xList.Count - 2] * pixelSize * xPixels / xFirstValue),
                (float)(zero.y + yList[yList.Count - 2] * scale * pixelSize * yPixels / yFirstValue), -0.5f);
=======
            (float)(zero.y + y * scale * pixelSize * yPixels / yFirstValue), -100f);
        if (xList.Count > 1)
        {
            var previousDotPosition = new Vector3((float)(zero.x + xList[xList.Count - 2] * pixelSize * xPixels / xFirstValue),
                (float)(zero.y + yList[yList.Count - 2] * scale * pixelSize * yPixels / yFirstValue), -100f);
>>>>>>> Pc-and-Contraption
            var line = Instantiate(lineObj);
            line.transform.SetParent(graphField.transform, false);
            line.transform.localPosition = (dot.transform.localPosition + previousDotPosition) / 2;
            var angleRad = Math.Atan((dot.transform.localPosition.y - previousDotPosition.y) /
                (dot.transform.localPosition.x - previousDotPosition.x));
            var angleDeg = angleRad * Mathf.Rad2Deg;
            line.transform.localRotation = Quaternion.Euler(0, 0, 90 + (float)angleDeg);
            var inaccuracyX = (dot.transform.localPosition.x - previousDotPosition.x) / pixelSize * 0.1;
            var xScale = (float)((dot.transform.localPosition.x - previousDotPosition.x - inaccuracyX) / (pixelSize * 2));
            var inaccuracyY = (dot.transform.localPosition.y - previousDotPosition.y) / (pixelSize * scale) * 0.1;
            var yScale = (float)((dot.transform.localPosition.y - previousDotPosition.y + inaccuracyY) / (pixelSize * scale * 2));
            line.transform.localScale = new Vector3(0.5f, (float)Math.Sqrt(xScale * xScale + yScale * yScale), 0.5f);
        }
    }

    public void AddLineToGraphTest()
    {
        for (int i = 0; i < testXList.Count; i++)
        {
            var x = testXList[i];
            var y = testYList[i];
            xList.Add(x);
            yList.Add(y);
            var dot = Instantiate(dotObj);
            dot.transform.SetParent(graphField.transform, false);
            dot.transform.localPosition = new Vector3((float)(zero.x + x * pixelSize * xPixels / xFirstValue),
                (float)(zero.y + y * scale * pixelSize * yPixels / yFirstValue), -0.5f);
            if (xList.Count > 1)
            {
                var previousDotPosition = new Vector3((float)(zero.x + xList[xList.Count - 2] * pixelSize * xPixels / xFirstValue),
                    (float)(zero.y + yList[yList.Count - 2] * scale * pixelSize * yPixels / yFirstValue), -0.5f);
                var line = Instantiate(lineObj);
                line.transform.SetParent(graphField.transform, false);
                line.transform.localPosition = (dot.transform.localPosition + previousDotPosition) / 2;
                var angleRad = Math.Atan((dot.transform.localPosition.y - previousDotPosition.y) /
                    (dot.transform.localPosition.x - previousDotPosition.x));
                var angleDeg = angleRad * Mathf.Rad2Deg;
                line.transform.localRotation = Quaternion.Euler(0,0,90 + (float)angleDeg);
                var inaccuracyX = (dot.transform.localPosition.x - previousDotPosition.x) / pixelSize * 0.1;
                var xScale = (float)((dot.transform.localPosition.x - previousDotPosition.x - inaccuracyX) / (pixelSize * 2));
                var inaccuracyY = (dot.transform.localPosition.y - previousDotPosition.y) / (pixelSize * scale) * 0.1;
                var yScale = (float)((dot.transform.localPosition.y - previousDotPosition.y + inaccuracyY) / (pixelSize * scale * 2));
                line.transform.localScale = new Vector3(0.5f, (float)Math.Sqrt(xScale * xScale + yScale * yScale) ,0.5f);
            }
        }
    }
}
