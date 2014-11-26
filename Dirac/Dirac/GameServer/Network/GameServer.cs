using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

using Dirac.Logging;

namespace Dirac.GameServer.Network
{
    public class GameServer
    {
        public bool IsListening { get; set; }
        public int Port { get; set; }
        public int ClientCount;

        public static Dictionary<Socket, GameClient> ClientList = new Dictionary<Socket, GameClient>();

        public static Socket Listener;

        public bool Listen(string bindIP, int port)
        {

            if (IsListening) 
                throw new InvalidOperationException("Server is already listening.");

            Listener = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);

            Listener.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            Listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);

            try
            {
                Listener.Bind(new IPEndPoint(IPAddress.Any, port));
                this.Port = port;
            }
            catch (SocketException ex)
            {
                Logging.LogManager.DefaultLogger.Error(String.Format("{0} can not bind on {1}, server shutting down..", this.GetType().Name, bindIP));
                Logging.LogManager.DefaultLogger.Error(ex.Message);
                Logging.LogManager.DefaultLogger.Error(ex.StackTrace);
                //this.Shutdown();
                return false;
            }

            // Start listening for incoming connections.
            Listener.Listen(10);
            IsListening = true;

            // Begin accepting any incoming connections asynchronously.
            Listener.BeginAccept(AcceptCallback, null);

            return true;
        }

        private void AcceptCallback(IAsyncResult result)
        {
            if (Listener == null)
                return;
            try
            {
                Socket workerSocket = Listener.EndAccept(result); // Finish accepting the incoming connection.

                StateObject state = new StateObject();
                state.workSocket = workerSocket;

                GameClient gc = new GameClient();
                gc.Socket = state.workSocket;
                gc.clientobjectstate = state;
                gc.gameserver = this;

                ClientList.Add(state.workSocket, gc);

                state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                     
                Listener.BeginAccept(new AsyncCallback(AcceptCallback), Listener);

                Logging.LogManager.DefaultLogger.Trace("Connection Accepted");

                //workerSocket.BeginReceive(ReceiveCallback, null); // Begin receiving on the new connection connection.
                //Listener.BeginAccept(AcceptCallback, null); // Continue receiving other incoming connection asynchronously.
            }
            catch (Exception ex)
            {
                
                Logging.LogManager.DefaultLogger.Error(ex.Message + "AcceptCallback");
                Logging.LogManager.DefaultLogger.Error(ex.StackTrace);
            }
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;

                state.BytesRecv = state.workSocket.EndReceive(ar);

                Byte[] bufferUtil = state.ResizeBuffer(state.buffer, 0, state.BytesRecv);


                ClientList[state.workSocket].Parse(bufferUtil);

                state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);

            }
            catch(Exception ex)
            {
                ClientList.Clear();
                Type tipe = ex.GetType();
                Logging.LogManager.DefaultLogger.Warn(ex.Message);
                Logging.LogManager.DefaultLogger.Warn(ex.StackTrace);
            }
            //handlerSock.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }


        public GameServer()
        {

        }

        public void Run()
        {
            if (this.Listen("127.0.0.1", 1999)); 
                Logging.LogManager.DefaultLogger.Trace("GameServer is listening on {0}:{1}...", "127.0.0.1", 1999);
        }
        
    }

    public class StateObject
    {
        public int BytesRecv;
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 16*1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];

        public Byte[] ResizeBuffer(Byte[] array, int start, int len)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            Byte[] temp = new Byte[len];

            for (int i = 0; i < len; i++)
            {
                temp[i] = array[i];
            }

            return temp;
        }
        /*public GameServer()
        {
            this.OnConnect += ClientManager.Instance.OnConnect;
            this.OnDisconnect += ClientManager.Instance.OnDisconnect;
            this.DataReceived += GameServer_DataReceived;
            this.DataSent += (sender, e) => { };
        }

        void GameServer_DataReceived(object sender, ConnectionDataEventArgs e)
        {
            var connection = (Connection)e.Connection;
            ((GameClient)connection.Client).Parse(e);
        }*/

        /*public override void Run()
        {
            var bindIP = NetworkingConfig.Instance.EnableIPv6 ? Config.Instance.BindIPv6 : Config.Instance.BindIP;

            if (!this.Listen(bindIP, 1118Config.Instance.Port)) return;
            Logger.Info("Game-Server is listening on {0}:{1}...", bindIP, 1118Config.Instance.Port);
        }*/
    }
}
