using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContraptionZoneData : MonoBehaviour
{
    public bool IsSystemOn = false;
    public double Voltage = 0.00;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var onSwitchData = transform.GetChild(0).GetChild(0).GetComponent<OnSwitchButtonScript>();
        IsSystemOn = onSwitchData.IsActivated;
    }
}
