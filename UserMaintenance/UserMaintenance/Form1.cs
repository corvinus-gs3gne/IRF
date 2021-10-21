using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserMaintenance.Entities;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        BindingList<User> users = new BindingList<User>();
        public Form1()
        {
            InitializeComponent();
            lblFullName.Text = Resource1.FullName;
            btnAdd.Text = Resource1.Add;
            btnWrite.Text = Resource1.Write;
            btnRemove.Text = Resource1.Delete;

            listBoxUser.DataSource = users;
            listBoxUser.ValueMember = "ID";
            listBoxUser.DisplayMember = "FullName";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            User u = new User()
            {
                FullName = textBoxFullName.Text,
                
            };
            users.Add(u);

        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath;
            //sfd.Filter = "Comma Separated Values (*.csv) |*.csv";
            //sfd.DefaultExt = "csv";
            //sfd.AddExtension = true;
            if (sfd.ShowDialog() != DialogResult.OK) return;
            using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
            {
                foreach (var u in users)
                {
                    sw.WriteLine($"{u.ID}; {u.FullName}");
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            
            var torlendo = (User)listBoxUser.SelectedItem;
            users.Remove(torlendo);

            
        }
    }
}
