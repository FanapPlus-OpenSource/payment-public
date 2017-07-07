using System;
using System.Diagnostics;
using System.IO;
using Appson.Security.KeyGenerator.Core;

namespace Appson.Security.KeyGenerator.Console
{
	internal class Program
	{
		private static void Main()
		{
			do
			{
				CreateSignatureKey(GetDestinationDirectory());
			} while (Continue());
		}

		private static void CreateSignatureKey(string baseAddress)
		{
			var signatureInformation = Generator.Generate();


			File.WriteAllText(Path.Combine(baseAddress, "private.xml"), signatureInformation.PrivateXml);
			File.WriteAllText(Path.Combine(baseAddress, "public.xml"), signatureInformation.PublicXml);
			File.WriteAllText(Path.Combine(baseAddress, "private.rsa"), signatureInformation.PrivatePem);
			File.WriteAllText(Path.Combine(baseAddress, "public.rsa"), signatureInformation.PublicPem);
			System.Console.WriteLine($"Keys generated successfully at the following address\r\n{baseAddress}");

			Process.Start("explorer.exe", baseAddress);
		}

		private static bool Continue()
		{
			System.Console.WriteLine("Continue (y/N)?");
			var input = System.Console.ReadLine();
			return input != null && string.Equals(input, "y", StringComparison.InvariantCultureIgnoreCase);
		}

		private static string GetDestinationDirectory()
		{
			var flag = true;
			var path = "";
			do
			{
				try
				{
					System.Console.Write("Enter destination directory: ");
					path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Console.ReadLine() ?? "");
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					flag = false;
					return path;
				}
				catch (Exception ex)
				{
					System.Console.WriteLine(ex.Message);
					System.Console.WriteLine("Directory path not valid");
				}
			} while (flag);
			return path;
		}
	}
}