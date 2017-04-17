using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ReadingIsFun
{
    class Theme
    {
        public Theme()
        {
            //MenuColor = "";
            //MenuTextColor = "";
            BookBackgroundColor = "#ffa9a7a7";
            BookTypingColor = "#ff000000";
            BookToolBarColor = "#ff767474";
            Name = "Original";
        }
        public Theme(string name)
        {
            switch (name)
            {
                case "Original":
                    BookBackgroundColor = "#ffa9a7a7";
                    BookTypingColor = "#ff000000";
                    BookToolBarColor = "#ff767474";
                    Name = "Original";
                    break;
                case "Clasic":
                    BookBackgroundColor = "#fffffbf5";
                    BookTypingColor = "#ff000000";
                    BookToolBarColor = "#fffedd9a";
                    Name = "Clasic";
                    break;
                case "Black":
                    BookBackgroundColor = "#ff3b3b3a";
                    BookTypingColor = "#fffffbf5";
                    BookToolBarColor = "#ff212121";
                    Name = "Black";
                    break;
                case "Blue Light Filter":
                    BookBackgroundColor = "#c1454031";
                    BookTypingColor = "#ff201d13";
                    BookToolBarColor = "#e2524a32";
                    Name = "Blue Light Filter";
                    break;
            }
        }
        //public string MenuColor { get; set; }
        //public string MenuTextColor { get; set; }
        public string BookBackgroundColor { get; set; }
        public string BookTypingColor { get; set; }
        public string BookToolBarColor { get; set; }
        public string Name { get; set; }

        public void Save(BinaryWriter bw)
        {
            //bw.Write(MenuColor);
            //bw.Write(MenuTextColor);
            bw.Write(BookBackgroundColor);
            bw.Write(BookTypingColor);
            bw.Write(BookToolBarColor);
            bw.Write(Name);
        }
        public void Load(BinaryReader br)
        {
            //MenuColor=br.ReadString();
            //MenuTextColor=br.ReadString();
            BookBackgroundColor=br.ReadString();
            BookTypingColor=br.ReadString();
            BookToolBarColor=br.ReadString();
            Name = br.ReadString();
        }
    }
}
