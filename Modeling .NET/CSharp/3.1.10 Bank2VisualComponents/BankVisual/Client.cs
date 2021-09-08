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

        private int CashIndex;

        public override void StartEvent()
        {
            BankSimulation bsPar = Parent as BankSimulation;
            StartingTime = SimTime();
            ArrivedCount++;
            if (bsPar.Queue.Size < Param.MaxQueueLength)
            {
                Insert(bsPar.Queue);
                CashIndex = -1;
                for (int i = 0; i < Param.CashCount; i++)
                {
                    if (bsPar.CurrentClient[i] == null)
                    {
                        CashIndex = i;
                        break;
                    }
                }
                if (CashIndex >= 0)
                {
                    Remove();
                    bsPar.CurrentClient[CashIndex] = this;
                    bsPar.CashStat.Start(SimTime());
                    if (SimTime() == StartingTime)
                    {
                        bsPar.NotWaited++;
                    }
                    ReactivateDelay(Program.RandCashman.Uniform(Param.MinCashTime, Param.MaxCashTime), ServiceFinished);
                }
            }
            else
            {
                bsPar.NotServiced++;
            }
            if (ArrivedCount > Param.CashCount + Param.StartClientNum)
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
                clt.CashIndex = CashIndex;
                bsPar.CurrentClient[CashIndex] = clt;
                bsPar.CashStat.Start(SimTime());
                if (SimTime() == clt.StartingTime)
                {
                    bsPar.NotWaited++;
                }
                clt.ActivateDelay(Program.RandCashman.Uniform(Param.MinCashTime, Param.MaxCashTime), clt.ServiceFinished);
            }
            else
            {
                bsPar.CurrentClient[CashIndex] = null;
            }
            if (bsPar.CashStat.Finished == Param.MaxClientCount)
            {
                bsPar.Activate();
            }
        }
    }
}
