namespace DaprUnleashed.DomainModel
{
    public class QueueRequest
    {
        public Guid Id { get; set; }

        public required string Type { get; set; }
    }
}
