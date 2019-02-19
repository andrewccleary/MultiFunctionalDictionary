﻿using System;
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
using MultifunctionalDictionary.Helper;
using MultifunctionalDictionary.Models;

namespace MultifunctionalDictionary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseHelper dh;

        public MainWindow()
        {
            InitializeComponent();

            bookSelector.IsEditable = true;
            bookSelector.IsReadOnly = true;
            bookSelector.Text = "Book";

            chapterSelector.IsEditable = true;
            chapterSelector.IsReadOnly = true;
            chapterSelector.IsEnabled = false;
            chapterSelector.Text = "Chapter";

            verseSelector.IsEditable = true;
            verseSelector.IsReadOnly = true;
            verseSelector.IsEnabled = false;
            verseSelector.Text = "Verse";

            goButton.IsEnabled = false;
            clearButton.IsEnabled = false;
            
            dh = new DatabaseHelper("localhost", "5432", "postgres", "postgres", "MFD");
            dh.AcquireConnection();
            SelectionHelper sh = new SelectionHelper(dh.GetConnection());

            Dictionary<int, String> booksList = sh.GetBooksList();

            foreach(KeyValuePair<int, String> book in booksList)
            {
                bookSelector.Items.Insert(book.Key-1, book.Value);
            }
        }

        private void BookSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionHelper sh = new SelectionHelper(dh.GetConnection());
            List<int> chapters = sh.GetChapterList(bookSelector.SelectedIndex+1);

            chapterSelector.Items.Clear();
            goButton.IsEnabled = true;
            clearButton.IsEnabled = true;

            foreach(int chapter in chapters)
            {
                chapterSelector.Items.Insert(chapter - 1, chapter);
            }

            chapterSelector.Text = "Chapter";
            chapterSelector.IsEnabled = true;

            verseSelector.Text = "Verse";
            verseSelector.IsEnabled = false;
        }

        private void ChapterSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionHelper sh = new SelectionHelper(dh.GetConnection());
            List<int> verses = sh.GetVerseList(bookSelector.SelectedIndex+1, chapterSelector.SelectedIndex+1);

            verseSelector.Items.Clear();

            foreach (int verse in verses)
            {
                verseSelector.Items.Insert(verse - 1, verse);
            }

            verseSelector.Text = "Verse";
            verseSelector.IsEnabled = true;
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            verseBlock.Text = " ";

            int chapter = chapterSelector.SelectedIndex + 1;
            int verseNum = verseSelector.SelectedIndex + 1;

            VerseHelper vh = new VerseHelper(dh.GetConnection());
            List<Verse> verses = new List<Verse>();

            if (bookSelector.Text != "Book" && chapterSelector.Text == "Chapter")
            {
                verses = vh.GetVersesByBook(bookSelector.SelectedIndex + 1);
            }
            else if(bookSelector.Text != "Book" && chapterSelector.Text != "Chapter" && verseSelector.Text == "Verse")
            {
                verses = vh.GetVersesByBookChapter(bookSelector.SelectedIndex + 1, chapterSelector.SelectedIndex + 1);
            }
            else if (bookSelector.Text != "Book" && chapterSelector.Text != "Chapter" && verseSelector.Text != "Verse")
            {
                verses = vh.GetVerseByBookChapterVerse(bookSelector.SelectedIndex + 1, chapterSelector.SelectedIndex + 1, verseSelector.SelectedIndex + 1);
            }

            foreach (Verse verse in verses)
            {
                verseBlock.Text += verse.GetVerseNum().ToString();
                verseBlock.Text += " ";
                verseBlock.Text += verse.GetVerse();
                verseBlock.Text += " ";
            }
            
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            verseSelector.Items.Clear();
            chapterSelector.Items.Clear();

            bookSelector.Text = "Book";
            chapterSelector.Text = "Chapter";
            verseSelector.Text = "Verse";

            verseBlock.Text = "";

            chapterSelector.IsEnabled = false;
            verseSelector.IsEnabled = false;
            goButton.IsEnabled = false;
            clearButton.IsEnabled = false;
        }
    }
}
