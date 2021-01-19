using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssetInstaller
{
    public class ZipProgress
    {
        public int Total { get; }
        public int Increment { get; }
        public string Result { get; }

        public ZipProgress(int total, int processed, string result)
        {
            Total = total;
            Increment = processed;
            Result = result;
        }
    }

    public class Zip
    {
        public async static Task<bool> ExtractResourceAsync(string extractLocation, string resourceName, IProgress<ZipProgress> progress, CancellationToken stop = default, bool overwrite = false)
        {
            if (extractLocation == null || resourceName == null)
                return false;

            await Task.Run(() =>
            {
                try
                {
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                    {
                        if (stream == null)
                            throw new IOException("Extraction resouce not found.");

                        using (ZipArchive zip = new ZipArchive(stream))
                        {
                            // Rely on Directory.CreateDirectory for validation of destinationDirectoryName.
                            // Note that this will give us a good DirectoryInfo even if destinationDirectoryName exists:
                            DirectoryInfo di = Directory.CreateDirectory(extractLocation);
                            string destinationDirectoryFullPath = di.FullName;

                            int count = 0;
                            foreach (ZipArchiveEntry entry in zip.Entries)
                            {
                                if (stop.IsCancellationRequested)
                                {
                                    // MP! todo: consider deleting extracted items.
                                    throw new IOException("Extraction cancelled.  Remove partials before retry.");
                                }

                                count++;
                                string fileDestinationPath = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, entry.FullName));

                                if (!fileDestinationPath.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
                                    throw new IOException("File is extracting to outside of the folder specified.");

                                var zipProgress = new ZipProgress(zip.Entries.Count, count, entry.FullName);

                                progress.Report(zipProgress);

                                if (Path.GetFileName(fileDestinationPath).Length == 0)
                                {
                                    if (entry.Length != 0)
                                        throw new IOException("Directory entry with data.");

                                    // It's a directory, create it.
                                    Directory.CreateDirectory(fileDestinationPath);
                                }
                                else
                                {
                                    // It's a file, create containing directory first.
                                    Directory.CreateDirectory(Path.GetDirectoryName(fileDestinationPath));
                                    entry.ExtractToFile(fileDestinationPath, overwrite: overwrite);
                                }
                            }
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    var zipProgress = new ZipProgress(0, 0, ex.Message);
                    progress.Report(zipProgress);
                    return false;
                }
            }, stop);

            return false;
        }
    }
}
