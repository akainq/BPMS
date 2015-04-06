using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.Drawing
{
[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute :  Attribute
   {
       private string description;
    
     /// <summary>
       /// Gets the description stored in this attribute.
       /// </summary>
       /// <value>The description stored in the attribute.</value>
      public string Description
      {
           get
           {
               return this.description;
           }
       }
    
       /// <summary>
       /// Initializes a new instance of the 
       /// <see cref="EnumDescriptionAttribute"/> class.
       /// </summary>
       /// <param name="description">The description to store in this attribute.</param>
       public EnumDescriptionAttribute(string description)
           : base()
       {
           this.description = description;
       }
   }


public static class EnumHelper { 

  public static string GetDescription(Enum value)
     {
         if (value == null)
         {
            throw new ArgumentNullException("value");
         }
    
         string description = value.ToString();
         FieldInfo fieldInfo = value.GetType().GetField(description);
         EnumDescriptionAttribute[] attributes = (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
    
         if (attributes != null && attributes.Length > 0)
         {
            description = attributes[0].Description;
         }
         return description;
      }

      public static IList ToList(Type type)
      {
         if (type == null)
         {
            throw new ArgumentNullException("type");
        }
    
         ArrayList list = new ArrayList();
         Array enumValues = Enum.GetValues(type);
    
         foreach (Enum value in enumValues)
         {
            list.Add(new KeyValuePair<string,Enum>( GetDescription(value), value));
         }
    
         return list;
     } 


}
}
