using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassObject;


namespace LeThiKimLe_20110248_Bai1
{
    public partial class Form1 : Form
    {
        Bitmap bm;
        Graphics g;
        bool paint = false;
        Point px, py;
        Pen penShape, penDraw;
        Brush shapeBrush;
        Pen eraser = new Pen(Color.White, 10);
        int index=0;
        
        int x, y, sX, sY, cX, cY;
    
        List<Control> list_control=new List<Control>();
        ColorDialog cd = new ColorDialog();
        Color pen_color;
        List<Hinh> list_hinh = new List<Hinh>();
        Hinh newhinh;
        bool hinhmoi = false;
        List<Hinh> chosen_list = new List<Hinh>();
        List<Brush> list_brush = new List<Brush>();


        public Form1()
        {
            InitializeComponent();
            bm = new Bitmap(pic.Width, pic.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            pic.Image = bm;
            panel_Shape.Visible = false;
            Pen_Init();
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag=="panel")
                {
                    list_control.Add(x);
                }

            }
            listBrush.SelectedIndex = 0;
            panel_Brush.Parent = this;
            panel_Pen.Parent = this;
            panel_Shape.Parent = this;
            btn_bkColor.BackColor = Color.Blue;
            btn_LineColor.BackColor = Color.White;
        }

        private void off_panel()
        {
            foreach(Control x in list_control)
            {
                if (x.Visible==true)
                    x.Visible = false;
            }
        }

        private void Pen_Init()
        {
            penShape = new Pen(Color.Blue, 1);
            penShape.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            penDraw = new Pen(Color.Black, 1);
            pic_Color.BackColor = penDraw.Color;
            shapeBrush = new SolidBrush(Color.Blue);
        }
        private void btn_Shape_Click(object sender, EventArgs e)
        {
            off_panel();
            panel_Shape.Visible = true;
        }

        private void btn_Ellipse_Click(object sender, EventArgs e)
        {
            index = 6;
            btn_Shape.Image = btn_Ellipse.Image;
            panel_Shape.Visible = false;
            hinhmoi = true;
        }

        private void btn_Rect_Click(object sender, EventArgs e)
        {
            index = 4;
            btn_Shape.Image = btn_Rect.Image;
            panel_Shape.Visible = false;
            hinhmoi = true;
        }

        private void btn_Line_Click(object sender, EventArgs e)
        {

            index = 3;
            btn_Shape.Image = btn_Line.Image;
            panel_Shape.Visible = false;
            hinhmoi = true;

        }

        private void btn_Square_Click(object sender, EventArgs e)
        { 
            index = 5;
            btn_Shape.Image = btn_Square.Image;
            panel_Shape.Visible = false;
            hinhmoi = true;
        }

        private void btn_Circle_Click(object sender, EventArgs e)
        {
            index = 7;
            btn_Shape.Image = btn_Circle.Image;
            panel_Shape.Visible = false;
            hinhmoi = true;
        }

        private void btn_Curve_Click(object sender, EventArgs e)
        {
            index = 8;
            btn_Shape.Image = btn_Curve.Image;
            panel_Shape.Visible = false;
            hinhmoi = true;
        }

        private void btn_Polygon_Click(object sender, EventArgs e)
        {
            index = 9;
            btn_Shape.Image = btn_Polygon.Image;
            panel_Shape.Visible = false;
            hinhmoi = true;
        }

        private void pic_Click(object sender, EventArgs e)
        {
            off_panel();
            foreach (Hinh h in chosen_list)
            {
                h.RemoveFrame();
                h.contain.Refresh();
            }
            chosen_list.Clear();
            
        }
        private void check_hinh(Point click_pos)
        {
            if (index == 11)
                foreach (Hinh h in list_hinh)
                {
                    if (h.contain.Location.X == click_pos.X && h.contain.Location.Y == click_pos.Y)
                    {
                        Modify_Brush(h);
                        Get_Brush(h);
                        h.signal = 1;
                        h.contain.Refresh();
                        h.To_lai();
                        break;
                    }
                }
            else
            {
                foreach (Hinh h in list_hinh)
                {
                    if (h.contain.Location.X == click_pos.X && h.contain.Location.Y == click_pos.Y)
                    {
                        if (Form1.ModifierKeys == Keys.Control)
                        {
                            if (chosen_list.Contains(h) == false)
                            {
                                chosen_list.Add(h);
                                h.AddFrame();
                                h.contain.Refresh();
                            }
                        }
                        else
                        {
                            foreach (Hinh k in chosen_list)
                            {
                                k.RemoveFrame();
                                k.contain.Refresh();
                            }
                            chosen_list.Clear();
                            chosen_list.Add(h);
                            h.AddFrame();
                            h.contain.Refresh();
                        }
                        break;
                    }
                        
                }
            }
        }
        
        private void pic_Paint(object sender, PaintEventArgs e)
        {
            Graphics grap = e.Graphics;
            if (paint)
            {
                if (index >= 3 && index < 10)
                {
                    newhinh.gp = grap;
                    newhinh.Ve();
                    newhinh.gp = g;
                }
               
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            off_panel();
            g.Clear(Color.White);
            pic.Image = bm;
            index = 0;
            foreach (Hinh h in list_hinh)
            {
                h.contain.Dispose();
            }
            list_hinh.Clear();

        }

        private void btn_Color_Click(object sender, EventArgs e)
        {
            off_panel();
            cd.ShowDialog();
            pen_color = cd.Color;
            pic_Color.BackColor = cd.Color;
            penDraw.Color = cd.Color;
        }
        private void Get_Pen(Hinh hinh)
        {
            hinh.nowPair.myPen.Color = penShape.Color;
            hinh.nowPair.myPen.Width = penShape.Width;
            hinh.nowPair.myPen.DashStyle = penShape.DashStyle;
        }

        private void Add_Brush()
        {
            if (shapeBrush is SolidBrush)
                list_brush.Add(new SolidBrush(btn_bkColor.BackColor));
            else
                list_brush.Add(new HatchBrush(HatchStyle.DarkHorizontal, btn_bkColor.BackColor, btn_LineColor.BackColor));
        }

        private void Get_Brush(Hinh hinh)
        {
            hinh.nowPair.myBrush = list_brush[list_hinh.IndexOf(hinh)];
        }

        private void Modify_Brush(Hinh hinh)
        {

            if (shapeBrush is SolidBrush)
                shapeBrush=new SolidBrush(btn_bkColor.BackColor);
            else
                shapeBrush=new HatchBrush(HatchStyle.DarkHorizontal, btn_bkColor.BackColor, btn_LineColor.BackColor);

            list_brush[list_hinh.IndexOf(hinh)] = shapeBrush;

        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            
            py = e.Location;
            cX = e.X;
            cY = e.Y;
            paint = true;

            if (hinhmoi==true)
            {
                if (index==3)
                    newhinh = new Hinh(e.Location);
                if (index == 4)
                    newhinh = new HinhChuNhat(e.Location);
                else if (index == 5)
                    newhinh = new HinhVuong(e.Location);
                else if (index == 6)
                    newhinh = new HinhEllipse(e.Location);
                else if (index == 7)
                    newhinh = new HinhTron(e.Location);
                else if (index == 8)
                    newhinh = new DuongCong(e.Location);
                else if (index == 9)
                    newhinh = new Dagiac(e.Location);
                newhinh.gp = g;
                Get_Pen(newhinh);
                Add_Brush();
                newhinh.controller = this;
                newhinh.bigContain = pic;
                hinhmoi = false;
            }
        }

        private void color_picker_MouseClick(object sender, MouseEventArgs e)
        {
            off_panel();
            Point point = set_point(color_picker, e.Location);
            pic_Color.BackColor = ((Bitmap)color_picker.Image).GetPixel(point.X, point.Y);
            pen_color = pic_Color.BackColor;
            penDraw.Color = pic_Color.BackColor;
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if(paint)
            {
                if (index==1)
                {
                    px = e.Location;
                    g.DrawLine(penDraw, px, py);
                    py = px;
                }
                if (index == 2)
                {
                    px = e.Location;
                    g.DrawLine(eraser, px, py);
                    py = px;
                }
                else if (index >=3 && index < 10)
                {
                    x = e.X;
                    y = e.Y;
                    newhinh.GetPoint(new Point(x, y));
                }
                pic.Refresh();
            }
           
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            if (index >= 3 && index<10)
            {
                newhinh.Ve();
                Add_newhinh();
                hinhmoi = true;
                g.Clear(Color.White);
                if (list_hinh.Count > list_brush.Count)
                    Add_Brush();
                Get_Brush(newhinh);
            }
        }

        private void Add_newhinh()
        {
            newhinh.Put_PictureBox();
            newhinh.contain.MouseClick += new MouseEventHandler(Fill_click);
            newhinh.contain.MouseDown += new MouseEventHandler(Drag);
            newhinh.contain.MouseMove += new MouseEventHandler(Drop);
            list_hinh.Add(newhinh);
        }
        Point local;
        private void Drag(object sender, MouseEventArgs e)
        {
            if (index==12)
                if (e.Button == MouseButtons.Left)
                {
                    local = e.Location;
                }
        }

        private void Drop(object sender, MouseEventArgs e)
        {
            if (index == 12)
            {
                PictureBox hinh = sender as PictureBox;
                if (e.Button == MouseButtons.Left)
                {
                    hinh.Left = e.X + hinh.Left - local.X;
                    hinh.Top = e.Y + hinh.Top - local.Y;
                }
            }
        }

        private void Fill_click(object sender, MouseEventArgs e)
        {
            if (index == 11 || index==12)
            {
                PictureBox picture = sender as PictureBox;
                check_hinh(picture.Location);
            }
        }

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            off_panel();
            if (index == 10)
            {
                Point point = set_point(pic, e.Location);
                Fill(bm, point.X, point.Y, pen_color);
            }
           
        }

        private void btn_Fill_Click(object sender, EventArgs e)
        {
            off_panel();
            index = 10;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            off_panel();
            var sfd = new SaveFileDialog();
            sfd.Filter = "Image(*.jpg)|*.jpg|(*.*|*.*";
            if(sfd.ShowDialog()==DialogResult.OK)
            {
                Bitmap btm = bm.Clone(new Rectangle(0, 0, pic.Width, pic.Height), bm.PixelFormat);
                btm.Save(sfd.FileName, ImageFormat.Jpeg);
                MessageBox.Show("Saved");
            }
        }

        private void btn_Brush_Click(object sender, EventArgs e)
        {
            off_panel();
            panel_Brush.BringToFront();
            index = 11;
            panel_Brush.Visible = true;
            pic.Cursor = Cursors.PanSW;
        }

        private void btn_Penn_Click(object sender, EventArgs e)
        {
            off_panel();
            panel_Pen.Visible = true;
            panel_Pen.BringToFront();
        }

        private void textWidth_TextChanged(object sender, EventArgs e)
        {
            if (text_Width.Text == "" || text_Width.Text == "0")
                penShape.Width = 1;
            else
                penShape.Width = Int32.Parse(text_Width.Text);
        }

        private void listPen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listPen.SelectedIndex==0)
                penShape.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            else if (listPen.SelectedIndex == 1)
                penShape.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            else if (listPen.SelectedIndex == 2)
                penShape.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
        }

        private void btn_BbkColor_Click(object sender, EventArgs e)
        {
            cd.ShowDialog();
            pen_color = cd.Color;
            btn_bkColor.BackColor = cd.Color;
            off_panel();
        }

        private void btn_BLColor_Click(object sender, EventArgs e)
        {
            cd.ShowDialog();
            pen_color = cd.Color;
            btn_LineColor.BackColor = cd.Color;
            off_panel();
        }

        private void listBrush_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBrush.SelectedIndex == 0)
            {
                shapeBrush = new SolidBrush(btn_bkColor.BackColor);
            }
            else if (listBrush.SelectedIndex == 1)
                shapeBrush = new HatchBrush(HatchStyle.Horizontal, btn_LineColor.BackColor, btn_bkColor.BackColor);
            off_panel();
        }

        private void btn_DeleteS_Click(object sender, EventArgs e)
        {
            foreach (Hinh h in chosen_list)
            {
                h.contain.Dispose();
                list_hinh.Remove(h);
            }
        }

        private void move_Click(object sender, EventArgs e)
        {
            index = 12;
        }

        private void btn_Group_Click(object sender, EventArgs e)
        {
            index = 13;
            Group_hinh();
        }

        private void Group_hinh()
        {
            if (chosen_list.Count >= 2)
            {
                newhinh = new HinhPhucHop();
                newhinh.Get_hinh(chosen_list);
                newhinh.controller = this;
                newhinh.bigContain = pic;
                Add_newhinh();

                foreach (Hinh h in chosen_list)
                    h.RemoveFrame();

                chosen_list.Clear();
                chosen_list.Add(newhinh);
                newhinh.AddFrame();
            }
        }

        private void btn_Ungroup_Click(object sender, EventArgs e)
        {
            foreach (Hinh h in chosen_list)
                if (h is HinhPhucHop)
                {
                    h.Ungroup();
                    list_hinh.Remove(h);
                }
        }

        private void btn_PColor_Click(object sender, EventArgs e)
        {
            cd.ShowDialog();
            pen_color = cd.Color;
            btn_PeColor.BackColor = cd.Color;
            penShape.Color = cd.Color;
        }

        private void btn_Pen_Click(object sender, EventArgs e)
        {
            off_panel();
            index = 1;
            hinhmoi = false;
        }

        private void btn_Eraser_Click(object sender, EventArgs e)
        {
            off_panel();
            index = 2;
            hinhmoi = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        static Point set_point(PictureBox pb, Point pt)
        {
            float pX = 1f * pb.Image.Width / pb.Width;
            float pY = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
        }

        private void validate(Bitmap bm, Stack<Point> sp, int x, int y, Color old_color, Color new_color)
        {
            Color cx = bm.GetPixel(x, y);
            if (cx==old_color)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }
        }

        public void Fill(Bitmap bm, int x, int y, Color new_cir)
        {
            Color old_color = bm.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, new_cir);
            if (old_color == new_cir) return;
            while (pixel.Count>0)
            {
                Point pt = (Point)pixel.Pop();
                if(pt.X>0 && pt.Y>0 && pt.X<bm.Width-1 && pt.Y<bm.Height-1)
                {
                    validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_cir);
                    validate(bm, pixel, pt.X, pt.Y-1, old_color, new_cir);
                    validate(bm, pixel, pt.X+1, pt.Y, old_color, new_cir);
                    validate(bm, pixel, pt.X, pt.Y +1, old_color, new_cir);
                }
            }
        }

    }
}
