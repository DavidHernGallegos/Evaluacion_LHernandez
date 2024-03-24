
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Archivos
    {
        public string Id_Tienda { get; set; }
        public string Id_Registradora { get; set; }
        public DateTime FechaHora { get; set; }
        public int Ticket { get; set; }
        public decimal Inpuesto { get; set; }
        public decimal Total { get; set; }
        public TimeSpan FechaHora_Creacion { get; set; }


        public static void LeerArchivos()
        {
            
            string carpetaPendientes = @"http://34.218.6.36:18080/svn/Desarrollo/Evaluacion.net/archivos.fct/";
            string rutaDestinoLocal = @"Pendientes"; 

            // Ejecutar el comando svn checkout para obtener archivos desde el repositorio SVN
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "svn.exe",
                Arguments = $"checkout \"{carpetaPendientes}\" \"{rutaDestinoLocal}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                if (process != null)
                {
                    // Leer la salida estándar del proceso
                    string output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(output);
                    process.WaitForExit();
                }
                else
                {
                    Console.WriteLine("No se pudo iniciar el proceso.");
                }
            }



            // Obtener una lista de archivos con la extensión .fct en el directorio
            string[] archivos = Directory.GetFiles(rutaDestinoLocal, "*.fct");

        
            foreach (string archivo in archivos)
            {
                Console.WriteLine($"Leyendo archivo: {archivo}");
                LeerArchivo(archivo);
            }
        }





     

   





        public static void LeerArchivo(string archiv)
        {

            string carpetaProcesados = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Procesados";

            if (!Directory.Exists(carpetaProcesados))
            {
                try
                {
                    Directory.CreateDirectory(carpetaProcesados);
                    Console.WriteLine("Carpeta 'Procesados' creada correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear la carpeta 'Procesados': {ex.Message}");
                    return;
                }
            }



            string CarpetaProcesados = @"C:\Users\digis\Documents\Procesados"; 
            string rutaArchivo = archiv;
           
            Archivos archivo = new Archivos();
            
            string contenido = File.ReadAllText(rutaArchivo);

            Console.WriteLine("Contenido del archivo:");
            Console.WriteLine(contenido);
            DateTime fecha;
            TimeSpan fecha_creacion;


            string[] elementos = contenido.Split('|');
            
            string[] nuevosElementos = new string[elementos.Length];

            for (int i = 0; i < elementos.Length; i++)
            {
              
                if (elementos[i].StartsWith("-->"))
                {
                   
                    nuevosElementos[i] = elementos[i].Substring(3); 
                }
                else
                {
                 
                    nuevosElementos[i] = elementos[i];
                }
            }



            DateTime.TryParseExact(nuevosElementos[2] ,"yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha ) ;

            string tiempoStr = nuevosElementos[3].ToString();
            string formatoHora = tiempoStr.Insert(2, ":").Insert(5, ":");

            TimeSpan.TryParseExact(formatoHora, "hh':'mm':'ss", CultureInfo.CurrentCulture, TimeSpanStyles.None, out fecha_creacion);

            archivo.Id_Tienda = nuevosElementos[0].ToString();
            archivo.Id_Registradora = nuevosElementos[1].ToString();
            archivo.FechaHora = fecha;
            archivo.FechaHora_Creacion = fecha_creacion;
            archivo.Ticket = int.Parse(nuevosElementos[4]);
            archivo.Inpuesto = decimal.Parse(nuevosElementos[5]);
            archivo.Total = decimal.Parse(nuevosElementos[6]);
          

         

            Dictionary<string,object> diccionario = Add(archivo);

            bool respuesta = (bool)diccionario["Respuesta"];

            string nombreArchivo = Path.GetFileName(rutaArchivo);
           
            if (respuesta)
            {
                File.Move(archiv, Path.Combine(carpetaProcesados, nombreArchivo));
                Console.WriteLine($"Archivo {nombreArchivo} movido a {carpetaProcesados}");



                
            }




        }


        public static Dictionary<string, object> Add(BL.Archivos archivo )
        {
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Respuesta", false }, {"Mensaje", "" } };

            try
            {

                using(DL.PruebaTecnicaContext context = new DL.PruebaTecnicaContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"AddTicket '{archivo.Id_Tienda}', '{archivo.Id_Registradora }' , '{archivo.FechaHora}', {archivo.Ticket}, {archivo.Inpuesto}, {archivo.Total}, '{archivo.FechaHora_Creacion}'");

                    if(query > 0)
                    {
                        diccionario["Respuesta"] = true;
                        diccionario["Mensaje"] = "Se insertaron los datos";

                    }
                    else
                    {
                        diccionario["Mensaje"] = "No se insertaron los datos";
                    }
                }
            }
            catch (Exception ex)
            {
                diccionario["Mensaje"] = "No se insertaron los datos" + ex;
            }

            return diccionario;
        }

    }
}
