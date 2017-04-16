using System.IO;

namespace ReadingIsFun
{
    class Book
    {
        public double LastPage { get; set; }
        public Book()
        {
            this.LastPage = 0;
        }
        public void Save(BinaryWriter bw)
        {
            bw.Write(LastPage);
        }
        public void Read(BinaryReader br)
        {
            this.LastPage = br.ReadDouble();
        }
    }
}
