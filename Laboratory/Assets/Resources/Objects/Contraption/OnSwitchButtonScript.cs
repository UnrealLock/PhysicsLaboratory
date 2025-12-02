using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnSwitchButtonScript : MonoBehaviour, IClickable
{
    SocketsSystemScript socketsSystemScript;
    public bool IsActivated = false;
    private bool isCooldown = false;
    private float rotationDelta = 15f;
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
        var renderer = GetComponent<MeshRenderer>();
        var mat = renderer.material;

        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.red * 1f);

        var rot = transform.eulerAngles;
        rot.x -= rotationDelta;
        transform.eulerAngles = rot;

        IsActivated = true;
    }

    void FinishSystem()
    {
        var renderer = GetComponent<MeshRenderer>();
        var mat = renderer.material;

        mat.DisableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.black);

        var rot = transform.eulerAngles;
        rot.x += rotationDelta;
        transform.eulerAngles = rot;

        IsActivated = false;
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    var mouseAction = InputSystem.GetDevice<Mouse>().leftButton.isPressed ? 1 : 0;
    //    if (collision.gameObject.tag != "ContraptionMousePointer" || isCooldown || mouseAction == 0)
    //        return;
    //    if (socketsSystemScript.IsAllMatched && !IsActivated){ 
    //        StartSystem();
    //        StartCoroutine(Cooldown(0.5f));
    //    }
    //    else if (socketsSystemScript.IsAllMatched && IsActivated){
    //        FinishSystem(); 
    //        StartCoroutine(Cooldown(0.5f));
    //    }
    //}

    //private void OnMouseDown()
    //{
    //    if (socketsSystemScript.IsAllMatched && !IsActivated)
    //    {
    //        StartSystem();
    //        StartCoroutine(Cooldown(0.5f));
    //    }
    //    else if (socketsSystemScript.IsAllMatched && IsActivated)
    //    {
    //        FinishSystem();
    //        StartCoroutine(Cooldown(0.5f));
    //    }
    //}

    System.Collections.IEnumerator Cooldown(float cooldownTimer)
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTimer);
        isCooldown = false;
    }

    public void OnLeftClick()
    {
        if (socketsSystemScript.IsAllMatched && !IsActivated)
        {
            StartSystem();
            StartCoroutine(Cooldown(0.5f));
        }
        else if (socketsSystemScript.IsAllMatched && IsActivated)
        {
            FinishSystem();
            StartCoroutine(Cooldown(0.5f));
        }
    }

    public void OnRightClick()
    {
    }
}
