using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using yurist_SQS.Model;

namespace yurist_SQS
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "Service1"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서Service1.svc나 Service1.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class SqsService : ISqsService
    {
        //인메모리 캐시 램에 저장 
        static Dictionary<string, Queue> queueList;

        public ResultMessage DeleteQueue(string queueId)
        {
            var queueList = SimpleQueue.instance();
            ResultMessage result = new ResultMessage() ;
            try
            {
                result = queueList.delete(queueId);
            }
            catch (KeyNotFoundException e)
            {
                return new ResultMessage() { code = 500, message = "실패 ..... " + e.Message };
            }
            catch (Exception ex ) 
            {
                return new ResultMessage() { code = 500, message = "실패 ..... " + ex.Message };
            }

            return result;
        }

        public string GetData(string queueId)
        {
            //존재하는 id인지 확인
            var queueList = SimpleQueue.instance();
            QueueData data;
            try {
                data = queueList.dequeue(queueId);
            }
            catch (KeyNotFoundException e) 
            {
                return "존재하지 않은 큐 아이디입니다.";
            }
            string result = string.Empty;
            if (data == null)
            {
                result = "메세지가 존재하지 않습니다.";
            }
            else 
            {
                result = data.queueData;
            }

            return result;
        }

        public string PostData(Message value)
        {
            string result = "200";
            //유효성 검사 ? 큐 아이디 없으면 ㄴㄴ 
            if (!isValidate(value))
                result = "유효하지 않은 큐 입니다.";
            //큐아이디 별 큐에 넣기
            var queueList = SimpleQueue.instance();
            result = queueList.enqueue(value);

            return result;
        }

        private bool isValidate(Message value) 
        {
            if (string.IsNullOrEmpty(value.QueueId))
                return false;


            return true;
        }
    }
}
