using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SocketScript : MonoBehaviour
{
    public int SocketID;
    bool isMatched = false;
    bool isBeingMatched = false;
    bool isCooldown = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "ContraptionMousePointer" || isCooldown)
            return;
        var mouseAction = InputSystem.GetDevice<Mouse>().leftButton.isPressed ? 1 : 0;
        if (mouseAction > 0)
        {
            var socketSystem = GameObject.FindGameObjectWithTag("SocketsSystem").GetComponent<SocketsSystemScript>();
            if (isBeingMatched || isMatched)
            {
                socketSystem.MatchTrippleOrReset(gameObject);
                StartCoroutine(Cooldown(0.5f));
                return;
            }
            isBeingMatched = true;
            Material matchMaterial = Resources.Load<Material>("Objects/Contraption/SocketMatchingMaterial");
            gameObject.GetComponent<MeshRenderer>().material = matchMaterial;
            StartCoroutine(Cooldown(0.5f));
            socketSystem.RegisterSocket(gameObject);
        }
    }

    public void ResetSocket()
    {
        isBeingMatched = false;
        isMatched = false;
        Material defaultMaterial = Resources.Load<Material>("Objects/Contraption/SocketMaterial");
        Material wrongMaterial = Resources.Load<Material>("Objects/Contraption/SocketWrongMaterial");
        gameObject.GetComponent<MeshRenderer>().material = wrongMaterial;
        StartCoroutine(ResetMaterialAfterDelay(0.5f, defaultMaterial));
    }

    public void ApplyMatching() 
    {
        isMatched = true;
        Material matchedMaterial = Resources.Load<Material>("Objects/Contraption/SocketMatchedMaterial");
        gameObject.GetComponent<MeshRenderer>().material = matchedMaterial;
    }

    System.Collections.IEnumerator Cooldown(float cooldownTimer)
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTimer);
        isCooldown = false;
    }

    System.Collections.IEnumerator ResetMaterialAfterDelay(float delay, Material defaultMaterial)
    {
        isCooldown = true;
        yield return new WaitForSeconds(delay);
        gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
        isCooldown = false;
    }
}
