using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Zwiggy.View.User_Control
{
    public sealed partial class RatingControl : UserControl
    {
        public RatingControl()
        {
            this.InitializeComponent();
        }

        private void OnRatingControlExited(object sender, PointerRoutedEventArgs e)
        {
            FirstStarTextBlock.Text = "";
            SecondStarTextBlock.Text = "";
            ThirdStarTextBlock.Text = "";
            FourthStarTextBlock.Text = "";
            FifthStarTextBlock.Text = "";
        }

        private void OnFirstQuarterEntered(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = sender as Grid;

            switch (grid.Name)
            {
                case "FirstStarFirstQuarterGrid":
                    FirstStarTextBlock.Text = "\xF0CA";
                    SecondStarTextBlock.Text = "";
                    ThirdStarTextBlock.Text = "";
                    FourthStarTextBlock.Text = "";
                    FifthStarTextBlock.Text = "";
                    break;
                case "SecondStarFirstQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xF0CA";
                    ThirdStarTextBlock.Text = "";
                    FourthStarTextBlock.Text = "";
                    FifthStarTextBlock.Text = "";
                    break;
                case "ThirdStarFirstQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE735";
                    ThirdStarTextBlock.Text = "\xF0CA";
                    FourthStarTextBlock.Text = "";
                    FifthStarTextBlock.Text = "";
                    break;
                case "FourthStarFirstQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE735";
                    ThirdStarTextBlock.Text = "\xE735";
                    FourthStarTextBlock.Text = "\xF0CA";
                    FifthStarTextBlock.Text = "";
                    break;
                case "FifthStarFirstQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE735";
                    ThirdStarTextBlock.Text = "\xE735";
                    FourthStarTextBlock.Text = "\xE735";
                    FifthStarTextBlock.Text = "\xF0CA";
                    break;
            }
        }

        private void OnSecondQuarterEntered(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = sender as Grid;

            switch (grid.Name)
            {
                case "FirstStarSecondQuarterGrid":
                    FirstStarTextBlock.Text = "\xE7C6";
                    SecondStarTextBlock.Text = "";
                    ThirdStarTextBlock.Text = "";
                    FourthStarTextBlock.Text = "";
                    FifthStarTextBlock.Text = "";
                    break;
                case "SecondStarSceondQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE7C6";
                    ThirdStarTextBlock.Text = "";
                    FourthStarTextBlock.Text = "";
                    FifthStarTextBlock.Text = "";
                    break;
                case "ThirdStarSecondQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE735";
                    ThirdStarTextBlock.Text = "\xE7C6";
                    FourthStarTextBlock.Text = "";
                    FifthStarTextBlock.Text = "";
                    break;
                case "FourthStarSecondQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE735";
                    ThirdStarTextBlock.Text = "\xE735";
                    FourthStarTextBlock.Text = "\xE7C6";
                    FifthStarTextBlock.Text = "";
                    break;
                case "FifthStarSecondQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE735";
                    ThirdStarTextBlock.Text = "\xE735";
                    FourthStarTextBlock.Text = "\xE735";
                    FifthStarTextBlock.Text = "\xE7C6";
                    break;
            }
        }

        private void OnThirdQuarterEntered(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = sender as Grid;

            switch (grid.Name)
            {
                case "FirstStarThirdQuarterGrid":
                    FirstStarTextBlock.Text = "\xF0CC";
                    SecondStarTextBlock.Text = "";
                    ThirdStarTextBlock.Text = "";
                    FourthStarTextBlock.Text = "";
                    FifthStarTextBlock.Text = "";
                    break;
                case "SecondStarThirdQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xF0CC";
                    ThirdStarTextBlock.Text = "";
                    FourthStarTextBlock.Text = "";
                    FifthStarTextBlock.Text = "";
                    break;
                case "ThirdStarThirdQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE735";
                    ThirdStarTextBlock.Text = "\xF0CC";
                    FourthStarTextBlock.Text = "";
                    FifthStarTextBlock.Text = "";
                    break;
                case "FourthStarThirdQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE735";
                    ThirdStarTextBlock.Text = "\xE735";
                    FourthStarTextBlock.Text = "\xF0CC";
                    FifthStarTextBlock.Text = "";
                    break;
                case "FifthStarThirdQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE735";
                    ThirdStarTextBlock.Text = "\xE735";
                    FourthStarTextBlock.Text = "\xE735";
                    FifthStarTextBlock.Text = "\xF0CC";
                    break;
            }
        }

        private void OnFourthQuarterEntered(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = sender as Grid;

            switch (grid.Name)
            {
                case "FirstStarFourthQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "";
                    ThirdStarTextBlock.Text = "";
                    FourthStarTextBlock.Text = "";
                    FifthStarTextBlock.Text = "";
                    break;
                case "SecondStarFourthQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE735";
                    ThirdStarTextBlock.Text = "";
                    FourthStarTextBlock.Text = "";
                    FifthStarTextBlock.Text = "";
                    break;
                case "ThirdStarFourthQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE735";
                    ThirdStarTextBlock.Text = "\xE735";
                    FourthStarTextBlock.Text = "";
                    FifthStarTextBlock.Text = "";
                    break;
                case "FourthStarFourthQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE735";
                    ThirdStarTextBlock.Text = "\xE735";
                    FourthStarTextBlock.Text = "\xE735";
                    FifthStarTextBlock.Text = "";
                    break;
                case "FifthStarFourthQuarterGrid":
                    FirstStarTextBlock.Text = "\xE735";
                    SecondStarTextBlock.Text = "\xE735";
                    ThirdStarTextBlock.Text = "\xE735";
                    FourthStarTextBlock.Text = "\xE735";
                    FifthStarTextBlock.Text = "\xE735";
                    break;
            }
        }
    }
}
