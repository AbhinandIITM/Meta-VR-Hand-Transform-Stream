using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandTree : MonoBehaviour
{
    [System.Serializable]
    public class HandNode
    {
        public string name;
        public Transform transform;
        public List<HandNode> children = new List<HandNode>();

        public HandNode(string name, Transform transform)
        {
            this.name = name;
            this.transform = transform;
        }
    }

    [System.Serializable]
    public class SerializableHandNode
    {
        public string name;
        public double[] localPosition;
        public double[] localRotation;
        public List<SerializableHandNode> children = new List<SerializableHandNode>();

        public SerializableHandNode(HandNode node)
        {
            name = node.name;
            localPosition = Vector3ToArray(node.transform.localPosition);
            localRotation = Vector3ToArray(node.transform.localEulerAngles);

            foreach (var child in node.children)
            {
                children.Add(new SerializableHandNode(child));
            }
        }

        private double[] Vector3ToArray(Vector3 vec)
        {
            return new double[]
            {
                System.Math.Round(vec.x, 3),
                System.Math.Round(vec.y, 3),
                System.Math.Round(vec.z, 3)
            };
        }
    }

    [System.Serializable]
    public class HandPoseRecord
    {
        public double timestamp;
        public SerializableHandNode hand;

        public HandPoseRecord(SerializableHandNode handNode)
        {
            timestamp = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000.0;
            hand = handNode;
        }
    }

    public HandNode rootNode;
    private bool treeBuilt = false;
    private float saveInterval = 1f;
    private float nextSaveTime = 0f;
    private List<HandPoseRecord> poseLog = new List<HandPoseRecord>();

    void Start()
    {
        StartCoroutine(WaitForFirstChildAndBuildTree());
    }

    IEnumerator WaitForFirstChildAndBuildTree()
    {
        while (transform.childCount == 0)
            yield return null;

        yield return new WaitForSeconds(0.05f);

        Transform handRoot = transform.GetChild(0);
        rootNode = BuildHandTree(handRoot);
        treeBuilt = true;

        PrintTree(rootNode, "");
    }

    HandNode BuildHandTree(Transform current)
    {
        HandNode node = new HandNode(current.name, current);
        foreach (Transform child in current)
            node.children.Add(BuildHandTree(child));

        return node;
    }

    void PrintTree(HandNode node, string indent)
    {
        Debug.Log($"{indent}- {node.name}");
        foreach (var child in node.children)
            PrintTree(child, indent + "  ");
    }

    void Update()
    {
        if (treeBuilt && Time.time >= nextSaveTime)
        {
            SaveAndStreamHandPose();
            nextSaveTime = Time.time + saveInterval;
        }
    }

    void SaveAndStreamHandPose()
    {
        if (rootNode == null) return;

        var serializableRoot = new SerializableHandNode(rootNode);
        var record = new HandPoseRecord(serializableRoot);

        // Log to local JSON file
        poseLog.Add(record);
        string jsonLog = JsonHelper.ToJson(poseLog.ToArray(), true);
        string path = Path.Combine(Application.dataPath, "hand_pose_log.json");
        File.WriteAllText(path, jsonLog);

        // Stream latest record over WebSocket
        if (WebSocketClient.Instance != null)
        {
            string jsonRecord = JsonUtility.ToJson(record);
            WebSocketClient.Instance.SendJson(jsonRecord);
        }

        Debug.Log("Hand pose saved & streamed.");
    }
}
