﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using Spinet;

namespace coins_hockey
{
    class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        //[STAThread]
        public static game MainGame = new game();
        public const int countcoin = 26;
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            Z.MainForm = new Form1();
            Z.sit = -1;
            Z.MainForm.Show();
            Z.MainForm.drawoll(); 
            set_curs();
            if (Z.sit != -2)
                Z.sit = 1;

            Z.clwidth = Z.MainForm.ClientSize.Width;
            Z.clheight = Z.MainForm.ClientSize.Height;
            var ticktim = new System.Diagnostics.Stopwatch();
            ticktim.Start();
            while (Z.MainForm.Created)
            {
                Z.MainForm.drawoll();
                Application.DoEvents();
                if (MainGame.sit == 0) 
                    MainGame.prd(ticktim.ElapsedMilliseconds);
                if (Z.sit == 1 || Z.sit == 2)
                    MainGame.menuprd();
                ticktim.Restart();
            }
        }
        public static void set_curs()
        {
                 try
                {
                    Ping ping = new Ping();
                    PingReply reply = ping.Send("ya.ru");
                    Z.sit = -1;
                }
                catch
                {
                    Z.sit = -2;
                }
                 if (Z.sit != -2)
                 {
                     WebRequest wrGETURL;
                     wrGETURL = WebRequest.Create("https://query.yahooapis.com/v1/public/yql?q=select+*+from+yahoo.finance.xchange+where+pair+=+%22USDRUB,EURRUB,GBPRUB,CADRUB%22&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=");
                     var objStream = wrGETURL.GetResponse().GetResponseStream();
                     var objReader = new StreamReader(objStream);
                     string json = objReader.ReadLine();
                     var prprp = new { query = new { results = new { rate = new valut[3] } } };
                     prprp = JsonConvert.DeserializeAnonymousType(json, prprp);
                     Z.usd = double.Parse(prprp.query.results.rate[0].Rate);
                     Z.eur = double.Parse(prprp.query.results.rate[1].Rate);
                     Z.gbp = double.Parse(prprp.query.results.rate[2].Rate);
                     for (int i = 1; i < countcoin; i++)
                     {
                         if (Z.tcoin[i].valut == 2) Z.tcoin[i].stoim = (int)(Z.tcoin[i].stoim * Z.usd);
                         if (Z.tcoin[i].valut == 3) Z.tcoin[i].stoim = (int)(Z.tcoin[i].stoim * Z.eur);
                         if (Z.tcoin[i].valut == 4) Z.tcoin[i].stoim = (int)(Z.tcoin[i].stoim * Z.gbp);
                     }
                     double k1 = 20;
                     double k2 = (Z.usd * 100 + Z.eur * 150);
                     double k3 = (Z.eur * 500);
                     double k4 = (Z.eur * 200 + Z.gbp * 300);
                     double k5 = (Z.gbp * 200 + Z.eur * 600);
                     double got = 100000000;
                     double na, nb, ga = 0, gb = 0;
                     double ot = 100000000;
                     na = 0;
                     nb = 0;
                     while (na <= 30)
                     {
                         nb = 0;
                         while (nb < 50)
                         {
                             ot = 0;
                             ot += Math.Pow(f(na, nb, k1) - 20, 2);
                             ot += Math.Pow(f(na, nb, k2) - k2 / 4 * 6 / 10, 2);
                             ot += Math.Pow(f(na, nb, k3) - k3 / 4 * 6 / 15, 2);
                             ot += Math.Pow(f(na, nb, k4) - k4 / 4 * 6 / 18, 2);
                             ot += Math.Pow(f(na, nb, k5) - k5 / 4 * 6 / 25, 2);
                             if (ot < got)
                             {
                                 ga = na;
                                 gb = nb;
                                 got = ot;
                             }
                             nb += 0.06;
                         }
                         na += 0.05;
                     }
                     Z.koofa = ga;
                     Z.koofb = gb;
                     //Application.Exit();
                 }
        }
        static double f(double a, double b, double x)
        {
            return a + Math.Sqrt(x) * b;
        }
        public static void getgrp0(System.Drawing.Graphics g)
        {
            g.Clear(System.Drawing.Color.Maroon);
            var f = new System.Drawing.Font("Arial", 20);
            g.DrawString("Загрузка и анализ курсов валют...", f, System.Drawing.Brushes.White, 190, 250);
            border(g);
        }
        public static void getgrp4(System.Drawing.Graphics g)
        {
            g.Clear(System.Drawing.Color.Maroon);
            var f = new System.Drawing.Font("Arial", 150);
            g.DrawString(":(", f, System.Drawing.Brushes.White, 20, 30);
            f = new System.Drawing.Font("Courier", 25);
            g.DrawString("Your PC hasn't internet connection", f, System.Drawing.Brushes.White, 20, 360);
            border(g);
        }
        public static void border(System.Drawing.Graphics g)
        {
            if (Z.mininmon && Z.sit <= -1)
                g.DrawString("-", new Font("Arial", 15), Brushes.Bisque, Z.clwidth - 35, -3);
            else
                g.DrawString("-", new Font("Arial", 15), Brushes.CornflowerBlue, Z.clwidth - 35, -3);

            if (Z.closeon && Z.sit <= -1)
                g.DrawString("X", new Font("Arial", 15), Brushes.Bisque, Z.clwidth - 18, -3);
            else
                g.DrawString("X", new Font("Arial", 15), Brushes.CornflowerBlue, Z.clwidth - 18, -3);

        }
        public static void klik(object sender, KeyEventArgs e)
        {
             MainGame.klik(sender, e);
        }
        public static void mdklik(object sender, MouseEventArgs e)
        {
            MainGame.mdklik(sender, e);
        }
        public static void mmklik(object sender, MouseEventArgs e)
        {
            MainGame.mmklik(sender, e);
        }
        public static void muklik(object sender, MouseEventArgs e)
        {
            MainGame.muklik(sender, e);
        }
        public static long replay_add(long time, user[] us, coins[] coins, string filename = "replay.chrpl")
        {
            if (time < 21) return time;
            rec.oup.WriteLine(us[0].vrat + " " + ((int)coins[0].x) + " " + ((int)coins[0].y));
            rec.oup.WriteLine(us[0].nap + " " + ((int)coins[1].x) + " " + ((int)coins[1].y));
            rec.oup.WriteLine(us[0].nap + " " + ((int)coins[2].x) + " " + ((int)coins[2].y));
            rec.oup.WriteLine(us[0].nap + " " + ((int)coins[3].x) + " " + ((int)coins[3].y));
            rec.oup.WriteLine(us[1].vrat + " " + ((int)coins[4].x) + " " + ((int)coins[4].y));
            rec.oup.WriteLine(us[1].nap + " " + ((int)coins[5].x) + " " + ((int)coins[5].y));
            rec.oup.WriteLine(us[1].nap + " " + ((int)coins[6].x) + " " + ((int)coins[6].y));
            rec.oup.WriteLine(us[1].nap + " " + ((int)coins[7].x) + " " + ((int)coins[7].y));
            rec.oup.WriteLine("0 " + ((int)coins[8].x) + " " + ((int)coins[8].y));
            rec.oup.WriteLine("0 " + ((int)coins[9].x) + " " + ((int)coins[9].y));
            rec.oup.WriteLine("0 " + ((int)coins[10].x) + " " + ((int)coins[10].y));
            return time - 21;
        }
    }

    class coins
    {
        public int r { get; set; }
        public int m { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public int nu { get; set; }
        public double zam { get; set; }
        public Point vec { get; set; }
        Point prov { get; set; }
        bool or = false;
        public int otb { get; set; }
        int typecoin = 0;
        public int is_gool { get; set; }

        public bool stop()
        {
            if (vec.x == 0 && vec.y == 0)
                return true;
            return false;
        }

        public void Draw(Graphics g)
        {
            if (or)
                g.DrawImage(Z.tcoin[typecoin].picob, (int)x - r, (int)y - r, r * 2, r * 2);
            else
                g.DrawImage(Z.tcoin[typecoin].picre, (int)x - r, (int)y - r, r * 2, r * 2);
        }

        public static coins get_number_coin(int nus, int sx, int sy, bool orl, int num, double zams = 100.0, double spx = 0, double spy = 0)
        {
            var an = new coins();
            an.nu = nus;
            an.r = Z.tcoin[num].r;
            an.m = Z.tcoin[num].m;
            an.x = sx;
            an.y = sy;
            an.zam = zams;
            an.or = orl;
            an.typecoin = num;
            an.otb = -1;
            an.vec = new Point();
            an.prov = new Point();
            return an;
        }

        public static coins get_coin(int nus, int rad, int mass, int sx, int sy, double zams, double spx, double spy, bool orl, int tp)
        {
            var an = new coins();
            an.nu = nus;
            an.r = rad;
            an.m = mass;
            an.x = sx;
            an.y = sy;
            an.zam = zams;
            an.or = orl;
            an.typecoin = tp;
            an.otb = -1;
            an.vec = new Point();
            an.prov = new Point();
            return an;
        }

        public void sdvig(double time, coins[] coin)
        {
            double spd = Math.Sqrt(vec.x * vec.x + vec.y * vec.y);
           
            double spdm = spd - time * zam / 1000;
            if (spd <= time * zam / 1000)
            {
                vec.x = 0;
                vec.y = 0;
                if (otb != -1)
                {
                    coin[otb].otb = -1;
                    otb = -1;
                }
            }
            else
            {
                vec.x = vec.x * spdm / spd;
                vec.y = vec.y * spdm / spd;
                x += time * vec.x / 1000;
                y += time * vec.y / 1000;
            }


            if (x < r || x > Z.clwidth - r)
            {
                vec.x = -vec.x;
                if (x < r)
                    x = r;
                else
                    x = Z.clwidth - r;
                if (otb != -1)
                {
                    coin[otb].otb = -1;
                    otb = -1;
                }

                if (x <= r && y >= Z.clheight / 2 - 50 && y <= Z.clheight / 2 + 50)
                    is_gool = 1;
                if (x >= Z.clwidth - r && y >= Z.clheight / 2 - 50 && y <= Z.clheight / 2 + 50)
                    is_gool = 2;
            }
            if (y < r || y > Z.clheight - r)
            {
                vec.y = -vec.y;
                if (y < r) y = r; else y = Z.clheight - r;
                if (otb != -1)
                {
                    coin[otb].otb = -1;
                    otb = -1;
                }
            }
            on_board();
        }

        public void prd(double time, coins[] coins)
        {
            sdvig(time, coins);
            for (int i = nu + 1; i < coins.Length; i++)
            {
                if (otb == i)
                {
                    if (Math.Abs(vec.y * prov.y - vec.x * prov.x) >= 1e-5 || Math.Abs(coins[i].vec.y * coins[i].prov.y - coins[i].vec.x * coins[i].prov.x) >= 1e-5)
                    {
                        otb = -1;
                        coins[i].otb = -1;
                    }
                }
                if (otb != i && Math.Pow(coins[i].x - x, 2) + Math.Pow(coins[i].y - y, 2) < Math.Pow(coins[i].r + r, 2))
                {
                    double skpr = (vec.x - coins[i].vec.x) * (x - coins[i].x) + (vec.y - coins[i].vec.y) * (y - coins[i].y);
                    double dist = (x - coins[i].x) * (x - coins[i].x) + (y - coins[i].y) * (y - coins[i].y);
                    Point hvec1 = new Point();
                    Point hvec2 = new Point();
                    hvec1.x = vec.x - (2 * coins[i].m * skpr) / (m + coins[i].m) / dist * (x - coins[i].x);
                    hvec1.y = vec.y - (2 * coins[i].m * skpr) / (m + coins[i].m) / dist * (y - coins[i].y);
                    hvec2.x = coins[i].vec.x - (2 * m * skpr) / (m + coins[i].m) / dist * (-x + coins[i].x);
                    hvec2.y = coins[i].vec.y - (2 * m * skpr) / (m + coins[i].m) / dist * (-y + coins[i].y);
                    vec.x = hvec1.x;
                    vec.y = hvec1.y;
                    coins[i].vec.x = hvec2.x;
                    coins[i].vec.y = hvec2.y;
                    otb = i;
                    coins[i].otb = nu;
                    coins[i].prov.x = coins[i].vec.x;
                    coins[i].prov.y = coins[i].vec.y;
                    prov.x = vec.x;
                    prov.y = vec.y;
                }
            }
        }

        public void change_for_board(int helpsit)
        {
            if (helpsit == 1 || helpsit == 3)
            {
                x = Z.clwidth - x;
                vec.x = -vec.x;
            }
            if (helpsit == 2 || helpsit == 3)
            {
                y = Z.clheight - y;
                vec.y = -vec.y;
            }
        }

        public void on_board()
        {
            int helpsit = -1;
            if ((Z.radangl - x) * (Z.radangl - x) + (Z.radangl - y) * (Z.radangl - y) > (Z.radangl - r) * (Z.radangl - r) && x < Z.radangl && y < Z.radangl)
                helpsit = 0;
            if ((Z.clwidth - Z.radangl - x) * (Z.clwidth - Z.radangl - x) + (Z.radangl - y) * (Z.radangl - y) > (Z.radangl - r) * (Z.radangl - r) && x > Z.clwidth - Z.radangl && y < Z.radangl)
                helpsit = 1;
            if ((Z.clheight - Z.radangl - y) * (Z.clheight - Z.radangl - y) + (Z.radangl - x) * (Z.radangl - x) > (Z.radangl - r) * (Z.radangl - r) && x < Z.radangl && y > Z.clheight - Z.radangl)
                helpsit = 2;
            if ((Z.clwidth - Z.radangl - x) * (Z.clwidth - Z.radangl - x) + (Z.clheight - Z.radangl - y) * (Z.clheight - Z.radangl - y) > (Z.radangl - r) * (Z.radangl - r) && x > Z.clwidth - Z.radangl && y > Z.clheight - Z.radangl)
                helpsit = 3;
            if (helpsit == -1)
                return;
            change_for_board(helpsit);
            double hx = Z.radangl - x, hy = Z.radangl - y;
            double crpr = vec.x * hx + vec.y * hy, dtpr = vec.x * hy - vec.y * hx;
            double oldv = vec.x * vec.x + vec.y + vec.y; 
            vec.x = (-crpr * hx + dtpr * hy) / (hx * hx + hy * hy);
            vec.y = (-dtpr * hx - hy * crpr) / (hx * hx + hy * hy);
            otb = -1;
            double dist = Math.Sqrt((Z.radangl - x) * (Z.radangl - x) + (Z.radangl - y) * (Z.radangl - y));
            x = (x - Z.radangl) * (Z.radangl - r) / dist + Z.radangl;
            y = (y - Z.radangl) * (Z.radangl - r) / dist + Z.radangl;
            change_for_board(helpsit); 
        }
    }

    class game
    {
        public coins[] coinarr = new coins[11];
        public int per = 0;
        public bool gol1 = false, gol2 = false;
        public int ch1 = 0, ch2 = 0;
        public int vb = -1;
        public Point vbp = new Point();
        public int sit = 0;
        public user[] us = new user[2];
        public int immunlast = 4;
        public const int shopfull = 1000;
        public int shopvb = 1;
        public int shoppl = 1;
        public int shopcur = 1;
        public long shopwidthanim = shopfull;
        public bool shopold = false;
        public int shopoldcoin = 1;
        public Stopwatch shoptim = new Stopwatch();
        public int shopspeed = 2;
        public bool shopanimon = false;

        public game()
        {
            shoppl = 1;
            us[0] = new user();
            us[1] = new user();
            new_game();
            sit = 1;
        }
        public void new_game(bool f = true)
        {
            coinarr[0] = coins.get_number_coin(0, Z.tcoin[us[0].vrat].r + 5, 532 / 2, false, us[0].vrat);
            coinarr[1] = coins.get_number_coin(1, 300, 532 / 2, false, us[0].nap);
            coinarr[2] = coins.get_number_coin(2, 200, 532 / 2 - 100, false, us[0].nap);
            coinarr[3] = coins.get_number_coin(3, 200, 532 / 2 + 100, false, us[0].nap);
            coinarr[4] = coins.get_number_coin(4, 788 - Z.tcoin[us[1].vrat].r, 532 / 2, true, us[1].vrat);
            coinarr[5] = coins.get_number_coin(5, 500, 532 / 2, true, us[1].nap);
            coinarr[6] = coins.get_number_coin(6, 600, 532 / 2 - 100, true, us[1].nap);
            coinarr[7] = coins.get_number_coin(7, 600, 532 / 2 + 100, true, us[1].nap);
            coinarr[8] = coins.get_number_coin(8, 400, 532 / 2 - 50, true, 0);
            coinarr[9] = coins.get_number_coin(9, 400, 532 / 2, true, 0);
            coinarr[10] = coins.get_number_coin(10, 400, 532 / 2 + 50, true, 0);
            if (f) ch1 = 0;
            if (f) ch2 = 0;
            immunlast = 5;
        }
        public void prd(long tick)
        {
            double hl = tick;
            while (hl > 0)
            {
                for (int i = 0; i < coinarr.Length; i++)
                {
                    coinarr[i].prd(Math.Min(0.1, hl), coinarr);
                }
                hl -= 0.1;
            }

            if ((!rec.record && rec.rectim.IsRunning) || coin_stop())
            {
                rec.rectim.Reset();
                rec.rectim.Stop();
                rec.timerec = 0;
            }
            else
            {
                rec.timerec += rec.rectim.ElapsedMilliseconds;
                rec.rectim.Restart();
                rec.timerec = Program.replay_add(rec.timerec, us, coinarr);
            }

            for (int i = coinarr.Length - 3; i < coinarr.Length; i++)
            {
                if (coinarr[i].x < coinarr[i].r && coinarr[i].y >= Z.clheight / 2 - 50 && coinarr[i].y <= Z.clheight / 2 + 50 && immunlast == 0)
                    gol1 = true;
                if (coinarr[i].x > Z.clwidth - coinarr[i].r && coinarr[i].y >= Z.clheight / 2 - 50 && coinarr[i].y <= Z.clheight / 2 + 50 && immunlast == 0)
                    gol2 = true;
            }
            for (int i = coinarr.Length - 3; i < coinarr.Length; i++)
            {
                if (coinarr[i].is_gool == 1)
                {
                    if (immunlast == 0)
                        gol1 = true;
                    coinarr[i].is_gool = 0;
                }
                if (coinarr[i].is_gool == 2)
                {
                    if (immunlast == 0)
                        gol2 = true;
                    coinarr[i].is_gool = 0;
                }
            }

            if (gol1 || gol2)
            {
                new_game(false);
            }
            if (gol1)
            {
                gol1 = false;
                ch2++;
                per = 0;
            }
            if (gol2)
            {
                gol2 = false;
                ch1++;
                per = 1;
            }
            if (ch1 >= 2)
            {
                var k1 = (2 * us[0].sen_arm() + us[1].sen_arm()) / 3;
                var k2 = (2 * us[1].sen_arm() + us[0].sen_arm()) / 3;
                us[0].mon += (int)(Z.koofa + Z.koofb * Math.Sqrt(k1));
                if (ch2 > 0)
                    us[1].mon += (int)((Z.koofa + Z.koofb * Math.Sqrt(k2)) * 2 / 3);
                sit = 1;
                shoppl = 1;
            }
            else
                if (ch2 >= 2)
                {
                    var k1 = (2 * us[0].sen_arm() + us[1].sen_arm()) / 3;
                    var k2 = (2 * us[1].sen_arm() + us[0].sen_arm()) / 3;
                    us[1].mon += (int)(Z.koofa + Z.koofb * Math.Sqrt(k2));
                    if (ch1 > 0)
                        us[0].mon += (int)((Z.koofa + Z.koofb * Math.Sqrt(k1)) * 2 / 3);
                    sit = 1;
                    shoppl = 1;
                }
        }
        public void menuprd()
        {
            if (shopanimon)
            {
                if (shopold)
                {
                    shopwidthanim -= shopspeed * shoptim.ElapsedMilliseconds;
                    shoptim.Restart();
                    if (shopwidthanim <= 0)
                    {
                        shopold = false;
                        shopwidthanim = 0;
                    }
                }
                else
                {
                    shopwidthanim += shopspeed * shoptim.ElapsedMilliseconds;
                    shoptim.Restart();
                    if (shopwidthanim >= shopfull)
                    {
                        shopanimon = false;
                        shopwidthanim = shopfull;
                    }
                }
            }
        }
        public void getgrp1(System.Drawing.Graphics g)
        {
            g.Clear(System.Drawing.Color.White);
            g.DrawLine(new Pen(Color.Maroon), 0, 0, Z.clwidth, 0);
            g.DrawLine(new Pen(Color.Maroon), 0, 0, 0, Z.clheight);
            g.DrawLine(new Pen(Color.Maroon), Z.clwidth - 1, 0, Z.clwidth - 1, Z.clheight - 1);
            g.DrawLine(new Pen(Color.Maroon), 0, Z.clheight - 1, Z.clwidth - 1, Z.clheight - 1);
            
            g.FillRectangle(System.Drawing.Brushes.Maroon, 0, 0, Z.radangl, Z.radangl);
            g.FillEllipse(System.Drawing.Brushes.White, 1, 0, Z.radangl * 2, Z.radangl * 2);
            g.FillRectangle(System.Drawing.Brushes.Maroon, Z.clwidth - Z.radangl, 0, Z.radangl, Z.radangl);
            g.FillEllipse(System.Drawing.Brushes.White, Z.clwidth - 2 * Z.radangl - 1, 0, Z.radangl * 2, Z.radangl * 2);
            g.FillRectangle(System.Drawing.Brushes.Maroon, 0, Z.clheight - Z.radangl, Z.radangl, Z.radangl);
            g.FillEllipse(System.Drawing.Brushes.White, 1, Z.clheight - 2 * Z.radangl - 1, Z.radangl * 2, Z.radangl * 2);
            g.FillRectangle(System.Drawing.Brushes.Maroon, Z.clwidth - Z.radangl, Z.clheight - Z.radangl, Z.radangl, Z.radangl);
            g.FillEllipse(System.Drawing.Brushes.White, Z.clwidth - 2 * Z.radangl - 1, Z.clheight - 2 * Z.radangl - 1, Z.radangl * 2, Z.radangl * 2);

            g.FillRectangle(System.Drawing.Brushes.Red, 0, 532 / 2 - 50, 3, 100);
            g.FillRectangle(System.Drawing.Brushes.Red, 790, 532 / 2 - 50, 3, 100);
            
            for (int i = 0; i < 11; i++)
            {
                if (i == 0 || i == 4)
                {
                    g.FillEllipse(Brushes.Blue, (int)(coinarr[i].x - coinarr[i].r) - 1, (int)(coinarr[i].y - coinarr[i].r) - 1, (int)(2 * coinarr[i].r) + 2, (int)(2 * coinarr[i].r) + 2);
                }
                coinarr[i].Draw(g);
            }

            if (vb != -1)
            {
                var spd = vbp.x * vbp.x + vbp.y * vbp.y;
                if (spd > Z.vismspd * Z.vismspd)
                {
                    g.DrawLine(System.Drawing.Pens.Red, (int)coinarr[vb].x, (int)coinarr[vb].y, (int)coinarr[vb].x + (int)(vbp.x * Z.vismspd / Math.Sqrt(spd)), (int)coinarr[vb].y + (int)(vbp.y * Z.vismspd / Math.Sqrt(spd)));
                }
                else
                {
                    g.DrawLine(System.Drawing.Pens.Green, (int)coinarr[vb].x, (int)coinarr[vb].y, (int)coinarr[vb].x + (int)(vbp.x), (int)coinarr[vb].y + (int)(vbp.y));
                }
            }
            var f = new System.Drawing.Font("Arial", 30);
            g.DrawString(ch1.ToString() + " : " + ch2.ToString(), f, System.Drawing.Brushes.Black, 793 / 2 - 40, 5);
            var fn = new System.Drawing.Font("Arial", 10);
            int yk = 0;
            if (rec.record) 
                g.DrawString("rec", fn, Brushes.Black, 60, 5 + (yk++) * 10);
            if (immunlast > 1) 
                g.DrawString("immun " + (immunlast - 1).ToString(), fn, Brushes.Black, 60, 5 + (yk++) * 10);
            border(g);
        }
        public void getgrp2(System.Drawing.Graphics g)
        {
            g.Clear(Color.Maroon);
            var fn = new System.Drawing.Font("Courier", 15);
            g.FillRectangle(Brushes.Red, 5, shopvb * 20, 170, 23);
            for (int i = 1; i < Program.countcoin; i++)
            {
                g.DrawString(Z.tcoin[i].name, fn, System.Drawing.Brushes.Bisque, 10, i * 20);
            }
            if (shopcur <= Program.countcoin && shopcur != -1)
                g.DrawString(Z.tcoin[shopcur].name, fn, System.Drawing.Brushes.White, 10, shopcur * 20);
            fn = new System.Drawing.Font("Courier", 30);
            //g.DrawLine(Pens.Black, 200, 0, 200, Z.clheight);
            g.DrawString("Масса: " + Z.tcoin[shopvb].m.ToString(), fn, System.Drawing.Brushes.White, 260, 50);
            g.DrawString("Диаметр: " + Z.tcoin[shopvb].r.ToString(), fn, System.Drawing.Brushes.White, 260, 100);
            g.DrawString("Кол-во: " + us[shoppl - 1].coin[shopvb].ToString(), fn, System.Drawing.Brushes.White, 260, 150);
            g.DrawString("Цена: " + Z.tcoin[shopvb].stoim.ToString(), fn, System.Drawing.Brushes.White, 260, 200);
            g.DrawString("Игрок " + shoppl, fn, System.Drawing.Brushes.White, 260, Z.clheight - 120);
            g.DrawString("Денег: " + us[shoppl - 1].mon.ToString(), fn, System.Drawing.Brushes.White, 260, Z.MainForm.ClientRectangle.Height - 70);
            g.DrawString("Купить", fn, System.Drawing.Brushes.Bisque, 600, 250);
            g.DrawString("Атака", fn, System.Drawing.Brushes.Bisque, 600, 300);
            g.DrawString("Защита", fn, System.Drawing.Brushes.Bisque, 600, 350);
            g.DrawString("Далее", fn, System.Drawing.Brushes.Bisque, 600, 400);
            if (shopcur == Program.countcoin + 1)
                g.DrawString("Купить", fn, System.Drawing.Brushes.White, 600, 250);
            if (shopcur == Program.countcoin + 2)
            {
                g.DrawString("Атака", fn, System.Drawing.Brushes.White, 600, 300);
                g.DrawImage(Z.tcoin[us[shoppl - 1].nap].picob, 640, 80, 80, 80);
            }
            if (shopcur == Program.countcoin + 3)
            {
                g.DrawString("Защита", fn, System.Drawing.Brushes.White, 600, 350);
                g.DrawImage(Z.tcoin[us[shoppl - 1].vrat].picob, 640, 80, 80, 80);
            }
            if (shopcur == Program.countcoin + 4)
                g.DrawString("Далее", fn, System.Drawing.Brushes.White, 600, 400);
            if (!shopanimon)
            {
                g.DrawImage(Z.tcoin[shopvb].picob, 310, 300, 80, 80);
                g.DrawImage(Z.tcoin[shopvb].picre, 410, 300, 80, 80);
            }
            else if (shopold)
            {
                g.DrawImage(Z.tcoin[shopoldcoin].picob, 310 + 40 - 40 * shopwidthanim / shopfull, 300, 80 * shopwidthanim / shopfull, 80);
                g.DrawImage(Z.tcoin[shopoldcoin].picre, 410 + 40 - 40 * shopwidthanim / shopfull, 300, 80 * shopwidthanim / shopfull, 80);
            }
            else if (!shopold)
            {
                g.DrawImage(Z.tcoin[shopvb].picob, 310 + 40 - 40 * shopwidthanim / shopfull, 300, 80 * shopwidthanim / shopfull, 80);
                g.DrawImage(Z.tcoin[shopvb].picre, 410 + 40 - 40 * shopwidthanim / shopfull, 300, 80 * shopwidthanim / shopfull, 80);
            }
            border(g);
        }
        public void border(System.Drawing.Graphics g)
        {
            if (Z.mininmon)
                g.DrawString("-", new Font("Arial", 15), Brushes.Bisque, Z.clwidth - 35, -3);
            else
                g.DrawString("-", new Font("Arial", 15), Brushes.CornflowerBlue, Z.clwidth - 35, -3);

            if (Z.closeon)
                g.DrawString("X", new Font("Arial", 15), Brushes.Bisque, Z.clwidth - 18, -3);
            else
                g.DrawString("X", new Font("Arial", 15), Brushes.CornflowerBlue, Z.clwidth - 18, -3);

        }
        public void klik(object sender, KeyEventArgs e)
        {
            if (sit == 1 || sit == 2)
            {
                if (e.KeyCode == System.Windows.Forms.Keys.S && !e.Control)
                {
                    shopvb = (shopvb + 1) % Program.countcoin;
                    if (shopvb == 0) 
                        shopvb++;
                }
                if (e.KeyCode == System.Windows.Forms.Keys.W)
                {
                    shopvb = (shopvb + Program.countcoin - 1) % Program.countcoin;
                    if (shopvb == 0) shopvb--;
                    shopvb = (shopvb + Program.countcoin) % Program.countcoin;
                }
                if (e.KeyCode == System.Windows.Forms.Keys.Enter)
                    buy_coin();
                if (e.KeyCode == System.Windows.Forms.Keys.A)
                    set_atac();
                if (e.KeyCode == System.Windows.Forms.Keys.D)
                    set_def();
                if (e.KeyCode == System.Windows.Forms.Keys.N)
                    next_magaz();
            }
            if (e.KeyCode == System.Windows.Forms.Keys.S && e.Control)
            {
                try
                {
                    var oup = System.IO.File.CreateText("save.txt");
                    oup.WriteLine(us[0].ToString());
                    oup.WriteLine(us[1].ToString());
                    oup.WriteLine(sit + " " + ch1 + " " + ch2);
                    oup.Close();
                    Spinet.Spinet.SpiFile("save.txt");
                }
                catch
                {

                }
            }
            if (e.KeyCode == System.Windows.Forms.Keys.R && e.Control)
            {
                try
                {
                    Spinet.Spinet.UnSpiFile("save.txt");
                    var inp = System.IO.File.OpenText("save.txt");
                    us[0].ReadSave(inp.ReadLine());
                    us[1].ReadSave(inp.ReadLine());
                    var sr = inp.ReadLine().Split().Select(x1 => int.Parse(x1)).ToArray();
                    inp.Close();
                    Spinet.Spinet.SpiFile("save.txt");
                    sit = sr[0];
                    ch1 = sr[1];
                    ch2 = sr[2];
                    if (sit > 0) shoppl = sit;
                    new_game(false);
                }
                catch
                { }
            }
            if (e.KeyCode == System.Windows.Forms.Keys.R && !e.Control)
            {
                try
                {
                    rec.record = !rec.record;
                    if (rec.record)
                        rec.oup = System.IO.File.CreateText("replay.chrpl");
                    else
                        rec.oup.Close();
                }
                catch
                { }
            }
        }
        public bool coin_stop()
        {
            bool an = true;
            for (int i = 0; i < coinarr.Length; i++)
            {
                an = an && coinarr[i].stop();
            }
            return an;
        }
        public void mdklik(object sender, MouseEventArgs e)
        {
            if (e.Y < 20)
            {
                Z.movewindow = true;
                Z.movewindowx = -e.X;
                Z.movewindowy = -e.Y;
            }
            if (e.Y < 20 && e.X > Z.clwidth - 20 && e.Button == MouseButtons.Left)
                Application.Exit();
            if (e.Y < 20 && e.X > Z.clwidth - 35 && e.X <= Z.clwidth - 20)
            {
                Z.MainForm.WindowState = FormWindowState.Minimized;
                Z.movewindow = false;
                Z.mininmon = false;
            }
            if (sit == 0 && coin_stop())
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    vb = -1;
                    return;
                }
                for (int i = per * 4 + 1; i < per * 4 + 4; i++)
                {

                    if ((coinarr[i].x - e.X) * (coinarr[i].x - e.X) +
                        (coinarr[i].y - e.Y) * (coinarr[i].y - e.Y) <
                        coinarr[i].r * coinarr[i].r)
                    {
                        vb = i;
                    }
                }
            }
            if (sit == 1 || sit == 2)
            {
                if (e.Y > 20 && e.X < 300 && e.Y < Program.countcoin * 20 && shopvb != e.Y / 20)
                {
                    if (!shopold)
                        shopoldcoin = shopvb;
                    shopanimon = true;
                    shoptim.Restart();
                    shopold = true;
                    shopvb = e.Y / 20;
                }
                if (e.Y > 250 && e.Y < 300 && e.X > 600)
                    buy_coin();
                if (e.Y > 300 && e.Y < 350 && e.X > 600)
                    set_atac();
                if (e.Y > 350 && e.Y < 400 && e.X > 600)
                    set_def();
                if (e.Y > 400 && e.Y < 450 && e.X > 600)
                    next_magaz();
            }
        }
        public void mmklik(object sender, MouseEventArgs e)
        {
            if (Z.movewindow && e.Button != MouseButtons.None)
            {
                Z.MainForm.Left += e.X + Z.movewindowx;
                Z.MainForm.Top += e.Y + Z.movewindowy;
            }
            if (MouseButtons.None == e.Button)
                Z.movewindow = false;
            if (e.Y < 20 && e.X > Z.clwidth - 20)
                Z.closeon = true;
            else
                Z.closeon = false;
            if (e.Y < 20 && e.X > Z.clwidth - 35 && e.X <= Z.clwidth - 20)
                Z.mininmon = true;
            else
                Z.mininmon = false;
            if (sit == 1 || sit == 2)
            {
                if (e.Y > 20 && e.X < 300 && e.Y < Program.countcoin * 20)
                    shopcur = e.Y / 20;
                else if (e.Y > 250 && e.Y < 450 && e.X > 600)
                    shopcur = Program.countcoin + (e.Y - 250) / 50 + 1;
                else
                    shopcur = -1;
            }
            if (sit == 0)
            {
                if (e.Button != System.Windows.Forms.MouseButtons.None && vb != -1)
                {
                    vbp.x = -coinarr[vb].x + e.X;
                    vbp.y = -coinarr[vb].y + e.Y;
                }
            }
        }
        public void muklik(object sender, MouseEventArgs e)
        {
            Z.movewindow = false;
            if (sit == 0)
            {
                if (vb != -1)
                {
                    var otn = 1.0 * Z.mspd / Z.vismspd;
                    var spd = (vbp.x * vbp.x + vbp.y * vbp.y) * otn * otn;
                    if (spd > Z.mspd * Z.mspd)
                    {
                        coinarr[vb].vec.x = -(vbp.x * otn * Z.mspd / Math.Sqrt(spd));
                        coinarr[vb].vec.y = -(vbp.y * otn * Z.mspd / Math.Sqrt(spd));
                    }
                    else
                    {
                        coinarr[vb].vec.x = -vbp.x * otn;
                        coinarr[vb].vec.y = -vbp.y * otn;
                    }
                    vb = -1;
                    per = (per + 1) % 2;
                    immunlast = Math.Max(0, immunlast - 1);
                }
            }
        }
        public void set_def()
        {
            if (us[shoppl - 1].coin[shopvb] >= 1 &&
                       (Z.tcoin[shopvb].m >= Z.tcoin[us[shoppl - 1].nap].m ||
                        Z.tcoin[shopvb].r >= Z.tcoin[us[shoppl - 1].nap].r))
            {
                us[shoppl - 1].coin[us[shoppl - 1].vrat] += 1;
                us[shoppl - 1].coin[shopvb] -= 1;
                us[shoppl - 1].vrat = shopvb;
            }
        }
        public void set_atac()
        {
            if (us[shoppl - 1].coin[shopvb] >= 3 &&
               (Z.tcoin[shopvb].m <= Z.tcoin[us[shoppl - 1].vrat].m ||
                Z.tcoin[shopvb].r <= Z.tcoin[us[shoppl - 1].vrat].r))
            {

                us[shoppl - 1].coin[us[shoppl - 1].nap] += 3;
                us[shoppl - 1].coin[shopvb] -= 3;
                us[shoppl - 1].nap = shopvb;
            }

        }
        public void buy_coin()
        {
            if (us[shoppl - 1].mon >= Z.tcoin[shopvb].stoim)
            {
                us[shoppl - 1].mon -= Z.tcoin[shopvb].stoim;
                us[shoppl - 1].coin[shopvb]++;// -= Z.tcoin[magaz.vb].stoim;
            }
        }
        public void next_magaz()
        {
            if (sit == 1)
            {
                sit++;
                shoppl = 2;
            }
            else
            {
                sit = 0;
                new_game();
            }
        }   
    }

    class user
    {
        public int[] coin { get; set; }
        public int nap { get; set; }
        public int vrat { get; set; }
        public int mon { get; set; }
        public user()
        {
            coin = new int[Program.countcoin];
            nap = 1;
            vrat = 1;
            mon = 0;
        }
        public override string ToString()
        {
            var an = coin[1].ToString();
            for (int i = 2; i < Program.countcoin; i++)
            {
                an = an + " " + coin[i];
            }
            return an + " " + nap.ToString() + " " + vrat.ToString() + " " + mon;
        }
        public void ReadSave(string s)
        {
            var sr = s.Split().Select(x1 => int.Parse(x1)).ToArray();
            for (int i = 0; i < Program.countcoin; i++)
            {
                coin[i] = 0;
            }
            for (int i = 1; i < Math.Min(sr.Length - 3, Program.countcoin); i++)
            {
                coin[i] = sr[i - 1];
            }

            nap = sr[sr.Length - 3];
            vrat = sr[sr.Length - 2];
            mon = sr[sr.Length - 1];
        }
        public int sen_arm()
        {
            return Z.tcoin[nap].stoim * 3 + Z.tcoin[vrat].stoim;
        }
    }

    class cointype
    {
        public int r { get; set; }
        public int m { get; set; }
        public int stoim { get; set; }
        public string name { get; set; }
        public System.Drawing.Image picob { get; set; }
        public Image picre { get; set; }
        public int valut = 0;
        public cointype(int r1, int m1, int s1, string n, string fileor, string filere, string vlt, string ras = ".png")
        {
            m = m1;
            r = r1;
            stoim = s1;
            name = n;
            picob = Image.FromFile("textures\\" + fileor + ras);
            picre = Image.FromFile("textures\\" + filere + ras);
            if (vlt == "r") valut = 1;
            if (vlt == "s") valut = 2;
            if (vlt == "e") valut = 3;
            if (vlt == "f") valut = 4;
            if (vlt == "cd") valut = 5;
        }
    }
}
