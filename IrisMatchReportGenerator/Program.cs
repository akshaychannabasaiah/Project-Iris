using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.GPU;
using System.Drawing;
using Iris_Recognition;
using System.Threading;

namespace IrisMatchReportGenerator
{
    class Program
    {
        //set the values for these static variable before runing
        //Used @(verbatim) to skip the escape sequence character "\" that appears in the path
        static string DBRootDirectoryPath = @"C:\Users\AC\Desktop\IIT Delhi Database";
        static int startIndex = 1;
        static int endIndex = 4;
        static int matchcnt = 0, nomatchcnt = 0;

        static DirectoryInfo rootDirInfo = new DirectoryInfo(DBRootDirectoryPath);
        static void Main(string[] args)
        {
            //try
            //{
                //Helper class to perform match
                IrisRecognizer helperObject = new IrisRecognizer();

                DirectoryInfo[] ImageDirs = rootDirInfo.GetDirectories();
                FileInfo[] imageFiles;
                int i;
                //start reading folders
                for (i = startIndex - 1; i < endIndex - 1; i++)
                {
                    imageFiles = ImageDirs[i].GetFiles("*.bmp");
                    Console.WriteLine("Working on Image set : " + ImageDirs[i].Name);

                    //MatchSingleReport(ImageDirs, i, imageFiles);

                    //Thread startThread = new Thread(() => MatchInThreadFalseAccept(ImageDirs, i, imageFiles));
                    //startThread.Start();
                    //Thread.Sleep(new TimeSpan(0, 1, 10));

                    Thread startThread = new Thread(() => MatchInThreadFalseReject(ImageDirs, i, imageFiles));
                    startThread.Start();

                    //using (System.IO.StreamWriter logFileMatch = new System.IO.StreamWriter(ImageDirs[i].FullName + "\\Matched log.txt"))
                    //{
                    //    using (System.IO.StreamWriter logFileNoMatch = new System.IO.StreamWriter(ImageDirs[i].FullName + "\\Not Matched log.txt"))
                    //    {
                    //        for (int j = 0; j < imageFiles.Length; j++)
                    //        {
                    //            Console.WriteLine("Working on Image sub set : " + imageFiles[j].Name);
                    //            for (int k = j; k < imageFiles.Length; k++)
                    //            {
                    //                bool matchResult;
                    //                //matching done here. Make updates to Helper class along at the same time when u make changes in 12april code.
                    //                IrisImage iris1 = new IrisImage(imageFiles[j].FullName);
                    //                IrisImage iris2 = new IrisImage(imageFiles[k].FullName);
                    //                matchResult = helperObject.Match(iris1, iris2);
                    //                if (matchResult)
                    //                {
                    //                    matchcnt++;
                    //                    logFileMatch.WriteLine("Match between Image " + imageFiles[j].Name + " and " +
                    //                        imageFiles[k].Name + " : " + matchResult.ToString() + matchcnt + " " + helperObject.hammingDistance);

                    //                }
                    //                else
                    //                {
                    //                    nomatchcnt++;
                    //                    logFileNoMatch.WriteLine("Match between Image " + imageFiles[j].Name + " and " +
                    //                        imageFiles[k].Name + " : " + matchResult.ToString() + nomatchcnt + " " + helperObject.hammingDistance);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
            //}

            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}

        }

        public static void MatchInThreadFalseAccept(DirectoryInfo[] ImageDirs, int i, FileInfo[] imageFiles)
        {
            IrisRecognizer helperObject = new IrisRecognizer();
            using (System.IO.StreamWriter logFileMatch = new System.IO.StreamWriter(ImageDirs[i].FullName + "\\Cross Matched log.txt"))
            {
                using (System.IO.StreamWriter logFileNoMatch = new System.IO.StreamWriter(ImageDirs[i].FullName + "\\Cross Not Matched log.txt"))
                {
            
                    for (int j = 0; j < imageFiles.Length; j++)
                    {
                        Console.WriteLine("Working on Image sub set : " + imageFiles[j].Name);
                        for (int k = 19; k < 25; k++)
                        {
                            if (k == i)
                                continue;
                            DirectoryInfo[] ImageDirs1 = rootDirInfo.GetDirectories();
                            FileInfo[] imageFiles1 = ImageDirs1[k].GetFiles("*.bmp");
                            bool matchResult;
                            for (int l = 0; l < imageFiles1.Length; l++)
                            {
                                //matching done here. Make updates to Helper class along at the same time when u make changes in 12april code.
                                IrisImage iris1 = new IrisImage(imageFiles[j].FullName);
                                IrisImage iris2 = new IrisImage(imageFiles1[l].FullName);
                                matchResult = helperObject.Match(iris1, iris2);
                                if (matchResult)
                                {
                                    matchcnt++;
                                    logFileMatch.WriteLine("Match between Image " + imageFiles[j].FullName + " and " +
                                        imageFiles1[l].FullName + " : " + matchResult.ToString() + matchcnt + " " + helperObject.hammingDistance);

                                }
                                else
                                {
                                    nomatchcnt++;
                                    logFileNoMatch.WriteLine("Match between Image " + imageFiles[j].FullName + " and " +
                                        imageFiles1[l].FullName + " : " + matchResult.ToString() + nomatchcnt + " " + helperObject.hammingDistance);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void MatchSingleReport(DirectoryInfo[] ImageDirs, int i, FileInfo[] imageFiles)
        {
            IrisRecognizer helperObject = new IrisRecognizer();
            using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(@"C:\Users\AC\Desktop\report.txt",true))
            {
                for (int j = 0; j < imageFiles.Length; j++)
                {
                    IrisImage iris1 = new IrisImage(imageFiles[j].FullName);
                    Console.WriteLine("Working on Image sub set : " + imageFiles[j].Name);
                    IrisDBEntry entry = helperObject.Match(iris1);
                    if (entry != null)
                    {
                        logFile.WriteLine("Match of Image " + imageFiles[j].FullName + " with " + entry.name);
                    }
                    else
                    {
                        logFile.WriteLine("Match of Image " + imageFiles[j].FullName + " NOT MATCH ");
                    }
                    
                }
            }
        }


        public static void MatchInThreadFalseReject(DirectoryInfo[] ImageDirs, int i, FileInfo[] imageFiles)
        {
            IrisRecognizer helperObject = new IrisRecognizer();
            using (System.IO.StreamWriter logFileMatch = new System.IO.StreamWriter(ImageDirs[i].FullName + "\\Matched log.txt"))
            {
                using (System.IO.StreamWriter logFileNoMatch = new System.IO.StreamWriter(ImageDirs[i].FullName + "\\Not Matched log.txt"))
                {
                    for (int j = 0; j < imageFiles.Length; j++)
                    {
                        Console.WriteLine("Working on Image sub set : " + imageFiles[j].Name);
                        for (int k = j; k < imageFiles.Length; k++)
                        {
                            bool matchResult;
                            //matching done here. Make updates to Helper class along at the same time when u make changes in 12april code.
                            IrisImage iris1 = new IrisImage(imageFiles[j].FullName);
                            IrisImage iris2 = new IrisImage(imageFiles[k].FullName);
                            matchResult = helperObject.Match(iris1, iris2);
                            if (matchResult)
                            {
                                matchcnt++;
                                logFileMatch.WriteLine("Match between Image " + imageFiles[j].Name + " and " +
                                    imageFiles[k].Name + " : " + matchResult.ToString() + matchcnt + " " + helperObject.hammingDistance);

                            }
                            else
                            {
                                nomatchcnt++;
                                logFileNoMatch.WriteLine("Match between Image " + imageFiles[j].Name + " and " +
                                    imageFiles[k].Name + " : " + matchResult.ToString() + nomatchcnt + " " + helperObject.hammingDistance);
                            }
                        }
                    }
                }
            }
        }
    }
}
