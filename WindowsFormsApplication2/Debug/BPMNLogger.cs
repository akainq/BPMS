using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2.Debug
{
    public class BPMNLogger : TraceListener
    {

        RichTextBox _textControl = null;

        public delegate void myDeleg( RichTextBox rtb, string text);

        void SetText(RichTextBox rtb, string text)
        {
            rtb.Text = text;
        }

        public BPMNLogger(RichTextBox control):base() {

            _textControl = control;
        
        }


        public  override void Write(string message)
        {
            WriteEntry(message);
        }

        public  override void WriteLine(string message)
        {
            WriteEntry(message);
        }

        private  void WriteEntry(string message)
        {
            
            string msg = string.Format("{0},{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message);

         //   Trace.WriteLine(msg);
            if (_textControl != null)
            {

                Action action = () => _textControl.AppendText(msg+"\n");
                _textControl.Invoke(action);
            }

        }

    }
}
