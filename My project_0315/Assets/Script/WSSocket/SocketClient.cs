using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;               //C# ���� �� ��Ĺ�� �����ϴ� ���̺귯��
using System.Text;
using Newtonsoft.Json;              //JSON �� ����ϱ����� ���̺귯�� 

public class MyData
{
    public string clientID;                     //�������� ���� �ؼ� Ŭ���̾�Ʈ�� ���ӽ� ��
    public string message;
    public int requestType;                     // ��û ��ȣ json�� ����
}

public class SocketClient : MonoBehaviour
{
    private WebSocket webSocket;
    private bool isConnected = false;
    private int connectionAttempt = 0;              // ���� �õ� Ƚ�� 
    private const int maxConnectionAttempts = 3;    // �ִ� ���� �õ� Ƚ��

    MyData sendData = new MyData { message = "�޼��� ����" };

    // Start is called before the first frame update
    void Start()
    {
        ConnectWebSocekt();
    }

    void ConnectWebSocekt()
    {
        webSocket = new WebSocket("ws://localhost:8000");           //localhost 127.0.0.1 port : 8000 , ws => websocket
        webSocket.OnOpen += OnWebSocketOpen;                        //�� ��Ĺ�� ���� �Ǿ��� �� �̺�Ʈ�� �߻����Ѽ� �Լ��� ���� ��Ų��. 
        webSocket.OnMessage += OnWebSocketMessage;                  //�� ��Ĺ �޼����� ���� �� �̺�Ʈ�� �߻����� Message �Լ��� ���� ��Ų��.
        webSocket.OnClose += OnWebSocketClose;                      //�� ��Ĺ ������ ���������� �̺�Ʈ�� �߻����� Close �Լ��� ���� ��Ų��. 

        webSocket.ConnectAsync();
    }

    void OnWebSocketOpen(object sender, System.EventArgs e)         //�� ��Ĺ�� ���µǰ� ���� �Ǿ��� �� 
    {
        Debug.Log("WebSocket connected");
        isConnected = true;
        connectionAttempt = 0;
    }

    void OnWebSocketMessage(object sender, MessageEventArgs e)      //�� ��Ĺ�� ������� Message�� ���� �� 
    {
        string jsonData = Encoding.Default.GetString(e.RawData);    //MessageEventArgs�� ���� RawData�� Json���� ���ڵ� �Ѵ�. 
        Debug.Log("Received JSON data : " + jsonData);

        MyData receivedData = JsonConvert.DeserializeObject<MyData>(jsonData);          //JSON �����͸� ��ü�� ������ȭ

        if (receivedData != null && !string.IsNullOrEmpty(receivedData.clientID))        //receivedData ���� ��� ���� ���� ��
        {
            sendData.clientID = receivedData.clientID;                                  //�������� �޾ƿ� ID ���� MyData�� �ִ´�. 
        }

    }

    void OnWebSocketClose(object sender, CloseEventArgs e)              //�� ��Ĺ ������ ������ ��
    {
        Debug.Log("WebSocket connection closed");
        isConnected = false;                                            //���� ���� flag 

        if (connectionAttempt < maxConnectionAttempts)                   //�� 3���� �õ� 
        {
            connectionAttempt++;
            Debug.Log("Attempting to reconnect. Attempt : " + connectionAttempt);
            ConnectWebSocekt();                                                         //Connect �õ��� �Ѵ�.
        }
        else
        {
            Debug.Log("Failed to connect ");
        }
    }

    void OnApplicationQuit()                        //���α׷� ����ÿ� ȣ�� �Ǵ� �Լ� 
    {
        DisconnectWebSocket();
    }

    void DisconnectWebSocket()                      //����� socket�� Relese ���ش�. 
    {
        if (webSocket != null && isConnected)
        {
            webSocket.Close();
            isConnected = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (webSocket == null || !isConnected)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            sendData.requestType = 0;
            string jsonData = JsonConvert.SerializeObject(sendData);                //Mydata �� Json ���� ������� 

            webSocket.Send(jsonData);

        }
    }
}