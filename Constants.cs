using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliptMath
{
    public static class Constants
    {
        public const string DatabaseFilename = "UsuarioSQLite.db3";
        public const SQLite.SQLiteOpenFlags Flags =
            //abrir el modelo de la base de datos en modo leer/escribir
            SQLite.SQLiteOpenFlags.ReadWrite |
            //crear una base de datos si no existe
            SQLite.SQLiteOpenFlags.Create |
            //habilitar el acesso multi hilo
            SQLite.SQLiteOpenFlags.SharedCache;
        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}
