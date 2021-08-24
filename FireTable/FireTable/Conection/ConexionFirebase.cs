using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace FireTable.Conection
{
    public class ConexionFirebase
    {
        public static FirebaseClient firebase =
            new FirebaseClient("https://apifrase-default-rtdb.firebaseio.com/");
    }
}
