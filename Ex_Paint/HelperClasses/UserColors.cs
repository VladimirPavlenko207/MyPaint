using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex_Paint.HelperClasses {
    public class UserColors {

        private int maxSize;
        public PictureBox[] Boxes { get; }
        public List<Color> Colors { get; private set; }

        public UserColors(PictureBox[] boxes) {
            Boxes = boxes;
            maxSize = boxes.Length;
            Colors = new List<Color>();
            FillBoxes();
        }

        private void FillBoxes() {
            for (int i = 0; i < Colors.Count; i++) {
                Boxes[i].BackColor = Colors[i];
            }
        }

        public void AddColor(Color color) {
            if (Colors.Contains(color)) {
                Colors.Remove(color);
            }
             Colors.Add(color);
             if (Colors.Count > maxSize)
                 Colors.RemoveAt(0);
            
            FillBoxes();
        }
    }
}
