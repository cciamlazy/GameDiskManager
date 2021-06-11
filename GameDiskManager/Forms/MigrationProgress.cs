using GameDiskManager.Types;
using GameDiskManager.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameDiskManager.Forms
{
    public partial class MigrationProgress : Form
    {
        private readonly SynchronizationContext synchronizationContext;
        GameMigration gameMigration;
        DateTime startTime;
        public MigrationProgress(GameMigration migration)
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
            gameMigration = migration;
            startTime = DateTime.Now;
        }

        private void MigrationProgress_Load(object sender, EventArgs e)
        {
            
        }

        public void StartMigration()
        {
            Thread thread = new Thread(MigrateGame);
            thread.Start();
        }

        private async void MigrateGame()
        {
            startTime = DateTime.Now;

            int delay = this.gameMigration.PlannedDateTime.Millisecond - DateTime.Now.Millisecond;
            if (delay > 0)
            {
                Console.WriteLine("Delaying migration {0} seconds", delay / 1000);
                await Task.Delay(delay);
            }

            Game game = Data.GameByID(this.gameMigration.GameID);

            this.gameMigration.TotalSize = game.Size;

            progressBar.Maximum = (int)(this.gameMigration.TotalSize / 1000);

            this.gameMigration.SourceRoot = game.Location;

            if (this.gameMigration.DestinationRoot == "")
            {
                this.gameMigration.DestinationRoot = Data.GameByID(this.gameMigration.GameID).Location.Replace(Data.DriveByID(this.gameMigration.From_DriveID).Name, 
                    Data.DriveByID(this.gameMigration.To_DriveID).Name);
            }

            this.gameMigration.ActualDateTime = startTime;

            if (!Directory.Exists(this.gameMigration.DestinationRoot))
                Directory.CreateDirectory(this.gameMigration.DestinationRoot);

            foreach (string s in Data.GameByID(this.gameMigration.GameID).Folders)
            {
                string path = this.gameMigration.DestinationRoot + s;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }

            for (int i = 0; i < this.gameMigration.MigrationFiles.Length; i++)
            {
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    files.Text = String.Format("{0}/{1}", (i + 1).ToString(), this.gameMigration.MigrationFiles.Length);
                    migratingFileName.Text = this.gameMigration.MigrationFiles[i].source;
                }), true);
                FastMove.MoveGameFile(ref this.gameMigration.MigrationFiles[i]);

                if (this.gameMigration.MigrationFiles[i].Status == MigrationStatus.Failed)
                {
                    AddError(this.gameMigration.MigrationFiles[i].Exception);
                }
                this.gameMigration.Moved += this.gameMigration.MigrationFiles[i].sent;
            }
            /*
            Console.WriteLine("Migrating Configs");

            ConfigFile[] configs = game.ConfigFiles.Where(x => !x.KeepLocation).ToArray();
            for (int i = 0; i < configs.Count(); i++)
            {
                var relativePath = Utils.GetRelativePath(Path.GetDirectoryName(game.Location), configs[i].Location);
                var newPath = Path.Combine(this.gameMigration.DestinationRoot, relativePath);
                FastMove.FMove(configs[i].Location, newPath);
            }*/

            // TODO: Implement Configuration migration

            this.gameMigration.Time_ms = (int)Math.Round((DateTime.Now - startTime).TotalMilliseconds);

            Console.WriteLine("Done");

            this.gameMigration.Failed = this.gameMigration.MigrationFiles
                .Where(x => x.Status == MigrationStatus.Failed)
                .ToArray();

            if (this.gameMigration.Failed.Where(x => x.Status == MigrationStatus.Failed && x.source.Contains(".exe")).Count() > 0)
                this.gameMigration.Status = MigrationStatus.Failed;

            game.DriveID = this.gameMigration.To_DriveID;
            game.ExecutableLocation = (game.ExecutableLocation == null ? "" : game.ExecutableLocation.Replace(game.Location, this.gameMigration.DestinationRoot));
            game.Location = this.gameMigration.DestinationRoot;

            Console.WriteLine(game.Location);


            this.gameMigration.Status = this.gameMigration.Status != MigrationStatus.Failed ? MigrationStatus.Successful : MigrationStatus.Failed;

            Data.UpdateGame(game);

            // size time in milliseconds per hour
            long tsize = game.Size * 3600000 / this.gameMigration.Time_ms;
            tsize = tsize / (int)Math.Pow(2, 30);

            TimeSpan t = TimeSpan.FromMilliseconds(this.gameMigration.Time_ms);
            string time = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);

            Console.WriteLine(tsize + "GB/hour");

            Console.WriteLine("Migration Complete in {0} and migrated at an average speed of {1}", tsize, time );

            Data.Store.Migrations.Add(this.gameMigration);

            Data.SaveDataStore();

            return;
        }

        private void AddError(Exception e)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                string err = string.Format("{0}: {1}", (errorList.Items.Count + 1), e.Message);
                errorList.Items.Add(err);
                errors.Text = errorList.Items.Count.ToString();
            }), true);
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            UpdateMigrationProgress();
        }

        private void UpdateMigrationProgress()
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                // Set Elapsed Time
                TimeSpan t = TimeSpan.FromMilliseconds((DateTime.Now - startTime).TotalMilliseconds);
                Console.WriteLine(t.ToString());
                elapsedTime.Text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);

                // Set time & speed
                long moved = this.gameMigration.MigrationFiles.Sum(x => x.sent);
                TimeSpan es =
                    TimeSpan.FromSeconds(
                        (gameMigration.TotalSize - moved) /
                        ((double)moved / t.TotalSeconds));

                estimatedTime.Text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            es.Hours,
                            es.Minutes,
                            es.Seconds);

                processedAmt.Text = Utils.BytesToString(this.gameMigration.Moved);

                speed.Text = Utils.TransferSpeed(moved, (int)Math.Round(t.TotalSeconds));

                // Progress bar
                progressBar.Value = (int)(moved / 1000);
            }), true);
        }
    }
}
