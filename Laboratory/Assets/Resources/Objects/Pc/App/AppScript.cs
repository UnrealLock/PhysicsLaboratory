using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AppScript : MonoBehaviour
{
    ContraptionZoneData contraptionZoneData;
    GameObject graphicPage;
    GameObject resultPage;
    GraphCreatorScript graphCreatorScript;
    TabletCeatorScript tabletCeatorScript;
    float currentVoltage;
    float currentAmperage;
    // Start is called before the first frame update
    void Start()
    {
        contraptionZoneData = GameObject.FindGameObjectWithTag("ContraptionZone").GetComponent<ContraptionZoneData>();
        graphicPage = gameObject.transform.GetChild(1).gameObject;
        resultPage = gameObject.transform.GetChild(2).gameObject;
        graphCreatorScript = gameObject.transform.GetComponent<GraphCreatorScript>();
        tabletCeatorScript = gameObject.transform.GetComponent<TabletCeatorScript>();
        currentAmperage = 0f;
        currentVoltage = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        MonitorVoltage();
    }

    void MonitorVoltage()
    {
        var voltageText = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        var amperageText = transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>();
        var voltageValue = double.Parse(voltageText.text.Replace('.',','));
        var amperageValue = double.Parse(amperageText.text.Replace('.', ','));
        if (contraptionZoneData.IsSystemOn && (System.Math.Abs(contraptionZoneData.Voltage - voltageValue) > 0.001 || 
            amperageValue < 0.1))
        {
            voltageValue = contraptionZoneData.Voltage;
            currentVoltage = (float)voltageValue;
            if (voltageValue < 0.01)
                voltageText.text = string.Concat(voltageValue.ToString(), ".00");
            else
                voltageText.text = contraptionZoneData.Voltage.ToString().Replace(',', '.');
            var k = 0.95;
            var T = 1950;
            amperageValue = 200 * k / 
                (System.Math.Exp((1.48 * System.Math.Pow(contraptionZoneData.Voltage, 0.5) - 3.1 * 0.0001 * T) /
                (8.63 * 0.00001 * T)) + 1) - 10 * k;
            if (amperageValue >= 100)
                amperageValue = System.Math.Round(amperageValue);
            else
                amperageValue = System.Math.Round(amperageValue);
            currentAmperage = (float)amperageValue;
            amperageText.text = amperageValue.ToString().Replace(',', '.');
        }
    }

    public void AddData()
    {
        graphCreatorScript.AddLineToGraph(currentVoltage, currentAmperage);
        tabletCeatorScript.FillFirstTabletRow(currentVoltage, currentAmperage);
        tabletCeatorScript.FillSecondTabletRow(currentVoltage);
    }

    public void ShowResultPage()
    {
        resultPage.SetActive(true);
        graphicPage.SetActive(false);
    }

    public void ShowGraphicPage()
    {
        graphicPage.SetActive(true);
        resultPage.SetActive(false);
    }
}
