using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace BankVisual
{
    internal class ArrivalEvent : Component
    {
        internal static int ArrivedCount;

        public override void StartEvent()
        {
            BankSimulation bsPar = Parent as BankSimulation;
            ClearFinished();
            if (ArrivedCount < Param.MaxClientCount)
            {
                Client clt = new Client(SimTime());
                ArrivedCount++;
                clt.Insert(bsPar.Queue);
                if (bsPar.CurrentClient == null)
                {
                    clt.Remove();
                    bsPar.CurrentClient = clt;
                    if (clt.StartingTime == SimTime())
                    {
                        bsPar.NotWaited++;
                    }
                    bsPar.CashStat.Start(SimTime());
                    (new FinishedEvent()).ActivateDelay(BankSimulation.RandCashman.Uniform(Param.MinCashTime, Param.MaxCashTime));
                }
                ReactivateDelay(BankSimulation.RandClient.Exponential(Param.MeanClientInterval));
            }
        }
    }
}
