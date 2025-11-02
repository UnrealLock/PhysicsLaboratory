using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitchSctipt : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject mousePointer;
    Collider objectCollider;
    public Camera CameraToSwitch;
    Camera mainCamera;
    bool isCameraSwitched = false;

    void Start()
    {
        objectCollider = GetComponent<Collider>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ExitIfPressed();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "MousePointer" || isCameraSwitched)
            return;
        mousePointer = collision.gameObject;
        var mouseAction = InputSystem.GetDevice<Mouse>().leftButton.isPressed ? 1 : 0;
        if (mouseAction > 0)
        {
            CameraToSwitch.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);
            mousePointer.SetActive(false);
            isCameraSwitched = true;
        }
    }

    void ExitIfPressed()
    {
        if (!isCameraSwitched)
            return;
        var mouseAction = InputSystem.GetDevice<Mouse>().rightButton.isPressed ? 1 : 0;
        if (mouseAction > 0)
        {
            mainCamera.gameObject.SetActive(true);
            CameraToSwitch.gameObject.SetActive(false);
            mousePointer.SetActive(true);
            isCameraSwitched = false;
        }
    }
}
