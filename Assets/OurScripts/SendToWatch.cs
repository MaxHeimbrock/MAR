using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;

#if !WINDOWS_UWP
using System.Net;
using System.Net.Sockets;
using System.Threading;

#else
using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
using Windows.Networking;
using System.Linq;
using System.IO;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.Storage;
#endif

public class SendToWatch : MonoBehaviour
{
    // Narvis
    public static string IPAddress = "192.168.178.57";

    public static int port = 52431;

    public bool autoConnect = false;

#if WINDOWS_UWP
 
    private DatagramSocket socket;

    private int UDPPingReplyLength = Encoding.UTF8.GetBytes("UDPPingReply").Length + 4;

    void initUDPReceiver() {
        Debug.Log("Waiting for a connection...");

        socket = new DatagramSocket();
        /*
        HostName IP = null;
        try {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            IP = Windows.Networking.Connectivity.NetworkInformation.GetHostNames()
            .SingleOrDefault(
                hn =>
                    hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                    == icp.NetworkAdapter.NetworkAdapterId);

            _ = socket.BindEndpointAsync(IP, port.ToString());

            initialized = true;
        }
        catch(Exception e) {
            Debug.Log("Hier gecrasht");
            Debug.Log(e.ToString());
            Debug.Log(SocketError.GetStatus(e.HResult).ToString());
            return;
        }
        */
        Debug.Log("exit start");
    }

    public async void sendUDPMessage(byte[] message) {
        Windows.Networking.HostName hnip = new Windows.Networking.HostName(IPAddress);
        Debug.Log("Send message to IPAddress " + hnip.DisplayName + " on Port " + port.ToString());
        using(var stream = await socket.GetOutputStreamAsync(hnip, port.ToString())) {
            using(var writer = new Windows.Storage.Streams.DataWriter(stream)) {
                writer.WriteBytes(message);
                await writer.StoreAsync();
            }
        }
    }
    public void Start() {    

        Debug.Log("start connection to watch");

        initUDPReceiver();

        sendUDPMessage(Encoding.UTF8.GetBytes("H:0.5"));

        Debug.Log("sent to watch");
    }
    
    void Update() {    

        if (autoConnect)
        {
            //sendUDPMessage(Encoding.UTF8.GetBytes("H:0.5"));
            //sendUDPMessage(Encoding.UTF8.GetBytes("H:-1"));
        }
    }
#endif
}