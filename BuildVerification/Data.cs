using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildVerification
{

    /// <summary>
    /// A class to hold information regarding specific data points in the dataset, such as the temperature at a station.
    /// While this will likely lead to unnecessary information being saved, e.g. we don't need StationName for the first assignment, it overall makes the code a lot more understandable. 
    /// While there might be some cost to saving this information, due to the unspecified nature of the assignment, I decided to treat it as if the information is only saved on a per-need basis, rather than save up a large cache of information.
    /// In a situation like this, the bigger cost is the download of information, which takes the same amount of time either way. As such, some very minor performance was sacrificed for a more maintainable code. 
    /// 
    /// This file can easily be expanded to hold more information without impacting any existing classes, as long as the values that are already here are not edited. 
    /// </summary>
    class Data
    {
        public string StationName { get; set; }
        public string Value { get; set; }
        public Data(string stationName, string value, string from = "", string to = "")
        {
            this.StationName = stationName;
            this.Value = value;
        }
    }
}
