// using System.Collections;
// using System.Collections.Generic;

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using NmeaParser;
using TMPro;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Gps : MonoBehaviour {
    private readonly TcpClient _connect = new TcpClient();
    private readonly IPEndPoint _endpoint = new IPEndPoint(IPAddress.Parse("192.168.1.127"), 6000);
    public TMP_Text altText;
    public Transform character;
    private double _oldlat;
    private float _newlat;
    private double _oldlong;
    private float _newlong;

    void Start()
    {
        Debug.Log("Started");
        altText.text = "N/A";
        _connect.Connect(_endpoint);
        character.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update() {
        Stream stream = Stream.Null;
        try {
            stream = _connect.GetStream();
        }
        catch (InvalidOperationException) {
            _connect.Connect(_endpoint);
        }
        
        var device = new StreamDevice(stream);
        device.MessageReceived += device_NmeaMessageReceived;
        device.OpenAsync();
        Move();
        altText.ForceMeshUpdate();
    }
    
    private void device_NmeaMessageReceived(object sender, NmeaMessageReceivedEventArgs args) {
        // called when a message is received
        if(args.Message is NmeaParser.Messages.Gga gga) {
            Debug.Log($"Your current location is: {gga.Latitude} , {gga.Longitude} at height , {Math.Round(gga.Altitude)}");
            _oldlat = _newlat;
            _newlat = (float)gga.Latitude;
            _oldlong = _newlong;
            _newlong = (float)gga.Longitude;
            altText.text = $"{Math.Round(gga.Altitude)}";
        }
    }

    private void Move() {
        /*float speed = 1f;
        Vector3 moveDir = new Vector3();
        
        if (Comparator(_newlat, _oldlat) == true) {
            moveDir = Vector3.right;
            //speed = Delta(_newlat, _oldlat);
        }
        if (Comparator(_newlat, _oldlat) == false) {
            moveDir = Vector3.left;
            //speed = Delta(_newlat, _oldlat);
        }
        if (Comparator(_newlong, _oldlong) == true) {
            moveDir = Vector3.up;
            //speed = Delta(_newlong, _oldlong);
        }
        if (Comparator(_newlong, _oldlong) == false) {
            moveDir = Vector3.down;
            //speed = Delta(_newlong, _oldlong);
        }
        if (Comparator(_newlong, _oldlong) == null && Comparator(_newlat, _oldlat) == null) {
            moveDir = Vector3.zero;
            //speed = 0f;
        }
        
        character.position += moveDir.normalized * speed;*/

        Vector2 coords = new Vector2(_newlat, _newlong);
        character.position = coords;
    }

    private static float Delta(double big, double small) {
        return (float)(big - small);
    }
    private bool? Comparator(double in1, double in2) {
        if (in1 > in2) {
            return true;
        } 
        if (in1 < in2) {
            return false;
        }
        return null;
    }
}
