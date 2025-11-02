using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SocketsSystemScript : MonoBehaviour
{
    Dictionary<int, List<int>> socketDictionary;
    GameObject[] sockets;
    List<int> matchingSockets;
    bool isAllMatched = false;
    bool isMatchingInProgress = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        socketDictionary = new Dictionary<int, List<int>>() {
            { 0, new List<int>(){13} }, { 13, new List<int>(){0} }, { 1, new List<int>(){6} }, { 6, new List<int>(){1, 10} },
            { 2, new List<int>(){7} }, { 7, new List<int>(){2, 11}}, { 3, new List<int>(){9} }, { 9, new List<int>(){3} },
            { 4, new List<int>(){8} }, { 8, new List<int>(){4} }, { 5, new List<int>(){12} }, { 12, new List<int>(){5} },
            { 10, new List<int>(){6}  }, { 11, new List<int>(){7} }};
        sockets = new GameObject[14];
        matchingSockets = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        MatchSockets();
    }

    public void RegisterSocket(GameObject socket)
    {
        var socketScript = socket.GetComponent<SocketScript>();
        sockets[socketScript.SocketID] = socket;
        matchingSockets.Add(socketScript.SocketID);
    }

    void MatchSockets()
    {
        if (matchingSockets.Count < 2)
            return;
        if (socketDictionary[matchingSockets[0]].Contains(matchingSockets[1]))
        {
            sockets[matchingSockets[0]].GetComponent<SocketScript>().ApplyMatching();
            sockets[matchingSockets[1]].GetComponent<SocketScript>().ApplyMatching();
        }
        else
        {
            sockets[matchingSockets[0]].GetComponent<SocketScript>().ResetSocket();
            sockets[matchingSockets[1]].GetComponent<SocketScript>().ResetSocket();
            sockets[matchingSockets[0]] = null;
            sockets[matchingSockets[1]] = null;
        }
        matchingSockets.Clear();
    }

    public void MatchTrippleOrReset(GameObject socket)
    {
        if (matchingSockets.Count < 1 || matchingSockets[0] == socket.GetComponent<SocketScript>().SocketID)
        {
            ResetMatched(socket);
            return;
        }
        MatchTripple(socket);
    }

    void ResetMatched(GameObject socket)
    {
        var socketScript = socket.GetComponent<SocketScript>();
        var connectedWithIds = socketDictionary[socketScript.SocketID];
        socketScript.ResetSocket();
        sockets[socketScript.SocketID] = null;
        foreach (var connectedId in connectedWithIds)
        {
            if (sockets[connectedId] == null)
                continue;
            ResetMatched(sockets[connectedId]);
        }
    }

    void MatchTripple(GameObject socketMatchWith)
    {
        var socket = matchingSockets[0];
        var socketScript = sockets[socket].GetComponent<SocketScript>();
        var matchedWithId = socketMatchWith.GetComponent<SocketScript>().SocketID;
        if (!socketDictionary[matchedWithId].Contains(socketScript.SocketID))
        {
            socketScript.ResetSocket();
            sockets[socket] = null;
            ResetMatched(sockets[matchedWithId]);
            matchingSockets.Clear();
            return;
        }
        socketScript.ApplyMatching();
        matchingSockets.Clear();
    }
}
