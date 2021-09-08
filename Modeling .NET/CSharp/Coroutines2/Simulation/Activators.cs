using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    /// <summary>
    /// Класс Activators содержит методы расширения для активации массивов и списков процессов
    /// </summary>
    public static class Activators
    {
        /// <summary>
        /// Активирует все пассивные процессы в массиве.
        /// Активные и приостановленные процессы игнорируются
        /// </summary>
        /// <param name="act">Массив ссылок на процессы</param>
        public static void ActivateAll(this ISchedulable[] act)
        {
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].Activate();
                }
            }
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        public static void ActivateAll(this List act)
        {
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).Activate();
                }
                l = l.Next;
            }
        }

        /// <summary>
        /// Активирует все пассивные процессы в массиве после указанного события.
        /// Активные и приостановленные процессы игнорируются
        /// </summary>
        /// <param name="act">Массив ссылок на процессы</param>
        /// <param name="act2">Событие, после которого следует активировать процессы из массива</param>
        public static void ActivateAllAfter(this ISchedulable[] act, ISchedulable act2)
        {
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivateAfter(act2);
                }
            }
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке после указанного события.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        /// <param name="act2">Событие, после которого следует активировать процессы из списка</param>
        public static void ActivateAllAfter(this List act, ISchedulable act2)
        {
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivateAfter(act2);
                }
                l = l.Next;
            }
        }

        /// <summary>
        /// Активирует все пассивные процессы в массиве в заданный момент времени.
        /// Активные и приостановленные процессы игнорируются
        /// </summary>
        /// <param name="act">Массив ссылок на процессы</param>
        /// <param name="t">Время активации всех процессов массива</param>
        public static void ActivateAllAt(this ISchedulable[] act, double t)
        {
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivateAt(t);
                }
            }
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке в заданный момент времени.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        /// <param name="t">Время активации всех процессов из списка</param>
        public static void ActivateAllAt(this List act, double t)
        {
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivateAt(t);
                }
                l = l.Next;
            }
        }

        /// <summary>
        /// Активирует все пассивные процессы в массиве перед указанным событием.
        /// Активные и приостановленные процессы игнорируются
        /// </summary>
        /// <param name="act">Массив ссылок на процессы</param>
        /// <param name="act2">Событие, перед которым следует активировать процессы из массива</param>
        public static void ActivateAllBefore(this ISchedulable[] act, ISchedulable act2)
        {
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivateBefore(act2);
                }
            }
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке перед указанным событием.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        /// <param name="act2">Событие, перед которым следует активировать процессы из списка</param>
        public static void ActivateAllBefore(this List act, ISchedulable act2)
        {
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivateBefore(act2);
                }
                l = l.Next;
            }
        }

        /// <summary>
        /// Активирует все пассивные процессы в массиве с заданной задержкой времени.
        /// Активные и приостановленные процессы игнорируются
        /// </summary>
        /// <param name="act">Массив ссылок на процессы</param>
        /// <param name="dt">Задержка активации всех процессов массива</param>
        public static void ActivateAllDelay(this ISchedulable[] act, double dt)
        {
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivateDelay(dt);
                }
            }
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке с заданной задержкой времени.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        /// <param name="dt">Задержка активации всех процессов из списка</param>
        public static void ActivateAllDelay(this List act, double dt)
        {
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivateDelay(dt);
                }
                l = l.Next;
            }
        }

        /// <summary>
        /// Активирует все пассивные процессы в массиве в заданный момент времени с приоритетом.
        /// Активные и приостановленные процессы игнорируются
        /// </summary>
        /// <param name="act">Массив ссылок на процессы</param>
        /// <param name="t">Время активации всех процессов массива</param>
        public static void ActivateAllPriorAt(this ISchedulable[] act, double t)
        {
            ISchedulable Prev = null;
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    if (Prev == null)
                    {
                        act[i].ActivatePriorAt(t);
                    }
                    else
                    {
                        act[i].ActivateAfter(Prev);
                    }
                    Prev = act[i];
                }
            }
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке в заданный момент времени с приоритетом.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        /// <param name="t">Время активации всех процессов из списка</param>
        public static void ActivateAllPriorAt(this List act, double t)
        {
            ISchedulable Prev = null;
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    if (Prev == null)
                    {
                        (l as ISchedulable).ActivatePriorAt(t);
                    }
                    else
                    {
                        (l as ISchedulable).ActivateAfter(Prev);
                    }
                    Prev = (l as ISchedulable);
                }
                l = l.Next;
            }
        }

        /// <summary>
        /// Активирует все пассивные процессы в массиве с заданной задержкой времени с приоритетом.
        /// Активные и приостановленные процессы игнорируются
        /// </summary>
        /// <param name="act">Массив ссылок на процессы</param>
        /// <param name="dt">Задержка активации всех процессов массива</param>
        public static void ActivateAllPriorDelay(this ISchedulable[] act, double dt)
        {
            ISchedulable Prev = null;
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    if (Prev == null)
                    {
                        act[i].ActivatePriorDelay(dt);
                    }
                    else
                    {
                        act[i].ActivateAfter(Prev);
                    }
                    Prev = act[i];
                }
            }
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке с заданной задержкой времени с приоритетом.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        /// <param name="dt">Задержка активации всех процессов из списка</param>
        public static void ActivateAllPriorDelay(this List act, double dt)
        {
            ISchedulable Prev = null;
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    if (Prev == null)
                    {
                        (l as ISchedulable).ActivatePriorDelay(dt);
                    }
                    else
                    {
                        (l as ISchedulable).ActivateAfter(Prev);
                    }
                    Prev = (l as ISchedulable);
                }
                l = l.Next;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        public static void ActivateFirst(this ISchedulable[] act)
        {
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].Activate();
                    return;
                }
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        public static void ActivateFirst(this List act)
        {
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).Activate();
                    return;
                }
                l = l.Next;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива после указанного. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        /// <param name="act2">Процесс, после которого активируется первый подходящий процесс из массива</param>
        public static void ActivateFirstAfter(this ISchedulable[] act, ISchedulable act2)
        {
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivateAfter(act2);
                    return;
                }
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка после указанного. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        /// <param name="act2">Процесс, после которого активируется первый подходящий процесс из списка</param>
        public static void ActivateFirstAfter(this List act, ISchedulable act2)
        {
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivateAfter(act2);
                    return;
                }
                l = l.Next;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива в заданный момент времени. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        /// <param name="t">Время активации процесса</param>
        public static void ActivateFirstAt(this ISchedulable[] act, double t)
        {
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivateAt(t);
                    return;
                }
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка в заданный момент времени. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        /// <param name="t">Время активации процесса</param>
        public static void ActivateFirstAt(this List act, double t)
        {
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivateAt(t);
                    return;
                }
                l = l.Next;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива перед указанным. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        /// <param name="act2">Процесс, перед которым активируется первый подходящий процесс из массива</param>
        public static void ActivateFirstBefore(this ISchedulable[] act, ISchedulable act2)
        {
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivateBefore(act2);
                    return;
                }
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка перед указанным. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        /// <param name="act2">Процесс, перед которым активируется первый подходящий процесс из списка</param>
        public static void ActivateFirstBefore(this List act, ISchedulable act2)
        {
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivateBefore(act2);
                    return;
                }
                l = l.Next;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива с заданным интервалом времени. 
        /// Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        /// <param name="dt">Интервал времени для активации процесса</param>
        public static void ActivateFirstDelay(this ISchedulable[] act, double dt)
        {
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivateDelay(dt);
                    return;
                }
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка с заданным интервалом времени. 
        /// Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        /// <param name="dt">Интервал времени для активации процесса</param>
        public static void ActivateFirstDelay(this List act, double dt)
        {
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivateDelay(dt);
                    return;
                }
                l = l.Next;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива в заданный момент времени с приоритетом. 
        /// Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        /// <param name="t">Время активации процесса</param>
        public static void ActivateFirstPriorAt(this ISchedulable[] act, double t)
        {
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivatePriorAt(t);
                    return;
                }
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка в заданный момент времени с приоритетом. 
        /// Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        /// <param name="t">Время активации процесса</param>
        public static void ActivateFirstPriorAt(this List act, double t)
        {
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivatePriorAt(t);
                    return;
                }
                l = l.Next;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива с заданным интервалом времени с приоритетом. 
        /// Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        /// <param name="dt">Интервал времени для активации процесса</param>
        public static void ActivateFirstPriorDelay(this ISchedulable[] act, double dt)
        {
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivatePriorDelay(dt);
                    return;
                }
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка с заданным интервалом времени с приоритетом. 
        /// Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        /// <param name="dt">Интервал времени для активации процесса</param>
        public static void ActivateFirstPriorDelay(this List act, double dt)
        {
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivatePriorDelay(dt);
                    return;
                }
                l = l.Next;
            }
        }
    }
}
