using System;
using System.Collections.Generic;
using System.Text;


namespace TECSO.FWK.Domain.Entities
{
    public class LogDto : EntityDto<Int64>
    {
        public override string Description => "";

        
        public LogLevel LogLevel
        {
            get
            {
                return (LogLevel)Level;
            }
            set
            {
                Level= (int)value;
            }
        }

        public int Level { get; set; }

        public LogType LogType { get; set; }

        public String LogMessage { get; set; }

        public DateTime LogDate { get; set; }

        public String UserName { get; set; }

        public String SessionId { get; set; }

        public string StackTrace { get; set; }


    }


    public enum LogType
    {
        Error,
        Log
    }
}
