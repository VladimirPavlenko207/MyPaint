using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_Paint.HelperClasses {
    public class BezierCurve {
        public Point p1, p2, p3, p4;
        public int count = 0;
        public int MaxCount { get; private set; } 

        public BezierCurve(Point p1, Point p4) {
            this.p1 = this.p2 = p1;
            this.p3 = this.p4 = p4;

            MaxCount = 2;
        }
        
    }
}
