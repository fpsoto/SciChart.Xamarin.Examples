﻿using System;
using System.Timers;
using SciChart.Examples.Demo.Fragments.Base;
using SciChart.iOS.Charting;
using Xamarin.Examples.Demo.iOS.Resources.Layout;
using Xamarin.Examples.Demo.iOS.Views.Base;

namespace Xamarin.Examples.Demo.iOS.Views.Examples
{
    [ExampleDefinition("FIFO Chart", description: "Demonstrates scrolling charts", icon: ExampleIcon.RealTime)]
    public class FifoChartViewController : ExampleBaseViewController
    {
        public override Type ExampleViewType => typeof(SingleRealtimeChartLayout);

        public SCIChartSurface Surface => ((SingleRealtimeChartLayout)View).SciChartSurface;

        private const int FifoCapacity = 50;
        private const long TimerInterval = 30;
        private const double OneOverTimeInteval = 1.0 / TimerInterval;
        private const double VisibleRangeMax = FifoCapacity * OneOverTimeInteval;
        private const double GrowBy = VisibleRangeMax * 0.1;

        private readonly Random _random = new Random();

        private readonly XyDataSeries<double, double> _ds1 = new XyDataSeries<double, double> { FifoCapacity = FifoCapacity };
        private readonly XyDataSeries<double, double> _ds2 = new XyDataSeries<double, double> { FifoCapacity = FifoCapacity };
        private readonly XyDataSeries<double, double> _ds3 = new XyDataSeries<double, double> { FifoCapacity = FifoCapacity };

        private readonly SCIDoubleRange _xVisibleRange = new SCIDoubleRange(-GrowBy, VisibleRangeMax + GrowBy);

        private double _t = 0;
        private volatile bool _isRunning = false;
        private Timer _timer;

        protected override void InitExample()
        {
            ((SingleRealtimeChartLayout)View).Start.TouchUpInside += (sender, args) => Start();
            ((SingleRealtimeChartLayout)View).Pause.TouchUpInside += (sender, args) => Pause();
            ((SingleRealtimeChartLayout)View).Reset.TouchUpInside += (sender, args) => Reset();

            var xAxis = new SCINumericAxis { VisibleRange = _xVisibleRange, AutoRange = SCIAutoRange.Never };
            var yAxis = new SCINumericAxis { GrowBy = new SCIDoubleRange(0.1, 0.1), AutoRange = SCIAutoRange.Always };

            var rs1 = new SCIFastLineRenderableSeries { DataSeries = _ds1, StrokeStyle = new SCISolidPenStyle(0xFF4083B7, 2f) };
            var rs2 = new SCIFastLineRenderableSeries { DataSeries = _ds2, StrokeStyle = new SCISolidPenStyle(0xFFFFA500, 2f) };
            var rs3 = new SCIFastLineRenderableSeries { DataSeries = _ds3, StrokeStyle = new SCISolidPenStyle(0xFFE13219, 2f) };

            using (Surface.SuspendUpdates())
            {
                Surface.XAxes.Add(xAxis);
                Surface.YAxes.Add(yAxis);
                Surface.RenderableSeries.Add(rs1);
                Surface.RenderableSeries.Add(rs2);
                Surface.RenderableSeries.Add(rs3);
            }
            Start();
        }

        private void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _timer = new Timer(TimerInterval);
                _timer.Elapsed += OnTick;
                _timer.AutoReset = true;
                _timer.Start();
            }
        }

        private void Pause()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _timer.Stop();
                _timer.Elapsed -= OnTick;
                _timer = null;
            }
        }

        private void Reset()
        {
            if (_isRunning)
            {
                Pause();
            }

            _ds1.Clear();
            _ds2.Clear();
            _ds3.Clear();
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            lock (_timer)
            {
                var y1 = 3.0 * Math.Sin(2 * Math.PI * 1.4 * _t) + _random.NextDouble() * 0.5;
                var y2 = 2.0 * Math.Cos(2 * Math.PI * 0.8 * _t) + _random.NextDouble() * 0.5;
                var y3 = 1.0 * Math.Sin(2 * Math.PI * 2.2 * _t) + _random.NextDouble() * 0.5;

                _ds1.Append(_t, y1);
                _ds2.Append(_t, y2);
                _ds3.Append(_t, y3);

                _t += OneOverTimeInteval;
                if (_t > VisibleRangeMax)
                {
                    _xVisibleRange.SetMinMax(_xVisibleRange.Min + OneOverTimeInteval, _xVisibleRange.Max + OneOverTimeInteval);
                }
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            Reset();
        }
    }
}