using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabletCeatorScript : MonoBehaviour
{
    GameObject firstTablet;
    GameObject firstTabletRowPrefab;
    GameObject firstTabletContent;
    GameObject firstTabletStartRow;
    GameObject secondTablet;
    GameObject secondTabletRowPrefab;
    GameObject secondTabletContent;
    GameObject secondTabletStartRow;
    List<float[]> firstTabletDataRows = new List<float[]>();
    List<float[]> secondTabletDataRows = new List<float[]>();
    ContraptionZoneData contraptionZoneData;
    SecondGraphCreatorScript secondGraphCreatorScript;
    int rowCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        firstTabletRowPrefab = Resources.Load<GameObject>("Objects/Pc/App/TextRow");
        firstTablet = this.transform.GetChild(2).GetChild(0).gameObject;
        firstTabletContent = firstTablet.transform.GetChild(0).GetChild(0).gameObject;
        firstTabletStartRow = firstTabletContent.transform.GetChild(0).gameObject;
        secondTabletRowPrefab = Resources.Load<GameObject>("Objects/Pc/App/SecondTextRow");
        secondTablet = this.transform.GetChild(2).GetChild(1).gameObject;
        secondTabletContent = secondTablet.transform.GetChild(0).GetChild(0).gameObject;
        secondTabletStartRow = secondTabletContent.transform.GetChild(0).gameObject;
        contraptionZoneData = GameObject.FindGameObjectWithTag("ContraptionZone").GetComponent<ContraptionZoneData>();
        secondGraphCreatorScript = transform.GetComponent<SecondGraphCreatorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillFirstTabletRow(float voltage, float amperage)
    {
        var derivative = 0f;
        firstTabletDataRows.Add(new float[4] { voltage, (float)System.Math.Round(System.Math.Sqrt(voltage),2),
            amperage, derivative });
        if (rowCount > 1)
        {
            var newRow = Instantiate(firstTabletRowPrefab, firstTabletContent.transform);
            newRow.transform.SetParent(firstTabletContent.transform);
            newRow.transform.localScale = new Vector3(1, 1, 1);
            newRow.transform.localPosition = firstTabletStartRow.transform.localPosition + new Vector3(0, -3.14f * (rowCount - 2), 0);
            var dataRow = firstTabletDataRows[firstTabletDataRows.Count - 2];
            derivative = System.Math.Abs(
                firstTabletDataRows[firstTabletDataRows.Count - 1][2] - firstTabletDataRows[firstTabletDataRows.Count - 3][2]) /
                (firstTabletDataRows[firstTabletDataRows.Count - 1][1] - firstTabletDataRows[firstTabletDataRows.Count - 3][1]);
            derivative = (float)System.Math.Round(derivative, 2);
            for (int i = 0; i < dataRow.Length - 1; i++)
                newRow.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = dataRow[i].ToString();
            newRow.transform.GetChild(dataRow.Length - 1).GetComponent<TextMeshProUGUI>().text = derivative.ToString();
            firstTabletDataRows[firstTabletDataRows.Count - 2][3] = derivative;
        }
    }

    public void FillSecondTabletRow(float voltage)
    {
        var velocity = System.Math.Round(592.7 * System.Math.Sqrt(voltage), 3);
        var powerTheor = velocity * velocity / System.Math.Pow(contraptionZoneData.T, 3/2) * 
            System.Math.Exp(-3.3 / 100 * velocity * velocity / contraptionZoneData.T);
        powerTheor = System.Math.Round(powerTheor, 3);
        secondTabletDataRows.Add(new float[3] { (float)velocity, (float)powerTheor, 0f });
        if (rowCount > 1)
        {
            var newRow = Instantiate(secondTabletRowPrefab, secondTabletContent.transform);
            newRow.transform.SetParent(secondTabletContent.transform);
            newRow.transform.localScale = new Vector3(1, 1, 1);
            newRow.transform.localPosition = secondTabletStartRow.transform.localPosition + new Vector3(0, -3.14f * (rowCount - 2), 0);
            var dataRow = secondTabletDataRows[secondTabletDataRows.Count - 2];
            for (int i = 0; i < dataRow.Length - 1; i++)
                newRow.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = dataRow[i].ToString();
            RefillPowerExpRow();
            secondGraphCreatorScript.AddLineToFirstGraph(dataRow[0], dataRow[1]);
            RedrawPowerExpGraph();
        }
        rowCount++;
    }

    void RefillPowerExpRow()
    {
        var maxDerivative = 0f;
        for (int i = 1; i < firstTabletDataRows.Count - 1; i++) 
        {
            if (firstTabletDataRows[i][3] > maxDerivative)
                maxDerivative = firstTabletDataRows[i][3];
        }
        for (int i = 3; i < secondTabletContent.transform.childCount; i++)
        {
            secondTabletDataRows[i - 2][2] = (float)System.Math.Round(firstTabletDataRows[i - 2][3] / maxDerivative, 3);
            secondTabletContent.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text =
                secondTabletDataRows[i - 2][2].ToString();
        }
    }

    void RedrawPowerExpGraph()
    {
        var powExpList = new List<float>();
        for (int i = 1; i < secondTabletDataRows.Count - 1; i++)
            powExpList.Add(secondTabletDataRows[i][2]);
        secondGraphCreatorScript.RedrawSecondGraph(powExpList);
    }

    public void FillTabletTestData()
    {
        for (int i = 0; i < 20; i++)
        {
            if (rowCount == 0)
            {
                foreach (Transform child in firstTabletStartRow.transform)
                    child.GetComponent<TextMeshProUGUI>().text = i.ToString();
            }
            else
            {
                var newRow = Instantiate(firstTabletRowPrefab, firstTabletContent.transform);
                newRow.transform.SetParent(firstTabletContent.transform);
                newRow.transform.localScale = new Vector3(1, 1, 1);
                newRow.transform.localPosition = firstTabletStartRow.transform.localPosition + new Vector3(0, -3.14f * rowCount, 0);
                foreach (Transform child in newRow.transform)
                    child.GetComponent<TextMeshProUGUI>().text = i.ToString();
            }
            rowCount++;
        }
    }
}
