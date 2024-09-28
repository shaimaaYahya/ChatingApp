using System;

namespace API.Helpers;

public class UploadcareSettings
{
    public required string CloudName { get; set; }
    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}
