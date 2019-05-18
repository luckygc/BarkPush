using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web;
using System.Net;

namespace BarkPush
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists(@"C:\Users\Public\Documents\Barkconfig.ini"))
            {
                FileStream Fs = new FileStream(@"C:\Users\Public\Documents\Barkconfig.ini", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamReader sr = new StreamReader(Fs);
                tbxKey.Text = sr.ReadLine();
                sr.Close();
                Fs.Close();
                tbxKey.IsEnabled = false;

                btnSaveKey.IsEnabled = false;


            }
            else
            {
                tbxKey.Text = "输入你的key";
            }

            if (!"".Equals(System.Windows.Clipboard.GetText()))
            {
                tbxContent.Text = System.Windows.Clipboard.GetText();
            }
        }

        private void BtnEditKey_Click(object sender, RoutedEventArgs e)
        {
            tbxKey.IsEnabled = true;
            btnSaveKey.IsEnabled = true;

        }

        private void BtnSaveKey_Click(object sender, RoutedEventArgs e)
        {
            FileStream Fs = new FileStream(@"C:\Users\Public\Documents\Barkconfig.ini", FileMode.Create);
            StreamWriter sw = new StreamWriter(Fs);
            sw.Write(tbxKey.Text);
            sw.Flush();
            sw.Close();
            Fs.Close();
            btnSaveKey.IsEnabled = false;
            tbxKey.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            String title = "";
            String Content = "";
            String Url = "";
            String Copy = "automaticallyCopy=1";
            String param = "";
            if (!"".Equals(tbxTitle.Text))
            {
                title = "/" + tbxTitle.Text;
            }

            if (!"".Equals(tbxContent.Text))
            {
                if (tbxContent.Text.Contains("http://") || tbxContent.Text.Contains("https://"))
                {
                    Content = "/链接已复制";
                    String copyurl = tbxContent.Text.ToString();
                    Copy = "automaticallyCopy=1&copy="+ copyurl;
                    ;

                }
                else
                {
                    Content = "/" + tbxContent.Text;
                }

            }
            

            if (!"".Equals(tbxUrl.Text))
            {
                Url = "url=" + tbxUrl.Text;
            }

            if (chboxCopy.IsChecked == false && !"".Equals(tbxUrl.Text))
            {
                param = "?" + Url + "&" + Copy;
            }
            else if (!"".Equals(tbxUrl.Text))
            {
                param = "?" + Url;
            }
            else if (chboxCopy.IsChecked == true)
            {
                param = "?" + Copy;
            }




            //String push = "https://www.gcgc.tech/notify/" + tbxKey.Text + title + Content + param;
            String push = "https://api.day.app/" + tbxKey.Text + title + Content + param;


            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(push);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            String result = "";
            try
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                    if (result.Contains("200"))
                    {
                        MessageBox.Show("推送成功");
                    }
                    else
                    {
                        MessageBox.Show("推送失败");
                    }

                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
            finally
            {
                stream.Close();
            }

        }


    }
}
