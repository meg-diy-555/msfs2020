using System;
using System.Linq;
using System.Windows.Forms;

namespace MSFS_Con
{
    /// <summary>
    /// Form1のRTB_DebugWindow(RichTextBox)にメッセージを書き込むためのもの
    /// </summary>
    static class TDebug
    {
        /// <summary>
        /// Form上のRTB_DebugWindowというRichTextBoxにメッセージを書き込む
        /// </summary>
        /// <param name="message">書き込みたい一行分の文字列</param>
        public static void WriteLine(String message)
        {
            try
            {
                Form1 f = Application.OpenForms.OfType<Form1>().FirstOrDefault();

                //FormのRichTextBoxコントロールのnameプロパティに合わせる
                var v = f?.Controls.Find("RTB_DebugWindow", true);

                if (v?.Length > 0 && v != null)
                {
                    DateTime dt = DateTime.Now;
                    RichTextBox rtb = (RichTextBox)v[0];
                    rtb.Text = dt.ToString("MM/dd HH:mm:ss - ") + message + "\r\n" + rtb.Text;
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

        }
    }
}
