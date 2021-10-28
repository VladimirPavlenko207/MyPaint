using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_Paint.HelperClasses {
    public class MyLine {
        public Point begin;
        public Point end;

        public MyLine() { }
        public MyLine(Point begin, Point end) {
            this.begin = begin;
            this.end = end;
        }
    }
}
