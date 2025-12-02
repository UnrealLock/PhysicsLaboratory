using UnityEngine;
using UnityEngine.InputSystem;

public class SocketScript : MonoBehaviour
{
    public int SocketID;
    [Tooltip("¬ каком направлении должен быть направлен коннектор")]
    public Vector3 direction = Vector3.forward;

    private bool isMatched = false;
    public bool IsMatched { get => isMatched; }

    private Outline outline;
    private bool isBeingMatched = false;

    public void ApplyMatching() => isMatched = true;

    public void ResetSocket()
    {
        isBeingMatched = false;
        isMatched = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "ContraptionMousePointer") return;

        var mouseAction = InputSystem.GetDevice<Mouse>().leftButton.isPressed ? 1 : 0;
        if (mouseAction > 0)
        {
            outline = GetComponentInChildren<Outline>();
            var socketSystem = GameObject.FindGameObjectWithTag("SocketsSystem").GetComponent<SocketsSystemScript>();

            if (isBeingMatched || isMatched || outline.IsOutlining) return;

            isBeingMatched = true;
            outline.StartOutline();
            socketSystem.RegisterSocket(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.005f); //ќтображает в какой точке по€витс€ коннектор
        Gizmos.DrawRay(transform.position, transform.TransformDirection(direction) * 0.2f); //ќтображает в какую сторону будет выходить провод
    }
}
