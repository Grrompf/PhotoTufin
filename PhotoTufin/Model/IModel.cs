using System;

namespace PhotoTufin.Model;

public interface IModel
{
    /// <summary>
    /// Primary Key 
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// when this file was created 
    /// </summary>
    public DateTime CreatedAt { get; set; }
}