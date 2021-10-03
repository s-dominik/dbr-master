using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Persistance
{
    public interface IDataAccess
    {
        Task<GameData> LoadAsync(string path);
        Task SaveAsync(string path, GameData gamedata);
    }
}
