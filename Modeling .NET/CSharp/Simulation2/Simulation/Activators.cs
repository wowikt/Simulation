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
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAll(this ISchedulable[] act)
        {
            bool res = false;
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].Activate();
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAll(this Queue act)
        {
            ILink l = act.First;
            bool res = false;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).Activate();
                    res = true;
                }
                l = l.Next;
            }
            return res;
        }

        /// <summary>
        /// Активирует все пассивные процессы в массиве после указанного события.
        /// Активные и приостановленные процессы игнорируются
        /// </summary>
        /// <param name="act">Массив ссылок на процессы</param>
        /// <param name="act2">Событие, после которого следует активировать процессы из массива</param>
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAllAfter(this ISchedulable[] act, ISchedulable act2)
        {
            bool res = false;
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivateAfter(act2);
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке после указанного события.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        /// <param name="act2">Событие, после которого следует активировать процессы из списка</param>
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAllAfter(this Queue act, ISchedulable act2)
        {
            ILink l = act.First;
            bool res = false;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivateAfter(act2);
                    res = true;
                }
                l = l.Next;
            }
            return res;
        }

        /// <summary>
        /// Активирует все пассивные процессы в массиве в заданный момент времени.
        /// Активные и приостановленные процессы игнорируются
        /// </summary>
        /// <param name="act">Массив ссылок на процессы</param>
        /// <param name="t">Время активации всех процессов массива</param>
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAllAt(this ISchedulable[] act, double t)
        {
            bool res = false;
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivateAt(t);
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке в заданный момент времени.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        /// <param name="t">Время активации всех процессов из списка</param>
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAllAt(this Queue act, double t)
        {
            ILink l = act.First;
            bool res = false;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivateAt(t);
                    res = true;
                }
                l = l.Next;
            }
            return res;
        }

        /// <summary>
        /// Активирует все пассивные процессы в массиве перед указанным событием.
        /// Активные и приостановленные процессы игнорируются
        /// </summary>
        /// <param name="act">Массив ссылок на процессы</param>
        /// <param name="act2">Событие, перед которым следует активировать процессы из массива</param>
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAllBefore(this ISchedulable[] act, ISchedulable act2)
        {
            bool res = false;
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivateBefore(act2);
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке перед указанным событием.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        /// <param name="act2">Событие, перед которым следует активировать процессы из списка</param>
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAllBefore(this Queue act, ISchedulable act2)
        {
            ILink l = act.First;
            bool res = false;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivateBefore(act2);
                    res = true;
                }
                l = l.Next;
            }
            return res;
        }

        /// <summary>
        /// Активирует все пассивные процессы в массиве с заданной задержкой времени.
        /// Активные и приостановленные процессы игнорируются
        /// </summary>
        /// <param name="act">Массив ссылок на процессы</param>
        /// <param name="dt">Задержка активации всех процессов массива</param>
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAllDelay(this ISchedulable[] act, double dt)
        {
            bool res = false;
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    act[i].ActivateDelay(dt);
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке с заданной задержкой времени.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        /// <param name="dt">Задержка активации всех процессов из списка</param>
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAllDelay(this Queue act, double dt)
        {
            ILink l = act.First;
            bool res = false;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    (l as ISchedulable).ActivateDelay(dt);
                    res = true;
                }
                l = l.Next;
            }
            return res;
        }

        /// <summary>
        /// Активирует все пассивные процессы в массиве в заданный момент времени с приоритетом.
        /// Активные и приостановленные процессы игнорируются
        /// </summary>
        /// <param name="act">Массив ссылок на процессы</param>
        /// <param name="t">Время активации всех процессов массива</param>
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAllPriorAt(this ISchedulable[] act, double t)
        {
            ISchedulable Prev = null;
            bool res = false;
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
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке в заданный момент времени с приоритетом.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        /// <param name="t">Время активации всех процессов из списка</param>
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAllPriorAt(this Queue act, double t)
        {
            ISchedulable Prev = null;
            ILink l = act.First;
            bool res = false;
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
                    res = true;
                }
                l = l.Next;
            }
            return res;
        }

        /// <summary>
        /// Активирует все пассивные процессы в массиве с заданной задержкой времени с приоритетом.
        /// Активные и приостановленные процессы игнорируются
        /// </summary>
        /// <param name="act">Массив ссылок на процессы</param>
        /// <param name="dt">Задержка активации всех процессов массива</param>
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAllPriorDelay(this ISchedulable[] act, double dt)
        {
            ISchedulable Prev = null;
            bool res = false;
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
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// Активирует все пассивные процессы в списке с заданной задержкой времени с приоритетом.
        /// Активные и приостановленные процессы, а также ячейки, не являющиеся процессами, игнорируются
        /// </summary>
        /// <param name="act">Список, в котором могут находиться процессы</param>
        /// <param name="dt">Задержка активации всех процессов из списка</param>
        /// <returns>true, если был активирован хотя бы один процесс или компонент</returns>
        public static bool ActivateAllPriorDelay(this Queue act, double dt)
        {
            ISchedulable Prev = null;
            ILink l = act.First;
            bool res = false;
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
                    res = true;
                }
                l = l.Next;
            }
            return res;
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirst(this ISchedulable[] act)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.Activate();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirst(this Queue act)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.Activate();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива после указанного. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        /// <param name="act2">Процесс, после которого активируется первый подходящий процесс из массива</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirstAfter(this ISchedulable[] act, ISchedulable act2)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.ActivateAfter(act2);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка после указанного. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        /// <param name="act2">Процесс, после которого активируется первый подходящий процесс из списка</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirstAfter(this Queue act, ISchedulable act2)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.ActivateAfter(act2);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива в заданный момент времени. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        /// <param name="t">Время активации процесса</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirstAt(this ISchedulable[] act, double t)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.ActivateAt(t);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка в заданный момент времени. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        /// <param name="t">Время активации процесса</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirstAt(this Queue act, double t)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.ActivateAt(t);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива перед указанным. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        /// <param name="act2">Процесс, перед которым активируется первый подходящий процесс из массива</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirstBefore(this ISchedulable[] act, ISchedulable act2)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.ActivateBefore(act2);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка перед указанным. Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        /// <param name="act2">Процесс, перед которым активируется первый подходящий процесс из списка</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirstBefore(this Queue act, ISchedulable act2)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.ActivateBefore(act2);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива с заданным интервалом времени. 
        /// Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        /// <param name="dt">Интервал времени для активации процесса</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirstDelay(this ISchedulable[] act, double dt)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.ActivateDelay(dt);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка с заданным интервалом времени. 
        /// Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        /// <param name="dt">Интервал времени для активации процесса</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirstDelay(this Queue act, double dt)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.ActivateDelay(dt);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива в заданный момент времени с приоритетом. 
        /// Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        /// <param name="t">Время активации процесса</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirstPriorAt(this ISchedulable[] act, double t)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.ActivatePriorAt(t);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка в заданный момент времени с приоритетом. 
        /// Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        /// <param name="t">Время активации процесса</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirstPriorAt(this Queue act, double t)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.ActivatePriorAt(t);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из массива с заданным интервалом времени с приоритетом. 
        /// Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Массив процессов</param>
        /// <param name="dt">Интервал времени для активации процесса</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirstPriorDelay(this ISchedulable[] act, double dt)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.ActivatePriorDelay(dt);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Активирует первый пассивный процесс из списка с заданным интервалом времени с приоритетом. 
        /// Остальные процессы не меняют состояние
        /// </summary>
        /// <param name="act">Список, в котором могут быть процессы. 
        /// Элементы списка, не являющиеся процессами, а также пассивные процессы, игнорируются</param>
        /// <param name="dt">Интервал времени для активации процесса</param>
        /// <returns>true, если был активирован процесс или компонент</returns>
        public static bool ActivateFirstPriorDelay(this Queue act, double dt)
        {
            ISchedulable idleProc = act.FirstIdle();
            if (idleProc != null)
            {
                idleProc.ActivatePriorDelay(dt);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Находит первый пассивный процесс или компонент в массиве
        /// </summary>
        /// <param name="act">Массив процессов и/или компонентов</param>
        /// <returns>Первый пассивный процесс или компонент</returns>
        public static ISchedulable FirstIdle(this ISchedulable[] act)
        {
            for (int i = 0; i < act.Length; i++)
            {
                if (act[i].Idle)
                {
                    return act[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Находит первый пассивный процесс или компонент в списке
        /// </summary>
        /// <param name="act">Список процессов и/или компонентов</param>
        /// <returns>Первый пассивный процесс или компонент</returns>
        public static ISchedulable FirstIdle(this Queue act)
        {
            ILink l = act.First;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    return l as ISchedulable;
                }
                l = l.Next;
            }
            return null;
        }

        /// <summary>
        /// Находит последний пассивный процесс или компонент в массиве
        /// </summary>
        /// <param name="act">Массив процессов и/или компонентов</param>
        /// <returns>Последний пассивный процесс или компонент</returns>
        public static ISchedulable LastIdle(this ISchedulable[] act)
        {
            for (int i = act.Length - 1; i >= 0; i--)
            {
                if (act[i].Idle)
                {
                    return act[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Находит последний пассивный процесс или компонент в списке
        /// </summary>
        /// <param name="act">Список процессов и/или компонентов</param>
        /// <returns>Последний пассивный процесс или компонент</returns>
        public static ISchedulable LastIdle(this Queue act)
        {
            ILink l = act.Last;
            while (l != null)
            {
                if (l is ISchedulable && (l as ISchedulable).Idle)
                {
                    return l as ISchedulable;
                }
                l = l.Prev;
            }
            return null;
        }
    }
}
