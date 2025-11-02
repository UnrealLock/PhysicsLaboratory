using UnityEngine;
using UnityEngine.InputSystem;

public class MoveAppScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Camera MainCamera;
    Vector3 offset;
    Renderer quadRenderer;
    public bool IsDragging = false;
    public bool CanBeDriven = true;
    void Start()
    {
        quadRenderer = GetComponent<Renderer>();
        offset = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanBeDriven || !MainCamera.isActiveAndEnabled)
            return;
        var mouseAction = InputSystem.GetDevice<Mouse>().leftButton.isPressed ? 1 : 0;
        if (mouseAction > 0)
            DragWindow(mouseAction);
        else
            IsDragging = false;
    }

    void DragWindow(float mouseAction)
    {
        if (!IsDragging)
            offset = MainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        var radius = quadRenderer.bounds.size.x / 2;
        var distanceMouse = Vector2.Distance(transform.position, MainCamera.ScreenToWorldPoint(Input.mousePosition));
        //Debug.Log(distanceMouse + " " + radius);
        if (distanceMouse <= radius || IsDragging)
        {
            var mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            IsDragging = true;
            transform.position = new Vector3(mousePosition.x - offset.x, mousePosition.y - offset.y
                , transform.position.z);
        }
        if (mouseAction == 0)
            IsDragging = false;
    }
}
