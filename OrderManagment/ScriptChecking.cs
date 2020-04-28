using System;
using System.Diagnostics;

namespace OrderManagment
{
    public class ScriptChecking
    {
        public void CheckProcesses()
        {
            string webSocket = "WebSocketManagment";

            Process[] websocketScript = Process.GetProcessesByName(webSocket);

            if (websocketScript.Length == 0)
            {
                try
                {
                    //Process.Start(@"C:\Users\Administrator\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\WebSocketManagment\WebSocketManagment.appref-ms");
                    Process.Start(@"C:\Users\root\AppData\Local\Apps\2.0\01A52EXV.C97\DBMGT1Z8.M2R\webs..tion_43b27b8b112fa857_0001.0000_3ec0995c18142a66\WebSocketManagment.exe");
                    //Process.Start(@"C:\Users\HP\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\WebSocketManagment\WebSocketManagment.appref-ms");
                    Console.WriteLine(webSocket + " script started.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured while starting Websocket Script...   " + ex.Message);
                }
            }
        }
}
}
