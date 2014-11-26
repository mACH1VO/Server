using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;
using Dirac.Logging;

namespace Dirac.GameServer
{
    public static class Executor
    {
        public static Boolean Initialized { get; private set; }

        private static TimeSpan updateFrequencyExecutor = TimeSpan.FromMilliseconds(100);
        private static Thread _backgroundExecutorThread;
        private static Stopwatch _tickWatch;
        private static ConcurrentDictionary<TickTimer, Action> _actions = new ConcurrentDictionary<TickTimer, Action>();

        public static void Initialize()
        {
            if (!Initialized)
            {
                _tickWatch = new Stopwatch();
                _backgroundExecutorThread = new Thread(_run);
                _backgroundExecutorThread.IsBackground = true; // close app -> close this thread
                _backgroundExecutorThread.Start();
                Initialized = true;
            }
            else
            {
                Logging.LogManager.DefaultLogger.Error("Executor already Initialized");
            }
        }

        private static void _run()
        {
            while (true)
            {
                _tickWatch.Restart();

                foreach (var time in _actions.Keys)
                {
                    if (time.TimedOut)
                    {
                        ThreadPool.QueueUserWorkItem(_execute, _actions[time]);
                        //_actions[time].Invoke();
                        Action todelete;
                        if (!_actions.TryRemove(time, out todelete))
                        {
                            Logging.LogManager.DefaultLogger.Error("Could not remove Action from Executor: Action [{0}]", _actions[time].Method.Name);
                            throw new InvalidOperationException();
                        };
                    }
                }

                _tickWatch.Stop();

                TimeSpan compensation = (updateFrequencyExecutor - _tickWatch.Elapsed);

                if (_tickWatch.Elapsed > updateFrequencyExecutor)
                    Logging.LogManager.DefaultLogger.Warn("Executor took [{0}ms] / [{1}ms].", _tickWatch.Elapsed.Milliseconds, updateFrequencyExecutor.Milliseconds);
                else
                    Thread.Sleep(compensation);
            }
        }

        private static void _execute(object action)
        {
            (action as Action).Invoke();
        }

        public static void Execute(int Milliseconds, Action action)
        {
            if (!Executor._actions.TryAdd(new TickTimer(Milliseconds), action))
            {
                Logging.LogManager.DefaultLogger.Error("Executor.TryAdd Action error");
                //throw new InvalidOperationException("Could");
            }
        }

        public static void Execute(TimeSpan timespan, Action action)
        {
            if (!Executor._actions.TryAdd(new TickTimer(timespan), action))
            {
                Logging.LogManager.DefaultLogger.Error("Executor.TryAdd Action error");
                //throw new InvalidOperationException();
            }
        }

        public static int PendingActionsCount
        {
            get { return Executor._actions.Count; }
        }
    }
}
