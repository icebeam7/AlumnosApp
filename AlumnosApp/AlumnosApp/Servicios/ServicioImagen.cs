using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Threading.Tasks;

namespace AlumnosApp.Servicios
{
    public static class ServicioImagen
    {
        public static async Task<MediaFile> TomarFoto(bool usarCamara)
        {
            await CrossMedia.Current.Initialize();

            if (usarCamara)
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    return null;
            }

            var file = usarCamara
                ? await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Alumno", Name = $"{DateTime.UtcNow}.jpg"
                    })
                : await CrossMedia.Current.PickPhotoAsync();

            return file;
        }
    }
}
