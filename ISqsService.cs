using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using yurist_SQS.Model;

namespace yurist_SQS
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IService1"을 변경할 수 있습니다.
    [ServiceContract]
    public interface ISqsService
    {

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/sqs/dequeue/{queueId}", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string GetData(string queueId);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/sqs/enqueue", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string PostData(Message value);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "/sqs/delete/{queueId}", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResultMessage DeleteQueue(string queueId);


        // TODO: 여기에 서비스 작업을 추가합니다.
    }


    // 아래 샘플에 나타낸 것처럼 데이터 계약을 사용하여 복합 형식을 서비스 작업에 추가합니다.
    [DataContract]
    public class Message
    {
        private string sendDate;
        private string queueId;
        private string data;
        private int? priority;

        [DataMember]
        public string SendDate
        {
            get { return sendDate; }
            set { sendDate = value; }
        }

        [DataMember]
        public string QueueId
        {
            get { return queueId; }
            set { queueId = value; }
        }

        [DataMember]
        public string Data
        {
            get { return data; }
            set { data = value; }
        }
        [DataMember]
        public int? Priority
        {
            get { return priority; }
            set { priority = value == null ? 0 : value; }
        }
    }
}
