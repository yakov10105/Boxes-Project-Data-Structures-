namespace Boxes.Models
{
    public class Box
    {
        public double Length { get; set; }
        public double Height { get; set; }

        public override string ToString()
        {
            return $"Length: {Length} , Height: {Height}";
        }
    }
}
