﻿using System;
using SciChart.Examples.Demo.Fragments.Base;
using SciChart.iOS.Charting;
using Xamarin.Examples.Demo.iOS.Resources.Layout;
using Xamarin.Examples.Demo.iOS.Views.Base;

namespace Xamarin.Examples.Demo.iOS.Views.Examples
{
    [ExampleDefinition("Stacked Bar Chart", description: "Demonstrates a Stacked Bar Chart", icon: ExampleIcon.StackedBar)]
    public class StackedBarChartViewController : ExampleBaseViewController
    {
        public override Type ExampleViewType => typeof(SingleChartViewLayout);

        public SCIChartSurface Surface => ((SingleChartViewLayout)View).SciChartSurface;

        protected override void InitExample()
        {
            var xAxis = new SCINumericAxis { AxisAlignment = SCIAxisAlignment.Left };
            var yAxis = new SCINumericAxis { AxisAlignment = SCIAxisAlignment.Bottom, FlipCoordinates = true };

            var yValues1 = new[] { 0.0, 0.1, 0.2, 0.4, 0.8, 1.1, 1.5, 2.4, 4.6, 8.1, 11.7, 14.4, 16.0, 13.7, 10.1, 6.4, 3.5, 2.5, 5.4, 6.4, 7.1, 8.0, 9.0 };
            var yValues2 = new[] { 2.0, 10.1, 10.2, 10.4, 10.8, 1.1, 11.5, 3.4, 4.6, 0.1, 1.7, 14.4, 16.0, 13.7, 10.1, 6.4, 3.5, 2.5, 1.4, 0.4, 10.1, 0.0, 0.0 };
            var yValues3 = new[] { 20.0, 4.1, 4.2, 10.4, 10.8, 1.1, 11.5, 3.4, 4.6, 5.1, 5.7, 14.4, 16.0, 13.7, 10.1, 6.4, 3.5, 2.5, 1.4, 10.4, 8.1, 10.0, 15.0 };

            var ds1 = new XyDataSeries<double, double> { SeriesName = "data 1" };
            var ds2 = new XyDataSeries<double, double> { SeriesName = "data 2" };
            var ds3 = new XyDataSeries<double, double> { SeriesName = "data 3" };

            for (var i = 0; i < yValues1.Length; i++)
            {
                ds1.Append(i, yValues1[i]);
                ds2.Append(i, yValues2[i]);
                ds3.Append(i, yValues3[i]);
            }

            var series1 = GetRenderableSeries(ds1, 0xFF567893, 0xFF567893, 0xFF3D5568);
            var series2 = GetRenderableSeries(ds2, 0xFFACBCCA, 0xFFACBCCA, 0xFF439AAF);
            var series3 = GetRenderableSeries(ds3, 0xFFDBE0E1, 0xFFDBE0E1, 0xFFB6C1C3);

            var columnsCollection = new SCIVerticallyStackedColumnsCollection();
            columnsCollection.Add(series1);
            columnsCollection.Add(series2);
            columnsCollection.Add(series3);

            var animation = new SCIWaveRenderableSeriesAnimation(3, SCIAnimationCurve.EaseOut);
            animation.StartAfterDelay(0.3f);
            columnsCollection.AddAnimation(animation);

            using (Surface.SuspendUpdates())
            {
                Surface.XAxes.Add(xAxis);
                Surface.YAxes.Add(yAxis);
                Surface.RenderableSeries.Add(columnsCollection);
                Surface.ChartModifiers = new SCIChartModifierCollection
                {
                    new SCICursorModifier(),
                    new SCIZoomExtentsModifier()
                };
            }
        }

        private SCIStackedColumnRenderableSeries GetRenderableSeries(IDataSeries dataSeries, uint strokeColor, uint fillColorStart, uint fillColorEnd)
        {
            return new SCIStackedColumnRenderableSeries
            {
                DataSeries = dataSeries,
                DataPointWidth = 0.8,
                StrokeStyle = new SCISolidPenStyle(strokeColor, 1f),
                FillBrushStyle = new SCILinearGradientBrushStyle(fillColorStart, fillColorEnd, SCILinearGradientDirection.Vertical)
            };
        }
    }
}