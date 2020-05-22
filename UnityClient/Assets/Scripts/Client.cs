using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class Client : MonoBehaviour
{
    [SerializeField]
    private string clientIp = "127.0.0.1";
    [SerializeField]
    private int clientPort = 54000;

    int bufferSize = 256;
    UdpClient udpClient;
    IPEndPoint ipEP;
    string message;
    byte[] data;

    [SerializeField]
    public Vector3 object_translation = new Vector3();

    [SerializeField]
    public Quaternion object_rotation;

    // Start is called before the first frame update
    void Start()
    {
        udpClient = new UdpClient(clientPort);
        data = new byte[bufferSize];
        ipEP = new IPEndPoint(IPAddress.Parse(clientIp), clientPort);
    }

    // Update is called once per frame
    void Update()
    {
        if (udpClient.Available > 0)
        {
            data = udpClient.Receive(ref ipEP);
            message = Encoding.ASCII.GetString(data, 0, data.Length);
            Debug.Log("Received " + message);
            Message2UpdateObjTranslationRotation(message);
            MoveObject(object_translation, object_rotation);
        }
        else
        {
            Debug.Log("No message received from server");
        }  
    }

    private void Message2UpdateObjTranslationRotation(string input)
    {
        string[] buffer;
        buffer = input.Split(',');
        object_translation.x = (float)Convert.ToDouble(buffer[0]);
        object_translation.y = (float)Convert.ToDouble(buffer[1]);
        object_translation.z = (float)Convert.ToDouble(buffer[2]);

        object_rotation.z = -(float)Convert.ToDouble(buffer[3]);
        object_rotation.x = -(float)Convert.ToDouble(buffer[4]);
        object_rotation.y = -(float)Convert.ToDouble(buffer[5]);
        object_rotation.w = (float)Convert.ToDouble(buffer[6]);
    }

    private void MoveObject(Vector3 translation, Quaternion rotation)
    {
        transform.position = translation;
        transform.rotation = rotation; ;
    }
}
