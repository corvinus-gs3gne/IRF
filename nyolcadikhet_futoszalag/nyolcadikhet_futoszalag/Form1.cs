using nyolcadikhet_futoszalag.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nyolcadikhet_futoszalag
{
    public partial class Form1 : Form
    {
        List<Ball> _balls = new List<Ball>();
        private BallFactory _factory;

        public BallFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }

        public Form1()
        {
            InitializeComponent();
            Factory = new BallFactory();
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var MaxPosition = 0;
            foreach (var ball in _balls)
            {
                ball.MoveBall();
                if (ball.Left > MaxPosition)
                {
                    MaxPosition = ball.Left;
                }
            }

            if (MaxPosition > 1000 )
            {
                var oldestBall = _balls[0];
                MainPanel.Controls.Remove(oldestBall);
                _balls.Remove(oldestBall);
            }
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            var ball = Factory.CreateNew();
            _balls.Add(ball);
            MainPanel.Controls.Add(ball);
            ball.Left = -ball.Width;
        }
    }
}
