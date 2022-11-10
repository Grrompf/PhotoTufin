using System;

namespace PhotoTufin.Model;

public class BaseModel : IModel
{
    public long? ID { get; set; }
    
    /// <summary>
    /// when this file was created 
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}