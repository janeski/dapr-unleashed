namespace DaprUnleashed.DomainModel
{
    public class Promt
    {
        public Promt()
        {
            id = Guid.NewGuid();
            Response = string.Empty;
            StateTransitions = new List<StateTransition>
            {
                new StateTransition() { State = "Created", DateTime = DateTime.UtcNow }
            };
        }

        public required Guid id { get; set; }

        public required string RawText { get; set; }

        public required string Context { get; set; }

        public string Response { get; set; }

        public required string Type { get; set; }

        public List<StateTransition> StateTransitions { get; set; }
    }
}