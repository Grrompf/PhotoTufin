using System;

namespace PhotoTufin.Model;

public class FilterConfig : IModel
{
    /// <summary>
    /// Primary key
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// eg jpg, jpeg, jpe, jfif OR raw
    /// </summary>
    public string Filter { get; set; } = null!;
    
    /// <summary>
    /// True if filter is checked
    /// </summary>
    public bool IsChecked { get; set; }
    
    /// <summary>
    /// when this model was created 
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}