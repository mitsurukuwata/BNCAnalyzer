using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace btcChart
{   
    public partial class Form1 : Form
    {
        int globSayac = 0;
        int dataCount = 30;
        readonly List<double> globList = new List<double>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        public static double MtdCal(List<double> numberList)
        {
            double result;
            double avarage;
            double sum = 0;
            double mtdSum = 0;
            List<double> mtdList = new List<double>();

            for (int i = 0; i < numberList.Count; i++)
            {
                sum += numberList.ElementAt(i);
            }
            avarage = sum / numberList.Count;
            Console.WriteLine(avarage);
            for (int i = 0; i < numberList.Count; i++)
            {
                mtdList.Add(numberList.ElementAt(i) - avarage);
                if (mtdList.ElementAt(i) < 0)
                {
                    mtdList[i] = Math.Abs(mtdList.ElementAt(i));
                }
                Console.WriteLine(mtdList.ElementAt(i));
                mtdSum += mtdList.ElementAt(i);
            }

            result = mtdSum / mtdList.Count;
            return result;
        }

        //Standart Sapma Hesaplayıcı
        public double StdCal(List<double> numberList)
        {
            double result = 0;
            double sum = 0;
            double stdSum = 0;
            List<double> stdList = new List<double>();
            double avarage;
            for (int i = 0; i < numberList.Count; i++)
            {
                sum += numberList.ElementAt(i);
            }

            avarage = sum / numberList.Count;

            for (int i = 0; i < numberList.Count; i++)
            {
                stdList.Add((numberList.ElementAt(i) - avarage) * (numberList.ElementAt(i) - avarage));
                stdSum += stdList.ElementAt(i);
            }

            double stdFin = stdSum / (numberList.Count - 1);
            VaryansLabel.Text += stdFin.ToString() + "\n";
            result = Math.Sqrt(stdFin);

            return result;
        }

        public double AvarageStd()
        {
            if(globSayac % dataCount == 0)
            {
                double std = StdCal(globList);
                return std;
            }
            else
            {
                return 0;
            }   
        }

        public double AvarageMtd()
        {
            if (globSayac % dataCount == 0)
            {
                double mtd = MtdCal(globList);
                return mtd;
            }
            else
            {
                return 0;
            }
        }
     
        public void AvarageCal()
        {
            try
            {
                String URLString = "https://api.kucoin.com/api/v1/market/histories?symbol=XRP-USDT";
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString(URLString);
                    Root Test = JsonConvert.DeserializeObject<Root>(json);
                    double sum = 0;
                    for (int i = 0; i < Test.data.Count; i++)
                    {
                        sum += Convert.ToDouble(Test.data.ElementAt(i).price);
                    }
                    globList.Add(sum);
                    AvarageLabel.Text += (sum / Test.data.Count).ToString() + "\n";
                    if (globSayac % dataCount == 0)
                    {
                        double result = 0;
                        if(globList.Last() < globList.First())
                        {
                            result = ((globList.Last() - globList.First()) / globList.First()) * 100;
                            label1.Text += "\n Düşme Oranı : %" + result.ToString() + "\n ";
                            StdLabel.Text += "\n " + AvarageStd().ToString();
                            MtdLabel.Text += "\n" + AvarageMtd().ToString();
                     
                        }
                        else if (globList.Last() > globList.First())
                        {
                            result = ((globList.Last() - globList.First()) / globList.First()) * 100;
                            label1.Text += "\n Yükselme Oranı : %" + result.ToString() + "\n ";
                            StdLabel.Text += "\n " + AvarageStd().ToString();
                            MtdLabel.Text += "\n" + AvarageMtd().ToString();

                        }
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
      
        public void Moment()
        {      
            int sellSayac = 0;
            int buySayac = 0;
            
            try
            {
                String URLString = "https://api.kucoin.com/api/v1/market/histories?symbol=XRP-USDT";
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString(URLString);
                    Root Test = JsonConvert.DeserializeObject<Root>(json);
                    
                    for (int i = 0; i < Test.data.Count; i++)
                    {  
                        
                        PriceNumberLabel.Text += Test.data.ElementAt(i).price + "\n";
                        if (Test.data.ElementAt(i).side == "sell")
                            sellSayac++;
                        else
                            buySayac++;
                    }

                    if (sellSayac > buySayac)
                    {
                        PercentNumberLabel.Text += "sell " + $"%{sellSayac} \nDurum : ";
                    }
                    else if (buySayac > sellSayac)
                    {
                        
                        PercentNumberLabel.Text += "buy " + $"%{buySayac} \nDurum : ";
                    }
                    else
                    {
                        PercentNumberLabel.Text += "%50 %50 \nDurum : ";
                    }
                    label3.Text = globSayac.ToString() + "\n";
                    globSayac++;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        public void Moment24Hour()
        {
            try
            {
                String URLString = "https://api.kucoin.com/api/v1/market/stats?symbol=XRP-USDT";
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString(URLString);
                    Hour24 Test = JsonConvert.DeserializeObject<Hour24>(json);

                    Hour24Label.Text += $"\n En iyi alış fiyatı : {Test.data.buy} \n" + $"En iyi satış fiyatı : {Test.data.sell} \n" + $"Değişim Oranı : {Test.data.changeRate} \n" + $"En yüksek fiyat : {Test.data.high} \n" + $"En düşük fiyat : {Test.data.low} \n" + $"Ortalama işlem fiyatı : {Test.data.averagePrice} \n";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Hata 24 Saat");
            }           
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Moment();
            AvarageCal();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void Hour24Btn_Click(object sender, EventArgs e)
        {
            Moment24Hour();
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            PercentNumberLabel.Text = "Durum : ";
        }

        private void DataCountBtn_Click(object sender, EventArgs e)
        {
            dataCount = int.Parse(DataCountLabel.Text);
        }
    }
}
