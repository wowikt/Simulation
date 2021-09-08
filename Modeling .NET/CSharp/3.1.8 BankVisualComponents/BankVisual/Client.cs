using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulation;

namespace BankVisual
{
    /// <summary>
    /// Класс, имитирующий клиента в банке
    /// </summary>
    class Client : Component
    {
        public static int ArrivedCount;

        public override void StartEvent()
        {
            BankSimulation bsPar = Parent as BankSimulation;
            StartingTime = SimTime();
            Insert(bsPar.Queue);
            ArrivedCount++;
            if (bsPar.CurrentClient == null)
            {
                Remove();
                bsPar.CurrentClient = this;
                bsPar.CashStat.Start(SimTime());
                if (SimTime() == StartingTime)
                {
                    bsPar.NotWaited++;
                }
                ReactivateDelay(Program.RandCashman.Uniform(Param.MinCashTime, Param.MaxCashTime), ServiceFinished);
            }
            if (ArrivedCount < Param.MaxClientCount)
            {
                (new Client()).ActivateDelay(Program.RandClient.Exponential(Param.MeanClientInterval));
            }
        }

        public void ServiceFinished()
        {
            BankSimulation bsPar = Parent as BankSimulation;
            bsPar.InBankHist.AddData(SimTime() - StartingTime);
            bsPar.InBankStat.AddData(SimTime() - StartingTime);
            bsPar.CashStat.Finish(SimTime());
            if (bsPar.Queue.Size > 0)
            {
                Client clt = bsPar.Queue.First as Client;
                clt.Remove();
                bsPar.CurrentClient = clt;
                bsPar.CashStat.Start(SimTime());
                if (SimTime() == clt.StartingTime)
                {
                    bsPar.NotWaited++;
                }
                clt.ActivateDelay(Program.RandCashman.Uniform(Param.MinCashTime, Param.MaxCashTime), clt.ServiceFinished);
            }
            else
            {
                bsPar.CurrentClient = null;
            }
            if (bsPar.CashStat.Finished == Param.MaxClientCount)
            {
                bsPar.Activate();
            }
        }
    }
}
