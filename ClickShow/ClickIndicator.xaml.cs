﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClickShow
{
    /// <summary>
    /// Interaction logic for ClickIndicator.xaml
    /// </summary>
    public partial class ClickIndicator : Window
    {
        private Storyboard _storyboard;

        public ClickIndicator()
        {
            ShowActivated = false;
            InitializeComponent();

            SourceInitialized += OnSourceInitialized;
            DpiChanged += OnDpiChanged;

            RenderOptions.SetBitmapScalingMode(TheCircle, BitmapScalingMode.LowQuality);

            // 初始化动画
            double interval = 0.4;
            _storyboard = new Storyboard();
            _storyboard.FillBehavior = FillBehavior.Stop;


            var widthAnimation = new DoubleAnimation(toValue: this.Width, new Duration(TimeSpan.FromSeconds(interval)));
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath("Width"));
            Storyboard.SetTarget(widthAnimation, TheCircle);
            _storyboard.Children.Add(widthAnimation);

            var heightAnimation = new DoubleAnimation(toValue: this.Height, new Duration(TimeSpan.FromSeconds(interval)));
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath("Height"));
            Storyboard.SetTarget(heightAnimation, TheCircle);
            _storyboard.Children.Add(heightAnimation);

            var opacityAnimation = new DoubleAnimation(toValue: 0, new Duration(TimeSpan.FromSeconds(interval)));
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));
            Storyboard.SetTarget(opacityAnimation, TheCircle);
            _storyboard.Children.Add(opacityAnimation);

            _storyboard.Completed += StoryboardOnCompleted;
            if (_storyboard.CanFreeze)
            {
                _storyboard.Freeze();
            }

            //Play();
        }

        private void OnDpiChanged(object sender, DpiChangedEventArgs e)
        {
            DpiHasChanged = true;
            _currentDpi = e.NewDpi;
        }

        public double GetDpiScale()
        {
            if (_currentDpi.DpiScaleX < 0.1)
            {
                _currentDpi = VisualTreeHelper.GetDpi(this);

                
            }

            return _currentDpi.DpiScaleX;
        }


        public bool IsIdle { get; private set; } = false;

        private DpiScale _currentDpi;

        

        public bool DpiHasChanged { get; private set; } = false;

        public void Prepare()
        {
            IsIdle = false;
        }

        public void Play(Brush circleBrush)
        {
            Opacity = 1;
            TheCircle.Stroke = circleBrush;

            IsIdle = false;

            _storyboard.Begin();

            this.Show();
        }



        private void OnSourceInitialized(object sender, EventArgs e)
        {
            WindowHelper.SetWindowExTransparent(new WindowInteropHelper(this).Handle);
        }

        private void StoryboardOnCompleted(object sender, EventArgs e)
        {

            TheCircle.Width = 0;
            TheCircle.Height = 0;
            Opacity = 0;

            IsIdle = true;
            DpiHasChanged = false;
        }
    }
}
