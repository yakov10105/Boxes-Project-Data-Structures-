using System;

namespace Boxes.Models
{
    [Serializable]
    public class TimeData
    {
        public double Length { get; set; }
        public double Height { get; set; }
        public DateTime AddedToStoreDate { get;private set; }
        public DateTime LastDealDate{ get; set; }

        public HeightData FatherY { get; set; } //references for its X and Y datas (for deleting with O(1) )
        public LengthData FatherX { get; set; }


        public TimeData()
        {
            AddedToStoreDate = DateTime.Now;
        }
    }
}