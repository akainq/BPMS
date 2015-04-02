using Inq.BPMN.Types;
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

       const string _LogFile = "bpmn.engine.log";
      //
       //    string SchemaPath = @"E:\MyWork\BPM\Standart\";
       string TestBPMN = @"E:\MyWork\BPM\BPMN\pool.bpmn.bpmn";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public void WriteLog(string Message){

                File.WriteAllText(_LogFile, Message);         

            }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static eBPMNShapeType GetShapeType(tFlowElement element)
        {

            var type = element.GetType();
           

            if (type == typeof(tStartEvent)) { return eBPMNShapeType.BPMNEventStart; }
            if (type == typeof(tGateway))   { return eBPMNShapeType.BPMNGateway; }
            if (type == typeof(tEndEvent)) { return eBPMNShapeType.BPMNEventEnd; }
            if (type == typeof(BPMNShape)) { return eBPMNShapeType.BPMNShape; }
 
            return eBPMNShapeType.BPMNTopLevel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public tDefinitions ReadBPMN(string filename)
        {

            tDefinitions result = new tDefinitions();


            XmlSerializer ser = new XmlSerializer(typeof(tDefinitions));
            ser.UnknownAttribute += ser_UnknownAttribute;
            ser.UnknownElement += ser_UnknownElement;
            ser.UnknownNode += ser_UnknownNode;
            ser.UnreferencedObject += ser_UnreferencedObject;


           try{
               result = (tDefinitions)ser.Deserialize(new StreamReader(filename));

               }
                catch(Exception e)
               {
                   WriteLog(e.Message);
            
                }
          

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
