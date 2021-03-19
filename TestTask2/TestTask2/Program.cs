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
            addressObjectFields = new Dictionary<string, byte>();
            addressObjects = new List<List<(byte, string)>>();

            try
            {

                using (var reader = XmlReader.Create(new StreamReader(filePath)))
                {

                    reader.ReadToDescendant("AddressObjects");
                    reader.ReadToDescendant("Object");

                    do
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(reader.ReadOuterXml());
                        XmlNode item = doc.DocumentElement;

                        var itemAttributes = item.Attributes;
                        var addressObject = new List<(byte, string)>();

                        foreach (XmlAttribute field in itemAttributes)
                        {
                            if (!addressObjectFields.ContainsKey(field.Name))
                                addressObjectFields.Add(field.Name, (byte)addressObjectFields.Values.Count);

                            addressObject.Add((addressObjectFields[field.Name], field.Value));
                        }

                        addressObjects.Add(addressObject);

                        GC.Collect();
                    }
                    while (reader.ReadToNextSibling("Object"));

                }
            }
            catch
            {
                return false;
            }
        
            return true;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите полный путь до файла:");
            string filePath= Console.ReadLine();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            bool result = TryParseXMLADDROBJ(filePath, out _, out _);

            stopwatch.Stop();

            Console.WriteLine($"Результат: {(result == true ? "удачно" : "не удачно")}");
            Console.WriteLine($"Затраченное время: { stopwatch.ElapsedMilliseconds} миллисекунд");
        }
    }
}
