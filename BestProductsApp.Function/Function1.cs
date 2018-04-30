using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace BestProductsApp.Function
{
    public static class Function1
    {
        [FunctionName("CacheUpdater")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {

        }
    }
}
