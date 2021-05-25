using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Interfaces;
namespace Shoot_Out_Game
{
    class FireBase
    {

        IFirebaseConfig DatabaseConfig = new FirebaseConfig()
        {
            // this will act as a (password) to the firebase database
            AuthSecret = "eQyTMuf12vPtRnLepXa0gWrKZbpL6tk8u7okkw56",
            //password to Firebase Database
            BasePath = "https://zombiehighscore-default-rtdb.europe-west1.firebasedatabase.app/",
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
                // todo add later
                return "A problem occoured connecting to the server";

            }
            
        }

        public void SendPackage( string Username, string Score, string Date )
        {
            DataPackage package = new DataPackage() {
              username =  Username,
              score = Score,
              date = Date,
            };

            server.Set("user highscore", package);
            //Send package to firebase using the connection made before
        }
        class DataPackage
        {
            public string username  { get; set; }
            public string score { get; set; }
            public string date { get; set; }
            
        }

    }
}
