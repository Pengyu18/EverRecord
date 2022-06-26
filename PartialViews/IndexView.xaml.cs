using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace EverRecord.PartialViews
{
    /// <summary>
    /// IndexView.xaml 的交互逻辑
    /// </summary>
    public partial class IndexView : UserControl
    {
        public IndexView()
        {
            InitializeComponent();
            dateText.Text = DateTime.Now.GetDateTimeFormats('D')[0].ToString();
            

            InitEvents();
        }


        static bool attribute;
        static int genID;

        private void InitEvents()
        {
            RefreshDataGrid();
            int n;
            //sorteText.Text =  ;
            datePickerBirthDate.SelectedDate = DateTime.Today;
        }

        private void RefreshDataGrid()
        {
            dataGridTrue.SelectionMode = DataGridSelectionMode.Single;
            dataGridFalse.SelectionMode = DataGridSelectionMode.Single;

            int trueNum = RefreshSingleDataGrid(dataGridTrue,true);
            int falseNum = RefreshSingleDataGrid(dataGridFalse, false);
            sorteText.Text = string.Format("{0}={1}-{2}", trueNum - falseNum, trueNum, falseNum);
           
        }

        /// <summary>
        /// 在dataGrid上显示数据：今日的概要、内容、分数
        /// </summary>
        /// <param name="dataGrid">显示的dataGrid控件名</param>
        /// <param name="bo">选择属性</param>
        private int RefreshSingleDataGrid(DataGrid dataGrid,bool bo)
        {
            using (var c = new ERDbEntities())
            {
                var q = from t in c.Record
                        where t.Attribute == bo 
                        && t.Date.Year == DateTime.Now.Year && t.Date.Month == DateTime.Now.Month && t.Date.Day == DateTime.Now.Day
                        select new { t.Id , t.name , t.Text ,t.Score };
                dataGrid.ItemsSource = q.ToList();

                var n = from t in c.Record
                        where t.Attribute == bo
                        && t.Date.Year == DateTime.Now.Year && t.Date.Month == DateTime.Now.Month && t.Date.Day == DateTime.Now.Day
                        select t.Score;
                if (n.Sum() != null)
                {
                    return (int)n.Sum();
                }
                else return 0;

            }

        }

        ///// <summary>
        ///// 点击dataGridTrue，编辑选中数据
        ///// </summary>
        //private void dataGridTrue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    textTitle.Text = "编辑积极事项";
        //    drawerHost.IsRightDrawerOpen = true;        //

        //    labelId.Content = (dataGridTrue.SelectedItem as Record).Id;
        //    //textSort.Text = dataGridTrue.SelectedItem.ToString();

        //    //Record record = (Record)dataGridTrue.SelectedItem;
        //    //labelId.Content = record.Id;
        //    //textBoxName.Text = record.name;
        //    //textBoxText.Text = record.Text;
        //    //int so = (int)record.Score;
        //    //textSort.Text = so.ToString();
        //    //datePickerBirthDate.SelectedDate = record.Date;

        //    //Id = genID,
        //    //            name = textBoxName.Text,
        //    //            Attribute = attribute,
        //    //            Text = textBoxText.Text,
        //    //            Date = (DateTime)datePickerBirthDate.SelectedDate,
        //    //            Score = sort

        //}

        /// <summary>
        /// 点击添加积极事项
        /// </summary>
        private void addTrueBtn_Click(object sender, RoutedEventArgs e)
        {
            textTitle.Text = "添加积极事项";
            attribute = true;

            drawerHost.IsRightDrawerOpen = true;    //展开
            //生成唯一序列ID
            genID = (int)GenerateId();
            labelId.Content = genID;
            //隐藏提示
            LabelTip.Visibility = System.Windows.Visibility.Collapsed;

        }

        /// <summary>
        /// 点击添加其它事项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addFalseBtn_Click(object sender, RoutedEventArgs e)
        {
            drawerHost.IsRightDrawerOpen = true;
            textTitle.Text = "添加其它事项";
            attribute = false;

            drawerHost.IsRightDrawerOpen = true;    //展开
            
            genID = (int)GenerateId();
            labelId.Content = genID;
            //隐藏提示
            LabelTip.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// 生成唯一序列ID
        /// </summary>
        /// <returns>可以genID = (int)GenerateId();</returns>
        private long GenerateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// 侧边栏点击确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddRecord(attribute, genID);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            LabelTip.Visibility = System.Windows.Visibility.Hidden;
        }

        /// <summary>
        /// 从侧边框中向数据库中添加数据
        /// </summary>
        private void AddRecord(bool attribute, int genID)
        {
            
            bool sortBool = int.TryParse(textSort.Text, out int sort);
            if(sortBool==true)
            {
                using (var c = new ERDbEntities())
                {
                    Record record = new Record
                    {
                        Id = genID,
                        name = textBoxName.Text,
                        Attribute = attribute,
                        Text = textBoxText.Text,
                        Date = (DateTime)datePickerBirthDate.SelectedDate,
                        Score = sort
                    };
                    c.Record.Add(record);
                    c.SaveChanges();
                    drawerHost.IsRightDrawerOpen = false;
                    RefreshDataGrid();
                    Clear();
                }
            }
            else
            {
                LabelTip.Content = "提示：输入成绩需为数字！！！";
                LabelTip.Visibility = System.Windows.Visibility.Visible;
            }

        }

        private void Clear()
        {
            textBoxName.Text = null;
            textBoxText.Text = null;
            //datePickerBirthDate.SelectedDate = null;
            textSort.Text = null;
        }

        
    }
}
