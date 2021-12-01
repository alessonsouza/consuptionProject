/*########################################################################

  The contents of this file are subject to the Mozilla Public License
  Version 1.0(the "License");   You  may  NOT  use this file except in
  compliance with the License. You may obtain a copy of the License at
                http:// www.mozilla.org/MPL/
  Software distributed under the License is distributed on an "AS IS"
  basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See
  the License for the specific language governing rights and limitations
  under the License.

  The Initial Developer of the Original Code is Scott Robert Johnston.

  Copyright(C) 2004-2005. All Rights Reserved.

  Authors: Scott Robert Johnston
           
  This file contains functions that manipulate , extract features and match the 
  fingerprint image

########################################################################*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FingerAppNet
{
	/// <summary>
	/// Summary description for CFingerPrintGraphics.
	/// </summary>
	/// 
	

	public class CFingerPrintGraphics
	{
		public int FP_IMAGE_WIDTH = 323;
		public int FP_IMAGE_HEIGHT = 352;

		public CFingerPrintGraphics()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public CFingerPrintGraphics(int width , int height )
		{
			FP_IMAGE_WIDTH = width;
			FP_IMAGE_HEIGHT = height;
		}

		public Bitmap getGreyFingerPrintImage(Bitmap m_original_image)
		{
			Bitmap m_result_image = new Bitmap(FP_IMAGE_WIDTH,FP_IMAGE_HEIGHT);
			// float mask[] = {  -1f/5,1f/5,-1f/5
			//                    ,1f/5,9f/5,1f/5
			//                    ,-1f/5,1f/5,-1f/5};    
      
			//     float mask[] = { 
			//       5f/100,10f/100,5f/100
			//      ,10f/100,40f/100,10f/100
			//      ,5f/100,10f/100,5f/100
			//                 };    
     
			//float mask[] = {  -1f/4,0,0,0,0,0,0
			//                  ,0,-1f/2,0,0,0,0,0
			//                  ,0,0,-1f/4,0,0,0,0
			//                  ,0,0,0,1,0,0,0
			//                  ,0,0,0,0,0,0,0
			//                  ,0,0,0,0,0,0,0
			//                  ,0,0,0,0,0,0,0
              
			// };    
   
			float[] mask = 
			{
				1f/16,0f,-1f/16
				,2f/16,0f,-2f/16
				,1f/16,0f,-1f/16
                      
			};
                 
			System.Drawing.Drawing2D.Matrix k = new Matrix(); 
			
			//Kernel k = new Kernel(3,3, mask);                
			//BufferedImageOp con = new ConvolveOp(k, ConvolveOp.EDGE_NO_OP,null);
			//con.filter(m_original_image , m_result_image);
			/*
			   for (int i = 0; i<= FP_IMAGE_WIDTH-1;i++)
			   {
				  for (int j = 0;j<= FP_IMAGE_HEIGHT-1;j++)
				  {
			  Color c = new Color(m_result_image.getRGB(i,j));
					  if ((c.getBlue()  == 0) && (c.getRed()  == 0) && (c.getGreen()  == 0))
					  {
						m_result_image.setRGB(i,j,Color.blue.getRGB());
					  }
					  else
					  {
						m_result_image.setRGB(i,j,Color.white.getRGB());
					  }

				  }//end for j
			  }//end for i
		*/
       
			return m_result_image;
		}//getGreyFingerPrintImage
	}
}
