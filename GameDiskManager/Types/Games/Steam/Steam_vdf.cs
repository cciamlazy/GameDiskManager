using Gameloop.Vdf.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager.Types.Games.Steam
{
    class Steam_vdf : VProperty
    {
        public int appid { get; set; }
        public Steam_vdf(VProperty other) : base(other)
        {

        }
    }
}


/*
"LibraryFolders"
{
	"TimeNextStatsReport"		"1621810942"
	"ContentStatsID"		"6250372659548961639"
	"1"		"D:\\Program Files (x86)\\Steam"
	"2"		"E:\\Program Files (x86)\\Steam"
	"3"		"F:\\Program Files (x86)\\Steam"
}
*/