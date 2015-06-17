using System;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace DotNet40Library
{
    public class MessageSender
    {
        private readonly IAmazonSQS _sqs;
        private readonly string _queueUrl;

        public MessageSender(string queueUrl)
            : this(new AmazonSQSClient(), queueUrl)
        {
        }

        public MessageSender(IAmazonSQS sqs, string queueUrl)
        {
            _sqs = sqs;
            _queueUrl = queueUrl;
        }

        public Task SendAsync(string message)
        {
            var sendMessageRequest = new SendMessageRequest(_queueUrl, message);

            var taskCompletionSource = new TaskCompletionSource<SendMessageResponse>();

            AsyncCallback asyncCallback =
                ar =>
                {
                    var result = _sqs.EndSendMessage(ar);
                    taskCompletionSource.SetResult(result);
                };

            _sqs.BeginSendMessage(sendMessageRequest, asyncCallback, null);

            return taskCompletionSource.Task;
        }
    }
}
