using Firebase.Database.Query;
using Firebase.Storage;
using FireTable.Conection;
using FireTable.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireTable.ViewModels
{
    public class UserViewModel
    {


        string rutaFoto;
        string IdUsuario;

        List<User> Users = new List<User>();
        public async Task<List<User>> ShowUser()
        {
            var data = await ConexionFirebase.firebase
                .Child("Person")
                .OrderByKey()
                .OnceAsync<User>();

            foreach (var rdr in data)
            {
                var parametros = new User();
                parametros.Id_Usuario = rdr.Key;
                parametros.Name = rdr.Object.Name;
                parametros.Pass = rdr.Object.Pass;
                parametros.Icon = rdr.Object.Icon;
                parametros.State = rdr.Object.State;

                Users.Add(parametros);
            }
            return Users;
        }
        public async Task<string> InsertUser(User Parametro)
        {
            var data = await ConexionFirebase.firebase
                 .Child("Person")
                 .PostAsync(new User()
                 {
                     Name = Parametro.Name,
                     Pass = Parametro.Pass,
                     Icon = Parametro.Icon,
                     State = Parametro.State
                 });
            IdUsuario = data.Key;
            return IdUsuario;
        }

        public async Task<string> UploadImage(Stream ImageStrem, string IdUser)
        {
            var dataAlmacenamiento = await new FirebaseStorage("apifrase.appspot.com")
                .Child("User")
                .Child(IdUser + ".jpg")
                .PutAsync(ImageStrem);

            rutaFoto = dataAlmacenamiento;
            return rutaFoto;
        }

        public async Task EditPhoto(User parametro)
        {
            var obtener = (await ConexionFirebase.firebase
                .Child("Person")
                .OnceAsync<User>()).Where(a => a.Key == parametro.Id_Usuario).FirstOrDefault();

            await ConexionFirebase.firebase
                .Child("Person")
                .Child(obtener.Key)
                .PutAsync(new User()
                {
                    Name = parametro.Name,
                    Pass = parametro.Pass,
                    Icon = parametro.Icon,
                    State = parametro.State

                });
        }
        public async Task DeleteUser(User parametro)
        {
            var data = (await ConexionFirebase.firebase
                .Child("Person")
                .OnceAsync<User>()).Where(a => a.Key == parametro.Id_Usuario).FirstOrDefault();

            await ConexionFirebase.firebase
                .Child("Person")
                .Child(data.Key)
                .DeleteAsync();

        }
        public async Task<List<User>> ObtenerDatosUser(User parametros)
        {
            var data = (await ConexionFirebase.firebase
                .Child("Person")
                .OrderByKey()
                .OnceAsync<User>()).Where(a => a.Key == parametros.Id_Usuario);

            foreach (var rdr in data)
            {
                parametros.Name = rdr.Object.Name;
                parametros.Pass = rdr.Object.Pass;
                parametros.Icon = rdr.Object.Icon;
                parametros.State = rdr.Object.State;
                Users.Add(parametros);

            }
            return Users;
        }
        public async Task DeleteImg(string nombre)
        {
            await new FirebaseStorage("apifrase.appspot.com")
                .Child("User")
                .Child(nombre)
                .DeleteAsync();
        }
    }
}
