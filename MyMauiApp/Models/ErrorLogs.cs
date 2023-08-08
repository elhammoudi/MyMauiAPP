using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMauiApp
{
    public class ErrorLogs
    {
        private string ErroLogePath => Path.Combine(FileSystem.AppDataDirectory, "ErrorLogs.txt");

        public void Add_Error_Log(Exception ex, string Function)
        {
            try
            {
                File.AppendAllText(ErroLogePath, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace('.', '/') + "|" + Function + "|" + ex.Source + "|" + ex.Message + Environment.NewLine);
            }
            catch
            {
            }
        }
    }
}
