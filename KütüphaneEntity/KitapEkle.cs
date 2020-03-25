using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KütüphaneEntity
{
    public partial class KitapEkle : Form
    {
        public KitapEkle()
        {
            InitializeComponent();
        }

        TurkiyeKutuphaneEntityEntities2 db = new TurkiyeKutuphaneEntityEntities2();

        void combodoldur()
        {
            comboBox1.ValueMember = "yazarno";
            comboBox1.DisplayMember = "yazarad";
            comboBox1.DataSource = db.yazarlar.ToList();
            comboBox2.ValueMember = "turid";
            comboBox2.DisplayMember = "turad";
            comboBox2.DataSource = db.turler.ToList();
            comboBox3.ValueMember = "kurumid";
            comboBox3.DisplayMember = "kurumad";
            comboBox3.DataSource = db.kurumlar.ToList();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            kitaplar kit = new kitaplar();
            kit.isbno = Convert.ToInt16(textBox3.Text);
            kit.kitapadi = textBox4.Text;
            kit.yazarno = (int)comboBox1.SelectedValue;
            kit.turno = (int)comboBox2.SelectedValue;
            kit.kurumid = (int)comboBox3.SelectedValue;
            kit.sayfasayisi = Convert.ToInt16(textBox5.Text);
            kit.kitapresim = textBox6.Text;
            db.kitaplar.Add(kit);
            db.SaveChanges();
        }

        
        private void KitapEkle_Load(object sender, EventArgs e)
        {
            combodoldur();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            yazarlar yaz = new yazarlar();
            yaz.yazarad = textBox1.Text;
            db.yazarlar.Add(yaz);
            db.SaveChanges();
            combodoldur();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            turler tur = new turler();
            tur.turad = textBox2.Text;
            db.turler.Add(tur);
            db.SaveChanges();
            combodoldur();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            kurumlar kur = new kurumlar();
            kur.kurumad = textBox7.Text;
            db.kurumlar.Add(kur);
            db.SaveChanges();
            combodoldur();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            yazarlar y = db.yazarlar.Find(comboBox1.SelectedValue);
            db.yazarlar.Remove(y);
            db.SaveChanges();
            combodoldur();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            turler t = db.turler.Find(comboBox2.SelectedValue);
            db.turler.Remove(t);
            db.SaveChanges();
            combodoldur();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            kurumlar k = db.kurumlar.Find(comboBox3.SelectedValue);
            db.kurumlar.Remove(k);
            db.SaveChanges();
            combodoldur();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            yazarlar y = db.yazarlar.Find(comboBox1.SelectedValue);
            y.yazarad = textBox1.Text;
            db.SaveChanges();
            combodoldur();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            turler t = db.turler.Find(comboBox2.SelectedValue);
            t.turad = textBox2.Text;
            db.SaveChanges();
            combodoldur();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            kurumlar k = db.kurumlar.Find(comboBox3.SelectedValue);
            k.kurumad = textBox7.Text;
            db.SaveChanges();
            combodoldur();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var yazarvarmi = db.yazarlar.Where(y => y.yazarad.Contains(textBox1.Text)).FirstOrDefault();
            if (yazarvarmi != null)
            {
                comboBox1.SelectedValue = yazarvarmi.yazarno;
            }
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            kitaplar kit = db.kitaplar.Find(comboBox1.SelectedValue);
            kit.isbno = Convert.ToInt16(textBox3.Text);
            kit.kitapadi = textBox4.Text;
            kit.yazarno = (int)comboBox1.SelectedValue;
            kit.turno = (int)comboBox2.SelectedValue;
            kit.kurumid = (int)comboBox3.SelectedValue;
            kit.sayfasayisi = Convert.ToInt16(textBox5.Text);
            kit.kitapresim = textBox6.Text;
            db.SaveChanges();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var kitapsil = db.kitaplar.Where(k => k.yazarno.ToString()==comboBox1.SelectedValue.ToString()).FirstOrDefault();
            if (kitapsil != null)
            {

                db.kitaplar.Remove(kitapsil);
                db.SaveChanges();
            }
        }
    }
}
