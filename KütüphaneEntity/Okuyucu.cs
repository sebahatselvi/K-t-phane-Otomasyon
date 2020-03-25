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
    public partial class Okuyucu : Form
    {
        public Okuyucu()
        {
            InitializeComponent();
        }

        TurkiyeKutuphaneEntityEntities2 db = new TurkiyeKutuphaneEntityEntities2();

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            groupBox1.Enabled = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            groupBox4.Visible = true;
            groupBox1.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            okuyucular o = new okuyucular();
            o.okuyucuad = textBox3.Text;
            o.okuyucusoyad = textBox4.Text;
            o.okuyucuTC = textBox5.Text;
            o.okuyucuDogumTarih = dateTimePicker1.Value.ToShortDateString();
            o.okuyucuTel = textBox7.Text;
            o.okuyucuSifre = textBox8.Text;
            db.okuyucular.Add(o);
            db.SaveChanges();
            groupBox2.Visible = false;
            groupBox1.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var sifresifirla = db.okuyucular.Where(o => o.okuyucuTC == textBox14.Text).FirstOrDefault();
            if (sifresifirla != null)
            {
                if (sifresifirla.okuyucuad == textBox12.Text.ToLower() &&
                    sifresifirla.okuyucusoyad == textBox13.Text.ToLower() &&
                    sifresifirla.okuyucuDogumTarih == dateTimePicker2.Value.ToShortDateString() &&
                    sifresifirla.okuyucuTel == textBox16.Text)
                {
                    MessageBox.Show("Şifreniz Telefonunuza Gönderildi...");
                    groupBox4.Visible = false;
                    groupBox1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Şifre Sıfırlama Başarısız. Lütfen Girdiğiniz Bilgileri Gözden Geçirin...");
                }
            }
            else
            {
                MessageBox.Show("Böyle Bir TC Bulunamadı...");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var okuyucukontrol = db.okuyucular.Where(o => o.okuyucuTC == textBox1.Text).FirstOrDefault();
            if (okuyucukontrol != null)
            {
                if (okuyucukontrol.okuyucuSifre == textBox2.Text)
                {
                    MessageBox.Show("Giriş Yapıldı...");
                    groupBox3.Visible = true;
                    dataGridView1.Visible = true; groupBox1.Visible = false;
                }
                else
                {
                    MessageBox.Show("Şifre Yanlış...");
                }
            }
            else
            {
                MessageBox.Show("Böyle Bir TC Bulunamadı...");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Okuyucu_Load(object sender, EventArgs e)
        {
            comboBox1.ValueMember = "ilid";
            comboBox1.DisplayMember = "ilad";
            comboBox1.DataSource = db.iller.ToList();
            comboBox3.ValueMember = "turid";
            comboBox3.DisplayMember = "turad";
            comboBox3.DataSource = db.turler.ToList();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var okuyucukontrol = db.kurumlar.Select(x=>new { x.kurumad, x.ilid }).Where(k => k.ilid.ToString() == comboBox1.SelectedValue.ToString()).FirstOrDefault();
            if (okuyucukontrol != null)
            {
                comboBox2.ValueMember = "kurumid";
                comboBox2.DisplayMember = "kurumad";
                comboBox2.DataSource = okuyucukontrol.kurumad.ToList();
            }
            else
            {
                comboBox2.Text = "Kurum Seçiniz...";
            }
        }

        void kitapbul()
        {
            var list = db.kitaplar.Select(x => new
            {
                x.turler.turid,
                x.kurumid,
                x.kitapno,
                x.kitapadi,
                x.yazarlar.yazarad,
                x.yazarlar.yazarsoyad,
                x.turler.turad,
                x.sayfasayisi,
                x.kurumlar.kurumad,
                x.kitapdurum
            }).Where(x => x.kitapadi.Contains(textBox9.Text) && x.yazarad.Contains(textBox10.Text) && x.turid == (int)comboBox3.SelectedValue).ToList();
            dataGridView1.DataSource = list;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            kitapbul();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            kitapbul();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBox11.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            kayitlar k = new kayitlar();
            var bilgi = db.okuyucular.Where(o => o.okuyucuTC == textBox1.Text).FirstOrDefault();
            if(bilgi!=null)
            {
                k.okuyucuid = bilgi.okuyucuid;
                k.okudugukitapid = (int)dataGridView1.CurrentRow.Cells[2].Value;
                k.aldigikurumid = (int)dataGridView1.CurrentRow.Cells[1].Value;
                k.alisTarih = DateTime.Now.ToShortDateString();
                db.kayitlar.Add(k);
                db.SaveChanges();
                MessageBox.Show("İşlem Başarılı");
            }
            else
            {
                MessageBox.Show("Sistem Arızası");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var bilgi = db.okuyucular.Where(o => o.okuyucuTC == textBox1.Text).FirstOrDefault();
            var obilgi = db.kayitlar.Where(ob => ob.okuyucuid == bilgi.okuyucuid).FirstOrDefault();
            var kbilgi = db.kitaplar.Where(k => k.kitapno == obilgi.okudugukitapid).FirstOrDefault();
            if (obilgi != null)
            {
                MessageBox.Show("Kitap ID:    " + kbilgi.kitapadi + "     Alış Tarihi:   " + obilgi.alisTarih + "     İade Tarihi:   " + obilgi.iadeTarih);
            }
            else
            {
                MessageBox.Show("TC No Kontrol Ediniz...");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
