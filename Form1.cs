using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mini_GameFinalFlappy
{
    class BackgroundMap
    {
        public Bitmap Image;
        public int x, y, width, height;
    }

    class Bird
    {
        public Bitmap Image;
        public int x, y, width, height;
    }

    class Pipes
    {
        public Bitmap Image;
        public int x, y, width, height;
    }

    public partial class Form1 : Form
    {
        List<Pipes> LPipes = new List<Pipes>();
        List<BackgroundMap> LBackgroundMap = new List<BackgroundMap>();
        List<Bird> LBirds = new List<Bird>();
        Timer tt = new Timer();
        Bitmap off;
        int s_Ct = 0;
        int ctTick = 0;
        int score = 0;
        int prev = 1000;
        int flagDead = 0;

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.Load += new EventHandler(Form1_Load);
            this.Paint += new PaintEventHandler(Form1_Paint);
            tt.Tick += new EventHandler(tt_Tick);
            tt.Start();
            tt.Interval = 10;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            Create_World();
            Create_Bird();
        }

        void Create_Bird()
        {
            Bird pnn = new Bird();
            pnn.Image = new Bitmap("YB1.png"); 
            pnn.x = 50;
            pnn.y = this.Height / 2;
            LBirds.Add(pnn);
        }

        void Hit_Wnd()
        {
            for (int i = 0; i < LPipes.Count; i++)
            {
                if (LBirds[0].x > LPipes[i].x && LBirds[0].x < LPipes[i].x + 30)
                {
                    if (LBirds[0].y < LPipes[i + 1].y + LPipes[i + 1].height || LBirds[0].y + 20  > LPipes[i].y)
                    {
                        flagDead = 1;
                        MessageBox.Show("GAME OVER: SCORE " + score);
                    }
                    i++;
                }
            }
        }

        void Calculate_Score()
        {
            for (int i = 0; i < LPipes.Count; i++)
            {
                if (i != prev && i != prev + 1)
                {
                    if (LBirds[0].x >= LPipes[i].x && LBirds[0].x <= LPipes[i].x + 20)
                    {
                        if (LBirds[0].y > LPipes[i + 1].y + LPipes[i + 1].height && LBirds[0].y < LPipes[i].y)
                        {
                            score++;
                            prev = i;
                            i++;
                            break;
                        }

                    }
                }
            }
        }
        void Create_World()
        {
            BackgroundMap pnn = new BackgroundMap();
            pnn.Image = new Bitmap("BG.png");
            pnn.x = -20;
            pnn.y = 0;
            pnn.width = pnn.Image.Width;
            pnn.height = pnn.Image.Height;
            LBackgroundMap.Add(pnn);

            Random r = new Random();
            int x = 400;
            for (int i = 0; i < 10; i++)
            {
                Pipes Column;

                if (i % 2 == 0)
                {
                    Column = new Pipes();
                    Column.Image = new Bitmap("BP2.png");
                    Column.Image.MakeTransparent(Color.White);
                    Column.height = r.Next(300, 500);
                    Column.x = x;
                    Column.y = this.Height - Column.height + 50;

                    LPipes.Add(Column);
                }
                else
                {
                    Column = new Pipes();
                    Column.Image = new Bitmap("TP2.png");
                    Column.Image.MakeTransparent(Color.White);
                    Column.height = LPipes[LPipes.Count - 1].y - 150;
                    Column.x = x;
                    Column.y = 0;
                    LPipes.Add(Column);

                    x += 400;
                }
            }
        }

        void Create_Pipes()
        {
            int x = LPipes[LPipes.Count - 1].x + 400;
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                Pipes Column;

                if (i % 2 == 0)
                {
                    Column = new Pipes();
                    Column.Image = new Bitmap("BP2.png");
                    Column.Image.MakeTransparent(Color.White);
                    Column.height = r.Next(300, 500);
                    Column.x = x;
                    Column.y = this.Height - Column.height + 50;

                    LPipes.Add(Column);
                }
                else
                {
                    Column = new Pipes();
                    Column.Image = new Bitmap("TP2.png");
                    Column.Image.MakeTransparent(Color.White);
                    Column.height = LPipes[LPipes.Count - 1].y - 150;
                    Column.x = x;
                    Column.y = 0;
                    LPipes.Add(Column);

                    x += 400;
                }
            }
        }

        void tt_Tick(object sender, EventArgs e)
        {
            if (flagDead == 0)
            {
                for (int i = 0; i < LPipes.Count; i++)
                {
                    LPipes[i].x -= 20;
                }
                for (int i = 0; i < LBirds.Count; i++)
                {
                    LBirds[i].y += 10;
                }
                if (ctTick % 2 == 0)
                {
                    LBackgroundMap[s_Ct].x += 20;
                    if (LBackgroundMap.Count - 1 > s_Ct)
                    {
                        LBackgroundMap[s_Ct + 1].x -= 20;
                    }

                    if (LBackgroundMap[s_Ct].x <= 1000)
                    {
                        //MessageBox.Show("Comp");
                        if (LBackgroundMap[s_Ct].x + this.Width >= LBackgroundMap[s_Ct].Image.Width)
                        {
                            //MessageBox.Show("Completed");
                            BackgroundMap pnn = new BackgroundMap();
                            pnn.Image = new Bitmap("BG.png");
                            pnn.x = LBackgroundMap[s_Ct].x + LBackgroundMap[s_Ct].Image.Width;
                            pnn.y = 0;
                            pnn.width = pnn.Image.Width;
                            pnn.height = pnn.Image.Height;
                            LBackgroundMap.Add(pnn);
                            s_Ct++;
                        }
                    }
                    if (ctTick % 10 == 0)
                    {
                        Create_Pipes();
                    }
                }

                Hit_Wnd();
                Calculate_Score();
                ctTick++;
                DrawDubb(this.CreateGraphics());
            }
        }


        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (flagDead == 0)
            {
                if (e.KeyCode == Keys.Space)
                {
                    for (int i = 0; i < LBirds.Count; i++)
                    {
                        LBirds[i].y -= 20;
                    }
                }

                LBackgroundMap[s_Ct].x += 20;
                if (LBackgroundMap.Count - 1 > s_Ct)
                {
                    LBackgroundMap[s_Ct + 1].x -= 20;
                }
                for (int i = 0; i < LPipes.Count; i++)
                {
                    LPipes[i].x -= 20;
                }

                Calculate_Score();
                Hit_Wnd();
                DrawDubb(this.CreateGraphics());
            }
        }


        void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(this.CreateGraphics());
        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }

        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            for (int i = 0; i < LBackgroundMap.Count; i++)
            {
                g.DrawImage(LBackgroundMap[i].Image,
                    new Rectangle(0, LBackgroundMap[i].y, LBackgroundMap[i].width, LBackgroundMap[i].height),
                    new Rectangle(LBackgroundMap[i].x, 0, ClientSize.Width, ClientSize.Height),
                    GraphicsUnit.Pixel);
            }

            for (int i = 0; i < LPipes.Count; i++)
            {
                if (i % 2 == 0)
                {
                    g.DrawImage(LPipes[i].Image,
                        new Rectangle(LPipes[i].x, LPipes[i].y, LPipes[i].Image.Width + 100, LPipes[i].height),
                        new Rectangle(0, 0, ClientSize.Width, LPipes[i].height),
                        GraphicsUnit.Pixel);
                }
                else
                {
                    g.DrawImage(LPipes[i].Image,
                        new Rectangle(LPipes[i].x, LPipes[i].y, LPipes[i].Image.Width+100, LPipes[i].height),
                        new Rectangle(0, 0, ClientSize.Width, LPipes[i].Image.Height),
                        GraphicsUnit.Pixel);
                }
            }

            for (int i = 0; i < LBirds.Count; i++)
            {
                g.DrawImage(LBirds[i].Image,LBirds[i].x,LBirds[i].y);
            }
        }
    }
}
