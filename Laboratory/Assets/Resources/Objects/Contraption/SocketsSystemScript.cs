using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SocketsSystemScript : MonoBehaviour
{
    Dictionary<int, List<int>> socketDictionary = new();
    GameObject[] sockets;
    List<int> matchingSockets;
    public bool IsAllMatched = false;

    [Tooltip("Можно указать связи, например: 1-2 1-3 4-5")]
    [TextArea(2, 5)]
    [SerializeField]
    private string connectionsText;

    [SerializeField] private GameObject connectorPrefab;
    [SerializeField] private GameObject wirePrefab;
    [SerializeField] private Material wireMaterial;
    private float wireOffset = 0.25f; //изменяет насколько будет провод изогнут, чем меньше значение тем более прямой
    private float wireThickness = 0.008f; //толщина провода

    private void Start()
    {
        ParseConnections();

        var allSockets = FindObjectsOfType<SocketScript>();

        sockets = new GameObject[14];
        matchingSockets = new List<int>();

        foreach (var socket in allSockets)
        {
            sockets[socket.SocketID] = socket.gameObject;
        }
    }

    private void Update()
    {
        MatchSockets();
        //if (sockets.Contains(null))
        //    IsAllMatched = false;
        //else
        //    IsAllMatched = true;
        IsAllMatched = CheckAllConnectionsMatched();
    }

    public void AutoCreateAllConnections()
    {
        foreach (var pair  in socketDictionary)
        {
            int startID = pair.Key;

            if (startID < 0 || startID >= sockets.Length) continue;
            if (sockets[startID] == null) continue;

            foreach (var endID in pair.Value)
            {
                if (endID <= startID) continue;

                if (endID < 0 || endID >= sockets.Length) continue;
                if (sockets[endID] == null) continue;

                GameObject startSocket = sockets[startID];
                GameObject endSocket = sockets[endID];

                CreateWireConnection(startSocket, endSocket);

                startSocket.GetComponent<SocketScript>().ApplyMatching();
                endSocket.GetComponent<SocketScript>().ApplyMatching();
            }
        }
    }

    public void RegisterSocket(GameObject socket)
    {
        var socketScript = socket.GetComponent<SocketScript>();
        sockets[socketScript.SocketID] = socket;
        matchingSockets.Add(socketScript.SocketID);
    }

    private bool CheckAllConnectionsMatched()
    {
        foreach (var pair in socketDictionary)
        {
            int socketID = pair.Key;
            if (sockets[socketID] == null) return false;

            var socketScript = sockets[socketID].GetComponent<SocketScript>();
            if (!socketScript.IsMatched) return false;
        }

        return true;
    }

    private void MatchSockets()
    {
        if (matchingSockets.Count < 2)
            return;

        GameObject startSocket = sockets[matchingSockets[0]];
        GameObject endSocket = sockets[matchingSockets[1]];

        Outline startSocketOutline = startSocket.GetComponentInChildren<Outline>();
        Outline endSocketOutline = endSocket.GetComponentInChildren<Outline>();

        if (socketDictionary[matchingSockets[0]].Contains(matchingSockets[1]))
        {
            CreateWireConnection(startSocket, endSocket);
            startSocketOutline.StopOutline();
            endSocketOutline.StopOutline();

            sockets[matchingSockets[0]].GetComponent<SocketScript>().ApplyMatching();
            sockets[matchingSockets[1]].GetComponent<SocketScript>().ApplyMatching();
        }
        else if (!startSocket.GetComponent<SocketScript>().IsMatched && !endSocket.GetComponent<SocketScript>().IsMatched)
        {
            startSocketOutline.FlashRed();
            endSocketOutline.FlashRed();

            sockets[matchingSockets[0]].GetComponent<SocketScript>().ResetSocket();
            sockets[matchingSockets[1]].GetComponent<SocketScript>().ResetSocket();
            //sockets[matchingSockets[0]] = null;
            //sockets[matchingSockets[1]] = null;
        }

        matchingSockets.Clear();
    }

    private void ParseConnections()
    {
        if (string.IsNullOrWhiteSpace(connectionsText))
            return;

        var pairs = connectionsText.Split(new char[] { ' ', ',', '\n', '\r', ';' },
                                          System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var pair in pairs)
        {
            var parts = pair.Split(new char[] { '-', ':', '=' },
                                   System.StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2
                && int.TryParse(parts[0], out int socketA)
                && int.TryParse(parts[1], out int socketB))
            {
                AddLink(socketA, socketB);
                AddLink(socketB, socketA);
            }
        }
    }

    private void AddLink(int from, int to)
    {
        if (!socketDictionary.TryGetValue(from, out var list))
        {
            list = new List<int>();
            socketDictionary[from] = list;
        }

        if (!list.Contains(to)) list.Add(to);
    }

    private void CreateWireConnection(GameObject startSocket, GameObject endSocket)
    {
        GameObject startConnector = CreateConnector(startSocket);
        GameObject endConnector = CreateConnector(endSocket);

        SetupConnectorComponents(startConnector, startSocket);
        SetupConnectorComponents(endConnector, endSocket);

        GameObject wire = CreateWireObject(startSocket, endSocket);

        StartWireRender(wire, startConnector, endConnector);
    }

    private GameObject CreateConnector(GameObject socket)
    {
        SocketScript ss = socket.GetComponent<SocketScript>();

        GameObject connector = Instantiate(connectorPrefab, socket.transform.position, Quaternion.identity);
        connector.name = "WireConnector";

        Vector3 directionWorld = socket.transform.TransformDirection(ss.direction.normalized);
        connector.transform.rotation = Quaternion.LookRotation(directionWorld, Vector3.up);
        connector.transform.localScale = connectorPrefab.transform.lossyScale;

        connector.transform.SetParent(socket.transform, true);

        return connector;
    }

    private void SetupConnectorComponents(GameObject connector, GameObject parentSocket)
    {
        SocketScript childrenSocket = connector.GetComponentInChildren<SocketScript>();
        childrenSocket.SocketID = parentSocket.GetComponent<SocketScript>().SocketID;
    }

    private GameObject CreateWireObject(GameObject startSocket, GameObject endSocket)
    {
        GameObject wire = Instantiate(wirePrefab, startSocket.transform);

        var startID = startSocket.GetComponent<SocketScript>().SocketID;
        var endID = endSocket.GetComponent<SocketScript>().SocketID;
        wire.name = $"Wire:{startID}-{endID}";

        LineRenderer lineRenderer = wire.GetComponent<LineRenderer>();
        if (wireMaterial) lineRenderer.material = wireMaterial;
        lineRenderer.startWidth = wireThickness;
        lineRenderer.endWidth = wireThickness;

        return wire;
    }

    private void StartWireRender(GameObject wire, GameObject startConnector, GameObject endConnector)
    {
        LineRenderer lineRenderer = wire.GetComponent<LineRenderer>();

        Vector3 wireExitStartPoint = startConnector.transform.Find("WireIn")?.position ?? startConnector.transform.position;
        Vector3 wireExitEndPoint = endConnector.transform.Find("WireIn")?.position ?? endConnector.transform.position;

        Vector3[] beizerPoints = CalculateBezierPoints(
            wireExitStartPoint,
            wireExitEndPoint,
            startConnector.transform.forward,
            endConnector.transform.forward,
            wireOffset);

        lineRenderer.positionCount = beizerPoints.Length;
        lineRenderer.SetPositions(beizerPoints);
    }

    private Vector3[] CalculateBezierPoints(Vector3 startPos, Vector3 endPos, Vector3 startDir, Vector3 endDir, float offset)
    {
        Vector3 p0 = startPos;
        Vector3 p3 = endPos;
        Vector3 p1 = p0 + startDir * offset;
        Vector3 p2 = p3 + endDir * offset;

        const int SEGMENT_COUNT = 30;
        Vector3[] points = new Vector3[SEGMENT_COUNT];

        for (int i = 0; i < SEGMENT_COUNT; i++)
        {
            float t = i / (SEGMENT_COUNT - 1f);
            points[i] = CalculateBezierPoint(t, p0, p1, p2, p3);
        }

        return points;
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        return (u * u * u) * p0
             + 3 * (u * u) * t * p1
             + 3 * u * (t * t) * p2
             + (t * t * t) * p3;
    }
}