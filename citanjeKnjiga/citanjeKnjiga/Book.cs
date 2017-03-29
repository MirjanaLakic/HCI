using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace citanjeKnjiga
{
    class Book
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private bool favorite;

        public bool Favorite
        {
            get { return favorite; }
            set { favorite = value; }
        }

        private bool recently;

        public bool Recently
        {
            get { return recently; }
            set { recently = value; }
        }

        private bool recentlyAdded;

        public bool RecentlyAdded
        {
            get { return recentlyAdded; }
            set { recentlyAdded = value; }
        }
        

        public Book(string name, bool favorite, bool recently, bool recentlyAdded)
        {
            this.Name = name;
            this.Favorite = favorite;
            this.Recently = recently;
            this.RecentlyAdded = recentlyAdded;
        }
    }
}