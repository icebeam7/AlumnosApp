using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using AlumnosApp.Clases;
using System.Net.Http.Headers;
using System.Net;
using System.IO;

namespace AlumnosApp.Servicios
{
    public static class ServicioWebApi
    {
        const string WebApiURL = "actualizar-valor";

        private static HttpClient Cliente = new HttpClient() { BaseAddress = new Uri(WebApiURL) };

        public static async Task<Alumno> GetAlumnoByCredentials(string usuario, string password)
        {
            Alumno dato = null;
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"{WebApiURL}/api/Alumnos/GetAlumnoByCredentials/{usuario}/{password}";
            var respuesta = await Cliente.GetAsync(url);

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var json = await respuesta.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(json))
                {
                    dato = JsonConvert.DeserializeObject<Alumno>(json);
                    dato.FotoURLSAS = new ServicioStorage().GetFullDownloadAlumnoURL(dato.Id);
                }
            }

            return dato;
        }

        public static async Task<Alumno> GetAlumno(int id)
        {
            Alumno dato = null;
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"{WebApiURL}/api/Alumnos/{id}";
            var respuesta = await Cliente.GetAsync(url);

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var json = await respuesta.Content.ReadAsStringAsync();
                dato = JsonConvert.DeserializeObject<Alumno>(json);
                dato.FotoURLSAS = new ServicioStorage().GetFullDownloadAlumnoURL(dato.Id);
            }

            return dato;
        }

        public static async Task UpdateAlumno(Alumno info, MemoryStream stream)
        {
            if (stream != null)
            {
                var servicioStorage = new ServicioStorage();
                info.FotoURL = await servicioStorage.UploadAlumno(info.Id, stream);
            }

            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"/api/Alumnos/{info.Id}";
            var jsonContent = JsonConvert.SerializeObject(info);
            var respuesta = await Cliente.PutAsync(url, new StringContent(jsonContent.ToString(), Encoding.UTF8, "application/json"));
        }

        public static async Task<List<Tarea>> GetTareasNoRealizadasByAlumno(int idAlumno)
        {
            List<Tarea> datos = null;
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //var url = $"{WebApiURL}/api/Tareas/GetTareasNoRealizadasByAlumno/{idAlumno}";
            var url = $"{WebApiURL}/api/Tareas/";
            var respuesta = await Cliente.GetAsync(url);

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var json = await respuesta.Content.ReadAsStringAsync();
                datos = JsonConvert.DeserializeObject<List<Tarea>>(json);
            }

            return datos;
        }

        public static async Task<List<TareaAlumno>> GetTareasRealizadasByAlumno(int idAlumno, bool evaluado)
        {
            List<TareaAlumno> datos = null;
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"{WebApiURL}/api/TareaAlumnos/GetTareaAlumnosByEval/{evaluado}";
            //var url = $"{WebApiURL}/api/TareaAlumnos/GetTareasRealizadasByAlumno/{idAlumno}/{evaluado}";
            var respuesta = await Cliente.GetAsync(url);

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var json = await respuesta.Content.ReadAsStringAsync();
                datos = JsonConvert.DeserializeObject<List<TareaAlumno>>(json);
            }

            return datos;
        }

        public static async Task<TareaAlumno> GetTareaAlumno(int idTarea, int idAlumno)
        {
            TareaAlumno dato = null;
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"{WebApiURL}/api/TareaAlumnos/{idTarea}/{idAlumno}";
            var respuesta = await Cliente.GetAsync(url);

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var json = await respuesta.Content.ReadAsStringAsync();
                dato = JsonConvert.DeserializeObject<TareaAlumno>(json);
            }

            return dato;
        }

        public static async Task<TareaAlumno> AddTareaAlumno(TareaAlumno info, MemoryStream stream)
        {
            try
            {

                if (stream != null)
                {
                    var servicioStorage = new ServicioStorage();
                    info.ArchivoURL = await servicioStorage.UploadTareaAlumno(info.IdTarea, info.IdAlumno, stream);
                }

                info.Alumno = null;
                info.Tarea = null;

                TareaAlumno dato = null;
                Cliente.DefaultRequestHeaders.Accept.Clear();
                Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var url = $"/api/TareaAlumnos/";
                var jsonContent = JsonConvert.SerializeObject(info);
                var respuesta = await Cliente.PostAsync(url, new StringContent(jsonContent.ToString(), Encoding.UTF8, "application/json"));

                //if (respuesta.StatusCode == HttpStatusCode.Created)
                {
                    var json = await respuesta.Content.ReadAsStringAsync();
                    dato = JsonConvert.DeserializeObject<TareaAlumno>(json);
                }

                return dato;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static async Task UpdateTareaAlumno(TareaAlumno info, MemoryStream stream)
        {
            if (stream != null)
            {
                var servicioStorage = new ServicioStorage();
                info.ArchivoURL = await servicioStorage.UploadTareaAlumno(info.IdTarea, info.IdAlumno, stream);
            }

            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"/api/TareaAlumnos/{info.IdTarea}/{info.IdAlumno}";
            var jsonContent = JsonConvert.SerializeObject(info);
            var respuesta = await Cliente.PutAsync(url, new StringContent(jsonContent.ToString(), Encoding.UTF8, "application/json"));
        }

        public static async Task<bool> DeleteTareaAlumno(int idTarea, int idAlumno)
        {
            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"/api/TareaAlumnos/{idTarea}/{idAlumno}";
            var respuesta = await Cliente.DeleteAsync(url);
            return respuesta.IsSuccessStatusCode;
        }
    }
}
