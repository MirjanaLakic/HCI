using System.IO;

namespace ReadingIsFun
{
    class Book
    {
        public int LastPage { get; set; }
        public double FontSize { get; set; }
        public Book()
        {
            this.LastPage = 1;
            this.FontSize = 80;
        }
        public Book(double fontSize)
        {
            this.LastPage = 1;
            this.FontSize = fontSize;
        }
        public void Save(BinaryWriter bw)
        {
            bw.Write(LastPage);
            bw.Write(FontSize);
        }
        public void Read(BinaryReader br)
        {
            this.LastPage = br.ReadInt32();
            this.FontSize = br.ReadDouble();
        }
    }
}
