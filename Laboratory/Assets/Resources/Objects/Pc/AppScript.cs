using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AppScript : MonoBehaviour
{
    ContraptionZoneData contraptionZoneData;
    // Start is called before the first frame update
    void Start()
    {
        contraptionZoneData = GameObject.FindGameObjectWithTag("ContraptionZone").GetComponent<ContraptionZoneData>();
    }

    // Update is called once per frame
    void Update()
    {
        MonitorVoltage();
    }

    void MonitorVoltage()
    {
        var voltageText = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        var voltageValue = double.Parse(voltageText.text.Replace('.',','));
        if (contraptionZoneData.IsSystemOn && contraptionZoneData.Voltage != voltageValue)
            voltageText.text = contraptionZoneData.Voltage.ToString().Replace(',','.');
    }
}
