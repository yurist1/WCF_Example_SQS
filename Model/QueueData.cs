using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace yurist_SQS.Model
{
    public class QueueData
    {
        private string _date;
        private string _queue;
        private int _priority;
        public QueueData(string date, string queue, int priority = 0) 
        {
            _date = date;
            _queue = queue;
            _priority = priority;
        }
        public string sendDate { get { return _date; } set { value = _date; } }
        public string queueData { get => _queue; set => value = _queue; }
        public int priority { get => _priority; set => value = _priority; }
    }
}