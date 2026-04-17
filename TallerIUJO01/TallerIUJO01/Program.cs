/*
 * Created by SharpDevelop.
 * User: admin
 * Date: 17/4/2026
 * Time: 10:53 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TallerIUJO01
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("=== TALLER 01 ===");
			
			//  1. El dato del usuario
			
			string registroUsuario = "    ID_777;  JUAN PENEZ ;  MANIPULACIÓN DE DATOS; 95";
			
			string registroLimpio = registroUsuario.Trim();
			
			Console.Write(registroLimpio);
			
			string [] partes = registroLimpio.Split(';');
			
			string ID  = partes[0].Trim();
			string nombre = partes[1].Trim();
			string evaluacion = partes[2].Trim();
			string nota = partes[3].Trim();
	
			Console.WriteLine(string.Format("EL ID ES: {0}. EL NOMBRE ES: {1}. LA EVALUACION ES: {2}. LA NOTA ES: {3}",ID
			                                ,nombre, evaluacion,nota));
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}