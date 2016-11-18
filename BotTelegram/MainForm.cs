using Cleverbot.Net;
using System;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace BotTelegram
{
    public partial class MainForm : Form
    {
        const string token = "220172800:AAEGNxGw_LelRyBHfYMgXqtKRm-5jgo6B8g";

        public TelegramBotClient bot = new TelegramBotClient(token);

        public bool Active { get; private set; } = false;

        ComputerUsage mComputerUsage = new ComputerUsage();

        //Cleverbot
        CleverbotSession session = CleverbotSession.NewSession("jzgikgzjzCinNaIH", "45OE8rALcSmL1zjMlGorkpdRPpj5bsQe");

        public MainForm()
        {
            InitializeComponent();
            bot.OnMessage += BotOnMessageReceived;
            bot.OnMessageEdited += BotOnMessageReceived;
            bot.OnReceiveError += BotOnReceiveError;
        }


        private async void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            if (Active)
            {
                listBox1.Invoke(
                    new add(addToListbox),
                    new object[] {
                    e.Message.From.FirstName + ": "+ e.Message.Text.ToString()
                    }
                );
                switch (e.Message.Text)
                {
                    case "?chatfirstname":
                        await bot.SendTextMessageAsync(e.Message.From.Id, e.Message.From.FirstName);
                        break;
                    case "?chatlastname":
                        await bot.SendTextMessageAsync(e.Message.From.Id, e.Message.From.LastName);
                        break;
                    case "?chatusername":
                        await bot.SendTextMessageAsync(e.Message.From.Id, e.Message.From.Username);
                        break;
                    case "?cpu":
                        await bot.SendTextMessageAsync(e.Message.From.Id, mComputerUsage.getCurrentCpuUsage());
                        break;
                    case "?ram":
                        await bot.SendTextMessageAsync(e.Message.From.Id, mComputerUsage.getAvailableRAM());
                        break;
                    //case "/shutdown":
                    //    await bot.SendTextMessageAsync(e.Message.From.Id, mComputerUsage.shutdown());
                    //    break;
                    //case "/restart":
                    //    await bot.SendTextMessageAsync(e.Message.From.Id, mComputerUsage.restart());
                    //    break;
                    case "/sperren":
                        await bot.SendTextMessageAsync(e.Message.From.Id, mComputerUsage.lockWorkstation());
                        break;
                    default:
                            if (e.Message.Text.StartsWith("?") || e.Message.Text.StartsWith("/"))
                            {
                                await bot.SendTextMessageAsync(e.Message.From.Id, "Diese Befehl ist mir leider nicht bekannt");
                            }
                            else
                            {
                            string answer = session.Send(e.Message.Text);
                            await bot.SendTextMessageAsync(e.Message.From.Id, answer);
                            listBox1.Invoke(
                                new add(addToListbox),
                                new object[] {
                                    e.Message.From.FirstName + ": "+ "Message: " + e.Message.Text + "\n" + " ~~ Bot: " + answer
                                });
                            }

                        break;
                }
            }
        }

        private void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            if (Active)
            {
                listBox1.Invoke(
                    new add(addToListbox),
                    new object[] {
                    "--> FEHLER: " + e.ApiRequestException.Message
                    }
                );
            }
        }

        private void activateButton_Click(object sender, EventArgs e)
        {
            if (Active)
            {
                activateButton.Text = "Offline";
                activateButton.BackColor = System.Drawing.Color.Red;
                listBox1.Items.Add("*** Bot ist jetzt offline! ***");
                Active = false;
            }
            else
            {
                listBox1.Items.Clear();
                activateButton.Text = "Online";
                activateButton.BackColor = System.Drawing.Color.Green;
                listBox1.Items.Add("*** Bot ist jetzt online! ***");
                listBox1.Items.Add("--> Waiting for Messages");
                Active = true;
            }
            if (Active)
            {
                testApi();
            }
        }
        public void testApi()
        {
            var me = bot.GetMeAsync().Result;
            var result = "API-TEST: [Botusername: " + me.Username + " (" + me.FirstName + " " + me.LastName + ")" + " ID: " + me.Id + "]";
            listBox1.Items.Add(result);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var Sender = new Ping();
            var Result = Sender.Send("telegram.org");
            if (Result.Status == IPStatus.Success)
                toolStripStatusLabel1.Text = "Internetverbindung hergestellt";
            else
                toolStripStatusLabel1.Text = "Keine Verbindung zum Internet möglich";
            Sender.Dispose();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = -1;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            bot.StartReceiving();
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        public delegate void add(string Text);

        public void addToListbox(string Text)
        {
            listBox1.Items.Add(Text);
        }
    }
}
