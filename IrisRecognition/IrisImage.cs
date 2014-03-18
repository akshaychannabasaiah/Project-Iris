using System;
using Emgu.CV;
using System.Drawing;

public class IrisImage
{
    //private Image<> object
    //constructor
	public IrisImage()
	{
        //make modification such that it has to take image from file location, or Database or WEbcam
        //write overloaded constructors.
        //do only what is possible now. leave other cases undefined
	}
    public void GetImageFromWebCam()
    {

    }

    public void GetImageFromFile(string fileLocation)
    {

    }
    
    public void GetImageFromDB(string userName, string password)
    {

    }

    public Image< > GetImage()
    {
        //   return Image<>;
    }
    public void PerformXXoverImage()
    {
        //XX is replaced by various operations,
        //like edge detection etc.
    }

}
