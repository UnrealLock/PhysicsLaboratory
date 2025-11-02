using UnityEngine;

public class MousePointerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LayerMask mask;
    public Camera cameraToUse;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var boxCollider = gameObject.GetComponent<BoxCollider>();
        if (!cameraToUse.isActiveAndEnabled)
        {
            if (boxCollider.enabled)
                gameObject.GetComponent<BoxCollider>().enabled = false;
            return;
        }
        if (!boxCollider.enabled)
            boxCollider.enabled = true;
        var ray = cameraToUse.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, mask))
            transform.position = raycastHit.point;
    }
}
