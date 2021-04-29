using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CosmeticShop
{
    public partial class Autorization : Form
    {
        DataBase db = new DataBase();

        int counter;

        public List<int> idList = new List<int>();

        List<PictureBox> mainImage = new List<PictureBox>();

        List<PictureBox> mainPanel = new List<PictureBox>();

        List<Label> name = new List<Label>();

        List<List<PictureBox>> checkList = new List<List<PictureBox>>();

        List<List<string>> allImageList = new List<List<string>>();

        List<string> allNameList = new List<string>();

        List<int> idSearch = new List<int>();


        public Autorization()
        {
            InitializeComponent();
            FillIdlist();
            CreatePanel(idList);

        }

        private void FillIdlist()
        {
            foreach (Product p in db.Product)
            {
                idList.Add(p.ID);
                idSearch.Add(p.ID);
                allNameList.Add(p.Title);
            }
        }

        private void CreatePanel(List<int> id)
        {
            DeletePanel();
            int count = id.Count;
            int stringNum = 0;
            for (int f = 0; f < count; f++)
            {
                counter = 1;
                Product prd = db.Product.Find(id[f]);

                List<string> imageList = new List<string>();
                foreach (ProductPhoto pp in db.ProductPhoto)
                {
                    if (pp.ProductID == id[f])
                        imageList.Add(pp.PhotoPath);
                }
                allImageList.Add(imageList);
                counter += allImageList[f].Count;
                    

                if (f / (3 * (stringNum+1)) == 1)
                {
                    stringNum++;
                }
                PictureBox mainPn = new PictureBox()
                {
                    Size = new Size(200, 300),
                    Location = new Point(100 + (250 * (f - 3*stringNum)), 50 + (Size.Height * stringNum/5*3)),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.White,
                    Name = $"{f}"
                };
                if (prd.IsActive == false)
                {
                    mainPn.BackColor = Color.LightGray;
                }
                
                mainPanel.Add(mainPn);
                panel2.Controls.Add(mainPanel[f]);

                
                Label text = new Label()
                {
                    Size = new Size(150, mainPanel[f].Height / 5),
                    Location = new Point(mainPanel[f].Location.X + ((mainPanel[f].Width - 150) / 2), (mainPanel[f].Location.Y + mainPanel[f].Height) - mainPanel[f].Height / 5 - 10),
                    Text = prd.Title,
                    BackColor = mainPanel[f].BackColor,
                    TextAlign = ContentAlignment.TopCenter,
                    Font = new Font("Tahoma", 9f, FontStyle.Regular)
                };
                name.Add(text);
                panel2.Controls.Add(name[f]);
                name[f].BringToFront();

                PictureBox mainImg = new PictureBox()
                {
                    Size = new Size(150, 200),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackgroundImage = Image.FromFile($@"..\..\{prd.MainImagePath}"),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Location = new Point(mainPanel[f].Location.X + ((mainPanel[f].Width - 150) / 2), mainPanel[f].Location.Y + 10),
                    Name = $"{f}"
                };
                mainImage.Add(mainImg);
                panel2.Controls.Add(mainImage[f]);
                mainImage[f].BringToFront();

                List<PictureBox> pb = new List<PictureBox>();
                for (int i = 0; i < counter; i++)
                {
                    PictureBox rdP = new PictureBox
                    {
                        Size = new Size(15, 15),
                        BackColor = Color.FromArgb(225, 228, 255),
                        Location = new Point((mainImage[f].Location.X + ((mainImage[f].Width - (30 * counter / 2)) / 2) + (30 * i) / 2), mainImage[f].Location.Y + mainImage[f].Height)
                    };
                    if (i == 0)
                        rdP.BackColor = Color.FromArgb(255, 74, 109);
                    pb.Add(rdP);
                    panel2.Controls.Add(pb[i]);
                    pb[i].BringToFront();
                }
                checkList.Add(pb);
            }
            label2.Text = $"{id.Count} из {idList.Count}";
            HookUpEventHandlers();
        }

        void DeletePanel()
        {
            panel2.Controls.Clear();
            name.Clear();
            checkList.Clear();
            mainPanel.Clear();
            mainImage.Clear();
            allImageList.Clear();
        }

        private void HookUpEventHandlers()
        {
            foreach (var p in mainImage)
            {
                p.MouseMove += P_MouseMove;
                p.MouseLeave += P_MouseLeave;

            }
        }


        private void P_MouseMove(object sender, MouseEventArgs e)
        {
            
            var pb = (PictureBox)sender;
            Product prd = db.Product.Find(idSearch[Convert.ToInt32(pb.Name)]);
            counter = 1 + allImageList[Convert.ToInt32(pb.Name)].Count;
            double areaChange = pb.Width / counter;
            for (int i = 0; i < counter; i++)
            {
                if (MousePosition.X >= this.Left + pb.Location.X + areaChange * i - 1
                    && MousePosition.X <= this.Location.X + pb.Location.X + areaChange * (i + 1) + 1)
                {
                    if (i == 0)
                    {
                        pb.BackgroundImage = Image.FromFile($@"..\..\{prd.MainImagePath}");
                    }
                    else
                    {
                        pb.BackgroundImage = Image.FromFile($@"..\..\{allImageList[Convert.ToInt32(pb.Name)][i-1]}");
                    }

                    pb.BackgroundImageLayout = ImageLayout.Stretch;
                    for (int j = 0; j < counter; j++)
                    {
                        if (j == i)
                        {
                            checkList[Convert.ToInt32(pb.Name)][j].BackColor = Color.FromArgb(255, 74, 109);
                        }
                        else
                        {
                            checkList[Convert.ToInt32(pb.Name)][j].BackColor = BackColor = Color.FromArgb(225, 228, 255);
                        }
                    }
                }
            }
        }

        private void P_MouseLeave(object sender, EventArgs e)
        {
            var pb = (PictureBox)sender;
            Product prd = db.Product.Find(idSearch[Convert.ToInt32(pb.Name)]);
            mainImage[Convert.ToInt32(pb.Name)].BackgroundImage = Image.FromFile($@"..\..\{prd.MainImagePath}");
            counter = 1 + allImageList[Convert.ToInt32(pb.Name)].Count;
            for (int j = 0; j < counter; j++)
            {
                if (j == 0)
                {
                    checkList[Convert.ToInt32(pb.Name)][j].BackColor = Color.FromArgb(255, 74, 109);
                }
                else
                {
                    checkList[Convert.ToInt32(pb.Name)][j].BackColor = BackColor = Color.FromArgb(225, 228, 255);
                }
            }
        }


      

        private void SearchText_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void DropDown(object sender, EventArgs e)
        {
            idSearch.Clear();
            for (int i = 0; i < idList.Count(); i++)
            {
                if (allNameList[i].Contains($"{textBox1.Text}") == true)
                {
                        idSearch.Add(idList[i]);
                }
            }
            CreatePanel(idSearch);
            
            
            
            
        }

        private void SearchText_KeyUp(object sender, KeyEventArgs e)
        {
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
