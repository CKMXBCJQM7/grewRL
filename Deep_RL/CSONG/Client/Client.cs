﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour
{
    public struct UdpState
    {
        public UdpClient udpClient;
        public IPEndPoint remoteEP;
    }

    string strIP = "127.0.0.1";
    int port = 8000;

    UdpClient udpClient;
    IPEndPoint remoteEP;
    UdpState udpState = new UdpState();

    byte[] data;

    public Transform plane;

    void Start()
    {
        remoteEP = new IPEndPoint(IPAddress.Parse(strIP), port);
        udpClient = new UdpClient(remoteEP);
        udpClient.Client.Blocking = false;
        udpClient.Client.ReceiveTimeout = 1000;

        udpState.udpClient = udpClient;
        udpState.remoteEP = remoteEP;

        //udpClient.Client.Bind(remoteEP);

        //udpClient.BeginReceive(new System.AsyncCallback(ReceiveCallback), udpState); //한번만 받음(비동기)

        while (udpClient.Available > 0)
        {
            // receive bytes
            data = udpClient.Receive(ref remoteEP);
            print("clear data : " + Encoding.Default.GetString(data));
        }

        StartCoroutine(ReceiveData());
    }

    void Update()
    {
        //data = udpClient.Receive(ref remoteEP); //받을때까지 멈춤(동기)
        //print(string.Format("{0} : 수신 : {1}", remoteEP.ToString(), Encoding.Default.GetString(data)));
    }

    private void OnDestroy()
    {
        udpClient.Close();
    }

    IEnumerator ReceiveData()
    {
        while (true)
        {
            try
            {
                if (udpClient.Available > 0)
                {
                    // receive bytes
                    data = udpClient.Receive(ref remoteEP);
                    print(Encoding.Default.GetString(data));
                    string str = Encoding.Default.GetString(data);

                    string[] sp = str.Split('$');

                    Vector3 temp = plane.localScale;

                    for (int i = 0; i < sp.Length; i++)
                    {


                        print(sp[0]);
                        print(sp[1]);

                        
                        temp.x = int.Parse(sp[0]);
                        temp.z = int.Parse(sp[1]);
                        plane.localScale = temp;

                    }


                }
            }
            catch (System.Exception err)
            {
                //print(err.ToString());
            }

            yield return new WaitForEndOfFrame();
        }
    }

    //public void ReceiveCallback(System.IAsyncResult ar)
    //{
    //    UdpClient u = ((UdpState)(ar.AsyncState)).udpClient;
    //    IPEndPoint e = ((UdpState)(ar.AsyncState)).remoteEP;

    //    byte[] receiveBytes = u.EndReceive(ar, ref e);
    //    string receiveString = Encoding.ASCII.GetString(receiveBytes);

    //    print($"Received: {receiveString}");

    //    u.BeginReceive(new System.AsyncCallback(ReceiveCallback), (UdpState)(ar.AsyncState)); //한번만 받고 끝나지 않도록
    //}

    //public void asdf()
    //{
    //    var academyParameters =
    //                new MLAgents.CommunicatorObjects.UnityRLInitializationOutput();
    //    academyParameters.Name = gameObject.name;
    //    academyParameters.Version = kApiVersion;
    //    foreach (var brain in brains)
    //    {
    //        var bp = brain.brainParameters;
    //        academyParameters.BrainParameters.Add(
    //            MLAgents.Batcher.BrainParametersConvertor(
    //                bp,
    //                brain.gameObject.name,
    //                (MLAgents.CommunicatorObjects.BrainTypeProto)
    //                brain.brainType));
    //    }

    //    //academyParameters.EnvironmentParameters =
    //    //    new MLAgents.CommunicatorObjects.EnvironmentParametersProto();
    //    //foreach (var key in resetParameters.Keys)
    //    //{
    //    //    academyParameters.EnvironmentParameters.FloatParameters.Add(
    //    //        key, resetParameters[key]
    //    //    );
    //    //}

    //    marathonAcademy.brainBatcher.SendAcademyParameters(academyParameters);
    //}
}