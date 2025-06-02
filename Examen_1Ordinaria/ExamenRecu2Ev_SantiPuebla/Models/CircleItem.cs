using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

    namespace ExamenRecu2Ev_SantiPuebla.Models
    {
        public partial class CircleItem : ObservableObject
        {
            [ObservableProperty]
            private string imagePath;

            public CircleItem(string path)
            {
                imagePath = path;
            }
        }
    }


