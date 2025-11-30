using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class VoltageControllerScriptllerScript : MonoBehaviour
{
    bool isCooldown = false;
    public double VoltageChangeValue = 0.01;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        var leftMouseAction = InputSystem.GetDevice<Mouse>().leftButton.isPressed ? 1 : 0;
        var rightMouseAction = InputSystem.GetDevice<Mouse>().rightButton.isPressed ? 1 : 0;
        if (collision.gameObject.tag != "ContraptionMousePointer" || isCooldown)
            return;

        var rot = transform.eulerAngles;

        if (leftMouseAction > 0)
        {
            var contraptionZoneData = GameObject.FindGameObjectWithTag("ContraptionZone").GetComponent<ContraptionZoneData>();
            contraptionZoneData.Voltage += VoltageChangeValue;
            if (contraptionZoneData.Voltage > 0.5)
                contraptionZoneData.Voltage = 0.0;

            rot.z += 2f;
            transform.eulerAngles = rot;

            StartCoroutine(Cooldown(0.2f));
        }
        else if (rightMouseAction > 0)
        {
            var contraptionZoneData = GameObject.FindGameObjectWithTag("ContraptionZone").GetComponent<ContraptionZoneData>();
            contraptionZoneData.Voltage -= 1.0;
            if (contraptionZoneData.Voltage < 0.0)
                contraptionZoneData.Voltage = 5.0;

            rot.z -= 2f;
            transform.eulerAngles = rot;

            StartCoroutine(Cooldown(0.2f));
        }
    }

    System.Collections.IEnumerator Cooldown(float cooldownTimer)
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTimer);
        isCooldown = false;
    }
}
