using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace TestTask2
{
    class Program
    {
        private static bool TryParseXMLADDROBJ(string filePath,
                                                out Dictionary<string, byte> addressObjectFields,
                                                out List<List<(byte, string)>> addressObjects)
        {
            try
            {
                addressObjectFields = new Dictionary<string, byte>();
                addressObjects = new List<List<(byte, string)>>();

                using (var reader = XmlReader.Create(filePath)){

                    while (reader.Read())
                    {
                        var addressObject = new List<(byte, string)>(addressObjectFields.Values.Count);

                        if (reader != null && reader.Name == "Object")
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                if (!addressObjectFields.ContainsKey(reader.Name))
                                    addressObjectFields.Add(reader.Name, (byte)addressObjectFields.Values.Count);

                                addressObject.Add((addressObjectFields[reader.Name], reader.Value));
                            }

                            addressObjects.Add(addressObject);
                        }
                    }
                }
            }
            catch
            {
                addressObjectFields = null;
                addressObjects = null;
                return false;
            }

            return true;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите полный путь до файла:");
            string filePath = Console.ReadLine();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            bool result = TryParseXMLADDROBJ(filePath, out _, out _);

            stopwatch.Stop();

            Console.WriteLine($"Результат: {(result == true ? "удачно" : "не удачно")}");
            Console.WriteLine($"Затраченное время: { stopwatch.ElapsedMilliseconds} миллисекунд");

            Console.ReadKey();
        }
    }
}
