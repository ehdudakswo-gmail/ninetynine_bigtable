using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace NinetyNine
{
    public partial class MainForm : Form
    {
        private readonly string DESKTOP_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private readonly string FILE_OPEN_BASIC_TITLE = "열기";
        private readonly string FILE_SAVE_BASIC_TITLE = "저장";
        private readonly string FILE_OPEN_COMPLETE_MESSAGE = "열기 완료";
        private readonly string FILE_SAVE_COMPLETE_MESSAGE = "저장 완료";
        private readonly string FILE_SAVE_EMPTY_MESSAGE = "데이터가 없습니다.";

        private TabControlItemManager tabControlItemManager;
        private ExcelEPPlusManager excelEPPlusManager = new ExcelEPPlusManager();
        private DataGridViewManager dataGridViewManager = new DataGridViewManager();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetForm();
            SetTabControlItems();
            SetFileDialog();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            tabControl.Width = Width - 35;
            tabControl.Height = Height - 85;

            if (tabControlItemManager != null)
            {
                tabControlItemManager.Resize();
            }
        }

        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage selectedTab = tabControl.SelectedTab;
            tabControlItemManager.SetSelectedItem(selectedTab);
        }

        private void SetForm()
        {
            WindowState = FormWindowState.Maximized;
        }

        private void SetTabControlItems()
        {
            List<TabControlItem> tabControlItems = new List<TabControlItem>();
            tabControlItems.Add(new TabControlItem(tabPage_Form, dataGridView_Form));
            tabControlItems.Add(new TabControlItem(tabPage_Statement, dataGridView_Statement));
            tabControlItems.Add(new TabControlItem(tabPage_Schedule, dataGridView_Schedule));
            tabControlItems.Add(new TabControlItem(tabPage_Organization, dataGridView_Organization));

            tabControlItemManager = new TabControlItemManager(tabControlItems);
            tabControlItemManager.SetSelectedItem(tabPage_Form);
        }

        private void SetFileDialog()
        {
            openFileDialog.InitialDirectory = DESKTOP_PATH;
            openFileDialog.Filter =
                "엑셀 파일 (*.xlsx)|*.xlsx";

            saveFileDialog.InitialDirectory = DESKTOP_PATH;
            saveFileDialog.Filter =
                "엑셀 파일 (*.xlsx)|*.xlsx";
        }

        private async void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabControlItem selectedItem = tabControlItemManager.GetSelectedItem();
            TabPage selectedTabPage = selectedItem.tabPage;
            DataGridView selectedDataGridView = selectedItem.dataGridView;

            openFileDialog.Title = string.Format
                ("{0} {1}", selectedTabPage.Text, FILE_OPEN_BASIC_TITLE);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SetWaitState();
                    string fileName = openFileDialog.FileName;
                    DataTable dataTable = await excelEPPlusManager.GetDataTable(fileName);
                    dataGridViewManager.Refresh(selectedDataGridView, dataTable);

                    string completeMessage = string.Format
                        ("{0} {1}", selectedTabPage.Text, FILE_OPEN_COMPLETE_MESSAGE);
                    MessageBox.Show(completeMessage);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    SetDefaultState();
                }
            }
        }

        private async void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabControlItem selectedItem = tabControlItemManager.GetSelectedItem();
            TabPage selectedTabPage = selectedItem.tabPage;
            DataGridView selectedDataGridView = selectedItem.dataGridView;

            saveFileDialog.Title = string.Format
                ("{0} {1}", selectedTabPage.Text, FILE_SAVE_BASIC_TITLE);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SetWaitState();
                    string fileName = saveFileDialog.FileName;

                    DataTable dataTable = (DataTable)selectedDataGridView.DataSource;
                    if (dataTable == null)
                    {
                        string errorMessage = string.Format
                            ("{0} {1}", selectedTabPage.Text, FILE_SAVE_EMPTY_MESSAGE);
                        MessageBox.Show(errorMessage);
                        return;
                    }

                    string result = await excelEPPlusManager.Save(fileName, dataTable);
                    string completeMessage = string.Format
                        ("{0} {1}", selectedTabPage.Text, FILE_SAVE_COMPLETE_MESSAGE);
                    MessageBox.Show(completeMessage);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    SetDefaultState();
                }
            }
        }

        private void SetWaitState()
        {
            Application.UseWaitCursor = true;
            foreach (Control control in Controls)
            {
                control.Enabled = false;
            }
        }

        private void SetDefaultState()
        {
            Application.UseWaitCursor = false;
            foreach (Control control in Controls)
            {
                control.Enabled = true;
            }
        }
    }
}