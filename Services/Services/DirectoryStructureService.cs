using System.IO;
using Services.Interfaces;

namespace Services.Services
{
    public class DirectoryStructureService : IDirectoryStructureService
    {
        public void CreateDirectoryStructureForBothParticipants(string storageFolderPath)
        {
            Directory.CreateDirectory(storageFolderPath);
            Directory.CreateDirectory($"{storageFolderPath}/A");
            Directory.CreateDirectory($"{storageFolderPath}/B");
            Directory.CreateDirectory($"{storageFolderPath}/A/{Resources.Resources.InputFolder}");
            Directory.CreateDirectory($"{storageFolderPath}/B/{Resources.Resources.InputFolder}");
            Directory.CreateDirectory($"{storageFolderPath}/A/{Resources.Resources.OutputFolder}");
            Directory.CreateDirectory($"{storageFolderPath}/B/{Resources.Resources.OutputFolder}");
        }
    }
}
