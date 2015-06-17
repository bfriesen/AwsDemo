using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace DotNet40Library
{
    public class MessageSender
    {
        private readonly IAmazonSQS _sqs;
        private readonly string _queueUrl;

        private readonly Func<string, Task> _sendAsync;

        public MessageSender(string queueUrl)
            : this(new AmazonSQSClient(), queueUrl)
        {
        }

        public MessageSender(IAmazonSQS sqs, string queueUrl)
        {
            _sqs = sqs;
            _queueUrl = queueUrl;

            var imageRuntimeVersion = sqs.GetType().Assembly.ImageRuntimeVersion;

            if (imageRuntimeVersion.StartsWith("v2"))
            {
                var beginSendMessageMethod = _sqs.GetType().GetMethod("BeginSendMessage", new[] { typeof(SendMessageRequest), typeof(AsyncCallback), typeof(object) });
                var endSendMessageMethod = _sqs.GetType().GetMethod("EndSendMessage", new[] { typeof(IAsyncResult) });

                _sendAsync = message =>
                {
                    var sendMessageRequest = new SendMessageRequest(_queueUrl, message);

                    var taskCompletionSource = new TaskCompletionSource<SendMessageResponse>();

                    AsyncCallback asyncCallback =
                        ar =>
                        {
                            var result = (SendMessageResponse)endSendMessageMethod.Invoke(_sqs, new object[] { ar });
                            taskCompletionSource.SetResult(result);
                        };

                    beginSendMessageMethod.Invoke(_sqs, new object[] { sendMessageRequest, asyncCallback, null });

                    return taskCompletionSource.Task;
                };
            }
            else
            {
                var sendMessageAsyncMethod = _sqs.GetType().GetMethod("SendMessageAsync", new[] { typeof(SendMessageRequest), typeof(CancellationToken) });

                _sendAsync = message => (Task)sendMessageAsyncMethod.Invoke(_sqs, new object[] { new SendMessageRequest(_queueUrl, message), default(CancellationToken) });
            }
        }

        public Task SendAsync(string message)
        {
            return _sendAsync(message);
        }
    }
}
