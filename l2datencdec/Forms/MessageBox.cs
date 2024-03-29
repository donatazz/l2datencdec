#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#endregion

namespace L2DatEncDec
{
    public partial class MessageBox : Form
    {
        public static string TitleFormat = "{0}";
        public static string MessageFormat = "{0}";

        public static string TextSeparator = "________________________________________________";
        public static string TextError = "Error";
        public static string TextInfo = "Information";

        public static Form parentForm = null;
        private delegate DialogResult ShowDialogDelegate(Form parent);

        public MessageBox(string message, string title, bool info)
        {
            InitializeComponent();

            if (title == TextError)
                title = Program.language.getMessage(MsgList.MsgBoxTitle_Error);
            else
                title = Program.language.getMessage(MsgList.MsgBoxTitle_Information);

            this.Text = String.Format(MessageBox.TitleFormat, title);
            this.text.Text = String.Format(MessageBox.MessageFormat, message);

            if (info)
                this.picture.Image = Properties.Resources.Information;

            if (MessageBox.parentForm != null)
            {
                if (MessageBox.parentForm.InvokeRequired)
                {
                    ShowDialogDelegate d = new ShowDialogDelegate(this.ShowDialog);
                    MessageBox.parentForm.Invoke(d, MessageBox.parentForm);
                }
                else
                {
                    this.ShowDialog(MessageBox.parentForm);
                }
            }
            else
                this.ShowDialog();
        }

        public MessageBox(Exception e) : this(e, false)
        {
        }
        public MessageBox(Exception e, bool info) : this(e.ToString(), MessageBox.TextError, info)
        {
        }
        public MessageBox(string message, Exception e) : this(message, e, false)
        {
        }
        public MessageBox(string message, Exception e, bool info) : this(String.Format("{0}\n\n\n\nTechnical error description:\n" + MessageBox.TextSeparator + "\n{1}\n" + MessageBox.TextSeparator + "\n{2}", message, e.Message, e.ToString()), MessageBox.TextError, info)
        {
        }
        public MessageBox(string message, bool info) : this(message, info ? MessageBox.TextInfo : MessageBox.TextError, info)
        {
        }
        public MessageBox(string message) : this(message, false)
        {
        }

        private void MessageBox_Load(object sender, EventArgs e)
        {

        }

        private void MessageBox_Shown(object sender, EventArgs e)
        {
            this.BringToFront();
            this.Activate();
        }
    }
}