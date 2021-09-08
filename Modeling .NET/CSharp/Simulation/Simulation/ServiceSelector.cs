using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    class ServiceSelector
    {
        public enum QueueSelectionMode
        {
            Order,
            Cyclic,
            Random,
            MinAverageSize,
            MaxAverageSize,
            MinFirstWaiting,
            MaxFirstWaiting,
            MinSize,
            MaxSize,
            MinFreeSize,
            MaxFreeSize,
            Custom
        }

        public enum ServiceSelectionMode
        {
            Order,
            Cyclic,
            MinBusyTime,
            MaxBusyTime,
            MinIdleTime,
            MaxIdleTime,
            Random,
            Custom
        }

        internal List<Service> Services;

        internal List<List> Queues;

        public void AddService(Service newService)
        {
        }

        public void AddQueue(List newQueue)
        {
        }
    }
}
