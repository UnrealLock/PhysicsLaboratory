using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContraptionZoneData : MonoBehaviour
{
    public bool IsSystemOn = false;
    public double Voltage = 0.00;
    public double k;
    public double T;
    // Start is called before the first frame update
    void Start()
    {
        var random = new System.Random();
        k = (double)random.Next(900, 1101) / 1000;
        T = random.Next(1855, 2086);
    }

    // Update is called once per frame
    void Update()
    {
        //var onSwitchData = transform.GetChild(0).GetChild(0).GetComponent<OnSwitchButtonScript>();
        var onSwitchData = transform.GetComponentInChildren<OnSwitchButtonScript>();
        IsSystemOn = onSwitchData.IsActivated;
    }
}
