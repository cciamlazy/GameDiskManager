using Gameloop.Vdf.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager.Types.Games.Steam
{
    class Steam_acf : VProperty
    {
        public int appid { get; set; }
        public Steam_acf(VProperty other) : base(other)
        {

        }
    }
}

/*
"AppState"
{
	"appid"		"1217060"
	"Universe"		"1"
	"LauncherPath"		"C:\\Program Files (x86)\\Steam\\steam.exe"
	"name"		"Gunfire Reborn"
	"StateFlags"		"4"
	"installdir"		"Gunfire Reborn"
	"LastUpdated"		"1619987258"
	"UpdateResult"		"0"
	"SizeOnDisk"		"2633939499"
	"buildid"		"6626816"
	"LastOwner"		"76561197994556641"
	"BytesToDownload"		"98267408"
	"BytesDownloaded"		"98267408"
	"BytesToStage"		"658457519"
	"BytesStaged"		"658457519"
	"AutoUpdateBehavior"		"0"
	"AllowOtherDownloadsWhileRunning"		"0"
	"ScheduledAutoUpdate"		"0"
	"InstalledDepots"
	{
		"1217061"
		{
			"manifest"		"6626400770826849783"
			"size"		"2633939499"
		}
	}
	"SharedDepots"
	{
		"228988"		"228980"
	}
	"UserConfig"
	{
		"language"		"english"
	}
}

 */