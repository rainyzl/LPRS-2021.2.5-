using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using sample;

namespace LPRS
{

    public partial class zmz : Form
    {
        private String name;
        private Bitmap m_Bitmap; //整幅图像
        int[] gray = new int[256];
        int[] rr = new int[256];
        int[] gg = new int[256];
        int[] bb = new int[256];
        private Bitmap m_Bitmap1;
        private Bitmap c_Bitmap; //车牌图像
        private int maxX;
        private int maxY;
        private float count;
        int flag = 0;
        int xx = -1;
        private bool aline = false;
        private float[] gl = new float[256];
        Pen pen1 = new Pen(Color.Black);

        public zmz()
        {
            m_Bitmap = new Bitmap(2, 2);
            InitializeComponent();
            c_Bitmap = new Bitmap(2, 2);
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void progressBar2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            button3.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            panel3.Enabled = false;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Jpeg文件(*.jpg)|*.jpg|Bitmap文件(*.bmp)|*.bmp| 所有合适文件(*.bmp/*.jpg)|*.bmp/*.jpg";
            openFileDialog.FilterIndex = 2;

            openFileDialog.RestoreDirectory = true;

            if (DialogResult.OK == openFileDialog.ShowDialog())

            {
                name = openFileDialog.FileName;
                m_Bitmap = (Bitmap) Bitmap.FromFile(name, false);

                this.panel2.AutoScroll = true;

                this.panel2.AutoScrollMinSize = new Size((int) (m_Bitmap.Width), (int) m_Bitmap.Height);

                this.button2.Enabled = true;
                this.button4.Enabled = true;


                panel2.Invalidate();
                panel1.Invalidate();
                panel3.Invalidate();

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (Filters.Brightness(m_Bitmap, gray, out gray, (int)count))
                graydo();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Bitmap文件(*.bmp)|*.bmp| Jpeg文件(*.jpg)|*.jpg| 所有合适文件(*.bmp/*.jpg)|*.bmp/*.jpg";
            saveFileDialog.FilterIndex = 1;

            saveFileDialog.RestoreDirectory = true;

            if (DialogResult.OK == saveFileDialog.ShowDialog())

            {
                c_Bitmap.Save(saveFileDialog.FileName);

            }

        }

        private void graydo()
        {
            this.flag = 1;
            count = m_Bitmap.Width * m_Bitmap.Height;
            gl = new float[256];
            for (int i = 0; i < 256; i++)
                gl[i] = gray[i] / count * 1500;
            pen1 = Pens.Red;
            panel2.Invalidate();
            panel4.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Filters.Invert(m_Bitmap, out gray))
            {
                panel5.Invalidate();
                graydo();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Filters.zft(m_Bitmap, out gray, out rr, out gg, out bb);
            this.button3.Enabled = true;
            this.button5.Enabled = true;
            panel5.Invalidate();
            graydo();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Filters.GaussianFilter(m_Bitmap, out gray))
                graydo();
            this.button6.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int ccount;
            int yl, yr, xu, xd;
            if (Filters.MarginalFilter(m_Bitmap, out gray, out ccount, 67, out xu, out xd, out yl, out yr, out maxX,
                out maxY))
            {

                m_Bitmap1 = (Bitmap)Bitmap.FromFile(name, false);
                Rectangle sourceRectangle = new Rectangle(yl, xu, yr - yl, xd - xu);
                c_Bitmap = m_Bitmap1.Clone(sourceRectangle,
                    PixelFormat.DontCare);
                groupBox2.Text = "车牌";
                groupBox2.Invalidate();

                this.button8.Enabled = true;
                this.button7.Enabled = true;
                panel3.Invalidate();
                graydo();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Filters.zft(c_Bitmap, out gray, out rr, out gg, out bb);
            Rectangle sourceRectangle = new Rectangle(maxY, maxX, 66, 14);
            Bitmap xc_Bitmap = c_Bitmap.Clone(sourceRectangle,
                PixelFormat.DontCare);
            if (Filters.TowValue(c_Bitmap, xc_Bitmap, maxY, maxY))
            {
                groupBox1.Text = "车牌（经二值化处理）";
                groupBox1.Invalidate();
                panel4.Invalidate();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(m_Bitmap, new Rectangle(this.panel2.AutoScrollPosition.X, this.panel2.AutoScrollPosition.Y,
                (int)(m_Bitmap.Width), (int)(m_Bitmap.Height)));
        }

        private void panel3_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Point ulCorner = new Point(0, 0);
            g.DrawImage(c_Bitmap, 0, 0);
           
        }

        private void panel4_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int p;
            p = m_Bitmap.Width * m_Bitmap.Height;
            //	this.l_pixel.Text=p.ToString();

            int height = this.panel4.Height;
            for (int j = 0; j < 256; j++)
            {
                if (gl[j] > height)
                    gl[j] = height;
                g.DrawLine(pen1, j, height, j, height - gl[j]);
            }
            if (aline)
            {
                g.DrawLine(Pens.OrangeRed, xx, 0, xx, height);
            }
        }

        private void panel4_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            xx = e.X;
            if (xx > 255)
                xx = 255;
            if (xx <= 0)
                xx = 0;

            aline = true;
            this.panel4.Invalidate();
        }

        private void panel4_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            if (new Rectangle(0, 0, 256, 127).Contains(p))
            {
                this.xx = e.X;
            }
            else
            {
                this.xx = -1;
            }

            this.panel4.Invalidate();
        }

        private void panel4_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            aline = false;
        }

        private void panel5_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int width = this.panel5.Width;
            int height = this.panel5.Height;
            int j;
            Color c;
            Graphics g = e.Graphics;

            switch (flag)
            {
                case 1:
                {
                    for (int i = width; i >= 0; i--)
                    {
                        j = i;
                        if (j > 255) j = 255;
                        c = Color.FromArgb(j, j, j);
                        Pen pen2 = new Pen(c, 1);
                        g.DrawLine(pen2, i, 0, i, height);
                    }
                    break;
                }
                case 2:
                {
                    for (int i = width; i >= 0; i--)
                    {
                        j = i;
                        if (j > 255) j = 255;
                        c = Color.FromArgb(j, 0, 0);
                        Pen pen2 = new Pen(c, 1);
                        g.DrawLine(pen2, i, 0, i, height);
                    }
                    break;
                }
                case 3:
                {
                    for (int i = width; i >= 0; i--)
                    {
                        j = i;
                        if (j > 255) j = 255;
                        c = Color.FromArgb(0, j, 0);
                        Pen pen2 = new Pen(c, 1);
                        g.DrawLine(pen2, i, 0, i, height);
                    }
                    break;
                }
                case 4:
                {
                    for (int i = width; i >= 0; i--)
                    {
                        j = i;
                        if (j > 255) j = 255;
                        c = Color.FromArgb(0, 0, j);
                        Pen pen2 = new Pen(c, 1);
                        g.DrawLine(pen2, i, 0, i, height);
                    }
                    break;
                }
                default:
                    break;

            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString(name, new Font("Arial", 8), new SolidBrush(Color.Black), 0, 0);
        }


        private void panel4_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int p;
            p = m_Bitmap.Width * m_Bitmap.Height;
            //	this.l_pixel.Text=p.ToString();

            int height = this.panel4.Height;
            for (int j = 0; j < 256; j++)
            {
                if (gl[j] > height)
                    gl[j] = height;
                g.DrawLine(pen1, j, height, j, height - gl[j]);
            }
            if (aline)
            {
                g.DrawLine(Pens.OrangeRed, xx, 0, xx, height);
            }
        }

        private void panel5_Paint_1(object sender, PaintEventArgs e)
        {
            int width = this.panel5.Width;
            int height = this.panel5.Height;
            int j;
            Color c;
            Graphics g = e.Graphics;

            switch (flag)
            {
                case 1:
                {
                    for (int i = width; i >= 0; i--)
                    {
                        j = i;
                        if (j > 255) j = 255;
                        c = Color.FromArgb(j, j, j);
                        Pen pen2 = new Pen(c, 1);
                        g.DrawLine(pen2, i, 0, i, height);
                    }
                    break;
                }
                case 2:
                {
                    for (int i = width; i >= 0; i--)
                    {
                        j = i;
                        if (j > 255) j = 255;
                        c = Color.FromArgb(j, 0, 0);
                        Pen pen2 = new Pen(c, 1);
                        g.DrawLine(pen2, i, 0, i, height);
                    }
                    break;
                }
                case 3:
                {
                    for (int i = width; i >= 0; i--)
                    {
                        j = i;
                        if (j > 255) j = 255;
                        c = Color.FromArgb(0, j, 0);
                        Pen pen2 = new Pen(c, 1);
                        g.DrawLine(pen2, i, 0, i, height);
                    }
                    break;
                }
                case 4:
                {
                    for (int i = width; i >= 0; i--)
                    {
                        j = i;
                        if (j > 255) j = 255;
                        c = Color.FromArgb(0, 0, j);
                        Pen pen2 = new Pen(c, 1);
                        g.DrawLine(pen2, i, 0, i, height);
                    }
                    break;
                }
                default:
                    break;

            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
    }
