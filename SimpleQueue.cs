using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using yurist_SQS.Model;

namespace yurist_SQS
{
    class SimpleQueue
    {
        private static SimpleQueue simpleQueue;
        private static Dictionary<string, Queue> queueList;
        public static SimpleQueue instance()
        {
            if (simpleQueue == null)
                simpleQueue = new SimpleQueue();

            if (queueList == null)
                queueList = new Dictionary<string, Queue>();

            return simpleQueue;
        }
        public string enqueue(Message value)
        {
            var queueData = new QueueData(value.SendDate, value.Data, value.Priority??0);

            if (queueList.Keys.Contains(value.QueueId))
                queueList[value.QueueId].Enqueue(queueData);
            else
            {
                var newQueue = new Queue();
                newQueue.Enqueue(queueData);
                queueList.Add(value.QueueId, newQueue);
            }
            return "200";
        }
        public QueueData dequeue(string queueId)
        {
            //id 존재하는지 확인
            if (queueList.ContainsKey(queueId))
            {
                QueueData result;


                if (queueList[queueId].Count > 0)
                {
                    var priorQueue = queueList[queueId].ToArray().ToList();
                    priorQueue = priorQueue.OrderByDescending(d => ((QueueData)d).priority).OrderBy(d => ((QueueData)d).sendDate).ToList();
                    result = (QueueData)priorQueue.First();

                    queueList[queueId].Clear();
                    if (priorQueue.Count() > 0)
                    {
                        foreach (var item in priorQueue.Skip(1))
                        {
                            queueList[queueId].Enqueue(item);
                        }
                    }

                }
                else
                {
                    result = null;
                }

                return result;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
        public ResultMessage delete(string queueId)
        {
            //id 존재하는지 확인
            if (queueList.ContainsKey(queueId))
            {
                ResultMessage result = new ResultMessage() { code = 200, message = "성공적 .... " };
                try
                {
                    queueList[queueId].Clear();
                    if(queueList[queueId].Count > 0 )
                        return new ResultMessage() { code = 500, message = "삭제 실패 ....." };
                }
                catch (Exception e) 
                {
                    return new ResultMessage() { code = 500 , message = "삭제 실패 ....."};
                }

                return result;
            }
            else
            {
                throw new KeyNotFoundException();
            }
            return null;
        }


    }
}