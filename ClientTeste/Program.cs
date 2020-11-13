using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ClientTeste
{
    class Program
    {
        public static List<object> messages = new List<object>();
        //url de conexão
        public static string url = "https://azuretwitchsignalr.azurewebsites.net/api";
        //public static string url = "http://localhost:7071/api/";

        public static int id = (new Random()).Next();

        static async Task Main(string[] args)
        {
            try
            {
                var firstInput = Console.ReadLine();

                switch (firstInput)
                {
                    case "0":
                        await testClientAsync();
                        break;
                }

                // declara variaveis 
                HubConnection connection;

                //passa as diretrizes da conexão 
                connection = new HubConnectionBuilder().WithUrl(url).WithAutomaticReconnect().Build();

                // faz os contratos para receber as info em tempo real 

                //contrato para receber quando o servidor fizer logout deste cliente 
                connection.On<object>("newMessage", (msg) => newMessage(msg));

                //inicia a conexão com o servidor 
                await connection.StartAsync();

                while (true)
                {
                    sendMessage(Console.ReadLine());
                }
            }
            catch (Exception except)
            {
                Console.WriteLine(except.Message + "\t\r\n");
            }
        }

        public static void newMessage(object message)
        {
            Console.Clear();

            var obj = new { sender = "", text = "" };

            obj = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(message.ToString(), obj);

            messages.Add(message);

            foreach (object item in messages)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public static async Task testClientAsync()
        {
            HttpClient http = new HttpClient();

            var response = await http.GetAsync(url + "/Function?name=MatWeb");
        }

        public static async void sendMessage(string msg)
        {
            HttpClient http = new HttpClient();

            var response = await http.PostAsJsonAsync(url + "/messages", new { sender = id.ToString(), text = "id enviou"+msg });
        }

    }
}
