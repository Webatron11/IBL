// using System.Collections;
// using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Gps : MonoBehaviour
{
    public TcpClient Client;
    // Start is called before the first frame update
    void Start() {
        Debug.Log("Started");
        var ipEndPoint = new IPEndPoint(IPAddress.Parse("10.169.134.229"), 6000);
        using TcpClient client = new();
        client.Connect(ipEndPoint);
    }

    // Update is called once per frame
    void Update() {
        using NetworkStream stream = Client.GetStream();

        var buffer = new byte[1_024];
        int received = stream.Read(buffer);

        var message = Encoding.UTF8.GetString(buffer, 0, received);
        Debug.Log($"Message received: \"{message}\"");
        
        /*IPAddress ip = IPAddress.Parse("10.169.176.229");
        // IPEndPoint ipEndPoint = new IPEndPoint(ip, 6000);

        using TcpClient client = new();
        await client.ConnectAsync(ip, 6000);
        await using NetworkStream stream = client.GetStream();

        var buffer = new byte[1_024];
        int received = await stream.ReadAsync(buffer);

        var message = Encoding.UTF8.GetString(buffer, 0, received);
        Debug.Log($"Message received: \"{message}\"");*/
    }
}
