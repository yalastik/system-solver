using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SolutionLibrary;

namespace SystemSolver
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool wasOpen;
        bool reswassaved;
        bool wasplayed;
        bool wassaved;
        bool pchanged;
        bool syschanged;
        string filepath;
        int poly;
        //MethodsToSolve solution;
        public MainWindow()
        {
            InitializeComponent();
        }

        public string[] GetSystemStrings()
        {
            return system.Text.Split('\n').Select(x => x.TrimEnd('\r')).ToArray();
        }

        private int GetP()
        {
            string pstring = p.Text;
            int poly = int.Parse(pstring);
            return poly;
        }

        private void RenameTitle(string path)
        {
            string str = path.Substring(path.LastIndexOf('\\')+1);
            window.Title = str;
        }
        private void Open(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            OpenFileDialog opdlg = new OpenFileDialog();

            // Set filter for file extension and default file extension
            opdlg.DefaultExt = ".in";
            opdlg.Filter = "Text documents (.in)|*.in";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = opdlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                filepath = opdlg.FileName;
                string[] allLines = Files.StringsFromFile(filepath);

                system.Text = Files.Equals(allLines);
                p.Text = Files.Poly(allLines);
                wasOpen = true;
                play.IsEnabled = true;
                //так как мы тоько открыли, файл считается сохраненным
                //он перестанет быть сохраненным, когда что-то в текстбоксах изменится
                wassaved = true;
                RenameTitle(filepath);
            }

        }
        private bool SavingMethod()
        {
            bool pathIsKnown = true;
            if (!wasOpen)
            {
                SaveFileDialog savedlg = new SaveFileDialog();

                // Set filter for file extension and default file extension
                savedlg.DefaultExt = ".in";
                savedlg.Filter = "Text documents (.in)|*.in";
                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = savedlg.ShowDialog();

                // Get the selected file name and display in a TextBox
                if (result == true)
                {
                    // Open document
                    filepath = savedlg.FileName;

                }
                else pathIsKnown = false;
            }
            if (pathIsKnown)
            {
                List<string> linesList = system.Text.Split('\n').Select(x => x.TrimEnd('\r')).ToList<string>();
                linesList.Insert(0, p.Text);
                string[] linesArray = linesList.ToArray();
                try
                {
                    Files.SaveToFile(linesArray, filepath);
                    RenameTitle(filepath);
                    MessageBox.Show($"Данные системы были сохранены в файл {filepath}");
                    return true;
                }
                catch (Exception) { MessageBox.Show("Ошибка сохранения!"); }
            }
            return false;
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            //если был открыт существующий файл,
            //то сохранить - в том же месте, но измененные данные
            SavingMethod();
            wassaved = true;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            if (!pchanged && !syschanged && !wasplayed)
            {
                this.Close();
            }
            else
            {

                if (!reswassaved && wasplayed)
                {
                    MessageBoxResult result = MessageBox.Show("Хотите ли сохранить?", "Выход", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        SavingResultMethod();
                        this.Close();
                    }
                    if (result == MessageBoxResult.No)
                    {
                        this.Close();
                    }
                    return;
                }
                if (!wassaved)
                {
                    MessageBoxResult result = MessageBox.Show("Хотите сохранить?", "Выоход", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (SavingMethod())
                            this.Close();
                    }
                    if (result == MessageBoxResult.No)
                    {
                        this.Close();
                    }
                }
                if (wassaved && !(!reswassaved && wasplayed)) this.Close();
            }
        }

        private void Info(object sender, RoutedEventArgs e)
        {
            string str = "На вход подаются: \r1) десятичный код порождающего полинома;\r2) система линейных уравнений. \r";
            str += "Данные можно вводить непосредственно в поля формы, или же загрузить из текстового файла с расширением .in . \rПорядок данных в файле: первая строка - ";
            str += "код полинома, \rвсе последующие строки должны содержать уравнения. \rДопустимый формат кода полинома: десятичное целое число. \rФормат строк с уравнениями: \r";
            str += " * допустимы цифры, знаки '+', '=' и символ 'x'\r * коэфициенты  - положительные целые числа\rКоличество уравнений должно совпадать с количеством неизвестных.\r";
            str += "Если в системе N уравнений, то обязательно использовать переменные x1..xN\rПример правильного ввода уравнения системы: x1+4x2+10x3=9\r";
            str += "Результат выполнения программы можно сохранить в текстовый файл с расширением .out .";
            MessageBox.Show(str, "Как пользоваться программой");
        }

        private void AboutPro(object sender, RoutedEventArgs e)
        {
            string str = "Решение систем линенйных уравнений в двоичных полиномиальных полях\r\nПрограмму выполнила Савва Яна\r\nНаучный руководитель - Авдошин Сергей Михайлович";
            MessageBox.Show(str);
        }

        private void SavingResultMethod()
        {
            if (!wassaved) SavingMethod();
            //нужно сохранить резалты в файл с тем же именем, но с расширением .out
            string outPath = filepath.Substring(0, filepath.Length - 3);
            outPath += ".out";
            try
            {
                Files.ResultsOut(result.Text, outPath);
                MessageBox.Show($"Результат был сохранен в файл {outPath}");
            }
            catch(Exception ) { MessageBox.Show("Неизвестен адрес для сохранения!"); }
            
        }
        private void SaveResult(object sender, RoutedEventArgs e)
        {
            SavingResultMethod();
            reswassaved = true;
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            try
            {
                poly = GetP();
                string[] lines = GetSystemStrings();
                result.Text = Files.GetResult(lines, poly);
                save_result.IsEnabled = true;
                wasplayed = true;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }

        private void polyTextChanged(object sender, TextChangedEventArgs e)
        {
            pchanged = true;
            wassaved = false;
            save_result.IsEnabled = false;
        }

        private void SestemTextChanged(object sender, TextChangedEventArgs e)
        {
            syschanged = true;
            wassaved = false;
            if (p.Text != string.Empty && system.Text != string.Empty) play.IsEnabled = true;
            save_result.IsEnabled = false;
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!pchanged && !syschanged && !wasplayed)
            {
                return;
            }
            else
            {

                if (!reswassaved && wasplayed)
                {
                    MessageBoxResult result = MessageBox.Show("Хотите ли сохранить?", "Выход", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        SavingResultMethod();
                        return;
                    }
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                    e.Cancel = true;
                    //return;
                }
                if (!wassaved)
                {
                    MessageBoxResult result = MessageBox.Show("Хотите сохранить?", "Выоход", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (SavingMethod())
                            return;
                    }
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                if (wassaved && !(!reswassaved && wasplayed)) return;
            }
            e.Cancel = true;
        }
    }
}
