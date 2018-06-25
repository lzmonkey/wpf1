using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        static char[] deviceChoice = new char[9] { '0', '0', '0', '0', '0', '0', '0', '0','0' };
        byte[] sendpak;
        static int sendConfigFlag = 0;
        public MainWindow()
        {

            Thread t = new Thread(ReciveMsg);//开启接收消息线程  
            t.Start();
            //udp---------------------
            InitializeComponent();

        }

        
        /// <summary>  
        /// 向特定ip的主机的端口发送数据报  
        /// </summary>  
        private void sendMsg()
        {
            EndPoint point = new IPEndPoint(IPAddress.Parse("192.168.100.233"), 6000);
            while (true)
            {
                //string msg = Console.ReadLine();
                //server.SendTo(Encoding.UTF8.GetBytes(msg), point);

            }


        }
        /// <summary>  
        /// 接收发送给本机ip对应端口号的数据报  
        /// </summary>  
        private void ReciveMsg()
        {

            ///////////////////////////////////////////////////////////////////////

            int recv;
            byte[] dataU = new byte[1024];

            //得到本机IP，设置TCP端口号         
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 2333);
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //绑定网络地址
            newsock.Bind(ip);

            Console.WriteLine("This is a Server, host name is {0}", Dns.GetHostName());

            //等待客户机连接
            Console.WriteLine("Waiting for a client");

            //得到客户机IP
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);


            //////////////////////////////////////////////////////////////////////
            while (true)
            {
                byte[] buffer = new byte[1024];
                recv = newsock.ReceiveFrom(buffer, ref Remote);
                Console.WriteLine("Message {1} received from {0}: ", Remote.ToString() ,recv);
                //newsock.SendTo(dataU, recv, SocketFlags.None, Remote);

                string idResult ="";
                //byte[] start = { 0xa5, 0x5a };
                if ( recv >= 182 && ( recv== 185 ) )
                {
                    if(buffer[0]==0xA5 && buffer[1]==0x5A)
                    {
                        Console.WriteLine("包头验证通过 " );

                        //取id号
                        byte[] id = new byte[12];
                        for(int i=0;i<12;i++)
                        {
                            id[i] = buffer[6 + i];
                        }
                        string bc;
                        foreach (byte b in id)
                        {
                            idResult+=b.ToString("x2");
                        }
                        Console.WriteLine(idResult);

                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            deviceIdlabel.Content = idResult;
                        });
                        //读测斜A
                        if(buffer[18]==1)
                        {
                            //byte[] byteTemp = new byte[4] { buffer[19], buffer[19], buffer[19], buffer[19] };
                            byte[] byteTemp = new byte[4]  ;
                            for(int dcount=0;dcount<6;dcount++)
                            {
                                for(int byteIndex=0;byteIndex<4;byteIndex++)
                                {
                                    byteTemp[byteIndex] = buffer[19 + dcount * 4 + byteIndex];
                                }
                                float fTemp = BitConverter.ToSingle(byteTemp, 0);
                                switch (dcount)
                                {
                                    case 0:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccaax.Content = fTemp.ToString();
                                        });break;
                                    case 1:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccaay.Content = fTemp.ToString();
                                        }); break;
                                    case 2:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccaaz.Content = fTemp.ToString();
                                        }); break;
                                    case 3:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccatx.Content = fTemp.ToString();
                                        }); break;
                                    case 4:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccaty.Content = fTemp.ToString();
                                        }); break;
                                    case 5:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccatz.Content = fTemp.ToString();
                                        }); break;
                                }

                            }

                        }
                        else
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Ccaax.Content = "";
                                Ccaay.Content = "";
                                Ccaaz.Content = "";
                                Ccatx.Content = "";
                                Ccaty.Content = "";
                                Ccatz.Content = "";
                            });
                        }

                        //读测斜B
                        if (buffer[43] == 1)
                        {
                            //byte[] byteTemp = new byte[4] { buffer[19], buffer[19], buffer[19], buffer[19] };
                            byte[] byteTemp = new byte[4] ;
                            for (int dcount = 0; dcount < 6; dcount++)
                            {
                                for (int byteIndex = 0; byteIndex < 4; byteIndex++)
                                {
                                    byteTemp[byteIndex] = buffer[43 + dcount * 4 + byteIndex];
                                }
                                float fTemp = BitConverter.ToSingle(byteTemp, 0);
                                switch (dcount)
                                {
                                    case 0:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccbax.Content = fTemp.ToString();
                                        }); break;
                                    case 1:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccbay.Content = fTemp.ToString();
                                        }); break;
                                    case 2:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccbaz.Content = fTemp.ToString();
                                        }); break;
                                    case 3:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccbtx.Content = fTemp.ToString();
                                        }); break;
                                    case 4:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccbty.Content = fTemp.ToString();
                                        }); break;
                                    case 5:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccbtz.Content = fTemp.ToString();
                                        }); break;
                                }

                            }

                        }
                        else
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Ccbax.Content = "";
                                Ccbay.Content = "";
                                Ccbaz.Content = "";
                                Ccbtx.Content = "";
                                Ccbty.Content = "";
                                Ccbtz.Content = "";
                            });
                        }
                        //读测斜D
                        if (buffer[93] == 1)
                        {
                            //byte[] byteTemp = new byte[4] { buffer[19], buffer[19], buffer[19], buffer[19] };
                            byte[] byteTemp = new byte[4];
                            for (int dcount = 0; dcount < 6; dcount++)
                            {
                                for (int byteIndex = 0; byteIndex < 4; byteIndex++)
                                {
                                    byteTemp[byteIndex] = buffer[93 + dcount * 4 + byteIndex];
                                }
                                float fTemp = BitConverter.ToSingle(byteTemp, 0);
                                switch (dcount)
                                {
                                    case 0:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccdax.Content = fTemp.ToString();
                                        }); break;
                                    case 1:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccday.Content = fTemp.ToString();
                                        }); break;
                                    case 2:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccdaz.Content = fTemp.ToString();
                                        }); break;
                                    case 3:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccdtx.Content = fTemp.ToString();
                                        }); break;
                                    case 4:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccdty.Content = fTemp.ToString();
                                        }); break;
                                    case 5:
                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            Ccdtz.Content = fTemp.ToString();
                                        }); break;
                                }

                            }

                        }
                        else
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Ccdax.Content = "";
                                Ccday.Content = "";
                                Ccdaz.Content = "";
                                Ccdtx.Content = "";
                                Ccdty.Content = "";
                                Ccdtz.Content = "";
                            });
                        }
                        //读沉降A
                        if (buffer[118] == 1)
                        {
                            byte[] data = new byte[4];
                            data[0] = buffer[119];
                            data[1] = buffer[120];
                            data[2] = buffer[121];
                            data[3] = buffer[122];
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Ccja.Content = BitConverter.ToInt32(data, 0);
                            });
                        }
                        else
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Ccja.Content = "";
                            });
                        }
                        //读沉降B
                        if (buffer[123] == 1)
                        {
                            byte[] data = new byte[4];
                            data[0] = buffer[124];
                            data[1] = buffer[125];
                            data[2] = buffer[126];
                            data[3] = buffer[127];
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Ccjb.Content = BitConverter.ToInt32(data, 0);
                            });
                        }
                        else
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Ccjb.Content = "";
                            });
                        }

                        //液位
                        if (buffer[172] == 1)
                        {
                            byte[] data = new byte[5];
                            data[0] = buffer[173];
                            data[1] = buffer[174];
                            data[2] = buffer[175];
                            data[3] = buffer[176];
                            data[4] = buffer[177];
                            ASCIIEncoding ASCIITochar = new ASCIIEncoding();
                            char[] ascii = ASCIITochar.GetChars(data);
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Ccsyw.Content = ASCIITochar.GetString(data);
                            });
                        }
                        else
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Ccsyw.Content = "";
                            });

                        }
                            //渗压计
                            if (buffer[146] == 1)
                        {
                            byte[] data = new byte[4];
                            data[0] = buffer[155];
                            data[1] = buffer[156];
                            data[2] = buffer[157];
                            data[3] = buffer[158];
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Csyj.Content = BitConverter.ToInt32(data, 0);
                            });
                        }
                        else
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Csyj.Content = "";
                            });
                        }

                        //读沉降A
                        if (buffer[178] == 1)
                        {
                            byte[] data = new byte[4];
                            data[0] = buffer[179];
                            data[1] = buffer[180];
                            data[2] = buffer[181];
                            data[3] = buffer[182];
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Cywj.Content = BitConverter.ToInt32(data, 0);
                            });
                        }
                        else
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Cywj.Content = "";
                            });
                        }
                    }
                }
                if(recv==31)//雨量
                {
                    if (buffer[0] == 0xA5 && buffer[1] == 0x5A)
                    {
                        byte[] data = new byte[4];
                        data[0] = buffer[23];
                        data[1] = buffer[24];
                        data[2] = buffer[25];
                        data[3] = buffer[26];
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            Cyl.Content = BitConverter.ToInt32(data, 0);
                        });
                    }
                    else
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            Cyl.Content ="";
                        });
                    }
                }
                string message = Encoding.UTF8.GetString(buffer, 0, recv);
                sendpak = new byte[12];
                sendpak[0] = 0xff;
                int sleeptime = 30;
                for(int i=1;i<10;i++)
                {
                    sendpak[i] = (byte)deviceChoice[i - 1];
                }
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (updataValue.Text.ToString().Length == 0)
                    {
                        sleeptime = 30;
                    }
                    else
                    {

                        try
                        {
                            sleeptime = Convert.ToInt32(updataValue.Text.ToString());
                            if (sleeptime > 300)
                            {
                                sleeptime = 60;
                            }
                        }
                        catch (Exception e)
                        {
                            sleeptime = 30;
                        }
                    }
                });
                
                sendpak[10] = (byte)(sleeptime%65536/256);
                sendpak[11] = (byte)(sleeptime % 256);
                //newsock.SendTo(dataU, recv, SocketFlags.None, Remote);
                if (sendConfigFlag==1)
                {

                    int sendcount = newsock.SendTo(sendpak, Remote);
                    Console.WriteLine("WWWwrite " + sendcount + "bytes to point\n");
                    sendConfigFlag = 0;


                }
            }
        }

        private void submitConfig(object sender, RoutedEventArgs e)
        {
            sendConfigFlag = 1;
            Console.WriteLine(deviceChoice);
        }

        private void cxDchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[1] = '1';
        }

        private void cxDunchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[1] = '0';
        }

        private void cxBchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[7] = '1';
        }

        private void cxBunchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[7] = '0';
        }

        private void cxAchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[8] = '1';
        }

        private void cxAunchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[8] = '0';
        }

        private void cjAchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[6] = '1';
        }

        private void cjAunchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[6] = '0';
        }

        private void cjBchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[5] = '1';
        }

        private void cjBunchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[5] = '0';
        }

        private void ylchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[4] = '1';
        }

        private void ylunchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[4] = '0';
        }

        private void ywchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[3] = '1';
        }

        private void ywunchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[3] = '0';
        }

        private void syjchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[2] = '1';
        }

        private void syjunchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[2] = '0';
        }

        private void ywjchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[0] = '1';
        }

        private void ywjunchecked(object sender, RoutedEventArgs e)
        {
            deviceChoice[0] = '0';
        }
    }

}
