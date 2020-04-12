using System;
using System.Diagnostics;
using System.Security.Authentication;
using System.Threading;
using WebSocket4Net;

namespace PolygonWeb
{
	public class PolygonWebConnect
	{
		public delegate void OnNoParamsDel();
		public delegate void OnTextParamDel( string Text );
		public delegate void OnBoolParamDel( bool Param );
		public delegate void OnTradeParamDel( Trade TradeRef );
		public delegate void OnQuoteParamDel( Quote QuoteRef );
		public delegate void OnAMinuteParamDel( AMinute AMRef );
		public delegate void OnASecondParamDel( ASecond ASRef );
		public delegate void OnLastTradeParamDel( LastTrade LastTradeRef, string JSONText );
		public delegate void OnLastQuoteParamDel( LastQuote LastQuoteRef, string JSONText );
		public delegate void OnPGStatusParamDel( string Status, string Message );

		#region Variables

		public static DateTime EarliestAggQueryDate = DateTime.ParseExact( "2004-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture );

		public bool IsConnected;
		public bool IsStarted;
		public bool IsReconnect;
		public bool Level1TradesEnabled;
		public bool Level1QuotesEnabled;

		public static bool LoggingEnabled;
		public static bool LogJSONText;

		bool LastEnable;
		WebSocket websocket;

		public string TradesSymbol, QuotesSymbol;
		public static string ApiKey;
		public static string PGonUrl;
		public static string PGonSocketUrl;

		//public static Dispatcher DispatcherRef;

		#endregion

		#region Events

		public event OnNoParamsDel OnWebSocketOpenedEvent;
		public event OnNoParamsDel OnWebSocketClosedEvent;
		public event OnTextParamDel OnWebSocketErrorEvent;
		public event OnTextParamDel OnPGWebReStartEvent;

		public event OnTextParamDel OnWebMessageEvent;
		public event OnTextParamDel OnJSONDataTextEvent;
		public event OnTradeParamDel OnTradeEvent;
		public event OnQuoteParamDel OnQuoteEvent;
		public event OnAMinuteParamDel OnAMinuteEvent;
		public event OnASecondParamDel OnASecondEvent;
		public event OnLastTradeParamDel OnLastTradeEvent;
		public event OnLastQuoteParamDel OnLastQuoteEvent;
		public event OnPGStatusParamDel OnPGStatusEvent;
		public event OnTextParamDel OnExecJsonSecureGetEvent;

		#endregion

		#region Start/Connect

		public void Start( string Message = "Starting Polygon..." )
		{
            //InitPGWebInterface();
            PGonSocketUrl = @"wss://socket.polygon.io/crypto";

            string Url = string.Format(@"{0}/crypto", PGonSocketUrl);
			this.websocket = new WebSocket(PGonSocketUrl, sslProtocols: SslProtocols.Tls);
			//this.websocket = new WebSocket( Url, sslProtocols: SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls );

			this.websocket.Opened += websocket_Opened;
			this.websocket.Error  += websocket_Error;
			this.websocket.Closed += websocket_Closed;
			this.websocket.MessageReceived += websocket_MessageReceived;

			OpenWebSocket();

            Console.ReadKey();
		}

		private void websocket_Opened( object sender, EventArgs e )
		{
			Console.WriteLine( "Connected!" );
            this.websocket.Send("{\"action\":\"auth\",\"params\":\"7Vab5kzOwQC3Sbpz_FJv13AEeM9po_RrC_TC96\"}");
            this.websocket.Send("{\"action\":\"subscribe\",\"params\":\"XA.BTC-USD\"}");

            IsConnected = true;
            OnWebSocketOpenedEvent?.Invoke();

            //new CallbackTimer( 100, FireWebSocketOpenedEvent );
        }

        private void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
		{
			Console.WriteLine( "WebSocket Error" );
			Console.WriteLine( e.Exception.Message );
            OnWebSocketErrorEvent?.Invoke(e.Exception.Message);
        }

		private void websocket_Closed( object sender, EventArgs e )
		{
			Console.WriteLine( "Connection Closed..." );
			OnWebSocketClosedEvent?.Invoke();

			string ClosedMessage = DateTime.Now.ToString();
			IsConnected = true;
            Thread.Sleep(2000);
            this.Start();
            // Reconnect logic...
            IsReconnect = true;
			//DispatcherRef.BeginInvoke( new Action(
			//delegate
			//{
			//	new OneShotTimer( 750, ReStart, ClosedMessage );
			//} ) );
		}

		private void ReStart(object ClosedMessage)
		{
			OnPGWebReStartEvent?.Invoke( ClosedMessage as string );
			OpenWebSocket( "Restarting Polygon..." );

			if ( !IsStarted )
				Start();
		}

		private void OpenWebSocket(string Message = "Starting Polygon...")
		{
			this.websocket.Open();
		}

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        #endregion

    }
}