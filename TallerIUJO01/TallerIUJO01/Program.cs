using System;
using System.IO;   // Para manejar carpetas y archivos
using System.Text; // Obligatorio para el traductor de letras a bytes (Encoding)

namespace TallerIUJO01
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== TALLER 01 ===\n");

            // --- 1. LIMPIEZA DE DATOS (Hacer que el texto se vea bien) ---

            // Este es el texto tal cual llega, con espacios feos y todo pegado por puntos y comas
            string registroSucio = "    ID_777;  JUAN PEREZ ;  MANIPULACION DE DATOS; 95";

            // Trim() quita los espacios vacíos que sobran al principio y al final
            string registroLimpio = registroSucio.Trim();

            Console.Write("Dato procesado: " + registroLimpio);

            // Split(';') es como un cuchillo: pica el texto donde encuentre un ';' y lo guarda en una lista (arreglo)
            string[] partes = registroLimpio.Split(';');

            // Sacamos cada pedazo de la lista y le volvemos a quitar los espacios por si acaso
            string ID = partes[0].Trim();
            string nombre = partes[1].Trim();
            string evaluacion = partes[2].Trim();
            string nota = partes[3].Trim();

            // Mostramos todo ordenado. El {4:yyyy...} es para que la fecha salga bonita
            Console.WriteLine(string.Format("\n\nID: {0} | NOMBRE: {1} | EVALUACION: {2} | NOTA: {3} | FECHA: {4:yyyy-MM-dd HH:mm}",
                ID, nombre, evaluacion, nota, DateTime.Now));


            // --- 2. RUTAS Y CARPETAS (¿Donde vamos a guardar todo?) ---

            // Buscamos donde está parado el programa y creamos una carpeta llamada 'Datos IUJO'
            string rutaRaiz = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Datos IUJO");

            // Dentro de esa carpeta, creamos otra subcarpeta llamada 'Reportes'
            string rutaReportes = Path.Combine(rutaRaiz, "Reportes");

            // El nombre del archivo final donde escribiremos el texto
            string archivotexto = Path.Combine(rutaReportes, "NOTAS.TXT");

            // Si la carpeta 'Reportes' no existe, la creamos de una vez para que no de error
            if (!Directory.Exists(rutaReportes))
            {
                Directory.CreateDirectory(rutaReportes);
                Console.WriteLine("\n> Carpeta de reportes creada con éxito.");
            }


            // --- 3. ESCRIBIR EN TEXTO (StreamWriter) ---

            // Usamos 'using' para que el archivo se cierre solo al terminar. 
            // El 'true' significa que no borre lo de antes, sino que escriba al final (Append)
            using (StreamWriter sw = new StreamWriter(archivotexto, true))
            {
                sw.WriteLine(string.Format("ID: {0} | NOMBRE: {1} | EVALUACION: {2} | NOTA: {3} | FECHA: {4:yyyy-MM-dd HH:mm}",
                    ID, nombre, evaluacion, nota, DateTime.Now));
            }

            Console.WriteLine("> Registro de texto guardado en el disco.");


            // --- 4. PERSISTENCIA BINARIA (FileStream - Solo para la compu) ---

            // Creamos un archivo .dat. Este no es para que nosotros lo leamos, es para puros bytes (ceros y unos)
            string archivoBin = Path.Combine(rutaRaiz, "auditoría.dat");

            // Aquí encendemos el 'motor pesando' de archivos
            // FileMode.Append: Escribe al final del archivo.
            // FileAccess.Write: Solo entra a escribir, no pierde tiempo intentando leer.
            using (FileStream fs = new FileStream(archivoBin, FileMode.Append, FileAccess.Write))
            {
                // TRADUCTOR: La computadora no entiende letras, entiende números.
                // Agarramos el ID, le pegamos una barra '|' y lo convertimos en una 'maleta' de bytes (byte[])
                // Para esto necesitamos el 'using System.Text' arriba
                byte[] bytesID = Encoding.UTF8.GetBytes(ID + "|");

                // LANZAMIENTO: Mandamos la maleta al disco duro
                // bytesID: La maleta con los datos.
                // 0: Empezamos desde el primer byte de la maleta.
                // bytesID.Length: Mandamos todo lo que pese la maleta, completica.
                fs.Write(bytesID, 0, bytesID.Length);

                Console.WriteLine("> Auditoría binaria (bytes) generada.");
            }


            // --- 5. FICHA TÉCNICA (FileInfo - El inspector) ---

            // Creamos un inspector llamado 'info' y le damos la dirección del archivo de texto
            FileInfo info = new FileInfo(archivotexto);

            // El inspector no abre el archivo, solo lo mira por fuera y nos dice cuánto pesa
            Console.WriteLine(string.Format("\n[ESTADÍSTICAS] El archivo de notas pesa: {0} bytes.", info.Length));

            // Otras cosas que el inspector sabe:
            // info.Extension -> Te diria ".txt"
            // info.CreationTime -> Cuándo se creó el archivo
            // info.FullName -> La ruta completa desde C:\ hasta el archivo


            // --- 6. LEER EL ARCHIVO (StreamReader - El ojo que lee) ---

            Console.WriteLine("\n>>> Contenido actual del Reporte:");

            // Abrimos el archivo para leerlo
            using (StreamReader sr = new StreamReader(archivotexto))
            {
                string linea;
                // Mientras lo que lea NO sea nulo (o sea, mientras el archivo no se acabe)...
                while ((linea = sr.ReadLine()) != null)
                {
                    // Mostramos la línea que acaba de leer
                    Console.WriteLine(" LÍNEA LEÍDA: " + linea);
                }
            }

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}