using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex_Paint.DrawingShapes {
    public static class CreatorIGraphicalObj {

        public static Selecting CrSelectingObj() {
            return new Selecting();
        }

        public static DrawingBezierCurve CrDrawingBezierCurveObj() {
            return new DrawingBezierCurve();
        }

        public static DrawingByPencil CrDrawingByPencilObj() { 
            return new DrawingByPencil();
        }

        public static DrawingEllipce CrDrawingEllipceObj() { 
            return new DrawingEllipce();
        }

        public static DrawingIsoscelesTriangle CrDrawingIsoscelesTriangleObj() { 
            return new DrawingIsoscelesTriangle();
        }

        public static DrawingLine CrDrawingLineObj() { 
            return new DrawingLine();
        }

        public static DrawingPoligon CrDrawingPoligonObj() { 
            return new DrawingPoligon();
        }

        public static DrawingRect CrDrawingRectObj() { 
            return new DrawingRect();
        }

        public static DrawingRoundedRect CrDrawingRoundedRectObj() { 
            return new DrawingRoundedRect();
        }

        public static FillingArea CrFillingAreaObj() { 
            return new FillingArea();
        }
    }
}
