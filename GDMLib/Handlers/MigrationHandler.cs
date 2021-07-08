using System;
using System.IO;
using System.Linq;
using GDMLib.Transitory;

namespace GDMLib.Handlers
{
    public class MigrationHandler
    {
        private UpdateMigrationProgressDelegate updateProgress;
        private GameMigration gameMigration;
        private Game game;
        public MigrationHandler(UpdateMigrationProgressDelegate progressDelegate, GameMigration migration)
        {
            this.updateProgress = progressDelegate;
            this.gameMigration = migration;
            game = Data.GameByID(this.gameMigration.GameID);
        }
        public void DoMigration()
        {
            Data.LauncherByID(game.LauncherID).CloseLauncher();

            FillMigrationData();

            SetupDestination();

            MigrateConfigurations();

            MigrateFiles();

            FinalizeMigration();

            Data.UpdateGame(game);

            updateProgress(new MigrationProgressState
            {
                State = "Complete",
                FileName = ""
            });

            Data.AddMigration(this.gameMigration);
        }

        private void MigrateFiles()
        {
            for (int i = 0; i < this.gameMigration.MigrationFiles.Length; i++)
            {
                if (i == this.gameMigration.MigrationFiles.Count())
                    break;

                MigrationProgressState progressState = new MigrationProgressState
                {
                    State = "Migrating Game Files...",
                    Files = string.Format("{0}/{1}", (i + 1).ToString(), this.gameMigration.MigrationFiles.Length),
                    FileName = this.gameMigration.MigrationFiles[i].source.Replace(game.Location, "")
                };

                updateProgress(progressState);

                FastMove.MoveGameFile(ref this.gameMigration.MigrationFiles[i]);

                if (this.gameMigration.MigrationFiles[i].Status == MigrationStatus.Failed)
                {
                    progressState.Error = this.gameMigration.MigrationFiles[i].Exception;
                    updateProgress(progressState);
                }
                this.gameMigration.Moved += this.gameMigration.MigrationFiles[i].sent;
            }
        }

        private void MigrateConfigurations()
        {
            ConfigFile[] configs = game.ConfigFiles.Where(x => !x.KeepLocation).ToArray();
            for (int i = 0; i < configs.Count(); i++)
            {
                if (!configs[i].KeepLocation)
                {
                    MigrationProgressState progressState = new MigrationProgressState
                    {
                        State = "Migration Configurations",
                        Files = string.Format("{0}/{1}", (i + 1).ToString(), configs.Count()),
                        FileName = configs[i].Location
                    };

                    string newPath = "";

                    if (configs[i].KeepRelative)
                    {
                        newPath = Path.Combine(this.gameMigration.DestinationRoot, configs[i].RelativeLocation);
                    }
                    MigrationFile configFile = new MigrationFile
                    {
                        source = configs[i].Location,
                        destination = newPath
                    };
                    FastMove.FMove(ref configFile);

                    if (configFile.Status == MigrationStatus.Failed)
                    {
                        progressState.Error = configFile.Exception;
                    }
                    updateProgress(progressState);
                }
            }
        }

        private void FinalizeMigration()
        {
            this.gameMigration.Time_ms = (int)Math.Round((DateTime.Now - this.gameMigration.ActualDateTime).TotalMilliseconds);

            this.gameMigration.Failed = this.gameMigration.MigrationFiles
                .Where(x => x.Status == MigrationStatus.Failed)
                .ToArray();

            if (this.gameMigration.Failed.Where(x => x.Status == MigrationStatus.Failed && x.source.Contains(".exe")).Count() > 0)
                this.gameMigration.Status = MigrationStatus.Failed;

            game.DriveID = this.gameMigration.To_DriveID;
            game.ExecutableLocation = (game.ExecutableLocation == null ? "" : game.ExecutableLocation.Replace(game.Location, this.gameMigration.DestinationRoot));
            game.Location = this.gameMigration.DestinationRoot;

            this.gameMigration.Status = this.gameMigration.Status != MigrationStatus.Failed ? MigrationStatus.Successful : MigrationStatus.Failed;
        }

        private void FillMigrationData()
        {
            this.gameMigration.TotalSize = game.Size;

            this.gameMigration.SourceRoot = game.Location;

            if (this.gameMigration.DestinationRoot == "")
            {
                this.gameMigration.DestinationRoot = Data.GameByID(this.gameMigration.GameID).Location.Replace(Data.DriveByID(this.gameMigration.From_DriveID).Name,
                    Data.DriveByID(this.gameMigration.To_DriveID).Name);
            }

            this.gameMigration.ActualDateTime = DateTime.Now;
        }

        private void SetupDestination()
        {
            FileSystemHandler.CreateDirectory(this.gameMigration.DestinationRoot);

            foreach (string s in Data.GameByID(this.gameMigration.GameID).Folders)
            {
                string path = this.gameMigration.DestinationRoot + s;
                FileSystemHandler.CreateDirectory(path);
            }
        }

        public GameMigration GetGameMigration()
        {
            return gameMigration;
        }
    }
}
