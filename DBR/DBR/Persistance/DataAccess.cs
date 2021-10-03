using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Persistance
{
    /// <summary>
    /// A játékállás mentését/betöltését végző osztály
    /// </summary>
    public class DataAccess : IDataAccess
    {

        /// <summary>
        /// Játékállás betöltáse fájlból
        /// </summary>
        /// <param name="path">Mentési fájl útvonala</param>
        /// <returns>Játékállás <c>GameData</c> típusként</returns>
        public async Task<GameData> LoadAsync(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    int gameTime = Convert.ToInt32(await reader.ReadLineAsync());
                    int cash = Convert.ToInt32(await reader.ReadLineAsync());
                    bool open = Convert.ToBoolean(await reader.ReadLineAsync());

                    string line = await reader.ReadLineAsync();
                    var values = line.Split(' ');

                    int n = Convert.ToInt32(values[0]);
                    int m = Convert.ToInt32(values[1]);

                    int[,] table = new int[n, m];

                    for (int i = 0; i < n; i++)
                    {
                        line = await reader.ReadLineAsync();
                        values = line.Split(' ');

                        for (int j = 0; j < m; j++)
                        {
                            table[i, j] = Convert.ToInt32(values[j]);
                        }
                    }

                    line = await reader.ReadLineAsync();
                    int numberOfInfrastructures = Convert.ToInt32(line);

                    List<List<int>> infrastructureProperties = new List<List<int>>();
                    List<string> infrastructureNames = new List<string>();

                    for( int i = 0; i < numberOfInfrastructures; i++)
                    {
                        line = await reader.ReadLineAsync();
                        values = line.Split(' ');

                        infrastructureNames.Add(values[0]);

                        List<int> list = new List<int>();

                        for (int j = 1; j < values.Length-1; j++)
                        {
                            list.Add(Convert.ToInt32(values[j]));
                        }

                        infrastructureProperties.Add(list);
                    }

                    int nodeCount = Convert.ToInt32(await reader.ReadLineAsync());
                    line = await reader.ReadLineAsync();
                    values = line.Split(' ');
                    int[] points = new int[nodeCount*2];
                    for(int i = 0; i<nodeCount*2; i++)
                    {
                        points[i] = Convert.ToInt32(values[i]);
                    }
                    int[,] neighbourhood = new int[nodeCount, nodeCount];
                    for(int i=0; i<nodeCount; i++)
                    {
                        line = await reader.ReadLineAsync();
                        values = line.Split(' ');
                        for(int j=0; j<nodeCount; j++)
                        {
                            neighbourhood[i, j] = Convert.ToInt32(values[j]);
                        }
                    }

                    int numberOfVisitors = Convert.ToInt32(await reader.ReadLineAsync());
                    List<List<int>> visitorProperties = new List<List<int>>();
                    for(int i=0; i<numberOfVisitors; i++)
                    {
                        line = await reader.ReadLineAsync();
                        values = line.Split(' ');
                        List<int> item = new List<int>();
                        for(int j= 0; j< values.Length-1; j++)
                        {
                            item.Add(Convert.ToInt32(values[j]));
                        }

                        visitorProperties.Add(item);
                    }

                    int numberOfRepairmen = Convert.ToInt32(await reader.ReadLineAsync());
                    List<List<int>> repairmanProperties = new List<List<int>>();
                    for(int i=0; i<numberOfRepairmen; i++)
                    {
                        line = await reader.ReadLineAsync();
                        values = line.Split(' ');
                        List<int> item = new List<int>();
                        for(int j = 0; j<values.Length-1; j++)
                        {
                            item.Add(Convert.ToInt32(values[j]));
                        }

                        repairmanProperties.Add(item);
                    }

                    return new GameData(table,gameTime,cash, open, numberOfInfrastructures, infrastructureProperties, infrastructureNames,
                        nodeCount, points, neighbourhood, numberOfVisitors, visitorProperties, numberOfRepairmen, repairmanProperties); 
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Játékállás mentése fájlba
        /// </summary>
        /// <param name="path">Mentési fájl útvonala</param>
        /// <param name="data">A játékállás adatai, modelltől érkezik</param>
        /// <returns></returns>
        public async Task SaveAsync(string path, GameData data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {

                    await writer.WriteLineAsync(data.GameTime.ToString()); // játékidő
                    await writer.WriteLineAsync(data.Cash.ToString()); // játékos pénze
                    await writer.WriteLineAsync(data.Open.ToString()); // vidámpark nyitása

                    int n = data.Table.GetLength(0);
                    int m = data.Table.GetLength(1);

                    await writer.WriteLineAsync($"{n} {m}"); // a tábla méretei

                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < m; j++)
                        {
                            await writer.WriteAsync($"{data.Table[i, j]} "); // tábla celláinak értékei
                        }
                        await writer.WriteLineAsync();
                    }

                    await writer.WriteLineAsync($"{data.NumberOfInfrastructures} "); // Infrastruktúrák darabszáma

                    for (int i = 0; i < data.NumberOfInfrastructures; i++)
                    {
                        await writer.WriteAsync($"{data.InfrastructureNames[i]} "); // Infrastruktúra neve
                        for (int j = 0; j < 26; j++)
                        {
                            await writer.WriteAsync($"{data.InfrastructureProperties[i][j]} "); //A lista első 25 eleme (mindegyiknek van)
                        }
                        await writer.WriteAsync($"{data.InfrastructureProperties[i][26]} "); // Használók száma
                        for (int j = 0; j < data.InfrastructureProperties[i][26]*2; j++)
                        {
                            await writer.WriteAsync($"{data.InfrastructureProperties[i][27+j]} "); //Használók id-a
                        }
                        await writer.WriteAsync($"{data.InfrastructureProperties[i][27 + data.InfrastructureProperties[i][26]*2]} "); //Várakozók száma
                        for(int j=0; j<data.InfrastructureProperties[i][27 + data.InfrastructureProperties[i][26]*2]; j++)
                        {
                            await writer.WriteAsync($"{data.InfrastructureProperties[i][27 + data.InfrastructureProperties[i][26]*2 + 1 + j]} "); //Várakozók id-a
                        }
                        await writer.WriteLineAsync();
                    }

                    await writer.WriteLineAsync($"{data.NodeCount}"); //Gráf csúcsainak darabszáma

                    for (int i=0; i<data.NodeCount*2; i++)
                    {
                        await writer.WriteAsync($"{data.Points[i]} "); //Gráf csúcskoordinátái
                    }
                    await writer.WriteLineAsync();

                    for(int i=0; i<data.NodeCount; i++)
                    {
                        for(int j=0; j<data.NodeCount; j++)
                        {
                            await writer.WriteAsync($"{data.Neighbourhood[i, j]} "); //Szomszédsági mátrix kiírása
                        }
                        await writer.WriteLineAsync();
                    }

                    await writer.WriteLineAsync($"{data.NumberOfVisitors}"); // Látogatók darabszáma

                    for(int i=0; i<data.NumberOfVisitors; i++)
                    {
                        for(int j = 0; j<11; j++)
                        {
                            await writer.WriteAsync($"{data.VisitorProperties[i][j]} "); // Látogatók tulajdonságai
                        }
                        for(int j=11; j<11+data.VisitorProperties[i][10]; j++)
                        {
                            await writer.WriteAsync($"{data.VisitorProperties[i][j]} "); // Látogatók útja
                        }
                        await writer.WriteLineAsync();
                    }

                    await writer.WriteLineAsync($"{data.NumberOfRepairmen}"); // Karbantartók darabszáma
                    for(int i=0; i<data.NumberOfRepairmen; i++)
                    {
                        for(int j=0; j<8; j++)
                        {
                            await writer.WriteAsync($"{data.RepairmanProperties[i][j]} "); // Karbantartók tulajdonságai
                        }
                        for(int j=8; j<8+data.RepairmanProperties[i][7]; j++)
                        {
                            await writer.WriteAsync($"{data.RepairmanProperties[i][j]} "); // Karbantartók útja
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }
}
