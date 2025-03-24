namespace FlexPro.Client.Models;

public class BlazorDownloadFileStream
{
    public byte[] FileBytes { get; }

    public BlazorDownloadFileStream(byte[] fileBytes)
    {
        FileBytes = fileBytes;
    }
}