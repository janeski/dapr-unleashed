namespace DaprUnleashed.ExtractionService
{
    public class Program
    {
        protected Program() { }
        static async Task Main(string[] args)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var app = new App();
            await app.Run(token);
        }
    }
}