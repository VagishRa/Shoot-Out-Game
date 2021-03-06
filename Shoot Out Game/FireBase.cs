using System.Collections.Generic;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
namespace Shoot_Out_Game
{
    public class FireBase
    {
        public const string UserHighscores = "Highscores";
        //creation directionary to store multiple data 
        //Loadhighscores gets called when we retrieve the variable highscores 
        public Dictionary<string, DataPackage> Highscores => LoadHighScores();

        readonly IFirebaseConfig DatabaseConfig = new FirebaseConfig()
        {
            // this will act as a (password) to the firebase database
            AuthSecret = "eQyTMuf12vPtRnLepXa0gWrKZbpL6tk8u7okkw56",
            //password to Firebase Database
            BasePath = "https://zombiehighscore-default-rtdb.europe-west1.firebasedatabase.app/"
            //path to Firebase Database
        };

        public IFirebaseClient server;
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
                Username = Username,
                Score = Score,
                Date = Date,
            };
            server.Push(UserHighscores, package);
            //Send package to firebase using the connection made before
        }
        //retrieve highscore from data base
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
            public string Username { get; set; }
            public int Score { get; set; }
            public string Date { get; set; }
        }
    }
}

