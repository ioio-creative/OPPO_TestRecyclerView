using System.Net.Sockets;

namespace TestRecyclerView
{
    public class Server
    {
        private const string ServerIP = "127.0.0.1";
        private const int ServerPort = 10086;

        private ServerThread st;
        private bool isWaitingForConnection;

        //private Timer timer = new Timer();

        public Server()
        {
            //timer.Elapsed += OnTimedEvent;
            //timer.Interval = 2000;
            //timer.AutoReset = true;
            //timer.Enabled = true;
        }

        public void StartServer()
        {
            //開始連線，設定使用網路、串流、TCP
            st = new ServerThread(AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp, ServerIP, ServerPort);
            st.Listen();//讓Server socket開始監聽連線
            st.StartConnect();//開啟Server socket
            isWaitingForConnection = true;          
        }

        public void Send(string msg)
        {
            if (st.IsConnected)
            {
                isWaitingForConnection = false;
                st.Send(msg + "[/TCP]");
            }
            else
            {
                if (!isWaitingForConnection)
                {
                    st.StopConnect();
                    st.StartConnect();
                    isWaitingForConnection = true;
                }
            }
        }

        //private void OnTimedEvent(object sender, ElapsedEventArgs e)
        //{
        //    Send("Hi");
        //}
    }
}