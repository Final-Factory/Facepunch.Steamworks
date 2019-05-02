﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Steamworks.Data;

namespace Steamworks
{
	public static class SteamNetworkingSockets
	{
		static ISteamNetworkingSockets _internal;
		internal static ISteamNetworkingSockets Internal
		{
			get
			{
				if ( _internal == null )
				{
					_internal = new ISteamNetworkingSockets();
					_internal.InitClient();
				}

				return _internal;
			}
		}

		internal static void Shutdown()
		{
			_internal = null;
		}

		internal static void InstallEvents()
		{
			SteamNetConnectionStatusChangedCallback_t.Install( x => OnConnectionStatusChanged( x ) );
		}

		private static void OnConnectionStatusChanged( SteamNetConnectionStatusChangedCallback_t data )
		{
			Console.WriteLine( $"data.Conn: {data.Conn.ToString()}" );
			Console.WriteLine( $"data.Conn.UserData: {data.Conn.UserData}" );
			Console.WriteLine( $"data.Conn.ConnectionName: {data.Conn.ConnectionName}" );

			Console.WriteLine( $"States: {data.Nfo.state} {data.OldState}" );
		}

		/// <summary>
		/// Creates a "server" socket that listens for clients to connect to by calling
		/// Connect, over ordinary UDP (IPv4 or IPv6)
		/// </summary>
		public static Socket CreateNormalSocket( NetworkAddress address )
		{
			return Internal.CreateListenSocketIP( ref address );
		}

		/// <summary>
		/// Connect to a socket created via <method>CreateListenSocketIP</method>
		/// </summary>
		public static NetConnection ConnectNormal( NetworkAddress address )
		{
			return Internal.ConnectByIPAddress( ref address );
		}

		/// <summary>
		/// Creates a server that will be relayed via Valve's network (hiding the IP and improving ping)
		/// </summary>
		public static Socket CreateRelaySocket( int virtualport = 0 )
		{
			return Internal.CreateListenSocketP2P( virtualport );
		}

		/// <summary>
		/// Connect to a relay server
		/// </summary>
		public static NetConnection ConnectRelay( SteamId serverId, int virtualport = 0 )
		{
			NetworkIdentity identity = serverId;
			return Internal.ConnectP2P( ref identity, virtualport );
		}
	}
}