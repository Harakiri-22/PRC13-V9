using Microsoft.Win32;
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
using LibMas;

namespace PRC13_V9
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private LibMas.Array _myArray;

        private void GetMinimalElement_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.Items.Count == 0)
            {
                MessageBox.Show("Заполните матрицу!");
            }
            else
            {
                List<int> biggestValues = new List<int> { };

                for (int i = 0; i < _myArray.ColumnLength; i++)
                {
                    int maxValue = int.MinValue;

                    for (int j = 0; j < _myArray.RowLength; j++)
                    {
                        if (_myArray[j, i] > maxValue)
                        {
                            maxValue = _myArray[j, i];
                        }

                        if (j == _myArray.RowLength - 1)
                        {
                            biggestValues.Add(maxValue);
                        }
                    }
                }
                value.Text = biggestValues.Min().ToString();
            }
        }

        private void FillUpArray_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(row.Text) || string.IsNullOrEmpty(column.Text))
            {
                MessageBox.Show("Введите размер матрицы!");
            }
            else
            {
                _myArray = new LibMas.Array(int.Parse(row.Text), int.Parse(column.Text));
                sizeRow.Text = $"Строк {row.Text}";
                sizeColumn.Text = $"Столбцов {column.Text}";
                _myArray.Fill();
                dataGrid.ItemsSource = _myArray.ToDataTable().DefaultView;
            }
        }

        private void SaveArray_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.ItemsSource == null)
            {
                MessageBox.Show("Заполните матрицу перед сохранением!", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*",
                DefaultExt = ".txt",
                FilterIndex = 1,
                Title = "Экспорт массива"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                _myArray.Export(saveFileDialog.FileName);
            }
            dataGrid.ItemsSource = null;
        }

        private void OpenArray_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*",
                DefaultExt = ".txt",
                FilterIndex = 1,
                Title = "Импорт массива"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _myArray.Import(openFileDialog.FileName);
            }
            dataGrid.ItemsSource = _myArray.ToDataTable().DefaultView;
        }

        private void AboutProgramm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(" Мишин Д. А. \n Вариант №9", "Информация о разработчике", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CellIndex_Click(object sender, MouseEventArgs e)
        {
            selectedCell.Text = $"[{dataGrid.Items.IndexOf(dataGrid.CurrentItem)};" +
                $"{dataGrid.CurrentColumn.DisplayIndex}]";
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
            row.Clear();
            column.Clear();
            value.Clear();
        }

        private void TBoxChangeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGrid.ItemsSource = null;
            value.Clear();
        }
    }
}
