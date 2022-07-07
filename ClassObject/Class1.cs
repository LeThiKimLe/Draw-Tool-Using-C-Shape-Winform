using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ClassObject
{
    public class Pair
    {
        public Point Diem1 = new Point();
        public Point Diem2 = new Point();
        public Point Diem3;
        public Pen myPen = new Pen(Color.Red, 1);
        public Brush myBrush = new SolidBrush(Color.Blue);
        public Rectangle myRec;
        public Point recLocation=new Point(5,5);
        public List<Point> listpoint = new List<Point>();
        public Pen myFPen = new Pen(Color.Black, 2);
        public Pair() 
        {
            myFPen.DashStyle = DashStyle.Dash;
        }
        public void ConvertToRec()
        {
            Size newsize = new Size(Math.Abs(Diem1.X - Diem2.X), Math.Abs(Diem1.Y - Diem2.Y));
            if (Diem2.X > Diem1.X)
            {
                if (Diem2.Y > Diem1.Y)
                    recLocation = Diem1;
                else
                    recLocation = new Point(Diem1.X, Diem2.Y);
            }
            else
            {
                if (Diem2.Y > Diem1.Y)
                    recLocation = new Point(Diem2.X, Diem1.Y);
                else
                    recLocation = Diem2;
            }
            myRec = new Rectangle(recLocation, newsize);
        }
       
        public void ModifyRec()
        {
            if (myRec.Width < myRec.Height)
                myRec.Height = myRec.Width;
            else
                myRec.Width = myRec.Height;
        }
        public void GetCurve()
        {
            int x1 = (Diem1.X + Diem2.X) / 2;
            int y1 = (Diem1.Y + Diem2.Y) / 2;

            int u = Diem1.X - Diem2.X;
            int v = Diem1.Y - Diem2.Y;

            if (u == 0 || v == 0)
            {
                Diem3 = new Point(x1, y1);
                listpoint.Add(Diem3);
                listpoint.Add(Diem2);
                return;
            }

            int x;
            int y = 0;

            int dmax = (int)Math.Sqrt((Math.Abs(Diem1.X - Diem2.X)) * (Math.Abs(Diem1.X - Diem2.X)) + (Math.Abs(Diem1.Y - Diem2.Y)) * (Math.Abs(Diem1.Y - Diem2.Y))) / 2;
            int d = dmax + 1;
            int minx = Math.Min(Diem1.X, Diem2.X);
            int maxx = Math.Max(Diem1.X, Diem2.X);

            for (x = minx; x < maxx && d>dmax; x++)
            {
                y = -u * (x - x1) / v + y1;
                d= (int)Math.Sqrt((Math.Abs(x - x1)) * (Math.Abs(x - x1)) + (Math.Abs(y - y1)) * (Math.Abs(y - y1)));
            } 

            Diem3 = new Point(x, y);
            listpoint.Add(Diem3);
            listpoint.Add(Diem2);
        }
    }

    public class Hinh
    {
        public Pair nowPair=new Pair();
        public Graphics gp;
        public Point locate;
        public PictureBox contain;
        public PictureBox bigContain;
        public Graphics gp2;
        public Form controller;
        public int signal = 0;
        public int old = 0;
        public Bitmap bm;
        public Hinh() { }
        public List<Hinh> lstHinh = new List<Hinh>();
        public Hinh(Point first)
        {
            locate = first;
            nowPair.Diem1 = first;
            nowPair.listpoint.Add(first);
        }

        public void GetPoint(Point second)
        {
            nowPair.Diem2 = second;
            nowPair.ConvertToRec();
        }
        public virtual void Ve()
        {
            gp.DrawLine(nowPair.myPen, nowPair.Diem1, nowPair.Diem2);
        }
        
        public virtual void Put_PictureBox()
        {
            contain = new PictureBox();
            contain.Location = new Point(nowPair.myRec.Location.X-3, nowPair.myRec.Location.Y-3);
            contain.Size = new Size(nowPair.myRec.Width + 6, nowPair.myRec.Height + 6);
            bm = new Bitmap(contain.Size.Width, contain.Size.Height);
            contain.BorderStyle = BorderStyle.None;
            gp2 = Graphics.FromImage(bm);
            gp2.Clear(Color.White);
            contain.Image = bm;
            contain.SizeMode = PictureBoxSizeMode.StretchImage;
            contain.Paint += new PaintEventHandler(repaint_Paint);
            contain.MouseWheel += new MouseEventHandler(Zoom);
            controller.Controls.Add(contain);
            contain.Parent = bigContain;
            contain.Visible = true;
            contain.BackColor = Color.White;
            contain.BringToFront();
            contain.Refresh();
            Ve_lai();
        }
        public virtual void repaint_Paint(object sender, PaintEventArgs e) 
        {
            Graphics g = e.Graphics;
            if (signal == 0)
            {
                g.DrawLine(nowPair.myPen,Math.Abs(nowPair.Diem1.X-nowPair.recLocation.X)+3, Math.Abs(nowPair.Diem1.Y - nowPair.recLocation.Y)+3, Math.Abs(nowPair.Diem2.X - nowPair.recLocation.X), Math.Abs(nowPair.Diem2.Y - nowPair.recLocation.Y));
                signal = 2;
            }
        }

        public void Zoom(object sender, MouseEventArgs e)
        {
            float scale = (float)contain.Height / contain.Width;
            if (e.Delta > 0)
            {
                contain.Width = contain.Width + 30;
                contain.Height = (int)(contain.Width * scale);
            }
            else
            {
                contain.Width = contain.Width - 30;
                contain.Height = (int)(contain.Width * scale);
            }
        }

        public virtual void Ve_lai()
        {
            gp2.DrawLine(nowPair.myPen,Math.Abs(nowPair.Diem1.X-nowPair.recLocation.X)+3, Math.Abs(nowPair.Diem1.Y - nowPair.recLocation.Y)+3, Math.Abs(nowPair.Diem2.X - nowPair.recLocation.X), Math.Abs(nowPair.Diem2.Y - nowPair.recLocation.Y));
        }
        public virtual void To_lai()
        { }

        public void AddFrame()
        {
            contain.BorderStyle = BorderStyle.FixedSingle;
        }

        public void RemoveFrame()
        {
            contain.BorderStyle = BorderStyle.None;
        }

        public virtual void Ungroup() { }

        public virtual void Get_hinh(List<Hinh> listhinh) { }
    }

    public class HinhChuNhat : Hinh
    {
        public HinhChuNhat(Point first) : base(first) { }
        public override void Ve()
        {
            gp.DrawRectangle(nowPair.myPen, nowPair.myRec);
            //Hello
            //World
        }

        public override void To_lai()
        {
            gp2.FillRectangle(nowPair.myBrush, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
        }

        public override void Ve_lai()
        {
            gp2.DrawRectangle(nowPair.myPen, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
        }

        public override void repaint_Paint(object sender, PaintEventArgs e)
        {
            
            Graphics g = e.Graphics;
            if (signal == 0)
            {
                g.DrawRectangle(nowPair.myPen, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
                signal = 2;
            }
            else if (signal==1)
            {
                g.FillRectangle(nowPair.myBrush, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
                signal = 2;
            }
        }
    }

    public class HinhEllipse : Hinh
    {
        public HinhEllipse(Point first) : base(first) { }

        public override void Ve()
        {
            gp.DrawEllipse(nowPair.myPen, nowPair.myRec);
        }

        public override void To_lai()
        {
            gp2.FillEllipse(nowPair.myBrush, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
        }
        public override void Ve_lai()
        {
            gp2.DrawEllipse(nowPair.myPen, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
        }

        public override void repaint_Paint(object sender, PaintEventArgs e)
        {
            
            Graphics g = e.Graphics;
            if (signal == 0)
            {
                g.DrawEllipse(nowPair.myPen, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
                signal = 2;
            }

            else if (signal == 1)
            {
                g.FillEllipse(nowPair.myBrush, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
                signal = 2;
            }
        }
    }
    public class HinhPhucHop: Hinh
    {
       

        int edge_left;
        int edge_right;
        int edge_top;
        int edge_bottom;
        public HinhPhucHop() { }

        public override void Get_hinh(List<Hinh> listhinh)
        {
            foreach (Hinh h in listhinh)
                lstHinh.Add(h);
        }
        
        private void get_dimension()
        {
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            List<int> top = new List<int>();
            List<int> bottom = new List<int>();

            foreach (Hinh h in lstHinh)
            {
                left.Add(h.contain.Location.X);
                right.Add(h.contain.Location.X + h.contain.Width);
                top.Add(h.contain.Location.Y);
                bottom.Add(h.contain.Location.Y + h.contain.Height);
            }

            edge_left=left.AsQueryable().Min()-5;
            edge_right = right.AsQueryable().Max()+5;
            edge_top = top.AsQueryable().Min() - 5;
            edge_bottom = bottom.AsQueryable().Max() + 5;
        
        }

        private void Get_control()
        {
            foreach (Hinh h in lstHinh)
            {
                h.contain.Parent = contain;
                h.contain.BringToFront();
                h.contain.Location = new Point(h.contain.Location.X - contain.Location.X, h.contain.Location.Y - contain.Location.Y);
            }
        }
        public override void Put_PictureBox()
        {
            get_dimension();
            contain = new PictureBox();
            contain.Location = new Point(edge_left, edge_top);
            contain.Size = new Size(edge_right - edge_left, edge_bottom - edge_top);
            contain.BorderStyle = BorderStyle.None;
            bm = new Bitmap(contain.Size.Width, contain.Size.Height);
            gp2 = Graphics.FromImage(bm);
            contain.Image = bm;
            contain.MouseWheel += new MouseEventHandler(Zoom);
            controller.Controls.Add(contain);
            contain.Parent = bigContain;
            Get_control();
            contain.Visible = true;
            contain.BackColor = Color.White;
            contain.BringToFront();
        }

        public override void Ungroup()
        {
            foreach (Hinh h in lstHinh)
            {
                h.contain.Parent = bigContain;
                h.contain.BringToFront();
                h.contain.Location = new Point(h.contain.Location.X + contain.Location.X, h.contain.Location.Y + contain.Location.Y);
            }
            lstHinh.Clear();
            contain.Dispose();
        }
    }
    public class HinhVuong : HinhChuNhat
    {
        public HinhVuong(Point first) : base(first) { }
        public override void Ve()
        {
            nowPair.ModifyRec();
            base.Ve();
        }
        public override void To_lai()
        {
            gp2.FillRectangle(nowPair.myBrush, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
        }

        public override void Ve_lai()
        {
            gp2.DrawRectangle(nowPair.myPen, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
        }
        public override void repaint_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (signal == 0)
            {
                g.DrawRectangle(nowPair.myPen, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
                signal = 2;
            }
            else if (signal == 1)
            {
                g.FillRectangle(nowPair.myBrush, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
                signal = 2;
            }
        }

    }

    public class HinhTron : HinhEllipse
    {
        public HinhTron(Point first) : base(first) { }
        public override void Ve()
        {
            nowPair.ModifyRec();
            base.Ve();
        }
        public override void To_lai()
        {
            gp2.FillEllipse(nowPair.myBrush, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
        }
        public override void Ve_lai()
        {
            gp2.DrawEllipse(nowPair.myPen, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
        }

        public override void repaint_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (signal == 0)
            {
                g.DrawEllipse(nowPair.myPen, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
                signal = 2;
            }
            else if (signal == 1)
            {
                g.FillEllipse(nowPair.myBrush, 3, 3, nowPair.myRec.Width, nowPair.myRec.Height);
                signal = 2;
            }
        }
    }

    public class DuongCong : Hinh
    {
        public DuongCong(Point first) : base(first) 
        { }

        List<Point> sublist = new List<Point>();

        public override void Ve()
        {
            nowPair.GetCurve();
            Point[] myArray = nowPair.listpoint.ToArray();
            gp.DrawCurve(nowPair.myPen, myArray);
            nowPair.listpoint.RemoveAt(2);
            nowPair.listpoint.RemoveAt(1); 
        }

        public void Reshape_Rec()
        {
            if (nowPair.Diem3.Y > nowPair.recLocation.Y && nowPair.Diem3.Y < nowPair.recLocation.Y+nowPair.myRec.Height)
                return;
            else
            {
                if (nowPair.Diem3.Y < nowPair.recLocation.Y)
                {
                    nowPair.myRec.Location = new Point(nowPair.recLocation.X, nowPair.Diem3.Y);
                    nowPair.myRec.Height = nowPair.recLocation.Y + nowPair.myRec.Height - nowPair.Diem3.Y;
                    nowPair.recLocation= nowPair.myRec.Location;
                }
                else
                {
                    nowPair.myRec.Height = nowPair.Diem3.Y - nowPair.recLocation.Y;
                }
            }
        }
        public override void Ve_lai()
        {
            gp2.DrawCurve(nowPair.myPen, sublist.ToArray());
        }

        public void Adj()
        {
            Point[] listp = { nowPair.Diem1, nowPair.Diem3, nowPair.Diem2 };
            foreach (Point p in listp)
            {
                sublist.Add(new Point(p.X - nowPair.recLocation.X+4, p.Y - nowPair.recLocation.Y+4));
            }
        }

        public override void Put_PictureBox()
        {
            Reshape_Rec();
            Adj();
            base.Put_PictureBox();
        }

        public override void repaint_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (signal == 0)
            {
                g.DrawCurve(nowPair.myPen, sublist.ToArray());
                signal = 2;
            }
        }

    }

    public class Dagiac : HinhVuong
    {
        public Dagiac(Point first) : base(first) { }

        List<Point> lstPoint = new List<Point>();
        Point[] array;

        private void Polygon_Point()
        {
            Point topleft = nowPair.myRec.Location;
            Point topright = new Point(nowPair.myRec.Location.X + nowPair.myRec.Width, nowPair.myRec.Location.Y);
            Point bottomleft = new Point(nowPair.myRec.Location.X, nowPair.myRec.Location.Y + nowPair.myRec.Height);
            Point bottomright = new Point(nowPair.myRec.Location.X + nowPair.myRec.Width, nowPair.myRec.Location.Y + nowPair.myRec.Height);
            Point topbot = new Point((bottomleft.X + bottomright.X) / 2, bottomleft.Y);

            lstPoint.Add(new Point((topleft.X + topright.X) / 2, topleft.Y));
            lstPoint.Add(new Point(topleft.X, (topleft.Y+bottomleft.Y)/2));
            lstPoint.Add(new Point((bottomleft.X + topbot.X) / 2, bottomleft.Y));
            lstPoint.Add(new Point((bottomright.X + topbot.X) / 2, bottomright.Y));
            lstPoint.Add(new Point(topright.X, (topright.Y + bottomright.Y) / 2));

            array = lstPoint.ToArray();
        }
        private void Reposition()
        {
            int n = array.Length;
            for (int i=0;i<n;i++)
            { 
                array[i].X = array[i].X - nowPair.recLocation.X;
                array[i].Y = array[i].Y - nowPair.recLocation.Y;
            }
        }
        public override void Ve()
        {
            nowPair.ModifyRec();
            Polygon_Point();
            gp.DrawPolygon(nowPair.myPen, lstPoint.ToArray());
            lstPoint.Clear();
        }
        public override void To_lai()
        {
            gp2.FillPolygon(nowPair.myBrush,array);
        }

        public override void Put_PictureBox()
        {
            Reposition();
            base.Put_PictureBox();
            contain.BringToFront();
        }

        public override void Ve_lai()
        {
            gp2.DrawPolygon(nowPair.myPen, array); 
        }
        public override void repaint_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (signal == 0)
            {
                g.DrawPolygon(nowPair.myPen, array);
                signal = 2;
            }
            else if (signal == 1)
            {
                g.FillPolygon(nowPair.myBrush, array);
                signal = 2;
            }
        }
    }
}
    