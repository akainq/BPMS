using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
//using Org.Omg.BPMN20;

namespace Inq.BPMN
{
    public class BPMNEngine
    {
        string SchemaPath = @"E:\MyWork\BPM\Standart\";
        string TestBPMN = @"E:\MyWork\BPM\BPMN\PizzaV4.bpmn";
        public tDefinitions ReadBPMN(string filename)
        {

            tDefinitions result = new tDefinitions();
            //result.rootElement = new List<tRootElement>(); 

         //   result.rootElement

            //var doc = new XmlDocument();


            //XmlTextReader reader = new XmlTextReader(SchemaPath+"BPMN20.xsd");
            //XmlSchema myschema = XmlSchema.Read(reader, null);
            
         

            //doc.Schemas.Add(myschema);
            //doc.Load(TestBPMN);
            //   var ns = new XmlNamespaceManager(doc.NameTable);
            //   ns.AddNamespace("semantic", "semantic");


            //   var def = doc.SelectSingleNode("/semantic");


            XmlSerializer ser = new XmlSerializer(typeof(tDefinitions));
            ser.UnknownAttribute += ser_UnknownAttribute;
            ser.UnknownElement += ser_UnknownElement;
            ser.UnknownNode += ser_UnknownNode;
            ser.UnreferencedObject += ser_UnreferencedObject;

            ////var ser = new XmlSerializer(typeof(tDefinitions));

       

            //var XmlStream = new StreamReader(TestBPMN);

            result = (tDefinitions)ser.Deserialize(new StreamReader(TestBPMN));
            //var tr = (TextReader)new StreamReader(TestBPMN);

            //XmlSerializer xs = new XmlSerializer(typeof(tDefinitions));
            //var td = xs.Deserialize(tr);

            //var sw = new StringWriter();
            //var xtw = XmlWriter.Create(sw, new XmlWriterSettings
            //{
            //    Indent = true
            //});
            //xs.Serialize(xtw, td);
            //xtw.Flush();
          //  Console.WriteLine(sw.ToString());

            return (tDefinitions)result;
            
       

          //  return result;
        }

        void ser_UnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {
            throw new Exception("Неизвестная ссылка на объект!");
        }

        void ser_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            throw new Exception("Неизвестная нода! Name:" + e.Name + " LocalName:" + e.LocalName + " NodeType:" + e.NodeType + " ObjectBeingDeserialized:" + e.ObjectBeingDeserialized);
        }

        void ser_UnknownElement(object sender, XmlElementEventArgs e)
        {
            throw new Exception("Неизвестный елемент!");
        }

        void ser_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            throw new Exception("Неизвестный аотрибут!");
        }

    }
}
