using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Scripts.UserData
{
    public interface ITowerSaver
    {
        public void SaveTowerData(CubeData[] data);
        public CubeData[] GetTowerData();
    }
}
