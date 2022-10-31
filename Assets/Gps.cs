// using System.Collections;
// using System.Collections.Generic;

using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using NmeaParser;
using TMPro;

public class Gps : MonoBehaviour {
    private TcpClient connect = new TcpClient();
    public TMP_Text altText;
    
    void Start() {
        Debug.Log("Started");
        IPAddress ip = IPAddress.Parse("192.168.1.127");
        IPEndPoint endpoint = new IPEndPoint(ip, 6000);
        connect.Connect(endpoint);
    }

    // Update is called once per frame
    void Update() {
        Stream stream = connect.GetStream();
        var device = new StreamDevice(stream);
        device.MessageReceived += device_NmeaMessageReceived;
        device.OpenAsync();
    }
    
    private void device_NmeaMessageReceived(object sender, NmeaMessageReceivedEventArgs args) {
        // called when a message is received
        if(args.Message is NmeaParser.Messages.Gga gga) {
            Debug.Log($"Your current location is: {gga.Latitude} , {gga.Longitude} at height , {gga.Altitude}");
            altText.text = gga.Altitude.ToString();
        }
    }
}
