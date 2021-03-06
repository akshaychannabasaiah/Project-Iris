﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.GPU;
using System.Speech;
using System.Speech.Synthesis;
using System.Drawing;

namespace Iris_Recognition
{
    enum HoughTransformFlag { Pupil, IrisOuterBoundary };
    public class IrisConstants
    {
        public static int imageWidth = 320;
        public static int imageHeight = 240;
        public static int SmoothConstant = 9;
        // this range is to search for black part of image (pupil)
        public static MCvScalar LowerBoundForMask = new MCvScalar(0, 0, 0);
        public static MCvScalar UpperBoundForMask = new MCvScalar(50, 50, 50);
        //Used first in Area of moments,'
        //getting center of pupil
        public static int Zero = 0;
        public static int MaxPupilArea = 9500;
        public static int MinPupilEyelashAreaCombined = 11000;
        public static int MinPupilArea = 2200;
        //Used in getting center of pupil
        public static int One = 1;
        //Constants for hough Circle
        public static int MinPupilHoughCircleAccumulator = 190;
        public static int HoughCircleThreshold = 149;
        public static int MaxPupilHoughCircleAccumulator = 300;
        public static double PupilHoughCircleResolution = 2.25;
        public static double MinPupilHoughCircleDistance = 10.0;
        public static int MinPupilHoughCircleRadius = 1;
        public static int MaxPupilHoughCircleRadius = 120;
        public static int MinOuterBoundaryHoughCircleAccumulator = 50;
        public static int MaxOuterBoundaryHoughCircleAccumulator = 400;
        public static double OuterBoundaryHoughCircleResolution = 3.4;
        public static double MinOuterBoundaryHoughCircleDistance = 30.0;
        public static int MaxOuterBoundaryHoughCircleRadius = 125;
        public static int MinOuterBoundaryHoughCircleRadius = 95;
        //used for segmentation
        public static MCvScalar WhiteColor = new MCvScalar(255, 255, 255);
        public static double ScaleMagnitude = 60.0;
        public static int Interpolation = 2;
        public static int Warp = 0;
        //used for cutting the normalised image
        public static int CutImageWidth = 60;
        //-----------------------------------------------------------------------------------------------------------------------------------------------
        //-----------GABOR FILTERS---------//
        //-----------------------------------------------------------------------------------------------------------------------------------------------
        public static Point GaborFilterAnchor = new Point(-1, -1);  //anchor for filter2D function

        //Filter 1- real
        public static Matrix<double> filter1r = new Matrix<double>(new double[9, 15] { { -0.25, -0.25, -0.25, -0.25, -0.25, 0.50, 0.50, 0.50, 0.50, 0.50, -0.25, -0.25, -0.25, -0.25, -0.25 }, { -0.25, -0.25, -0.25, -0.25, -0.25, 0.50, 0.50, 0.50, 0.50, 0.50, -0.25, -0.25, -0.25, -0.25, -0.25 }, { -0.25, -0.25, -0.25, -0.25, -0.25, 0.50, 0.50, 0.50, 0.50, 0.50, -0.25, -0.25, -0.25, -0.25, -0.25 },
                    {-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	1.00	,1.00	,1.00	,1.00,	1.00,	-0.50,	-0.50,	-0.50	,-0.50,	-0.50},{-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	1.00	,1.00	,1.00	,1.00,	1.00,	-0.50,	-0.50,	-0.50	,-0.50,	-0.50},{-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	1.00	,1.00	,1.00	,1.00,	1.00,	-0.50,	-0.50,	-0.50	,-0.50,	-0.50},
                    {-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25} });

        //Filter 1-Imaginary
        public static Matrix<double> filter1i = new Matrix<double>(new double[9, 15] { {0.25,	0.25,	0.25,	0.25,	0.25,	-0.50,	-0.50,	0.00,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{0.25,	0.25,	0.25,	0.25,	0.25,	-0.50,	-0.50,	0.00,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{0.25,	0.25,	0.25,	0.25,	0.25,	-0.50,	-0.50,	0.00,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},
                        {0.50,	0.50,	0.50,	0.50,	0.50,	-1.00,	-1.00,	0.00,	1.00,	1.00,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50},{0.50,	0.50,	0.50,	0.50,	0.50,	-1.00,	-1.00,	0.00,	1.00,	1.00,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50},{0.50,	0.50,	0.50,	0.50,	0.50,	-1.00,	-1.00,	0.00,	1.00,	1.00,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50},
                        {0.25,	0.25,	0.25,	0.25,	0.25,	-0.50,	-0.50,	0.00,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{0.25,	0.25,	0.25,	0.25,	0.25,	-0.50,	-0.50,	0.00,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{0.25,	0.25,	0.25,	0.25,	0.25,	-0.50,	-0.50,	0.00,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25} });

        //Filter 2- real
        public static Matrix<double> filter2r = new Matrix<double>(new double[9, 27] { {-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},
                         {-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	-0.50	,-0.50,	-0.50,	-0.50	,-0.50	,-0.50	,-0.50,	-0.50,	-0.50},{-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	-0.50	,-0.50,	-0.50,	-0.50	,-0.50	,-0.50	,-0.50,	-0.50,	-0.50},{-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	-0.50	,-0.50,	-0.50,	-0.50	,-0.50	,-0.50	,-0.50,	-0.50,	-0.50},
                         {-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25} });

        //Filter 2-Imaginary
        public static Matrix<double> filter2i = new Matrix<double>(new double[9, 27] { {0.25,	0.25,	0.25,	0.25,	0.25,	0.25	,0.25,	0.25,	0.25,	-0.50,	-0.50,	-0.50	,-0.50,	0.00,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{0.25,	0.25,	0.25,	0.25,	0.25,	0.25	,0.25,	0.25,	0.25,	-0.50,	-0.50,	-0.50	,-0.50,	0.00,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{0.25,	0.25,	0.25,	0.25,	0.25,	0.25	,0.25,	0.25,	0.25,	-0.50,	-0.50,	-0.50	,-0.50,	0.00,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},
                        {0.50	,0.50,	0.50,	0.50,	0.50,	0.50	,0.50,	0.50,	0.50,	-1.00,	-1.00,	-1.00,	-1.00,	0.00,	1.00,	1.00,	1.00,	1.00,	-0.50,	-0.50,	-0.50	,-0.50,	-0.50,	-0.50,	-0.50	,-0.50	,-0.50},{0.50	,0.50,	0.50,	0.50,	0.50,	0.50	,0.50,	0.50,	0.50,	-1.00,	-1.00,	-1.00,	-1.00,	0.00,	1.00,	1.00,	1.00,	1.00,	-0.50,	-0.50,	-0.50	,-0.50,	-0.50,	-0.50,	-0.50	,-0.50	,-0.50},{0.50	,0.50,	0.50,	0.50,	0.50,	0.50	,0.50,	0.50,	0.50,	-1.00,	-1.00,	-1.00,	-1.00,	0.00,	1.00,	1.00,	1.00,	1.00,	-0.50,	-0.50,	-0.50	,-0.50,	-0.50,	-0.50,	-0.50	,-0.50	,-0.50},
                        {0.25,	0.25,	0.25,	0.25,	0.25	,0.25	,0.25,	0.25,	0.25,	-0.50,	-0.50,	-0.50,	-0.50,	0.00,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{0.25,	0.25,	0.25,	0.25,	0.25	,0.25	,0.25,	0.25,	0.25,	-0.50,	-0.50,	-0.50,	-0.50,	0.00,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{0.25,	0.25,	0.25,	0.25,	0.25	,0.25	,0.25,	0.25,	0.25,	-0.50,	-0.50,	-0.50,	-0.50,	0.00,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25} });

        //Filter3-real

        public static Matrix<double> filter3r = new Matrix<double>(new double[9, 51] { {-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25	,-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50	,0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25	,-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50	,0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25	,-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50	,0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},
                        {-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50	,-0.50	,-0.50	,-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00	,1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50	,-0.50,	-0.50,	-0.50,	-0.50	,-0.50},{-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50	,-0.50	,-0.50	,-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00	,1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50	,-0.50,	-0.50,	-0.50,	-0.50	,-0.50},{-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50	,-0.50	,-0.50	,-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	1.00	,1.00,	1.00,	1.00,	1.00,	1.00,	1.00,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50	,-0.50,	-0.50,	-0.50,	-0.50	,-0.50},
                        {-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25	,-0.25,	-0.25,	-0.25,	-0.25	,-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25	,-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25	,-0.25,	-0.25,	-0.25,	-0.25	,-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25	,-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25	,-0.25,	-0.25,	-0.25,	-0.25	,-0.25,	-0.25,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25	,-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25} });

        //Filter 3-imaginary

        public static Matrix<double> filter3i = new Matrix<double>(new double[9, 51] { {0.25,	0.25,	0.25	,0.25,	0.25,	0.25	,0.25	,0.25	,0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25	,-0.50,	-0.50	,-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	0.00	,0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{0.25,	0.25,	0.25	,0.25,	0.25,	0.25	,0.25	,0.25	,0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25	,-0.50,	-0.50	,-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	0.00	,0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},{0.25,	0.25,	0.25	,0.25,	0.25,	0.25	,0.25	,0.25	,0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25	,-0.50,	-0.50	,-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	0.00	,0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25,	-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25},
                         {0.50,	0.50,	0.50,	0.50,	0.50	,0.50	,0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50	,0.50,	0.50,	0.50,	-1.00,	-1.00,	-1.00,	-1.00,	-1.00,	-1.00,	-1.00,	-1.00,	0.00,	1.00	,1.00,	1.00,	1.00	,1.00,	1.00,	1.00,	1.00,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50	,-0.50	,-0.50,	-0.50,	-0.50	,-0.50,	-0.50,	-0.50,	-0.50},{0.50,	0.50,	0.50,	0.50,	0.50	,0.50	,0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50	,0.50,	0.50,	0.50,	-1.00,	-1.00,	-1.00,	-1.00,	-1.00,	-1.00,	-1.00,	-1.00,	0.00,	1.00	,1.00,	1.00,	1.00	,1.00,	1.00,	1.00,	1.00,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50	,-0.50	,-0.50,	-0.50,	-0.50	,-0.50,	-0.50,	-0.50,	-0.50},{0.50,	0.50,	0.50,	0.50,	0.50	,0.50	,0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50,	0.50	,0.50,	0.50,	0.50,	-1.00,	-1.00,	-1.00,	-1.00,	-1.00,	-1.00,	-1.00,	-1.00,	0.00,	1.00	,1.00,	1.00,	1.00	,1.00,	1.00,	1.00,	1.00,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50	,-0.50	,-0.50,	-0.50,	-0.50	,-0.50,	-0.50,	-0.50,	-0.50},
                         {0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	0.00,	0.50,	0.50	,0.50	,0.50	,0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25	,-0.25	,-0.25,	-0.25,	-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25	,-0.25},{0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	0.00,	0.50,	0.50	,0.50	,0.50	,0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25	,-0.25	,-0.25,	-0.25,	-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25	,-0.25},{0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	0.25,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	-0.50,	0.00,	0.50,	0.50	,0.50	,0.50	,0.50,	0.50,	0.50,	0.50,	-0.25,	-0.25	,-0.25	,-0.25,	-0.25,	-0.25	,-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25,	-0.25	,-0.25}});



        public static int NumGaborFilters = 6;


    }
}
