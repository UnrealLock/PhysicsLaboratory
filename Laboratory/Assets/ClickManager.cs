using UnityEngine;

public interface IClickable
{
    void OnLeftClick();
    void OnRightClick();
}

public class ClickManager : MonoBehaviour
{
    private static ClickManager instance;
    public static ClickManager Instance => instance;

    private Camera currentActiveCamera;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SetActiveCamera(Camera.main);
    }

    public void SetActiveCamera(Camera newCamera)
    {
        if (newCamera == null || newCamera == currentActiveCamera) return;

        currentActiveCamera = newCamera;
    }

    public void FindAndSetActiveCamera()
    {
        Camera activeCam = null;
        Camera[] cameras = FindObjectsOfType<Camera>();

        foreach (Camera cam in cameras)
        {
            if (cam.gameObject.activeInHierarchy && cam.enabled)
            {
                activeCam = cam;
                break;
            }
        }

        if (activeCam != null)
        {
            SetActiveCamera(activeCam);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray ray = currentActiveCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                IClickable clickable = hit.collider.GetComponent<IClickable>();
                if (clickable != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        clickable.OnLeftClick();
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        clickable.OnRightClick();
                    }
                }
            }
        }
    }
}
