using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Specialized;
using System.Threading;
using System.Net.Http.Headers;

namespace clikerGame
{
    public partial class Form1 : Form
    {
        Player player;
        Enemy enemy;
        Shop shop;
        int paintPercent = 4;
        int imagenumber = 0;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.ReloadEnemy);
            player = new Player();
            enemy = new Enemy();
            shop = new Shop();
            button1.BackgroundImage = imageList1.Images[0];
            timer1.Start();
            UpdateData();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            enemy.DealDamage(player.damage);
            int percent = paintPercent * (100 - (enemy.HP / (enemy.MaxHP / 100)));
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            SolidBrush GreenBrush = new SolidBrush(Color.Green);
            g.FillRectangle(GreenBrush, 1, 1, 400 - percent, 20);

            Random r = new Random();
            int x = r.Next(25, 375);
            int y = r.Next(25, 320);
            Point p = new Point(x, y);
            Graphics b = button1.CreateGraphics();
            b.DrawString("-" + player.damage + "CLICK", new Font("Arial", 16), new SolidBrush(Color.BlueViolet), p);

            if (enemy.HP <= 0)
            {
                player.GetGold(enemy.price);
                g.FillRectangle(GreenBrush, 1, 1, 400, 20);
                enemy.LevelUp();
                if (imagenumber == 0)
                {
                    button1.BackgroundImage = imageList1.Images[1];
                    imagenumber = 1;
                }
                else if (imagenumber == 1)
                {
                    button1.BackgroundImage = imageList1.Images[0];
                    imagenumber = 0;
                }
            }
            UpdateData();

        }

        void ReloadEnemy(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush GreenBrush = new SolidBrush(Color.Green);

            g.FillRectangle(GreenBrush, 1, 1, 400, 20);
        }

        void UpdateData()
        {
            label1.Text = "Клик урон = " + player.damage;
            label2.Text = "Золото = " + player.gold;
            label3.Text = "Урон в секунду = " + player.autodamage;
            label4.Text = "HP: " + enemy.HP;
            button2.Text = "Прокачать Клик урон за " + shop.price1 + " золотих";
            button3.Text = "Прокачать Урон в секунду за " + shop.price2 + " золотих";
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (player.BuyAttack(shop.price1))
            {
                shop.PriceUp1();
                UpdateData();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (player.BuyAutoAttack(shop.price2))
            {
                shop.PriceUp2();
                UpdateData();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            enemy.DealDamage(player.autodamage/2);
            int percent = paintPercent * (100 - (enemy.HP / (enemy.MaxHP / 100)));
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            SolidBrush GreenBrush = new SolidBrush(Color.Green);
            g.FillRectangle(GreenBrush, 1, 1, 400 - percent, 20);

            Random r = new Random();
            int x = r.Next(25, 375);
            int y = r.Next(25, 320);
            Point p = new Point(x, y);
            Graphics b = button1.CreateGraphics();
            b.DrawString("-" + player.autodamage, new Font("Arial", 16), new SolidBrush(Color.Red), p);

            if (enemy.HP <= 0)
            {
                player.GetGold(enemy.price);
                g.FillRectangle(GreenBrush, 1, 1, 400, 20);
                enemy.LevelUp();
                if (imagenumber == 0)
                {
                    button1.BackgroundImage = imageList1.Images[1];
                    imagenumber = 1;
                }
                else if (imagenumber == 1)
                {
                    button1.BackgroundImage = imageList1.Images[0];
                    imagenumber = 0;
                }
            }
            UpdateData();
        }

    }
    public class Shop
    {
        public int price1 { get; private set; }
        public int price2 { get; private set; }

        public Shop()
        {
            price1 = 5;
            price2 = 5;
        }

        public void PriceUp1()
        {
            price1 = price1 * 2;
        }
        public void PriceUp2()
        {
            price2 = price2 * 2;
        }
    }
    public class Player
    {
        public int gold { get; private set; }
        public int damage { get; private set; }
        public int autodamage { get; private set; }
    
        public Player()
        {
            this.gold = 0;
            this.damage = 50;
            this.autodamage = 20;
        }

        public void GetGold(int gold)
        {
            this.gold += gold;
        }

        public bool BuyAttack(int price)
        {
            if(gold >= price)
            {
                gold -= price;
                damage = damage * 2;
                return true;
            }
            return false;

        }

        public bool BuyAutoAttack(int price)
        {
            if (gold >= price)
            {
                gold -= price;
                autodamage = autodamage * 2;
                return true;
            }
            return false;

        }
    }

    public class Enemy
    {
        public int MaxHP { get; private set; }
        public int HP { get; private set; }
        public int price { get; private set; }

        public Enemy()
        {
            this.MaxHP = 400;
            this.price = 5;
            HP = MaxHP;
        }

        public void DealDamage(int damage)
        {
            HP -= damage;
        }

        public void LevelUp()
        {
            MaxHP = MaxHP * 2;
            price = price * 2;
            HP = MaxHP;
        }
    }

    
}
