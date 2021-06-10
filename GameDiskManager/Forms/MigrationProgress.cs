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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameDiskManager.Forms
{
    public partial class MigrationProgress : Form
    {
        GameMigration gameMigration;
        public MigrationProgress(GameMigration migration)
        {
            InitializeComponent();
            gameMigration = migration;
        }

        private void MigrationProgress_Load(object sender, EventArgs e)
        {
            
        }

        public async Task<GameMigration> MigrateGame()
        {
            int delay = this.gameMigration.PlannedDateTime.Millisecond - DateTime.Now.Millisecond;
            if (delay > 0)
            {
                Console.WriteLine("Delaying migration {0} seconds", delay / 1000);
                await Task.Delay(delay);
            }

            Game game = Data.GameByID(this.gameMigration.GameID);

            this.gameMigration.SourceRoot = game.Location;

            if (this.gameMigration.DestinationRoot == "")
            {
                this.gameMigration.DestinationRoot = Data.GameByID(this.gameMigration.GameID).Location.Replace(Data.DriveByID(this.gameMigration.From_DriveID).Name, 
                    Data.DriveByID(this.gameMigration.To_DriveID).Name);
            }

            DateTime startTime = DateTime.Now;
            this.gameMigration.ActualDateTime = startTime;

            if (!Directory.Exists(this.gameMigration.DestinationRoot))
                Directory.CreateDirectory(this.gameMigration.DestinationRoot);

            Console.WriteLine("Creating Directories");

            foreach (string s in Data.GameByID(this.gameMigration.GameID).Folders)
            {
                string path = this.gameMigration.DestinationRoot + s;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }

            for (int i = 0; i < this.gameMigration.MigrationFiles.Length; i++)
            {
                Console.Write("\rMigrating Game Files: {0}/{1}", (i + 1).ToString(), this.gameMigration.MigrationFiles.Length);
                FastMove.MoveGameFile(ref this.gameMigration.MigrationFiles[i]);
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

            this.gameMigration.Time_ms = DateTime.Now.Millisecond - startTime.Millisecond;

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

            return this.gameMigration;
        }
    }
    }
}
