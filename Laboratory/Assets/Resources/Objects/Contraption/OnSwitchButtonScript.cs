using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnSwitchButtonScript : MonoBehaviour
{
    SocketsSystemScript socketsSystemScript;
    public bool IsActivated = false;
    bool isCooldown = false;
    // Start is called before the first frame update
    void Start()
    {
        socketsSystemScript = GameObject.FindGameObjectWithTag("SocketsSystem").transform.GetComponent<SocketsSystemScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartSystem()
    {
        transform.GetComponent<MeshRenderer>().material.color = Color.green;
        IsActivated = true;
    }

    void FinishSystem()
    {
        transform.GetComponent<MeshRenderer>().material.color = Color.red;
        IsActivated = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        var mouseAction = InputSystem.GetDevice<Mouse>().leftButton.isPressed ? 1 : 0;
        if (collision.gameObject.tag != "ContraptionMousePointer" || isCooldown || mouseAction == 0)
            return;
        if (socketsSystemScript.IsAllMatched && !IsActivated){ 
            StartSystem();
            StartCoroutine(Cooldown(0.5f));
        }
        else{
            FinishSystem(); 
            StartCoroutine(Cooldown(0.5f));
        }
    }
    System.Collections.IEnumerator Cooldown(float cooldownTimer)
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTimer);
        isCooldown = false;
    }
}
