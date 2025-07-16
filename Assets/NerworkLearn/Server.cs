using LC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Server : MonoBehaviour
{
    List<Socket> m_ConncetSockets = new List<Socket>();
    private const string m_ServerIP = "127.0.0.1";
    private const int m_ServerEndPoint = 8080;

    private Socket m_ServerSocket;

    private void Start()
    {
        //���������
        m_ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_ServerSocket.Bind(new IPEndPoint(IPAddress.Parse(m_ServerIP), m_ServerEndPoint));
        m_ServerSocket.Listen(1024);

        //ʹ���̴߳�������
        Thread accept_thread = new Thread(AcceptClient);
        accept_thread.Start();

        Thread receive_thread = new Thread(ReceiveMsg);
        receive_thread.Start();
    }

    private void Update()
    {

    }
    private void AcceptClient()
    {
        while(true)
        {
            Socket client_socket = m_ServerSocket.Accept();
            m_ConncetSockets.Add(client_socket);
        }
    }

    private void ReceiveMsg()
    {
        Socket socket;
        byte[] buffer = new byte[1024];
        int length;
        while(true)
        {
            for(int i = 0; i < m_ConncetSockets.Count; i++)
            {
                socket = m_ConncetSockets[i];
                if (socket != null && socket.Available > 0)
                {
                    //����ͻ��˷�������Ϣ
                    length = socket.Receive(buffer);
                    ThreadPool.QueueUserWorkItem(HandleMsg, (socket, buffer, length));  //���̳߳�ȡ�̴߳���Msg
                }
            }
        }
    }

    private void HandleMsg(object msg)
    {
        (Socket socket, byte[] buffer, int length) info = ((Socket socket, byte[] buffer, int length))msg;
        Debug.Log(string.Format("Server : ���յ����� {0} ����Ϣ:{1}"
            , info.socket.RemoteEndPoint
            , Encoding.UTF8.GetString(info.buffer, 0, info.length)));
    }

}
