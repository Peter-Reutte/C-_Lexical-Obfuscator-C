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
using System.IO;

namespace Obfuscator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Microsoft.Win32.OpenFileDialog dlg;
        bool? ifFilesSelected;
        //bool? ifFilesObfuscated;
        string progInfo;
        string obfInfo;
        
        public MainWindow()
        {
            InitializeComponent();
            dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "C source (*.c, *.h)|*.c;*.h";

            // поле с информацией об обфускации
            //FileStream fsProg = new FileStream("Resources/ObfusInfo.txt", FileMode.Open, FileAccess.Read);
            //StreamReader srProg = new StreamReader(fsProg, Encoding.Default);
            //obfInfo = srProg.ReadToEnd();
            //fsProg.Close();
            //srProg.Close();
            //tbl_AboutObfuscation.Text = obfInfo;

            obfInfo = Properties.Resources.ObfusInfo;
            tbl_AboutObfuscation.Text = obfInfo;

            // поле с информацией о программе
            //FileStream fsInfo1 = new FileStream("Resources/ProgramInfo.txt", FileMode.Open, FileAccess.Read);
            //StreamReader srInfo1 = new StreamReader(fsInfo1, Encoding.Default);
            //progInfo = srInfo1.ReadToEnd();
            //srInfo1.Close();
            //fsInfo1.Close();
            //tbl_Info.Text = progInfo;

            progInfo = Properties.Resources.ProgramInfo;
            tbl_Info.Text = progInfo;

        }



        // конпка "Обфусцировать"
        private void btn_Obfuscate_Click(object sender, RoutedEventArgs e)
        {
            StreamReader SR;
            StringBuilder Code = new StringBuilder();
            // если файл не выбран
            if ((ifFilesSelected == false) || (ifFilesSelected == null))
            {
                MessageBox.Show("Файл не был выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }
            // если файл выбран
            if (ifFilesSelected == true)//если файлы выбраны
            {
                for (int i = 0; i < dlg.FileNames.Length; i++)
                {
                    SR = new StreamReader(dlg.FileNames[i]);
                    Code.Append(SR.ReadToEnd().ToString());
                }
            }

            // Все подключенные библиотеки
            string[] includes = new Obfuscation_1.ObfuscatedCode(Code).GetIncludeLibs().ToArray();

            // Путь к файлу вместе с именем
            string WorkPath = dlg.FileNames[0];

            // Удаляем имя файла, оставляя только путь
            WorkPath = WorkPath.Replace(dlg.SafeFileNames[0], "");

            for (int i = 0; i < includes.Length; i++)
            {
                includes[i] = includes[i].Replace("/", "\\");
            }

            // Существующие подключенные библиотеки
            List<string> existIncludes = new List<string>();

            // Проверка на существование
            foreach (var lib in includes)
            {
                // Если файл-библиотека существует - заносим его в список
                if (File.Exists(WorkPath + lib))
                {
                    // Добавление в список
                    existIncludes.Add(WorkPath + lib);

                    // Добавление кода библиотеки к существующему
                    SR = new StreamReader(WorkPath + lib);
                    // В начало файла
                    Code.Insert(0, SR.ReadToEnd().ToString());
                }
            }

            // Обфускация
            Obfuscation_1.ObfuscatedCode OC = new Obfuscation_1.ObfuscatedCode(Code);

            // Удаление подключенных и существующих библиотек из кода
            foreach (var lib in existIncludes)
            {
                OC.RemoveLib(lib.Replace("\\", "/"));
            }

            SourceCodeTexBox.Document.Blocks.Clear();
            ObfuscatedCodeTextBox.Document.Blocks.Clear();

            SourceCodeTexBox.AppendText(Code.ToString());
            ObfuscatedCodeTextBox.AppendText(OC.GetObfuscatedCode().ToString());
        }

        // кнопка "Загрузить"
        private void btn_OpenFile_Click(object sender, RoutedEventArgs e) //кнопка открыть файлы 
        {
            ifFilesSelected = dlg.ShowDialog();//вызываем окно открытия

            // Запись файла и его адреса
            if (ifFilesSelected == true)
            {
                string WorkPath = dlg.FileNames[0];
                lbl_adress.Content = WorkPath;
            }
        }

        // кнопка "Сохранить"
        private void btn_SaveFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            // если файл не выбран
            if ((ifFilesSelected == false) || (ifFilesSelected == null))
            {
                MessageBox.Show("Файл не был выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }
            // фильтр файлов
            sfd.Filter = "C source (*.c)| *.c";
            bool? res = sfd.ShowDialog();
            if (true == res)
            {
                using (StreamWriter sw = new StreamWriter(sfd.OpenFile(), System.Text.Encoding.Default))
                {
                    sw.Write(new TextRange(ObfuscatedCodeTextBox.Document.ContentStart, ObfuscatedCodeTextBox.Document.ContentEnd).Text);
                    sw.Close();
                }
            }
        }

        private void btn_Hidden_Click(object sender, RoutedEventArgs e)
        {
            tbl_AboutObfuscation.Text = "Что такое обфускация..";
            btn_Visible.Visibility = Visibility.Visible;
            btn_Hidden.Visibility = Visibility.Hidden;
        }

        private void btn_Visible_Click(object sender, RoutedEventArgs e)
        {
            tbl_AboutObfuscation.Text = obfInfo;
            btn_Visible.Visibility = Visibility.Hidden;
            btn_Hidden.Visibility = Visibility.Visible;
        }

        private void btn_Hidden1_Click(object sender, RoutedEventArgs e)
        {
            tbl_Info.Text = "Как работает программа..";
            btn_Visible1.Visibility = Visibility.Visible;
            btn_Hidden1.Visibility = Visibility.Hidden;
        }

        private void btn_Visible1_Click(object sender, RoutedEventArgs e)
        {
            tbl_Info.Text = progInfo;
            btn_Visible1.Visibility = Visibility.Hidden;
            btn_Hidden1.Visibility = Visibility.Visible;
        }

        public static void ShowExceptionMessageBox()
        {
            MessageBox.Show("Произошла ошибка. Скорее всего Вы ввели некорректные данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.Cancel);
        }

    }
}