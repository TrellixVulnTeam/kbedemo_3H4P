﻿namespace KBEngine
{
	using UnityEngine;
	using System;
	using System.Net.Sockets;
	using System.Net;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Runtime.Remoting.Messaging;

	using MessageID = System.UInt16;
	using MessageLength = System.UInt16;

	/// <summary>
	/// 网络模块
	/// 处理连接、收发数据
	/// </summary>
	public class NetworkInterface
	{
		public delegate void AsyncConnectMethod(ConnectState state);
		public const int TCP_PACKET_MAX = 1460;
		public delegate void ConnectCallback(string ip, int port, bool success, object userData);

		protected Socket _socket = null;
		PacketReceiver _packetReceiver = null;
		PacketSender _packetSender = null;       
		public class ConnectState
		{
			// for connect
			public string connectIP = "";
			public int connectPort = 0;
			public ConnectCallback connectCB = null;
			public object userData = null;
			public Socket socket = null;
			public NetworkInterface networkInterface = null;
			public string error = "";
		}
		
		public NetworkInterface()
		{
			reset();
		}

		~NetworkInterface()
		{
            Util.Log("NetworkInterface::~NetworkInterface(), destructed!!!");
			reset();
		}

		public virtual Socket sock()
		{
			return _socket;
		}
		
		public void reset()
		{
			if(valid())
			{
                Util.Log(string.Format("NetworkInterface::reset(), close socket from '{0}'", _socket.RemoteEndPoint.ToString()));
         	   _socket.Close(0);
			}
			_socket = null;
			_packetReceiver = null;
			_packetSender = null;
		}
		

        public void close()
        {
           if(_socket != null)
			{
				_socket.Close(0);
				_socket = null;
				//Event.fireAll("onDisconnected", new object[]{});
            }

            _socket = null;
        }

		public virtual PacketReceiver packetReceiver()
		{
			return _packetReceiver;
		}
		
		public virtual bool valid()
		{
			return ((_socket != null) && (_socket.Connected == true));
		}
		
		public void _onConnectionState(ConnectState state)
		{
			KBEngine.Event.deregisterIn(this);

			bool success = (state.error == "" && valid());
			if (success)
			{
                Util.Log(string.Format("NetworkInterface::_onConnectionState(), connect to {0} is success!", state.socket.RemoteEndPoint.ToString()));
				_packetReceiver = new PacketReceiver(this);
				_packetReceiver.startRecv();
			}
			else
			{
                Util.LogError(string.Format("NetworkInterface::_onConnectionState(), connect is error! ip: {0}:{1}, err: {2}", state.connectIP, state.connectPort, state.error));
			}

                Util.Client.CallMethod("Event", "Brocast", new object[] { "onConnectionState", success });

			if (state.connectCB != null)
				state.connectCB(state.connectIP, state.connectPort, success, state.userData);
		}

		private static void connectCB(IAsyncResult ar)
		{
			ConnectState state = null;
			
			try 
			{
				// Retrieve the socket from the state object.
				state = (ConnectState) ar.AsyncState;

				// Complete the connection.
				state.socket.EndConnect(ar);

				Event.fireIn("_onConnectionState", new object[] { state });
			} 
			catch (Exception e) 
			{
				state.error = e.ToString();
				Event.fireIn("_onConnectionState", new object[] { state });
			}
		}

		/// <summary>
		/// 在非主线程执行：连接服务器
		/// </summary>
		private void _asyncConnect(ConnectState state)
		{
            Util.Log(string.Format("NetWorkInterface::_asyncConnect(), will connect to '{0}:{1}' ...", state.connectIP, state.connectPort));
			try
			{
				state.socket.Connect(state.connectIP, state.connectPort);
			}
			catch (Exception e)
			{
                Util.LogError(string.Format("NetWorkInterface::_asyncConnect(), connect to '{0}:{1}' fault! error = '{2}'", state.connectIP, state.connectPort, e));
				state.error = e.ToString();
			}
		}

		/// <summary>
		/// 在非主线程执行：连接服务器结果回调
		/// </summary>
		private void _asyncConnectCB(IAsyncResult ar)
		{
			ConnectState state = (ConnectState)ar.AsyncState;
			AsyncResult result = (AsyncResult)ar;
			AsyncConnectMethod caller = (AsyncConnectMethod)result.AsyncDelegate;

            Util.Log(string.Format("NetWorkInterface::_asyncConnectCB(), connect to '{0}:{1}' finish. error = '{2}'", state.connectIP, state.connectPort, state.error));

			// Call EndInvoke to retrieve the results.
			caller.EndInvoke(ar);
			Event.fireIn("_onConnectionState", new object[] { state });
		}

		public void connectTo(string ip, int port, ConnectCallback callback, object userData)
		{
			if (valid())
				throw new InvalidOperationException("Have already connected!");

			if (!(new Regex(@"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))")).IsMatch(ip))
			{
				IPHostEntry ipHost = Dns.GetHostEntry(ip);
				ip = ipHost.AddressList[0].ToString();
			}

			// Security.PrefetchSocketPolicy(ip, 843);
            IPAddress[] hostAddresses = Dns.GetHostAddresses(ip);
            IPAddress[] outIPs = hostAddresses;
            AddressFamily addressFamily = AddressFamily.InterNetwork;
            if (Socket.OSSupportsIPv6 && this.IsHaveIpV6Address(hostAddresses, ref outIPs))
            {
                addressFamily = AddressFamily.InterNetworkV6;
            }
            _socket = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, NetworkInterface.TCP_PACKET_MAX * 2);
            _socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, SocketOptionName.SendBuffer, NetworkInterface.TCP_PACKET_MAX * 2);
			_socket.NoDelay = true;
			//_socket.Blocking = false;

			ConnectState state = new ConnectState();
			state.connectIP = ip;
			state.connectPort = port;
			state.connectCB = callback;
			state.userData = userData;
			state.socket = _socket;
			state.networkInterface = this;

            Util.Log("connect to " + ip + ":" + port + " ...");

			// 先注册一个事件回调，该事件在当前线程触发
			Event.registerIn("_onConnectionState", this, "_onConnectionState");
			var v = new AsyncConnectMethod(this._asyncConnect);
			v.BeginInvoke(state, new AsyncCallback(this._asyncConnectCB), state);
		}

        private bool IsHaveIpV6Address(IPAddress[] IPs, ref IPAddress[] outIPs)
        {
            int length = 0;
            for (int index = 0; index < IPs.Length; ++index)
            {
                if (AddressFamily.InterNetworkV6.Equals((object)IPs[index].AddressFamily))
                    ++length;
            }
            if (length <= 0)
                return false;
            outIPs = new IPAddress[length];
            int num = 0;
            for (int index = 0; index < IPs.Length; ++index)
            {
                if (AddressFamily.InterNetworkV6.Equals((object)IPs[index].AddressFamily))
                    outIPs[num++] = IPs[index];
            }
            return true;
        }


		public bool send(MemoryStream stream)
		{
			if (!valid())
			{
				throw new ArgumentException("invalid socket!");
			}

			if (_packetSender == null)
				_packetSender = new PacketSender(this);

			return _packetSender.send(stream);
		}

		public void process()
		{
			if (!valid())
				return;

			if (_packetReceiver != null)
				_packetReceiver.process();
		}
	}
}
