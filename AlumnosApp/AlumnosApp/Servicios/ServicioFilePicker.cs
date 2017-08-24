using Plugin.FilePicker;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AlumnosApp.Servicios
{
    public class ServicioFilePicker
    {
        public async Task<MemoryStream> GetFile()
        {
            try
            {
                var data = await CrossFilePicker.Current.PickFile();
                var bytes = data.DataArray;
                return new MemoryStream(bytes);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}