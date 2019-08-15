using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Corouter
{
    public class Routine : IEnumerator
    {
        List<Func<IEnumerator>> baseEnumerators = new List<Func<IEnumerator>>();
        int enumeratorFuncIndex = 0;
        IEnumerator currentEnumerator;
        Stack<IEnumerator> enumeratorStack = new Stack<IEnumerator>();
        private bool _running = false;
        private float unpauseTime = 0;
        public bool Running
        {
            get
            {
                return _running;
            }
        }

        public object Current => currentEnumerator.Current;

        public Routine(params Func<IEnumerator>[] enumerators)
        {
            if (enumerators.Length > 0)
            {
                baseEnumerators = new List<Func<IEnumerator>>(enumerators);
                currentEnumerator = baseEnumerators[0]();
            }
        }

        public void Start()
        {
            _running = true;
            CorouterManager.Instance.RegisterRoutine(this);
        }

        public void Stop()
        {
            _running = false;
        }

        public void Reset()
        {
            enumeratorFuncIndex = 0;
            currentEnumerator = baseEnumerators[0]();
        }

        public void Reboot()
        {
            Reset();
            Start();
        }

        public bool Tick()
        {
            if (Time.time < unpauseTime)
                return true;
            bool currentHasNext = currentEnumerator != null && currentEnumerator.MoveNext();
            if (currentHasNext)
            {
                switch (currentEnumerator.Current)
                {
                    case IEnumerator e:
                        enumeratorStack.Push(currentEnumerator);
                        currentEnumerator = e;
                        break;
                    case float f:
                        unpauseTime = Time.time + f;
                        break;
                    case null:
                        break;
                    default:
                        break;
                }
                return true;
            }
            else if (enumeratorStack.Count > 0)
            {
                currentEnumerator = enumeratorStack.Pop();
                return true;
            }
            else if (enumeratorFuncIndex + 1 < baseEnumerators.Count)
            {
                enumeratorFuncIndex++;
                currentEnumerator = baseEnumerators[enumeratorFuncIndex]();
                return true;
            }
            else
            {
                _running = false;
                return false;
            }
        }
        #region Fluent API
        public Routine Do(Func<IEnumerator> other)
        {
            baseEnumerators.Add(other);
            return this;
        }
        public Routine Do(System.Action action)
        {
            baseEnumerators.Add(() => { action(); return null; });
            return this;
        }
        public Routine If(System.Func<bool> condition)
        {
            baseEnumerators.Add(() => IfEnumerator(condition));
            return this;
        }
        public Routine While(System.Func<bool> condition, Func<IEnumerator> doThat)
        {
            baseEnumerators.Add(() => WhileEnumerator(condition, doThat));
            return this;
        }
        public Routine While(System.Func<bool> condition, System.Action doThat)
        {
            baseEnumerators.Add(() => WhileEnumerator(condition, doThat));
            return this;
        }
        public Routine Map<T>(IEnumerable<T> list, System.Action<T> action)
        {
            baseEnumerators.Add(() => MapEnumerator(list, action));
            return this;
        }
        public Routine InsideThread(Action action, object lockObject = null)
        {
            baseEnumerators.Add(() => ThreadedEnumerator(action, lockObject));
            return this;
        }
        public Routine Wait(float duration)
        {
            baseEnumerators.Add(() => WaitEnumerator(duration));
            return this;
        }
        #endregion
        #region IEnumerator Definitions
        private IEnumerator IfEnumerator(System.Func<bool> condition)
        {
            while (!condition())
                yield return null;
        }
        private IEnumerator WhileEnumerator(System.Func<bool> condition, Func<IEnumerator> doThat)
        {
            while (condition())
            {
                yield return doThat;
            }
        }
        private IEnumerator WhileEnumerator(System.Func<bool> condition, System.Action doThat)
        {
            while (condition())
            {
                doThat();
                yield return null;
            }
        }
        private IEnumerator MapEnumerator<T>(IEnumerable<T> list, System.Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
                yield return null;
            }
        }
        private IEnumerator ThreadedEnumerator(System.Action action, object lockobject = null)
        {
            if (lockobject == null)
                lockobject = this;
            Thread t = new Thread(
                () =>
                {
                    lock (lockobject)
                    {
                        action();
                    }
                }
                );
            t.Start();
            while (t.IsAlive)
            {
                yield return null;
            }
        }
        private IEnumerator WaitEnumerator(float duration)
        {
            float startingTime = Time.time;
            while (Time.time - startingTime < duration)
            {
                yield return null;
            }
        }

        public bool MoveNext()
        {
            return Tick();
        }
        #endregion
    }
}