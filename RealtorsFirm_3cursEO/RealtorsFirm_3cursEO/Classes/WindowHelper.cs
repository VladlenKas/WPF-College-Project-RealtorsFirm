using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Win32;

namespace RealtorsFirm_3cursEO.Classes
{
    public static class WindowHelper
    {
        /// <summary>
        /// Скрывает пароль
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="passHid"></param>
        /// <param name="passVis"></param>
        public static void HiddenPassword(object sender, PasswordBox passHid, TextBox passVis)
        {
            CheckBox checkbox = sender as CheckBox;
            if (checkbox.IsChecked == true)
            {
                // Vissible pass
                passVis.Text = passHid.Password;
                passVis.Visibility = Visibility.Visible;
                passHid.Visibility = Visibility.Hidden;
            }
            else
            {
                // Hidden pass
                passHid.Password = passVis.Text;
                passVis.Visibility = Visibility.Hidden;
                passHid.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Предоставляет быстрый доступ к паролю
        /// </summary>
        /// <param name="passHid"></param>
        /// <param name="passVis"></param>
        /// <returns></returns>
        public static string GetPassword(PasswordBox passHid, TextBox passVis)
        {
            var pass = passVis.Visibility is Visibility.Visible ? passVis.Text : passHid.Password;
            return pass;
        }

        /// <summary>
        /// Вызывает сообщение с подтверждением о выходе/закрытии окна
        /// </summary>
        /// <returns></returns>
        public static bool WindowClose()
        {
            var resultChanged = MessageBox.Show("Вы действительно хотите выйти?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultChanged == MessageBoxResult.Yes)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Открывает файловый менджер для выбора изображений
        /// </summary>
        /// <param name="image"></param>
        public static bool OpenImage(Image image)
        {
            var selectImage = new OpenFileDialog();
            selectImage.Filter = "Image files (*.jpg, *.jpeg, *.png *.webp)|*.jpg;*.jpeg;*.png;*.webp;";
            selectImage.InitialDirectory = @"C:\Users";
            if (selectImage.ShowDialog() == true)
            {
                string selectedFilePath = selectImage.FileName;
                image.Source = new BitmapImage(new Uri(selectedFilePath));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Конвертирует изображение (Image) в массив байтов (byte[])
        /// </summary>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public static byte[]? ImageSourceToBytes(ImageSource? imageSource)
        {
            if (imageSource == null)
            {
                return null;
            }

            byte[]? bytes = null;

            var bitmapSource = imageSource as BitmapSource;
            if (bitmapSource != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }
            return bytes;   
        }
    }
}
