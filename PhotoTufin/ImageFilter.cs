using System.Windows.Controls;

namespace PhotoTufin;

public class ImageFilter
{
    public void makeFilter(MenuItem Filter)
    {
        var count = 0;
        foreach ( var Item in Filter.Items)
        {
            if (Item is MenuItem { IsChecked: true }) 
            {
                count++;   
            } 
        }
        
        var x = count;
    }
}