﻿namespace DaprUnleashed.DomainModel.Interfaces
{
    public interface IQueueService
    {
        Task SendAsync(string promt);
    }
}
