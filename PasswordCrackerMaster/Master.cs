using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using TcpFramework.TcpServer;

namespace PasswordCrackerMaster
{
    class Master
    {
        public void Start()
        {
            List<UserInfo> userInfos = PasswordFileHandler.ReadPasswordFileStack("passwords.txt");
            Console.WriteLine("passwords opened");

            List<UserInfoClearText> result = new List<UserInfoClearText>();


            foreach (var user in userInfos)
            {
                TcpClient client = new TcpClient("127.0.0.1", 7123);

                using (StreamReader sr = new StreamReader(client.GetStream()))
                using (StreamWriter sw = new StreamWriter(client.GetStream()))
                {
                    string json = JsonConvert.SerializeObject(user);

                    sw.WriteLine(json);
                    sw.Flush();

                    result.AddRange(JsonConvert.DeserializeObject<List<UserInfoClearText>>(sr.ReadLine()));
                }
            }

            Console.WriteLine(string.Join(", ", result));
            Console.WriteLine("Out of {0} password {1} was found ", userInfos.Count, result.Count);
            Console.WriteLine();
            Console.WriteLine("Time elapsed: {0}");
            Console.ReadKey();
        }
    }
}
