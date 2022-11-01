using System.IO;

namespace PhotoTufin.Search;

public struct FileRow
{
    public FileRow(FileInfo fileInfo)
    {
        fileName = fileInfo.Name;
        size = (fileInfo.Length / 1024).ToString();
        date = fileInfo.CreationTime.ToShortDateString();
        tuplet = false;
        action = false;
    }
    
    public string fileName { get; set; }
    public string size { get; set; }
    public string date { get; set; }
    public bool tuplet { get; set; }
    public bool action { get; set; }
}