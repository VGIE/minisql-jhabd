using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Network
{
    public class Server
    {
        public void Listen(int port)
        {
            //DEADLINE 6: Implement the server as specified (eGela)
            //Have a look at the project ServerConsole to see how a TcpListener is used
            //Use XmlSerializer to create Xml commands
            TcpListener server = null;
            Socket socket = null;
            Database database = null;

            try
            {
                //database = new Database("admin", "adminPassword");
                //Listen on port 1200. Accept connections from any IP address
                server = new TcpListener(IPAddress.Parse("0.0.0.0"), port);

                server.Start();

                Console.WriteLine("Server running and listening on port " + port);

                socket = server.AcceptSocket();

                Console.WriteLine("Connection accepted from " + socket.RemoteEndPoint);

                bool connected = true;

                while(connected == true)
                {
                    byte[] buffer = new byte[100];
                    int bytesRead = socket.Receive(buffer);
                    buffer[bytesRead] = 0;
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    string clientMessage = encoding.GetString(buffer).Substring(0,bytesRead);
                    Console.WriteLine("Message received from client: " + clientMessage);

                    string response = "";

                    // OPEN BLOCK
                    if (XmlDeserializer.ParseOpen(clientMessage, out string openDatabaseName, out string openUser, out string openPassword))
                    {
                        database = Database.Load(openDatabaseName, openUser, openPassword);

                        if (database == null)
                            response = XmlSerializer.OpenCreateError(Constants.IncorrectLogin);

                        else
                            response = XmlSerializer.OpenCreateSuccess;
                    }

                    // CREATE BLOCK
                    else if (XmlDeserializer.ParseCreate(clientMessage, out string createDatabaseName, out string createUser, out string createPassword))
                    {
                        database = new Database(createDatabaseName, createPassword);

                        if (database == null)
                            response = XmlSerializer.CreateError(Constants.CouldNotCreateDatabase);

                        else
                            response = XmlSerializer.CreateSuccess;
                    }

                    // QUERY BLOCK
                    else if (XmlDeserializer.ParseQuery(clientMessage, out string query))
                    {
                        if (database == null)
                            response = XmlSerializer.ErrorAnswer(Constants.NoDatabaseOpen);

                        else
                        {
                            string result = database.ExecuteMiniSQLQuery(query);

                            if (result.StartsWith(Constants.Error))
                                response = XmlSerializer.ErrorAnswer(result);

                            else
                                response = XmlSerializer.SucessfulAnswer(result);
                        }
                    }

                    // CLOSE BLOCK
                    else if(XmlDeserializer.IsCloseCommand(clientMessage))
                    {
                        response = XmlSerializer.CloseConnection;
                        connected = false;
                    }

                    byte[] responseBytes = encoding.GetBytes(response);
                    socket.Send(responseBytes);
                }

                Task.Delay(2000).Wait();
            }

            catch (Exception e)
            {
                Console.WriteLine("Unhandled error: " + e);
            }

            finally
            {
                socket?.Close();
                server?.Stop();

                Console.WriteLine("Server closed. Press any key to finish...");
                Console.ReadKey();
            }
        }
    }
}
