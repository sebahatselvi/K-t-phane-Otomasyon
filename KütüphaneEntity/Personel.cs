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
    public partial class Personel : Form
    {
        public Personel()
        {
            InitializeComponent();
        }

        TurkiyeKutuphaneEntityEntities2 db = new TurkiyeKutuphaneEntityEntities2();

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            groupBox1.Enabled = false;
        }
        //personel girişyap
        private void button2_Click(object sender, EventArgs e)
        {
            var personelkontrol = db.personeller.Where(p => p.personelTC == textBox1.Text).FirstOrDefault();
            if (personelkontrol != null)
            {
                if (personelkontrol.personelSifre == textBox2.Text)
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

        private void label3_Click(object sender, EventArgs e)
        {
            groupBox4.Visible = true;
            groupBox1.Enabled = false;
        }
        //personel ekleme-kayıt
        private void button3_Click(object sender, EventArgs e)
        {
            personeller p = new personeller();
            p.personelad = textBox3.Text;
            p.personelsoyad = textBox4.Text;
            p.personelTC = textBox5.Text;
            p.personelDogumTarih = dateTimePicker1.Value.ToShortDateString();
            p.personelTel = textBox7.Text;
            p.personelKurumid = Convert.ToInt16(textBox8.Text);
            p.personelSifre = textBox9.Text;
            db.personeller.Add(p);
            db.SaveChanges();

            groupBox2.Visible = false;
            groupBox1.Enabled = true;
        }
        //personel şifre sıfırlama
        private void button5_Click(object sender, EventArgs e)
        {
            var sifresifirla = db.personeller.Where(p => p.personelTC == textBox12.Text).FirstOrDefault();
            if (sifresifirla != null)
            {
                if (sifresifirla.personelad == textBox10.Text.ToLower() &&
                    sifresifirla.personelsoyad == textBox11.Text.ToLower() &&
                    sifresifirla.personelDogumTarih == dateTimePicker2.Value.ToShortDateString() &&
                    sifresifirla.personelTel == textBox14.Text &&
                    sifresifirla.personelKurumid == Convert.ToInt16(textBox15.Text))
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

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void kitapbul()
        {
            var list = db.kitaplar.Select(x => new
            {
                x.turler.turid,
                x.isbno,
                x.kitapno,
                x.kurumid,
                x.kitapadi,
                x.yazarlar.yazarad,
                x.yazarlar.yazarsoyad,
                x.turler.turad,
                x.sayfasayisi,
                x.kurumlar.kurumad,
                x.kitapdurum
            }).Where(x => (x.kitapadi.Contains(textBox18.Text) && x.yazarad.Contains(textBox19.Text) && x.turid == (int)comboBox1.SelectedValue) || x.isbno.ToString() == textBox17.Text).ToList();
            dataGridView1.DataSource = list;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].Visible = false;
        }
        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            kitapbul();
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            kitapbul();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            kitapbul();
        }

        private void Personel_Load(object sender, EventArgs e)
        {
            comboBox1.ValueMember = "turid";
            comboBox1.DisplayMember = "turad";
            comboBox1.DataSource = db.turler.ToList();
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            kitapbul();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBox20.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var kitapver = db.okuyucular.Where(o => o.okuyucuTC == textBox16.Text).FirstOrDefault();
            if (kitapver != null)
            {
                if (textBox20.Text != "")
                {
                    kayitlar kay = new kayitlar();
                    kay.okuyucuid = kitapver.okuyucuid;
                    kay.okudugukitapid = (int)dataGridView1.CurrentRow.Cells[2].Value;
                    kay.aldigikurumid = (int)dataGridView1.CurrentRow.Cells[3].Value;
                    kay.alisTarih = DateTime.Now.ToShortDateString();
                    kay.iadeTarih = DateTime.Now.AddDays(15).ToShortDateString();
                    db.kayitlar.Add(kay);
                    db.SaveChanges();
                    MessageBox.Show("İşlem Başarılı...");
                }
                else
                {
                    MessageBox.Show("Verilecek Kitabı Seçiniz...");
                }
            }
            else
            {
                MessageBox.Show("TC No Kontrol Ediniz...");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var bilgi = db.okuyucular.Where(o => o.okuyucuTC == textBox16.Text).FirstOrDefault();
            if (bilgi != null)
            {
                MessageBox.Show("Okuyucu Ad:    " + bilgi.okuyucuad + "     Okuyucu Soyad:   " + bilgi.okuyucusoyad + "     Okuyucu Puan:   " + bilgi.okuyucuPuan);
            }
            else
            {
                MessageBox.Show("TC No Kontrol Ediniz...");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            KitapEkle kit = new KitapEkle();
            kit.ShowDialog();
        }
    }
}
