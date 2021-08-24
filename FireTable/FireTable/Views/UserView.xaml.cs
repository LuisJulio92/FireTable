using Firebase.Storage;
using FireTable.Models;
using FireTable.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FireTable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserView : ContentPage
    {
        public UserView()
        {
            InitializeComponent();
            ShowUser();
        }

        MediaFile file;
        string rutaFoto;
        string IdUsuario;
        string status;
        string EstadoImagen;
       

        private async Task EditPhoto()
        {
            UserViewModel funcion = new UserViewModel();
            User parametro = new User();
            parametro.Name = txtUser.Text;
            parametro.Pass = txtPassword.Text;
            parametro.Icon = rutaFoto;
            parametro.Id_Usuario = IdUsuario;
            parametro.State = "Activo";
            await funcion.EditPhoto(parametro);
            await DisplayAlert("Listo", "Es mondá se agregó", "Ok");
            await ShowUser();
        }

        private async Task InsertUser()
        {
            UserViewModel funcion = new UserViewModel();
            User parametro = new User();
            parametro.Name = txtUser.Text;
            parametro.Pass = txtPassword.Text;
            parametro.Icon = "-";
            parametro.State = "-";

            IdUsuario = await funcion.InsertUser(parametro);
            

        }

        private async Task ShowUser()
        {
            UserViewModel funcion = new UserViewModel();
            var dt = await funcion.ShowUser();
            listUser.ItemsSource = dt;
        }

        private async Task ObtenerDatosUser()
        {
            UserViewModel funcion = new UserViewModel();
            User parametros = new User();
            parametros.Id_Usuario = IdUsuario;
            var dt = await funcion.ObtenerDatosUser(parametros);

            foreach (var fila in dt)
            {
                txtUser.Text = fila.Name;
                txtPassword.Text = fila.Pass;
                imageTe.Source = fila.Icon;
                status = fila.State;
                rutaFoto = fila.Icon;
            }
        }

        private async Task EliminarUsuario()
        {
            UserViewModel funcion = new UserViewModel();
            User parametro = new User();
            parametro.Id_Usuario = IdUsuario;
            await funcion.DeleteUser(parametro);
        }

        private async Task EliminarImagen()
        {
            UserViewModel funcion = new UserViewModel();
            await funcion.DeleteImg(IdUsuario + ".jpg");
        }

        private async void btnUploadImage_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null)
                {
                    EstadoImagen = "Nulo";
                    return;
                }
                else
                {
                    imageTe.Source = ImageSource.FromStream(() =>
                    {
                        var rutaImage = file.GetStream();

                        return rutaImage;
                    });
                    await EliminarImagen();
                }
                EstadoImagen = "Lleno";
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }
        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            await InsertUser();
            await UploadImageStorage();
            await EditPhoto();
        }

        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            await EliminarUsuario();
            await EliminarImagen();
            await ShowUser();

        }

        private async void btnIcon_Clicked(object sender, EventArgs e)
        {
            IdUsuario = (sender as ImageButton).CommandParameter.ToString();
            await ObtenerDatosUser();
        }

        private async void btnEditar_Clicked(object sender, EventArgs e)
        {
            if(EstadoImagen =="LLENO")
            {
                await EliminarImagen();
                await UploadImageStorage();

            }
            await EditPhoto();
        }
        private async Task UploadImageStorage()
        {
            UserViewModel funcion = new UserViewModel();
            rutaFoto = await funcion.UploadImage(file.GetStream(), IdUsuario);
        }


         
        



        
    }
}