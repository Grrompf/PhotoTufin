using System;

namespace PhotoTufin.Model;

public interface IModel
{
    public long? ID { get; set; }
    
    /// <summary>
    /// when this file was created 
    /// </summary>
    public DateTime CreatedAt { get; set; }
}