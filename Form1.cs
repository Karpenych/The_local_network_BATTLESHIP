using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ButtleShip
{
    public partial class Form1 : Form
    {
        public Socket listenSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Socket handlerSocket;
        public byte[] data = new byte[10];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawField(280);
            DrawField(850);
        }

        private void DrawField(int margin)
        {
            Graphics graphics = CreateGraphics();
            Pen pen = new(Color.Navy, 2.0f);

            Point pt1 = new(margin, 300);
            Point pt2 = new(margin, 800);
            for (int i = 0; i < 11; ++i)
            {
                graphics.DrawLine(pen, pt1, pt2);
                pt1.X += 50;
                pt2.X += 50;
            }

            pt1 = new(margin, 300);
            pt2 = new(margin + 500, 300);
            for (int i = 0; i < 11; ++i)
            {
                graphics.DrawLine(pen, pt1, pt2);
                pt1.Y += 50;
                pt2.Y += 50;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs mouse)
        {
            if (mouse.X >= 850 && mouse.X <= 1350 && mouse.Y >= 300 && mouse.Y <= 800)          // ONLY ENEMY FIELD CLICK_LISTENER
            {
                byte column = (byte)((mouse.X - 850) / 50 + 1);
                byte row = (byte)((mouse.Y - 300) / 50 + 1);
                byte bottom, right, shipLength = 1;
                bool isVertical = false, isError = false;
                
                if (Cells.enemyFieldCondition[row, column] == 1)      // IF SHIP
                {
                    Effects.ClickEffect(out PictureBox miss, mouse, "boom");
                    Controls.Add(miss);
                    Cells.enemyFieldCondition[row, column] = 3;
                }
                else                                               // IF EMPTY
                {
                    Effects.ClickEffect(out PictureBox miss, mouse, "splash");
                    Controls.Add(miss);
                    Cells.enemyFieldCondition[row, column] = 2;
                }

                if (Cells.enemyFieldCondition[row - 1, column] != 1 && Cells.enemyFieldCondition[row, column + 1] != 1 &&    // SPLASHS AROUNG DEAD SHIP
                    Cells.enemyFieldCondition[row + 1, column] != 1 && Cells.enemyFieldCondition[row, column - 1] != 1 &&
                    Cells.enemyFieldCondition[row, column] == 3)
                {
                    for (byte i = 0; i < 4; i++)          // SET ROW AND COLUMN TOP_LEFT_ANGLE VALUES. SET DIRECTION
                    {
                        if (Cells.enemyFieldCondition[row - 1, column] == 3) row--;
                        if (Cells.enemyFieldCondition[row, column - 1] == 3) column--; 
                    }
                    if (Cells.enemyFieldCondition[row + 1, column] == 3) isVertical = true;
                    if (Cells.enemyFieldCondition[row, column + 1] == 3) isVertical = false;

                    if (isVertical)     // FIND BOTTOM AND RIGHT FOR VERTICAL SHIP
                    {
                        for (byte i = 0; i < 4; i++)
                            if (Cells.enemyFieldCondition[row + shipLength, column] == 3)
                                shipLength++;

                        if (Cells.enemyFieldCondition[row + shipLength, column] == 1 || Cells.enemyFieldCondition[row - 1, column] == 1) isError = true;
                       
                        bottom = (byte)(row + shipLength);
                        right = (byte)(column + 1);
                    }
                    else               // FIND BOTTOM AND RIGHT FOR HORIZONTAL SHIP
                    {
                        for (byte i = 0; i < 4; i++)
                            if (Cells.enemyFieldCondition[row, column + shipLength] == 3)
                                shipLength++;

                        if (Cells.enemyFieldCondition[row, column + shipLength] == 1 || Cells.enemyFieldCondition[row, column - 1] == 1) isError = true;

                        bottom = (byte)(row + 1);
                        right = (byte)(column + shipLength);
                    }

                    if (!isError)
                        for (byte angleRow = (byte)(row - 1); angleRow <= bottom; angleRow++)
                            for (byte angleColumn = (byte)(column - 1); angleColumn <= right; angleColumn++)
                                if (angleRow > 0 && angleRow < 11 && angleColumn > 0 && angleColumn < 11 && Cells.enemyFieldCondition[angleRow, angleColumn] == 0)
                                {
                                    PictureBox miss = new()
                                    {
                                        Left = (angleColumn - 1) * 50 + 851,
                                        Top = (angleRow - 1) * 50 + 301,
                                        Width = 48,
                                        Height = 48,
                                        Image = new Bitmap(@"..\..\..\pictures\splash.png"),
                                        SizeMode = PictureBoxSizeMode.Normal
                                    };
                                    Controls.Add(miss);
                                    Cells.enemyFieldCondition[row, column] = 2;
                                }
                }
            }
        }
        
        private void btArrangeTheShips_Click(object sender, EventArgs e)
        {
            rbGuest.Visible = true;
            rbServer.Visible = true;

            if (Ships.myPbShipsList.Count > 0)
                for (byte i = 0; i < Ships.myPbShipsList.Count; i++)
                    Controls.Remove(Ships.myPbShipsList[i]);

            Ships.ArrandeTheShip();

            for (byte i = 0; i < Ships.myPbShipsList.Count; i++)
                Controls.Add(Ships.myPbShipsList[i]);
        }

        private void rbHost_CheckedChanged(object sender, EventArgs e) => btConnect.Visible = true;

        private void rbGuest_CheckedChanged(object sender, EventArgs e) => btConnect.Visible = true; 

        private void btConnect_Click(object sender, EventArgs e)
        {
            btArrangeTheShips.Enabled = false;

            if (rbServer.Checked == true)
            {
                rbGuest.Enabled = false;
                rbServer.Enabled = false;
                btConnect.Enabled = false;

                listenSocket.Bind(new IPEndPoint(IPAddress.Loopback, 5064));
                listenSocket.Listen(1);
                handlerSocket = listenSocket.Accept();
            }
            else
            {
                try 
                {
                    listenSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5064));

                    rbGuest.Enabled = false;
                    rbServer.Enabled = false;
                    btConnect.Enabled = false;
                }
                catch { MessageBox.Show("Server no respond"); return; }

                int bytesAmount = listenSocket.Receive(data);
                MessageBox.Show(Encoding.Unicode.GetString(data, 0, bytesAmount));
            }
        }

    }
}