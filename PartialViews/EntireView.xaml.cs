using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// EntireView.xaml 的交互逻辑
    /// </summary>
    public partial class EntireView : UserControl
    {
        public EntireView()
        {
            InitializeComponent();
            InitEvents();
            dataGrid1.SelectedIndex = -1;
            editGird.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void InitEvents()
        {
            dataGrid1.SelectionMode = DataGridSelectionMode.Single;
            RefreshDataGrid1();
            LabelTip.Visibility = System.Windows.Visibility.Collapsed;
            dataGrid1.SelectionChanged += (s, e) =>
            {
                if (dataGrid1.SelectedIndex == -1)
                {
                    editGird.Visibility = System.Windows.Visibility.Collapsed;
                         //edit
                    return;
                }
                else
                {
                    editGird.Visibility = System.Windows.Visibility.Visible;
                    LabelTip.Visibility = System.Windows.Visibility.Collapsed;
                }
                Record record = (Record)dataGrid1.SelectedItem;
                if (record.Attribute == true) attributeCombx2.SelectedIndex = 0;
                else attributeCombx2.SelectedIndex = 1;
                labelId.Content = record.Id;
                textBoxName.Text = record.name;
                textBoxText.Text = record.Text;
                int tso = (int)record.Score;
                textSort.Text = tso.ToString();
                datePickerDate.SelectedDate = record.Date;
            };

            btnModify.Click += (s, e) =>
            {
                bool sortBool = int.TryParse(textSort.Text, out int sort);
                bool att;
                if (attributeCombx2.SelectedIndex == 0) att= true;
                else att=false;
                if (dataGrid1.SelectedIndex == -1)
                {
                    LabelTip.Visibility = System.Windows.Visibility.Visible;
                    LabelTip.Content = "请先单击要修改的行";
                    return;
                }
                else if (sortBool == true)
                { 
                    
                    using (var c = new ERDbEntities())
                    {
                        int ID = ((Record)dataGrid1.SelectedItem).Id;
                        Record record = c.Record.Find(ID);
                        record.Attribute = att;
                        record.name = textBoxName.Text;
                        record.Text = textBoxText.Text;
                        record.Score = sort;
                        record.Date = (DateTime)datePickerDate.SelectedDate;
                        c.SaveChanges();                    
                    } 
                }
                else
                {
                    LabelTip.Content = "提示：输入成绩需为数字！！！";
                    LabelTip.Visibility = System.Windows.Visibility.Visible;
                }
            };

            btnDelete.Click += (s, e) =>
            {
                if (dataGrid1.SelectedIndex == -1)
                {
                    LabelTip.Visibility= System.Windows.Visibility.Visible;
                    LabelTip.Content="请先单击要删除的行";
                    return;
                }
                using (var c = new ERDbEntities())
                {
                    Record record = (Record)dataGrid1.SelectedItem;
                    var q = (from t in c.Record where t.Id == record.Id select t).FirstOrDefault();
                    c.Record.Remove(q);
                    c.SaveChanges();
                }
            };
        }

        private void RefreshDataGrid1()
        {
            using (var c = new ERDbEntities())
            {
                var q = from t in c.Record select t;
                dataGrid1.ItemsSource = q.ToList();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string str = seachText.Text;
            bool att;
            if (attributeCombx.SelectedIndex == 0) att = true;
            else att = false;

            using (var c = new ERDbEntities())
            {                
                if (attributeCombx.SelectedIndex != 2 && seachText.Text != "")
                {
                    att = true;
                    var q = from t in c.Record
                            where t.Attribute == att && t.name.Contains(str) || t.Text.Contains(str)
                            select t;
                    dataGrid1.ItemsSource = q.ToList();
                }
                else if(attributeCombx.SelectedIndex != 2)
                {
                    att = true;
                    var q = from t in c.Record
                            where t.Attribute == att
                            select t;
                    dataGrid1.ItemsSource = q.ToList();
                }
                else if (seachText.Text != "")
                {
                    var q = from t in c.Record
                            where t.name.Contains(str) || t.Text.Contains(str)
                            select t;
                    dataGrid1.ItemsSource = q.ToList();
                }
                else
                {
                    var q = from t in c.Record select t;
                    dataGrid1.ItemsSource = q.ToList();
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }
        private void clear()
        {
            seachText.Text = "";
            attributeCombx.SelectedIndex = 2;
            RefreshDataGrid1();
        }
    }
}
