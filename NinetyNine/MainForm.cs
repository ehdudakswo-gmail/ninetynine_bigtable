using System;
using System.Drawing;
using System.Windows.Forms;

namespace NinetyNine
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetForm();
            SetListView();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            fileListView.Width = Width - 40;
            fileListView.Height = Height - 110;
            ResizeFileListViewColumnWidth();
        }

        private void ResizeFileListViewColumnWidth()
        {
            for (int i = 0; i < fileListView.Columns.Count; i++)
            {
                fileListView.Columns[i].Width = -2;
            }
        }

        private void SetForm()
        {
            WindowState = FormWindowState.Maximized;
        }

        private void SetListView()
        {
            //config
            fileListView.View = View.Details;
            fileListView.FullRowSelect = true;

            //column
            fileListView.Columns.Add("종류", 100);
            fileListView.Columns.Add("파일명", -2);

            //item height
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 30);
            fileListView.SmallImageList = imgList;

            //item add
            ListViewItem item = new ListViewItem(new string[] { "산출서", "없음" });
            fileListView.Items.Add(item);

            ListViewItem item2 = new ListViewItem(new string[] { "공정표", "없음" });
            fileListView.Items.Add(item2);

            ListViewItem item3 = new ListViewItem(new string[] { "조직도", "없음" });
            fileListView.Items.Add(item3);
        }

        private void fileOpenButton_Click(object sender, EventArgs e)
        {
            int selectedCount = fileListView.SelectedItems.Count;
            if (selectedCount == 0)
            {
                MessageBox.Show("파일을 선택해주세요");
            }
            else if (selectedCount > 1)
            {
                MessageBox.Show("파일을 1개만 선택해주세요");
            }
            else
            {
                var selectedItem = fileListView.SelectedItems[0].SubItems[0].Text;
                MessageBox.Show(selectedItem);
            }
        }

        private void bigTableCreateButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("bigTableCreateButton_Click");
        }
    }
}