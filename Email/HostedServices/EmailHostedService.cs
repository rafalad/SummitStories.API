using SummitStories.API.Email.EmailProvider;
using SummitStories.API.Email.Interfaces;
using SummitStories.API.Email.Model;
using System.Threading.Tasks.Dataflow;

namespace SummitStories.API.Email.HostedServices
{
    public class EmailHostedService : IHostedService, IDisposable
    {
        private Task? _sendTask;
        private CancellationTokenSource? _cancellationToken;
        private readonly BufferBlock<EmailModel> _mailQueue;
        private readonly IEmailSender _mailSender;

        public EmailHostedService(IConfiguration configuration)
        {
            _mailSender = new MailJetProvider(configuration);
            _mailQueue = new BufferBlock<EmailModel>();
            _cancellationToken = new CancellationTokenSource();
        }

        public async Task SendEmailAsync(EmailModel emailModel) => await _mailQueue.SendAsync(emailModel);
        public void Dispose()
        {
            DestroyTask();
        }

        private void DestroyTask()
        {
            try
            {
                if (_cancellationToken != null)
                {
                    _cancellationToken.Cancel();
                    _cancellationToken = null;
                }
                Console.WriteLine("[EMAIL SERVICE] DESTROY SERVICE");
            }
            catch { }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("[EMAIL SERVICE] START SERVICE");
            _sendTask = BackgroundSendEmailAsync(_cancellationToken!.Token);
            return Task.CompletedTask;
        }

        private async Task BackgroundSendEmailAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var email = await _mailQueue.ReceiveAsync();
                    await _mailSender.SendEmail(email);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[EMAIL SERVICE] Exception: {ex.Message}");
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            DestroyTask();
            await Task.WhenAny(_sendTask!, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }
}
