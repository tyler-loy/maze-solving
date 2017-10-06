using SixLabors.ImageSharp;

namespace mazesolvinglib.Entities
{
    public class SourceImage
    {
        public Image<Rgba32> Source { get; set; }
        public CellType[,] Cells { get; set; }
        public int Width => Source.Width;
        public int Height => Source.Height;

        public CellType GetCell(int x, int y)
        {
            return Cells[y, x];
        }
    }
}
