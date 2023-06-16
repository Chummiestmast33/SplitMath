
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SliptMath.Models;


namespace SliptMath.Data
{
    public class JugadorDatabase
    {
        SQLiteAsyncConnection Database;
        public JugadorDatabase() 
        {
           
        }
        async Task Init()
        {
            if(Database is not null)
            {
                return;
            }
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<Jugador>();
        }
        public async Task<List<Jugador>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<Jugador>().ToListAsync();
        }

        public async Task<Jugador> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<Jugador>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(Jugador item)
        {
            await Init();
            if (item.ID != 0)
            {
                return await Database.UpdateAsync(item);
            }
            else
            {
                return await Database.InsertAsync(item);
            }
        }

        public async Task<int> DeleteItemAsync(Jugador item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
