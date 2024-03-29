#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using L2DatEncDec;

#endregion

namespace LmUtils
{
    #region LogLevel
    public enum LogLevel
    {
        Normal,
        Warning,
        Error
    }
    #endregion

    #region LogMessage
    public class LogMessage
    {
        public DateTime date;
        public LogLevel level;
        public string message;

        public LogMessage(DateTime date, LogLevel level, string message)
        {
            this.date = date;
            this.level = level;
            this.message = message;
        }
    }
    #endregion

    public class Log
    {
        public static string TextSeparator = "________________________________________________";

        public delegate void OnAddMessageDelegate(LogMessage message);

        private Xml xml;
        private XmlElement root;
        public event OnAddMessageDelegate OnAddMessage = null;

        #region Constructors, destructor...
        public Log(string file_name)
        {
            this.xml = new Xml(file_name);

            //add current date log
            this.root = this.xml.CreateNode("log[]");
            this.root.SetAttribute("date", DateTime.Now.ToString(LmUtils.ConvertUtilities.LmDateTimeLongFormat));

            this.Save();
        }

        ~Log()
        {
            this.Save();
        }
        #endregion

        #region Save
        public void Save()
        {
            try
            {
                this.xml.Save();
            }
            catch
            {
            }
        }
        #endregion

        #region Add
        public void Add(string message)
        {
            this.Add(message, false);
        }
        public void Add(string message, bool show_message_box)
        {
            this.Add(message, LogLevel.Normal, show_message_box);
        }

        public void Add(Exception e)
        {
            this.Add(e, false);
        }
        public void Add(Exception e, bool show_message_box)
        {
            this.Add("Unhandled exception", e, show_message_box);
        }

        public void Add(string message, Exception e)
        {
            this.Add(message, e, false);
        }
        public void Add(string message, Exception e, bool show_message_box)
        {
            this.Add(message, e, LogLevel.Error, show_message_box);
        }
        public void Add(string message, Exception e, LogLevel level)
        {
            this.Add(message, e, level, false);
        }
        public void Add(string message, Exception e, LogLevel level, bool show_message_box)
        {
            this.Add(String.Format("{0}\n\n\n\nTechnical information about error:\n" + MessageBox.TextSeparator + "\n{1}\n" + MessageBox.TextSeparator + "\n{2}", message, e.Message, e.ToString()), level);

            if (show_message_box)
            {
                new MessageBox(message, e, level != LogLevel.Error);
            }

            if (!this.SaveOnAdd)
                this.Save();
        }

        public void Add(string message, LogLevel level)
        {
            this.Add(message, level, false);
        }
        public void Add(string message, LogLevel level, bool show_message_box)
        {
            LogMessage log_message = new LogMessage(DateTime.Now, level, message);

            XmlElement node = LmUtils.Xml.AddElement("message", this.root as XmlElement);
            node.SetAttribute("date", log_message.date.ToString(LmUtils.ConvertUtilities.LmDateTimeLongFormat));
            node.SetAttribute("level", log_message.level.ToString());
            node.InnerText = log_message.message;

            if (this.OnAddMessage != null)
                this.OnAddMessage(log_message);

            if (this.SaveOnAdd)
                this.Save();

            if (show_message_box)
            {
                new MessageBox(message, level != LogLevel.Error);
            }
        }
        #endregion

        #region Get
        public List<LogMessage> Get()
        {
            return this.Get(null);
        }

        public List<LogMessage> Get(DateTime? log_date)
        {
            XmlElement cur_root = this.xml.Root;

            if (log_date != null)
            {
                foreach (XmlElement node in this.xml.GetNodes("log[]"))
                {
                    if (LmUtils.ConvertUtilities.LmDateTimeLongToDateTime(node.GetAttribute("date")) == log_date)
                    {
                        cur_root = node;
                        break;
                    }
                }
            }

            List<LogMessage> messages = new List<LogMessage>();
            foreach (XmlElement node in cur_root.GetElementsByTagName("message"))
            {
                messages.Add(new LogMessage(LmUtils.ConvertUtilities.LmDateTimeLongToDateTime(node.GetAttribute("date")), (LogLevel)Enum.Parse(typeof(LogLevel), node.GetAttribute("level")), node.InnerText));
            }

            return messages;
        }
        #endregion

        #region GetLogList
        public List<DateTime> GetLogList()
        {
            List<DateTime> log_list = new List<DateTime>();
            foreach (XmlElement node in this.xml.GetNodes("log[]"))
            {
                log_list.Add(LmUtils.ConvertUtilities.LmDateTimeLongToDateTime(node.GetAttribute("date")));
            }

            return log_list;
        }
        #endregion

        #region Accessors

        #region LogHistory
        private int log_history;

        /// <summary>
        /// Get/set amount of logs to store
        /// </summary>
        /// <value></value>
        public int LogHistory
        {
            get
            {
                return this.log_history;
            }
            set
            {
                if (value > 0)
                {
                    this.log_history = value;

                    //clean expired messages
                    List<XmlElement> remove_nodes = new List<XmlElement>();
                    XmlNodeList nodes = this.xml.GetNodes("log[]");
                    while (this.xml.Root.ChildNodes.Count > this.log_history)
                        this.xml.Root.RemoveChild(this.xml.Root.FirstChild);

                    this.root = this.xml.Root.LastChild as XmlElement;
                }
            }
        }
        #endregion

        #region SaveOnAdd
        private bool save_on_add = true;

        /// <summary>
        /// Get/set
        /// </summary>
        /// <value></value>
        public bool SaveOnAdd
        {
            get
            {
                return this.save_on_add;
            }
            set
            {
                this.save_on_add = value;
            }
        }
        #endregion

        #endregion
    }
}
