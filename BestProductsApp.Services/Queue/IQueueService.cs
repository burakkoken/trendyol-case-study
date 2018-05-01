using BestProductsApp.Models.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestProductsApp.Services.Queue
{
    public interface IQueueService
    {
        AzureStorageOptions StorageOptions { get; set; }
        bool AddQueue(QueueTypes type, string message);
    }
}
