﻿using System;
using System.Net.Sockets;
using System.IO;

namespace Hazzik.Net {
	public abstract class ClientBase {
		protected int _headerSize;
		protected Socket _socket;
		private Stream _stream;

		public ClientBase(int headerSize, Socket socket) {
			_socket = socket;
			_headerSize = headerSize;
		}

		public abstract byte[] ReadPacket();
		public abstract int PacketCode(byte[] header);
		public abstract int PacketSize(byte[] header);
		public abstract void ProcessData(byte[] data, int offset, int length);

		public virtual void Start() {
			try {
				byte[] data;
				while(true) {
					data = ReadPacket();
					ProcessData(data, 0, data.Length);
				}
			} catch(SocketException) {
			} catch(Exception e) {
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
			_socket.Close();
		}

		public virtual Stream GetStream() {
			if(_stream == null) {
				_stream = new NetworkStream(_socket, false);
			}
			return _stream;
		}

		/*
		public virtual void Send(IPacket packet) {
			var stream = packet.GetStream();
			var buffer = new byte[65536];
			var n = 0;
			while(n < packet.Size) {
				//stream.Read(buffer, n, 256);
				//_socket.Send (buffer ,n ,
			}
		}
		 */ 
	}
}
				  