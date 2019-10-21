using NinetyNine.BigTable;
using System;
using System.Data;
using System.Windows.Forms;

namespace NinetyNine
{
    public partial class MainForm : Form
    {
        private readonly string DESKTOP_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private readonly string FILE_OPEN_COMPLETE_MESSAGE = "열기 완료";
        private readonly string FILE_SAVE_COMPLETE_MESSAGE = "저장 완료";
        private readonly string BIGTABLE_COMPLETE_MESSAGE = "빅테이블 생성 완료";

        private TabControlManager tabControlManager;
        private ExcelEPPlusManager excelEPPlusManager = new ExcelEPPlusManager();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetForm();
            SetTabControl();
            SetFileDialog();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            tabControl.Width = Width - 35;
            tabControl.Height = Height - 85;

            if (tabControlManager != null)
            {
                tabControlManager.Resize();
            }
        }


        private void SetForm()
        {
            WindowState = FormWindowState.Maximized;
        }

        private void SetTabControl()
        {
            tabControlManager = new TabControlManager(tabControl);
            tabControlManager.Resize();

            DataSet dataSet = MainDataTableEnum.GetDataSetTemplate();
            tabControlManager.Refresh(dataSet);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabControlManager.SelectedIndexChanged();
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

        private async void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SetWaitState();
                    string fileName = openFileDialog.FileName;
                    DataSet dataSet = await excelEPPlusManager.GetDataSet(fileName);
                    tabControlManager.Check(dataSet);
                    tabControlManager.Refresh(dataSet);
                    tabControl.SelectedIndex = 0;
                    MessageBox.Show(FILE_OPEN_COMPLETE_MESSAGE);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                finally
                {
                    SetDefaultState();
                }
            }
        }

        private async void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SetWaitState();
                    string fileName = saveFileDialog.FileName;
                    DataSet dataSet = tabControlManager.GetDataSet();
                    string result = await excelEPPlusManager.Save(fileName, dataSet);
                    MessageBox.Show(FILE_SAVE_COMPLETE_MESSAGE);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                finally
                {
                    SetDefaultState();
                }
            }
        }

        private async void 생성ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetWaitState();
                BigTableManager bigTableManager = new BigTableManager();
                DataSet dataSet = tabControlManager.GetDataSet();
                string result = await bigTableManager.Refresh(dataSet);

                tabControl.SelectedTab = tabPage_BigTable;
                MessageBox.Show(BIGTABLE_COMPLETE_MESSAGE);
            }
            catch (Exception exception)
            {
                string tableName = (string)exception.Data[ExceptionDataParam.BigTableErrorTableName];
                BigTableErrorCell[] cells = (BigTableErrorCell[])exception.Data[ExceptionDataParam.BigTableErrorCells];
                string message = exception.Message;

                Array values = Enum.GetValues(typeof(MainDataTable));
                int tabIdx = EnumManager.GetIndex(values, tableName);

                tabControl.SelectedIndex = tabIdx;
                tabControlManager.HighLight(tabIdx, cells);
                MessageBox.Show(message);
            }
            finally
            {
                SetDefaultState();
            }
        }
    }
}