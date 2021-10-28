using Common.Drawing;
using Ex_Paint.DrawingShapes;
using Ex_Paint.DrawingShapes.Interfaces;
using Ex_Paint.HelperClasses;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ex_Paint {

    public partial class FMain : Form {

        float indent  = 5;
        Point deltaMouseLocation;
        Bitmap ScaleHBmp, ScaleVBmp;
        Bitmap gridBitmap;
        Bitmap mainBitmap;

        Control previous;
        Control[] toolsButtons;
        string[] PenSizes = new string[] { "1", "3", "5", "8" };
        string[] buttonFillContextMenuItems = new string[] { "без заливки", "однотонный цвет" };
        string[] fileDialogFilter;

        IGraphical graphical;

        Color colorForPaint1;
        Color colorForPaint2;
        Color defaultColor;
        Color defaultColor2;

        bool buttonColorFlag = true;
        bool withFill = false;
        bool isPipetka = false;
        bool withGrid = false;
        UserColors userColors;
        
        Pen buttonShapesPen = new Pen(Brushes.Blue, 1);
        Pen mainPen;
        Brush mainBrush;

        public FMain() {
            InitializeComponent();
            deltaMouseLocation = new Point();
            InitToolsButtons();

            InitUserColors();
            SetFileDialogFilter();
            InitSaveAsToolStripMenuItem();
            
            SetGroupBoxColors();
            SetButtonSizeContextMenu();
            SetButtonFillContextMenu();
            SetMainPicherBox();
            SetScales();
        }

        private void InitToolsButtons() {
            toolsButtons = new Control[]{
             buttonSelect,
             buttonTolls1,
             buttonTolls2,
             buttonTolls3,
             buttonTolls4,
             buttonTolls5,
             buttonTolls6,
             buttonShapes1,
             buttonShapes2,
             buttonShapes3,
             buttonShapes4,
             buttonShapes5,
             buttonShapes6,
             buttonShapes7
            };
            for (int i = 0; i < toolsButtons.Length; i++) {
                toolsButtons[i].Tag = toolsButtons[i].BackColor;
            }

        }
        private void SelectCurentButton(object sender) {
            for (int i = 0; i < toolsButtons.Length; i++) {
                if(toolsButtons[i]==(sender as Control)) {
                    toolsButtons[i].BackColor = defaultColor2;
                    continue;
                }
                try {
                    toolsButtons[i].BackColor = (Color)toolsButtons[i].Tag;
                }
                catch  {}
            }
        }

        private void SetMainPicherBox(Bitmap btm = null) {
            pictureBoxMain.Parent = panel3;
            pictureBoxMain.Left = (int)indent;
            pictureBoxMain.Top = (int)indent;
            pictureBoxMain.Width = 1100;
            pictureBoxMain.Height = 500;
            colorForPaint1 = Color.Black;
            colorForPaint2 = Color.White;
            defaultColor = Color.Linen;
            defaultColor2 = Color.LightGray;
            pictureBoxMain.BackColor = colorForPaint2;

            pictureBoxMain.Image = btm ?? new Bitmap(pictureBoxMain.Width, pictureBoxMain.Height);
            if (btm == null) {
                Graphics g = Graphics.FromImage(pictureBoxMain.Image);
                g.Clear(colorForPaint2);
                g.Dispose();
            }
            mainBitmap = new Bitmap(pictureBoxMain.Image);
            gridBitmap = Grid.CreateGridImage(pictureBoxMain.Width, pictureBoxMain.Height, indent * 2);

            SetMainBitmapOnScreen(withGrid);
            SetMainPen();
            SetMainBrush();

            toolStripStatusLabelX.Visible = false;
            toolStripStatusLabelY.Visible = false;
        }

        private void pictureBoxMain_MouseDown(object sender, MouseEventArgs e) {

            if (isPipetka) {
                GetPixelColor(e);
                return;
            }
            mainBitmap = (Bitmap)graphical?.WhenMouseDown(mainBitmap, e, mainPen, mainBrush);
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                SetMainBitmapOnScreen(withGrid);
        }

        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e) {
            ShowCursorLocation(e.Location, deltaMouseLocation);

            if (isPipetka) return;
            mainBitmap = (Bitmap)graphical?.WhenMouseMove(mainBitmap, e, mainPen, mainBrush);
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                SetMainBitmapOnScreen(withGrid);
        }

        private void pictureBoxMain_MouseUp(object sender, MouseEventArgs e) {
            if (isPipetka) {
                isPipetka = false;
                SelectCurentButton(previous);
                return;
            }
            mainBitmap = (Bitmap)graphical?.WhenMouseUp(mainBitmap, e, mainPen, mainBrush);
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                SetMainBitmapOnScreen(withGrid);
        }

        private void pictureBoxMain_MouseDoubleClick(object sender, MouseEventArgs e) {

            mainBitmap = (Bitmap)graphical?.WhenMouseDoubleClick(mainBitmap, e, mainPen, mainBrush);
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                SetMainBitmapOnScreen(withGrid);
        }

        private void GetPixelColor(MouseEventArgs e) {

            if (e.Button == MouseButtons.Left) {
                try {
                    colorForPaint1 = mainBitmap.GetPixel(e.X, e.Y);
                    mainPen.Color = colorForPaint1;
                }
                catch { }
                buttonColor1.Invalidate();
                buttonColor1.Select();
            }
            else if (e.Button == MouseButtons.Right) {
                try {
                    colorForPaint2 = mainBitmap.GetPixel(e.X, e.Y);
                    mainBrush = new SolidBrush(colorForPaint2);
                }
                catch { }
                buttonColor2.Invalidate();
                buttonColor2.Select();
            }
        }

        private void SetMainBitmapOnScreen(bool withGrid) {

            if (withGrid) {
                pictureBoxMain.BackgroundImage = mainBitmap;
                pictureBoxMain.Image = gridBitmap;
            }
            else
                pictureBoxMain.Image = mainBitmap;
        }

        private void SetScales() {
            
            ScaleHBmp = ScaleSetter.GetScaleH(pictureBoxScaleH.Width, pictureBoxScaleH.Height,
                indent, pictureBoxScaleH.BackColor, Color.Black);
            ScaleVBmp = ScaleSetter.GetScaleV(pictureBoxScaleV.Width, pictureBoxScaleV.Height,
                indent, pictureBoxScaleV.BackColor, Color.Black);

            pictureBoxScaleH.BackgroundImage = ScaleHBmp;
            pictureBoxScaleV.BackgroundImage = ScaleVBmp;
            pictureBoxScaleH.Image = new Bitmap(ScaleHBmp);
            pictureBoxScaleV.Image = new Bitmap(ScaleVBmp);
        }

        private void SetGroupBoxColors() {
            groupBoxColors.Parent = tabPageMain;
            groupBoxColors.Location = new Point(groupBoxSize.Right, 0);
        }

        private void SetMainBrush() {
            mainBrush = new SolidBrush(colorForPaint2);
            buttonColor2.Invalidate();
        }

        private void SetMainPen() {
            mainPen = new Pen(colorForPaint1);
            mainPen.LineJoin = LineJoin.Round; // округляем линии пера 
            mainPen.EndCap = LineCap.Round;
            mainPen.StartCap = LineCap.Round;

            buttonColor1.Invalidate();
        }

        private void SetButtonFillContextMenu() {
            ContextMenuSetting.CreateContextMenu(buttonFill, buttonFillContextMenuItems);
            ContextMenuSetting.SubScribeTsmiToClick(buttonFill, ButtonFillTsmi_Click);
            ContextMenuSetting.SubScribeContextMenuStripToOpened(buttonFill, ButtonFillContextMenuStrip_Opened);
        }

        private void ButtonFillContextMenuStrip_Opened(object sender, EventArgs e) {
            if (withFill) {
                buttonFill.ContextMenuStrip.Items[1].BackColor= Color.LightBlue;
                buttonFill.ContextMenuStrip.Items[0].BackColor = Color.Transparent;
            }
            else {
                buttonFill.ContextMenuStrip.Items[0].BackColor = Color.LightBlue;
                buttonFill.ContextMenuStrip.Items[1].BackColor = Color.Transparent;
            }
        }

        private void buttonFill_MouseClick(object sender, MouseEventArgs e) {
            Button button = sender as Button;
            button.ContextMenuStrip.Show(button, e.Location);
        }

        private void ButtonFillTsmi_Click(object sender, EventArgs e) {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            switch (tsmi.Text) {
                case "без заливки":
                    withFill = false;
                    break;
                case "однотонный цвет":
                    withFill = true;
                    break;
            }
            if (graphical is IFillable)
                (graphical as IFillable).WithFill = withFill;
        }

        private void buttonSize_Click(object sender, EventArgs e) {
            buttonSize.ContextMenuStrip.Show(buttonSize, buttonSize.Location);
        }

        private void SetButtonSizeContextMenu() {
            ContextMenuSetting.CreateContextMenu(buttonSize, PenSizes);
            ContextMenuSetting.SubScribeTsmiToClick(buttonSize, ButtonSizeTsmi_Click);
            ContextMenuSetting.SubScribeContextMenuStripToOpened(buttonSize, ButtonSizeContextMenuStrip_Opened);
            
        }

        private void ButtonSizeContextMenuStrip_Opened(object sender, EventArgs e) {
            for (int i = 0; i < PenSizes.Length; i++) {
                if (buttonSize.ContextMenuStrip.Items[i].Text == mainPen.Width.ToString()) {
                    buttonSize.ContextMenuStrip.Items[i].BackColor = Color.LightBlue;
                }
                else buttonSize.ContextMenuStrip.Items[i].BackColor = Color.Transparent;
            }
        }

        private void ButtonSizeTsmi_Click(object sender, EventArgs e) {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            int.TryParse(tsmi.Text, out int s);
            mainPen.Width = s;
        }
        
        private void ShowCursorLocation(Point location, Point delta) {
            toolStripStatusLabelX.Visible = true;
            toolStripStatusLabelY.Visible = true;
            toolStripStatusLabelX.Text = $"{location.X},";
            toolStripStatusLabelY.Text = $"{location.Y}пкс";


            SetCursorLocationH(location.X + delta.X);
            SetCursorLocationV(location.Y + delta.Y);
            
        }

        private void SetCursorLocationV(int locationY) {
            float k = indent * 1.7f;
            pictureBoxScaleV.Image = ScaleSetter.CursorLocationOnScaleV(
               pictureBoxScaleV.Image, locationY + (int)k);
        }

        private void SetCursorLocationH(int locationX) {
            float k = indent * 1.7f;
            pictureBoxScaleH.Image = ScaleSetter.CursorLocationOnScaleH(
               pictureBoxScaleH.Image, locationX + (int)k);
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e) {

            pictureBoxScaleH.Image = ScaleSetter.CursorLocationOnScaleH(
               pictureBoxScaleH.Image, e.Location.X);
            pictureBoxScaleV.Image = ScaleSetter.CursorLocationOnScaleV(
               pictureBoxScaleV.Image, e.Location.Y);
        }

        private void pictureBoxMain_MouseLeave(object sender, EventArgs e) {
            toolStripStatusLabelX.Visible = false;
            toolStripStatusLabelY.Visible = false;
        }
        
        private void pictureBoxBuf_Paint(object sender, PaintEventArgs e) {
            PrintTextInCenter("Буфер обмена", sender, e);
        }

        private void buttonSelect_Paint(object sender, PaintEventArgs e) {
            Control control = sender as Control;
            control.Text = "▾";
            Graphics g = e.Graphics;
            int w = (int)(control.Width * 0.6);
            int h = (int)(w * 0.8);
            int l = (control.Width - w) / 2;
            int t = (control.Height - h) / 3;
            Rectangle rec = new Rectangle(l, t, w, h);
            Pen p = new Pen(Brushes.Black, 1);
            p.DashStyle = DashStyle.DashDot;
            g.DrawRectangle(p, rec);
        }

        private void pictureBoxImage_Paint(object sender, PaintEventArgs e) {
            PrintTextInCenter("Изображение", sender, e);
        }

        private void PrintTextInCenter(string str, object sender, PaintEventArgs e) {
            Control control = sender as Control;
            Graphics g = e.Graphics;
            Font font = new Font("", indent * 1.7f, FontStyle.Regular);
            SizeF numSize = g.MeasureString(str, font);

            g.DrawString(str, font, Brushes.Black, (control.Width - numSize.Width) / 2, indent);
        }

        private void buttonResize_Paint(object sender, PaintEventArgs e) {
            Control control = sender as Control;
            control.Text = "";
            Graphics g = e.Graphics;
            Pen p = new Pen(Brushes.Black, 1);

            Rectangle rec = new Rectangle(10, 12, 10, 8);
            g.DrawRectangle(p, rec);
            rec = new Rectangle(15, 5, 12, 10);
            g.FillRectangle(Brushes.White, rec); ;
            g.DrawRectangle(p, rec);
        }

        private void pictureBoxTools_Paint(object sender, PaintEventArgs e) {
            PrintTextInCenter("Инструменты", sender, e);
        }
        
        private void buttonSize_Paint(object sender, PaintEventArgs e) {
            Control control = sender as Control;
            Graphics g = e.Graphics;
            Pen pen = new Pen(Brushes.Black, 1);
            
            for (int i = 0; i < 5; i++) {
                Point point1 = new Point((int)indent, (int)(indent + indent * i + pen.Width));
                Point point2 = new Point((int)(control.Width - indent), (int)(indent + indent * i + pen.Width));
                g.DrawLine(pen, point1, point2);
                pen.Width++;
            }
        }
        
        private void DrawRecOnButton(object sender, PaintEventArgs e, Color color) {
            Control control = sender as Control;
            Graphics g = e.Graphics;
            float w, h;
            h = w = control.Width - 4 * indent;
            
            Rectangle rec = new Rectangle((int)indent * 2, (int)indent * 2, (int)w, (int)h);
            Brush brush = new SolidBrush(color);
            g.FillRectangle(brush, rec);
            g.DrawRectangle(new Pen(Color.Black, 1), rec);
        }
        

        private void buttonSelect_Click(object sender, EventArgs e) {
            graphical = CreatorIGraphicalObj.CrSelectingObj();
            buttonContour.Enabled = false;
            buttonFill.Enabled = false;
            buttonSize.Enabled = false;
            SelectCurentButton(sender);
            previous = (Control)sender;
        }

       

        private void buttonTolls1_Click(object sender, EventArgs e) {
            SetModePencil();
            SelectCurentButton(sender);
            previous = (Control)sender;
        }

        private void SetModePencil() {
            graphical = CreatorIGraphicalObj.CrDrawingByPencilObj();
            buttonContour.Enabled = false;
            buttonFill.Enabled = false;
            buttonSize.Enabled = true;
        }

        private void buttonTolls2_Click(object sender, EventArgs e) {
            graphical = CreatorIGraphicalObj.CrFillingAreaObj();
            buttonContour.Enabled = false;
            buttonFill.Enabled = false;
            buttonSize.Enabled = false;
            SelectCurentButton(sender);
            previous = (Control)sender;
        }
        
        private void buttonTolls5_Click(object sender, EventArgs e) {
            isPipetka = true;
            SelectCurentButton(sender);
        }

        //--------------------panel3_Scroll----------------------

        private void panel3_Scroll(object sender, ScrollEventArgs e) {
            Point p = panel3.AutoScrollPosition;
            deltaMouseLocation = p;
            ToShiftScaleH(p.X);
            ToShiftScaleV(p.Y);
        }

        private void ToShiftScaleV(int y) {
            if (ScaleVBmp == null) return;

            panelScaleV.AutoScrollPosition = new Point(0, -y);
            Bitmap bp = new Bitmap(ScaleVBmp);
            Graphics g = Graphics.FromImage(bp);
            g.DrawImageUnscaled(ScaleVBmp, 0, y);
            pictureBoxScaleV.BackgroundImage = bp;
        }

        private void ToShiftScaleH(int x) {
            if (ScaleHBmp == null) return;

            panelScaleH.AutoScrollPosition = new Point(-x, 0);
            Bitmap bp = new Bitmap(ScaleHBmp);
            Graphics g = Graphics.FromImage(bp);
            g.DrawImageUnscaled(ScaleHBmp, x, 0);
            pictureBoxScaleH.BackgroundImage = bp;
        }

        private void pictureBoxScaleH_Resize(object sender, EventArgs e) {
            Point p = panel3.AutoScrollPosition;
            deltaMouseLocation = p;
            ToShiftScaleH(p.X);
        }

        private void pictureBoxScaleV_Resize(object sender, EventArgs e) {
            Point p = panel3.AutoScrollPosition;
            deltaMouseLocation = p;
            ToShiftScaleV(p.Y);
        }

        //----------------------Colors----------------------------------

        private void InitUserColors() {
            userColors = new UserColors(new PictureBox[]{
                pbColor21,
                pbColor22,
                pbColor23,
                pbColor24,
                pbColor25,
                pbColor26,
                pbColor27,
                pbColor28,
                pbColor29,
                pbColor30}
            );
        }

        private void buttonEditPalette_Click(object sender, EventArgs e) {
            using (ColorDialog cd = new ColorDialog()) {
                if (cd.ShowDialog() == DialogResult.OK)
                    userColors.AddColor(cd.Color);
            }
        }

        private void buttonColor1_Click(object sender, EventArgs e) {
            buttonColor1.BackColor = defaultColor2;
            buttonColor2.BackColor = defaultColor;
            buttonColorFlag = true;
        }

        private void buttonColor2_Click(object sender, EventArgs e) {
            buttonColor2.BackColor = defaultColor2;
            buttonColor1.BackColor = defaultColor;
            buttonColorFlag = false;
        }

        private void pbColors_Click(object sender, EventArgs e) {
            Control control = sender as Control;

            if (buttonColorFlag) {
                colorForPaint1 = control.BackColor;
                buttonColor1.Invalidate();
                buttonColor1.Select();
                mainPen.Color = control.BackColor;
            }
            else {
                colorForPaint2 = control.BackColor;
                buttonColor2.Invalidate();
                buttonColor2.Select();
                SetMainBrush();
            }
        }

        private void pictureBoxColors_Paint(object sender, PaintEventArgs e) {
            PrintTextInCenter("Цвета", sender, e);
        }

        private void pbColor_MouseEnter(object sender, EventArgs e) {
            (sender as PictureBox).BorderStyle = BorderStyle.FixedSingle;
        }

        private void pbColor_MouseLeave(object sender, EventArgs e) {
            (sender as PictureBox).BorderStyle = BorderStyle.None;
        }

        private void buttonColor1_Paint(object sender, PaintEventArgs e) {
            DrawRecOnButton(sender, e, colorForPaint1);
        }

        private void buttonColor2_Paint(object sender, PaintEventArgs e) {
            DrawRecOnButton(sender, e, colorForPaint2);
        }

        //----------------------Shapes-----------------------

        private void pictureBoxShapes_Paint(object sender, PaintEventArgs e) {
            PrintTextInCenter("Фигуры", sender, e);
        }

        private void buttonShapes1_Paint(object sender, PaintEventArgs e) {
            Control control = sender as Control;
            Graphics g = e.Graphics;
            Point point1 = new Point((int)indent, (int)indent);
            Point point2 = new Point((int)(control.Width - indent), (int)(control.Height - indent));
            g.DrawLine(buttonShapesPen, point1, point2);
        }

        private void buttonShapes2_Paint(object sender, PaintEventArgs e) {
            Control control = sender as Control;
            Graphics g = e.Graphics;
            Point point1 = new Point((int)indent, (int)(control.Height - indent));
            Point point2 = new Point(control.Width / 4, 0);
            Point point3 = new Point(control.Width / 4 * 3, (int)(control.Height * 1.3));
            Point point4 = new Point((int)(control.Width - indent), (int)indent);
            g.DrawBezier(buttonShapesPen, point1, point2, point3, point4);
        }

        private void buttonShapes3_Paint(object sender, PaintEventArgs e) {
            Control control = sender as Control;
            Graphics g = e.Graphics;
            float w, h;
            w = control.Width - 2 * indent;
            h = control.Height - 2 * indent;
            RectangleF rec = new RectangleF(indent, indent, w, h);
            g.DrawEllipse(buttonShapesPen, rec);
        }

        private void buttonShapes4_Paint(object sender, PaintEventArgs e) {
            Control control = sender as Control;
            Graphics g = e.Graphics;
            float w, h;
            w = control.Width - 2 * indent;
            h = control.Height - 2 * indent;
            Rectangle rec = new Rectangle((int)indent, (int)indent, (int)w, (int)h);
            g.DrawRectangle(buttonShapesPen, rec);
        }

        private void buttonShapes5_Paint(object sender, PaintEventArgs e) {
            Control control = sender as Control;
            Graphics g = e.Graphics;
            float w, h;
            w = control.Width - 2 * indent;
            h = control.Height - 2 * indent;
            Rectangle rec = new Rectangle((int)indent, (int)indent, (int)w, (int)h);

            DrawFigures.RoundedRectangle(rec, g, buttonShapesPen, Brushes.Navy, false);
        }

        private void buttonShapes6_Paint(object sender, PaintEventArgs e) {
            Control control = sender as Control;
            Graphics g = e.Graphics;
            GraphicsPath path = new GraphicsPath();
            Point point1 = new Point((int)indent, (int)(control.Height - indent));
            Point point2 = new Point((int)(indent * 1.5), (int)indent);
            Point point3 = new Point((int)(control.Width - indent * 1.5), (int)indent);
            Point point4 = new Point((int)(control.Width - indent * 2), (int)indent * 2);
            Point point5 = new Point((int)(control.Width - indent), (int)indent * 2);
            Point point6 = new Point((int)(control.Width - indent), (int)(control.Height - indent));
            path.AddLines(new Point[] { point1, point2, point3, point4, point5, point6 });
            path.CloseFigure();
            g.DrawPath(buttonShapesPen, path);
        }

        private void buttonShapes7_Paint(object sender, PaintEventArgs e) {
            Control control = sender as Control;
            Graphics g = e.Graphics;

            float w, h;
            w = control.Width - 2 * indent;
            h = control.Height - 2 * indent;
            Rectangle rec = new Rectangle((int)indent, (int)indent, (int)w, (int)h);
            DrawFigures.IsoscelesTriangle(rec, g, buttonShapesPen, Brushes.Navy, false);
        }

        private void buttonShapes1_Click(object sender, EventArgs e) {
            graphical = CreatorIGraphicalObj.CrDrawingLineObj();
            buttonContour.Enabled = true;
            buttonFill.Enabled = false;
            buttonSize.Enabled = true;
            SelectCurentButton(sender);
            previous = (Control)sender;
        }

        private void buttonShapes2_Click(object sender, EventArgs e) {
            graphical = CreatorIGraphicalObj.CrDrawingBezierCurveObj();
            buttonContour.Enabled = true;
            buttonFill.Enabled = false;
            buttonSize.Enabled = true;
            SelectCurentButton(sender);
            previous = (Control)sender;
        }

        private void buttonShapes3_Click(object sender, EventArgs e) {
            graphical = CreatorIGraphicalObj.CrDrawingEllipceObj();
            if (graphical is IFillable)
                (graphical as IFillable).WithFill = withFill;
            buttonContour.Enabled = true;
            buttonFill.Enabled = true;
            buttonSize.Enabled = true;
            SelectCurentButton(sender);
            previous = (Control)sender;
        }

        private void buttonShapes4_Click(object sender, EventArgs e) {
            graphical = CreatorIGraphicalObj.CrDrawingRectObj();
            if (graphical is IFillable)
                (graphical as IFillable).WithFill = withFill;
            buttonContour.Enabled = true;
            buttonFill.Enabled = true;
            buttonSize.Enabled = true;
            SelectCurentButton(sender);
            previous = (Control)sender;
        }

        private void buttonShapes5_Click(object sender, EventArgs e) {
            graphical = CreatorIGraphicalObj.CrDrawingRoundedRectObj();
            if (graphical is IFillable)
                (graphical as IFillable).WithFill = withFill;
            buttonContour.Enabled = true;
            buttonFill.Enabled = true;
            buttonSize.Enabled = true;
            SelectCurentButton(sender);
            previous = (Control)sender;
        }

        private void buttonShapes6_Click(object sender, EventArgs e) {
            graphical = CreatorIGraphicalObj.CrDrawingPoligonObj();
            if (graphical is IFillable)
                (graphical as IFillable).WithFill = withFill;
            buttonContour.Enabled = true;
            buttonFill.Enabled = true;
            buttonSize.Enabled = true;
            SelectCurentButton(sender);
            previous = (Control)sender;
        }
        
        private void buttonShapes7_Click(object sender, EventArgs e) {
            graphical = CreatorIGraphicalObj.CrDrawingIsoscelesTriangleObj();
            if (graphical is IFillable)
                (graphical as IFillable).WithFill = withFill;
            buttonContour.Enabled = true;
            buttonFill.Enabled = true;
            buttonSize.Enabled = true;
            SelectCurentButton(sender);
            previous = (Control)sender;
        }

        // -------------------- Files IO ---------------------------

        private void SetFileDialogFilter() {
            fileDialogFilter = new string[] {
                "Bitmap (.bmp)|*.bmp",
                "Gif (.gif)|*.gif",
                "JPEG (.jpeg)|*.jpeg",
                "Png (.png)|*.png",
                "Tiff (.tiff)|*.tiff",
           };
        }

        private void InitSaveAsToolStripMenuItem() {
            for (int i = 0; i < fileDialogFilter.Length; i++) {
                string s = fileDialogFilter[i].Split('|')[0];
                saveAsToolStripMenuItem.DropDownItems.Add(s);
                saveAsToolStripMenuItem.DropDownItems[i].Tag = i;
                saveAsToolStripMenuItem.DropDownItems[i].Click += SaveAsToolStripMenuItemDropDownItem_Click;
            }
        }

        private void SaveAsToolStripMenuItemDropDownItem_Click(object sender, EventArgs e) {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            Save((int)item.Tag + 1);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e) {
            Save();
        }

        private void Save(int filterIndex = 1) {
            if (pictureBoxMain.Image == null) return;

            using (SaveFileDialog sfd = new SaveFileDialog()) {
                sfd.Title = "Сохранить картинку как...";
                sfd.OverwritePrompt = true;
                sfd.CheckPathExists = true;
                sfd.Filter = string.Join("|", fileDialogFilter);
                sfd.FilterIndex = filterIndex;
                sfd.ShowHelp = true;
                if (sfd.ShowDialog() == DialogResult.OK) {
                    try {
                        mainBitmap.Save(sfd.FileName);
                    }
                    catch {
                        MessageBox.Show("Не получилось сохранить файл",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!DoYouWantToSave()) return;
            Open();
        }

        private void Open() {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.Title = "Открыть файл";
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                // добавляем к списку расширений "All files"
                string[] newFileDialogFilter = new string[fileDialogFilter.Length + 1];
                newFileDialogFilter[fileDialogFilter.Length] = "All files (*.*)|*.*";
                Array.Copy(fileDialogFilter, newFileDialogFilter, fileDialogFilter.Length);

                ofd.Filter = string.Join("|", newFileDialogFilter);
                ofd.FilterIndex = newFileDialogFilter.Length;
                ofd.ShowHelp = true;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    try {
                        SetMainPicherBox(new Bitmap(ofd.FileName));
                    }
                    catch {
                        MessageBox.Show("Не получилось открыть файл",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!DoYouWantToSave()) return;
            Close();
        }

        private void SaveAsToolStripMenuItem_MouseMove(object sender, MouseEventArgs e) {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            item.ShowDropDown();
        }

        private void CreateToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (!DoYouWantToSave()) return;
            SetMainPicherBox();
        }

        private void checkBoxGrid_Click(object sender, EventArgs e) {
            withGrid = (sender as CheckBox).Checked;
            SetMainBitmapOnScreen(withGrid);

        }

        private void FMain_Load(object sender, EventArgs e) {
            buttonTolls1.PerformClick();
            buttonColor1.PerformClick();
            SetScales();
        }

        private bool DoYouWantToSave() {
            var result = MessageBox.Show("Сохранить изменения?", "Paint", MessageBoxButtons.YesNoCancel);
            switch (result) {
                case DialogResult.Yes:
                    Save();
                    return true;
                case DialogResult.No:
                    return true;
                default:
                    return false;
            }
        }
    }
}
