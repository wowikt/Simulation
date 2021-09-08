using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс <c>TimeBetStatistics</c> собирает точечную статистику по интервалам времени между событиями
    /// </summary>
    public class TimeBetStatistics : Statistics
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public TimeBetStatistics()
        {
            Count = -1;
            LastTime = -1;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="AHeader">Заголовок для вывода на экран</param>
        public TimeBetStatistics(string AHeader)
            : base(AHeader)
        {
            Count = -1;
            LastTime = -1;
        }

        private double LastTime;

        /// <summary>
        /// Добавляет текущий момент имитационного времени, соответствующий активной имитации
        /// </summary>
        public void AddData()
        {
            AddData(Global.CurrSim.SimTime());
        }

        /// <summary>
        /// Добавляет новое значение как разность между значением параметра и последним добавленным значением
        /// </summary>
        /// <param name="newTime"></param>
        public override void AddData(double newTime)
        {
            if (Count < 0)
            {
                LastTime = newTime;
                Count++;
            }
            else
            {
                double dt = newTime - LastTime;
                base.AddData(dt);
                LastTime = newTime;
            }
        }

        /// <summary>
        /// Приведение статистики в исходное состояние
        /// </summary>
        public override void ClearStat()
        {
            base.ClearStat();
            Count = -1;
            LastTime = -1;
        }
    }
}
