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
using System.Linq;
using System.Runtime;


namespace EverRecord.PartialViews
{
    /// <summary>
    /// AddView.xaml 的交互逻辑
    /// </summary>
    public partial class AddView : UserControl
    {
        public bool attribute;

        public AddView()
        {
            InitializeComponent();
            
            //生成唯一序列ID
            textBoxId.Text= Guid.NewGuid().ToString();
            //隐藏提示
            LabelTip.Visibility = System.Windows.Visibility.Collapsed;
            if(attribute==true)
            {
                textTitle.Text = "添加积极事项";
            }
            else textTitle.Text = "添加其它事项";

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            int.TryParse(textBoxId.Text, out int id);
            bool sortBool = int.TryParse(textSort.Text, out int sort);
            if(sortBool==true)
            {
                using (var c = new ERDbEntities())
                {
                    Record record = new Record
                    {
                        Id = id,
                        name = textBoxName.Text,
                        Attribute = attribute,
                        Text = textBoxText.Text,
                        Date = (DateTime)datePickerBirthDate.SelectedDate,
                        Score = sort
                    };
                    c.Record.Add(record);
                    c.SaveChanges();
                }
            }
            else
            {
                LabelTip.Content = "提示：输入成绩需为数字！！！";
                LabelTip.Visibility = System.Windows.Visibility.Visible;
            }
            
        }
    }
}
