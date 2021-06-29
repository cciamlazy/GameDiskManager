using GDMLib.Transitory;

namespace GDMLib
{
    public delegate void UpdateViewDelegate();
    public delegate void UpdateProgressDelegate(ScanProgress scanProgress);
    public delegate void UpdateMigrationProgressDelegate(MigrationProgressState migrationProgress);
}
