using EnpassJSONViewer.Models;
using EnpassJSONViewer.Types;
using System.IO;

namespace EnpassJSONViewer.Services
{
    interface IEnpassDatabaseLoader
    {
        Result<EnpassDatabase> Load(Stream stream, string name);
        Result<EnpassDatabase> Load(string filePath);
    }
}
