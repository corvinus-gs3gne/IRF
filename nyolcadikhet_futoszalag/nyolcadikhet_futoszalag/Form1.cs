using nyolcadikhet_futoszalag.Abstractions;
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
        private List<Toy> _toys = new List<Toy>();
        private IToyFactory _factory;
        private Toy _nextToy;

        public IToyFactory Factory
        {
            get { return _factory; }
            set
            {
                _factory = value;
                DisplayNext();
            }
        }

        public Form1()
        {
            InitializeComponent();
            Factory = new BallFactory();
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            var toy = Factory.CreateNew();
            _toys.Add(toy);
            toy.Left = -toy.Width;
            MainPanel.Controls.Add(toy);
        }


        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var MaxPosition = 0;
            foreach (var toy in _toys)
            {
                toy.MoveToy();
                if (toy.Left > MaxPosition)
                {
                    MaxPosition = toy.Left;
                }
            }

            if (MaxPosition > 1000)
            {
                var oldestToy = _toys[0];
                MainPanel.Controls.Remove(oldestToy);
                _toys.Remove(oldestToy);
            }
        }

        private void carButton_Click(object sender, EventArgs e)
        {
            Factory = new CarFactory();
        }

        private void ballButton_Click(object sender, EventArgs e)
        {
            Factory = new BallFactory();
        }

        private void DisplayNext()
        {
            if (_nextToy != null)
            {
                Controls.Remove(_nextToy);
                _nextToy = Factory.CreateNew();
                _nextToy.Top = label1.Top + label1.Height + 20;
                _nextToy.Left = label1.Left;
                Controls.Add(_nextToy);
            }

        }
    }
}
