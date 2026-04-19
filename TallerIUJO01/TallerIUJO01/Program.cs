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
            
            // --- 7. DESAFÍOS ASIGNADOS POR EL PROFE ----

// A. EL DETECTOR DE CONTRASEÑAS "DÉBILES"
// Aquí simulamos que nos llega un texto con un nombre y una clave separados por ";"
string analisisDatos = ("Gabriel Corobo; 123");

// Usamos el Split para "picar" el texto en dos partes. 
// La posición [0] es el nombre y la posición [1] es la clave.
string[] datos = analisisDatos.Split(';');

// Sacamos la clave de la maleta (el arreglo 'datos')
string clave = datos[1];

// EL DETECTOR: Preguntamos si la clave tiene el "123" en algún lado
if(clave.Contains("123"))
{
    // Si la clave es débil, preparamos la ruta para el reporte de seguridad
    // Guardamos la carpeta nueva dentro de la ruta de Reportes que ya teníamos
    string rutaSeguridad = Path.Combine(rutaReportes, "Reportes de seguridad");
    
    // Le ponemos nombre al archivo de texto donde gritaremos el aviso
    string archivoSeguridad = Path.Combine(rutaSeguridad, "SEGURIDAD.TXT");

    // Revisamos si la carpeta de seguridad existe. Si no está, la fabricamos.
    if (!Directory.Exists(rutaSeguridad))
    {
        Directory.CreateDirectory(rutaSeguridad);
        Console.WriteLine("\n> Carpeta de reportes de seguridad creada con éxito.");
    }

    // Encendemos el escritor de archivos para dejar el mensaje
    // Ojo: Como no tiene el ", true", este archivo se sobreescribe siempre
    using(StreamWriter sw = new StreamWriter(archivoSeguridad))
    {
        sw.WriteLine(string.Format("CLAVE DÉBIL DETECTADA"));
        Console.Write("\n>>> Primer ejercicio realizado correctamente.");
    }
}


// B. EL COPIADOR DE IMÁGENES (Fuerza Bruta)
// Aquí vamos a mover bytes de una imagen a otra usando mangueras (FileStream)

// 1. Abrimos la manguera que succiona la imagen original (LECTURA)
using (FileStream fsOrigen = new FileStream("avatar.jpg", FileMode.Open, FileAccess.Read))     
// 2. Abrimos la manguera que escupe los datos en la copia (ESCRITURA)
using (FileStream fsDestino = new FileStream("respaldo.jpg", FileMode.Create, FileAccess.Write)) 
{
    // Creamos el "cubo" (buffer) para mover los datos de 1024 en 1024 bytes (1 KB)
    byte[] cubo = new byte[1024]; 
    
    // Un contador para saber cuánto líquido (bytes) entró en el cubo en cada viaje
    int cantidadCargada = 0; 
                                        
    // EL BUCLE: Mientras la manguera logre cargar algo en el cubo...
    // El Read llena el cubo y nos dice cuánto pesó lo que entró
    while ((cantidadCargada = fsOrigen.Read(cubo, 0, cubo.Length)) > 0) 
    {
        // Vaciamos el cubo en el destino, pero SOLO lo que cargamos de verdad
        fsDestino.Write(cubo, 0, cantidadCargada);
    }
    
    Console.WriteLine("\n\n>>> ¡Copia finalizada con éxito!");
    Console.WriteLine("Se han movido los bytes de forma segura usando un buffer de 1KB.");
    Console.Write("\n>>> Segundo ejercicio realizado correctamente.");
}


// C. EL LIMPIADOR DE ARCHIVOS PESADOS
// Aquí vamos a revisar una carpeta y borrar lo que estorbe

// 1. Le tomamos una "foto" a la carpeta 'Datos IUJO' para ver qué archivos hay
string[] listaArchivos = Directory.GetFiles("Datos IUJO");

// 2. Caminamos por esa lista de archivos uno por uno
foreach (string archivo in listaArchivos)
{
    // 3. Llamamos al 'inspector' (FileInfo) para que pese el archivo
    FileInfo inform = new FileInfo(archivo);

    // 4. LA REGLA: Si pesa más de 5KB (5 * 1024 = 5120 bytes), se borra
    if (inform.Length > 5120)
    {
        // Si es gordo, lo sacamos del disco
        Console.WriteLine("BORRANDO: " + inform.Name + " por pesado (" + inform.Length + " bytes)");
        File.Delete(archivo);
    }
    else
    {
        // Si es flaco, se queda
        Console.WriteLine("CONSERVADO: " + inform.Name + " es ligero.");
    }
}

Console.WriteLine("\n>>> Tercer ejercicio realizado correctamente.");

Console.WriteLine("\nPresiona cualquier tecla para salir...");
Console.ReadKey();
        }
    }
}