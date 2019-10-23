﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace TestRecyclerView
{
    public class ServerThread
    {
        //結構，儲存IP和Port
        private struct Struct_Internet
        {
            public string ip;
            public int port;
        }

        private Socket serverSocket;//伺服器本身的Socket
        private Socket clientSocket;//連線使用的Socket
        private Struct_Internet internet;//宣告結構物件
        public string receiveMessage;
        private string sendMessage;

        private Thread threadConnect;//連線的Thread
        private Thread threadReceive;//接收資料的Thread

        private readonly Func<Socket> SocketFactory;

        public bool IsConnected
        {
            get
            {
                if (clientSocket == null || !clientSocket.Connected)
                {
                    return false;
                }

                try
                {
                    return !(clientSocket.Poll(1, SelectMode.SelectRead) && clientSocket.Available == 0);
                }
                catch (SocketException)
                {
                    return false;
                }
            }
        }

        public ServerThread(AddressFamily family, SocketType socketType, ProtocolType protocolType, string ip, int port)
        {
            SocketFactory = () => new Socket(family, socketType, protocolType);//new server socket object
            internet.ip = ip;//儲存IP
            internet.port = port;//儲存Port
            receiveMessage = null;//初始化接受的資料            
        }

        //開始連線
        public void StartConnect()
        {
            serverSocket = SocketFactory();

            //讓Server socket開始監聽連線
            //伺服器本身的IP和Port
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(internet.ip), internet.port));
            serverSocket.Listen(1);//最多一次接受多少人連線

            //由於連線成功之前程式都會停下，所以必須使用Thread
            threadConnect = new Thread(Accept);
            threadConnect.IsBackground = true;//設定為背景執行續，當程式關閉時會自動結束
            threadConnect.Start();
        }

        //停止連線
        public void StopConnect()
        {
            try
            {
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }

                // added by Chris
                serverSocket.Close();
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }

        //寄送訊息
        public void Send(string message)
        {
            if (message == null)
            {
                throw new NullReferenceException("message不可為Null");
            }
            else
            {
                sendMessage = message;
            }
            SendMessage();//由於資料傳遞速度很快，沒必要使用Thread
        }

        public void Receive()
        {
            //先判斷原先的threadReceive若還在執行接收檔案的工作，則直接結束
            if (threadReceive != null && threadReceive.IsAlive)
            {
                return;
            }
            //由於在接收到所有資料前都會停下，所以必須使用Thread
            threadReceive = new Thread(ReceiveMessage);
            threadReceive.IsBackground = true;//設定為背景執行續，當程式關閉時會自動結束
            threadReceive.Start();
        }

        private void Accept()
        {
            try
            {
                clientSocket = serverSocket.Accept();//等到連線成功後才會往下執行
                Log("Connected!");

                //連線成功後，若是不想再接受其他連線，可以關閉serverSocket
                //serverSocket.Close();
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }

        private void SendMessage()
        {
            try
            {
                if (clientSocket.Connected)//若成功連線才傳遞資料
                {
                    //將資料進行編碼並轉為Byte後傳遞
                    clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }

        private void ReceiveMessage()
        {
            try
            {
                if (clientSocket.Connected == true)
                {
                    byte[] bytes = new byte[256];//用來儲存傳遞過來的資料
                    long dataLength = clientSocket.Receive(bytes);//資料接收完畢之前都會停在這邊
                    //dataLength為傳遞過來的"資料長度"

                    receiveMessage = Encoding.ASCII.GetString(bytes);//將傳過來的資料解碼並儲存
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }

        private void Log(string str)
        {
            System.Diagnostics.Debug.WriteLine(str);
        }
    }
}