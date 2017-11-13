# Project-Iris
Algorithmic model for efficient representation and accurate recognition of Iris

Perform Segmentation using Contour Deection along with Hough transform. The segmented Iris isnormalized using Daugman’s
rubber-sheet model and then apply Gabor filters to obtain the Iris code.
Matching of iris images is done by computing their Hamming distance. 
This was implemented using EmguCV, a C# wrapper for OpenCV.
The WPF application demo for users to register Iris into the DB and also look for a match to a iris
existing in the DB.
IrisMatchReport Generator generates report of successful and failed matches given an Iris database. We performed the matching on the Iris database obtained from IIT Delhi.
