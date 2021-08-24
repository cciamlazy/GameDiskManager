using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.Handlers
{
    public enum QueueState
    {
        Stopped,
        Running,
        Complete,
        Paused
    }
    public static class MigrationQueue
    {
        private static List<GameMigration> queue = new List<GameMigration>();

        public static QueueState CurrentState { get; private set; }
        private static UpdateMigrationProgressDelegate updateProgress;
        public static List<GameMigration> Completed { private set; get; } = new List<GameMigration>();
        public static bool Initialized { get; private set; } = false;


        public static void Initialize(UpdateMigrationProgressDelegate updateProgressDel)
        {
            updateProgress = updateProgressDel;

            Initialized = true;
        }

        private static void DoWork()
        {
            if (queue.Count > 0 && Initialized)
            {
                MigrationHandler handler = new MigrationHandler(updateProgress, queue[0]);
                handler.DoMigration();
                Pop();
            }
            else
            {
                CurrentState = QueueState.Complete;
                return;
            }
            DoWork();
        }

        public static void Push(GameMigration migration)
        {
            queue.Add(migration);
            if (CurrentState == QueueState.Stopped || CurrentState == QueueState.Complete)
                DoWork();
        }

        private static void Pop()
        {
            Completed.Add(queue[0]);
            queue.RemoveAt(0);
        }
    }
}
