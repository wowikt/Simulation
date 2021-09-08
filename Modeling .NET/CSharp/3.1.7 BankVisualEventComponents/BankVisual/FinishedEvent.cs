using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace BankVisual
{
    internal class FinishedEvent : Component
    {
        public override void StartEvent()
        {
            BankSimulation bsPar = Parent as BankSimulation;
            Client clt = bsPar.CurrentClient;
            bsPar.CashStat.Finish(SimTime());
            bsPar.InBankStat.AddData(SimTime() - clt.StartingTime);
            bsPar.InBankHist.AddData(SimTime() - clt.StartingTime);
            if (bsPar.CashStat.Finished == Param.MaxClientCount)
            {
                bsPar.Activate();
            }
            else if (bsPar.Queue.Size > 0)
            {
                clt = bsPar.Queue.First as Client;
                clt.Remove();
                bsPar.CurrentClient = clt;
                if (clt.StartingTime == SimTime())
                {
                    bsPar.NotWaited++;
                }
                bsPar.CashStat.Start(SimTime());
                ReactivateDelay(BankSimulation.RandCashman.Uniform(Param.MinCashTime, Param.MaxCashTime));
            }
            else
            {
                bsPar.CurrentClient = null;
            }
        }
    }
}
