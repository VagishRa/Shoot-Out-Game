using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
namespace Shoot_Out_Game
{
    public class FireBase
    {
        private const string UserHighscores = "Highscores";
        public Dictionary<string, DataPackage> Highscores => LoadHighScores();

        IFirebaseConfig DatabaseConfig = new FirebaseConfig()
        {
            // this will act as a (password) to the firebase database
            AuthSecret = "eQyTMuf12vPtRnLepXa0gWrKZbpL6tk8u7okkw56",
            //password to Firebase Database
            BasePath = "https://zombiehighscore-default-rtdb.europe-west1.firebasedatabase.app/"
            //path to Firebase Database
        };

        IFirebaseClient server;
        // function to connect the client to data base
        public string ConnectToDataBase()
        {
            try
            {
                server = new FireSharp.FirebaseClient(DatabaseConfig);
                return "Server connection succesfull";
            }

            catch
            {
                
                return "A problem occoured connecting to the server";
            }
        }

        public void SendPackage(string Username, int Score, string Date)
        {
            DataPackage package = new DataPackage()
            {
                username = Username,
                score = Score,
                date = Date,
            };
            server.Push(UserHighscores, package);
            //Send package to firebase using the connection made before
        }

        public Dictionary<string, DataPackage> LoadHighScores()
        {
            try
            {
                FirebaseResponse respone = server.Get(UserHighscores);
                return respone.ResultAs<Dictionary<string, DataPackage>>();
            }
            catch
            {
                return null;
            }
        }

        public class DataPackage
        {
            public string username { get; set; }
            public int score { get; set; }
            public string date { get; set; }
        }
    }
}

