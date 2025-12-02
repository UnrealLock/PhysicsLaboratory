using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    [SerializeField] private GameObject message;

    public void ShowMessage()
    {
        if (message.activeSelf)
            CloseMessage();
        else
            message.SetActive(true);
    }

    public void CloseMessage() => message.SetActive(false);
}
