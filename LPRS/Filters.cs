using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

namespace sample
{
	/// <summary>
	///
	public class Filters
	{
		public Filters()
		{
		
		}
		public static bool Invert(Bitmap b, out int[] outGray) 

		{ 
			int [] newGray = new int[256];

			foreach(int i in newGray)
			{
				newGray[i]=0;
			}

    
			BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), 

				ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb); 

			int stride = bmData.Stride; 

			System.IntPtr Scan0 = bmData.Scan0; 

			unsafe 

			{ 

				byte * p = (byte *)(void *)Scan0; 

				int nOffset = stride - b.Width*3; 

				int nWidth = b.Width; 

				int tt;

				for(int y=0;y < b.Height; ++y)
				{ 

					for(int x=0; x < nWidth; ++x ) 

					{ 

						tt = p[0] = (byte)(255-p[0]);
						p[1] = (byte)(255-p[1]);
						p[2] = (byte)(255-p[2]);

						newGray[tt]++;

						p += 3; 

					} 

					p += nOffset; 

				} 

			} 
			outGray = newGray ;

			b.UnlockBits(bmData); 

			return true; 

		} 

	
		public static bool Gray(Bitmap b, out int[] outGray) 
 
		{
			int [] newGray = new int[256];

			foreach(int i in newGray)
			{
				newGray[i]=0;
			}


			BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), 

				ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb); 

			int stride = bmData.Stride; 

			System.IntPtr Scan0 = bmData.Scan0; 

			unsafe 

			{ 

				byte * p = (byte *)(void *)Scan0; 

				int nOffset = stride - b.Width*3; 

				byte red, green, blue; 

				int tt;

				for(int y=0;y < b.Height; ++y)
				{ 

					for(int x=0; x < b.Width; ++x ) 

					{ 

						blue = p[0]; 

						green = p[1]; 

						red = p[2]; 

						tt = p[0] = p[1] = p[2] = (byte)(.299 * red + .587 * green + .114 * blue); 

						newGray[tt]++;

						p += 3; 

					} 

					p += nOffset; 

				} 

			} 
			outGray =newGray ;

			b.UnlockBits(bmData); 

			return true;
		}
	
		public static bool Brightness(Bitmap b,int[] FrequenceGray,out int[] all,int pixelsSource) 

		{ 

			BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, 

				PixelFormat.Format24bppRgb); 

			int stride = bmData.Stride; 

			System.IntPtr Scan0 = bmData.Scan0; 

			int tt=0;
			int [] gray=new int[256];
			int [] SumGray=new int[256]; 


			unsafe 

			{ 

				byte * p = (byte *)(void *)Scan0; 

				int nOffset = stride - b.Width*3; 

				int nHeight = b.Height;
				int nWidth = b.Width ; 

				foreach(int i in gray)
				{
					gray[i]=0;
				}

				//�ҶȾ��⻯ 
				SumGray[0]=FrequenceGray[0]; 
				for(int i=1;i<256;++i) 
					//�Ҷȼ�Ƶ�����ۼ� 
					SumGray[i]=SumGray[i-1]+FrequenceGray[i];  
				for(int i=0;i<256;++i)
					//��������Ҷ�ֵ 
					SumGray[i]=(int)(SumGray[i]*255/pixelsSource);  

				for(int y=0;y < nHeight;++y)
				{ 

					for(int x=0; x < nWidth; ++x ) 
					{ 
						tt = p[0] = p[1] = p[2] =(byte)(SumGray[p[0]]); 
				
						gray[tt]++;
						p+=3; 

					} 
					p += nOffset; 
				} 
			} 
			
			all=gray;

			b.UnlockBits(bmData); 

			return true; 

		} 

		public static bool modu1(Bitmap image,int r,int g,int b)
		{ 
			if(r==0) r=1;
			if(g==0) g=1;
			if(b==0) b=1;
			BitmapData bmData =  image.LockBits(new Rectangle(0, 0,image.Width , image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			unsafe
			{
				int stride = bmData.Stride;
				System.IntPtr Scan0 = bmData.Scan0;   
				byte * p = (byte *)(void *)Scan0;
				int nOffset = stride - image.Width*3;    
    
				int nWidth=image.Width;
				int nHeight=image.Height;
				int red,green,blue;

				int w0=(int)(nWidth/2);
				int h0=(int)(nHeight/2);
				int r0,t1,t2,kr=r,kg=g,kb=b;
				double z,z0;

				int k1r=r,k1g=g,k1b=b,
					k2r=r,k2g=g,k2b=b,
					k3r=r,k3g=g,k3b=b,
					k4r=r,k4g=g,k4b=b;
    
				if(w0>h0)
					r0=w0;
				else
					r0=h0;
				z0=Math.Sqrt(w0*w0+h0*h0);       
				z0=z0/1.5;

				for(int y=0;y<nHeight;++y)   
				{
					for(int x=0; x < nWidth; ++x )
					{
						red=p[2];
						green=p[1];
						blue=p[0];
      
						t1=x-w0;
						t2=y-h0;

						if(t1<0) t1=0-t1;      
						if(t2<0) t2=0-t2;
      
						z=Math.Sqrt(t1*t1+t2*t2);
						if(z<z0)
						{
							z=z0;
							kr=0;
							kg=0;
							kb=0;
						}
						else
						{
							double xx=(z-z0)/z;
							kr=(int)(15*r*xx*xx);
							kg=(int)(15*g*xx*xx);
							kb=(int)(15*b*xx*xx);
						} 
						red+=kr;
						green+=kg;
						blue+=kb; 

						if(red>255) red=255;
						if(red<0)   red=0;
						if(green>255) green=255;
						if(green<0)   green=0;
						if(blue>255) blue=255;
						if(blue<0)   blue=0;
      
						p[2]=(byte)red;
						p[1]=(byte)green;
						p[0]=(byte)blue;
      
						p+=3;      
					}
					p += nOffset; 
     
				}
			}   
			image.UnlockBits(bmData); 
			return true;
		}

		/// <summary>
		/// ����Բ����
		/// </summary>
		/// <param name="image">ͼ��</param>
		/// <param name="con">����ϵ��</param>
		/// <param name="k">Բ�뾶ϵ��</param>
		/// <returns></returns>
		public static bool ccon(Bitmap image,float con,float k)
		{    
			BitmapData bmData =  image.LockBits(new Rectangle(0, 0,image.Width , image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			unsafe
			{
				int stride = bmData.Stride;
				System.IntPtr Scan0 = bmData.Scan0;   
				byte * p = (byte *)(void *)Scan0;
				int nOffset = stride - image.Width*3;    
    
				int nWidth=image.Width;
				int nHeight=image.Height;
				int red,green,blue;

				int w0=(int)(nWidth/2);
				int h0=(int)(nHeight/2);
				int r0,t1,t2;
				double z,z0,pixel,contrast;
    
				if(w0>h0)
					r0=w0;
				else
					r0=h0;
				z0=Math.Sqrt(w0*w0+h0*h0);       
				z0=z0/k;

				for(int y=0;y<nHeight;++y)   
				{
					for(int x=0; x < nWidth; ++x )
					{
						red=p[2];
						green=p[1];
						blue=p[0];
      
						t1=x-w0;
						t2=y-h0;

						if(t1<0) t1=0-t1;      
						if(t2<0) t2=0-t2;
      
						z=Math.Sqrt(t1*t1+t2*t2);
						if(z>z0)
						{      
							contrast = (z-z0)/z * con ;

							pixel = red-(127-red) * contrast;
							if (pixel < 0) pixel = 0;
							if (pixel > 255) pixel = 255;
							p[2] = (byte) pixel;

							pixel = green-(127-green) * contrast;
							if (pixel < 0) pixel = 0;
							if (pixel > 255) pixel = 255;
							p[1] = (byte) pixel;

							pixel = blue-(127-blue) * contrast;
							if (pixel < 0) pixel = 0;
							if (pixel > 255) pixel = 255;
							p[0] = (byte) pixel;       
						}      
						p+=3;      
					}
					p += nOffset;     
				}
			}   
			image.UnlockBits(bmData); 
			return true;
		}
		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="b"></param>
		/// <param name="nContrast"></param>
		/// <returns></returns>
		public static bool Contrast(Bitmap b, int nContrast)
		{
			if (nContrast < -100) return false;
			if (nContrast >  100) return false;

			double pixel = 0, contrast = (100.0+nContrast)/100.0;

			contrast *= contrast;

			int red, green, blue;
   
			// GDI+ still lies to us - the return format is BGR, NOT RGB.
			BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;
			int h=b.Height,w=b.Width;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;

				for(int y=0;y<h;++y)
				{
					for(int x=0; x < w; ++x )
					{
						blue = p[0];
						green = p[1];
						red = p[2];
    
						pixel = red/255.0;
						pixel -= 0.5;
						pixel *= contrast;
						pixel += 0.5;
						pixel *= 255;
						if (pixel < 0) pixel = 0;
						if (pixel > 255) pixel = 255;
						p[2] = (byte) pixel;

						pixel = green/255.0;
						pixel -= 0.5;
						pixel *= contrast;
						pixel += 0.5;
						pixel *= 255;
						if (pixel < 0) pixel = 0;
						if (pixel > 255) pixel = 255;
						p[1] = (byte) pixel;

						pixel = blue/255.0;
						pixel -= 0.5;
						pixel *= contrast;
						pixel += 0.5;
						pixel *= 255;
						if (pixel < 0) pixel = 0;
						if (pixel > 255) pixel = 255;
						p[0] = (byte) pixel;     

						p += 3;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);

			return true;
		}
 
		public static bool zft(Bitmap b,out int[] all,out int[] rhow, out int[] ghow,out int[] bhow)
		{
			int [] gray=new int[256];
			int [] rr = new int[256];
			int [] gg = new int[256];
			int [] bb = new int[256];
			int tt=0;
			foreach(int i in gray)
			{
				gray[i]=0;
			}
			foreach(int i in rr)
			{
				rr[i]=0;
			}
			foreach(int i in gg)
			{
				gg[i]=0;
			}
			foreach(int i in bb)
			{
				bb[i]=0;
			}

			// GDI+ still lies to us - the return format is BGR, NOT RGB.
			BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;

				byte red, green, blue;
				int nWidth = b.Width;
				int nHeight= b.Height; 
 
				for(int y=0;y<nHeight;++y)
				{
					for(int x=0; x < nWidth; ++x )
					{
						blue = p[0];
						green = p[1];
						red = p[2];

						tt = p[0] = p[1] = p[2]  = (byte)(.299 * red + .587 * green + .114 * blue);
						rr[red]++;
						gg[green]++;
						bb[blue]++;
						gray[tt]++;   //ͳ�ƻҶ�ֵΪtt�����ص���Ŀ  
						p += 3;
					}
					p += nOffset;
				}
			}
			all=gray;
			rhow=rr;
			ghow=gg;
			bhow=bb;

			b.UnlockBits(bmData);

			return true;
		}

	
		/*
		 * ��˹�˲���
		 */	 
		public static bool GaussianFilter(Bitmap b,out int[] outGray)

		{
			BitmapData bmData =  b.LockBits(new Rectangle(0, 0,b.Width , b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
				
			int[] newGray = new int[256];

			foreach(int i in newGray)
			{
				newGray[i]=0;
			}
			
			unsafe
			{
				int stride = bmData.Stride;
				System.IntPtr Scan0 = bmData.Scan0;   
				byte * p = (byte *)(void *)Scan0;
				byte * pp;
				int tt;
				int nOffset = stride - b.Width*3;    
    
				int nWidth=b.Width;
				int nHeight=b.Height;

				long sum= 0;

				int[,] gaussianMatrix= {{1,2,3,2,1},{2,4,6,4,2},{3,6,7,6,3},{2,4,6,4,2},{1,2,3,2,1}};

				for(int y=0;y < nHeight;++y)
				{ 
					for(int x=0; x < nWidth; ++x ) 
					{
						

						if(!(x<=1||x>=nWidth-2||y<=1||y>=nHeight-2))
						{
							pp=p;
							sum=0;
							int dividend =79;
							for(int i = -2;i<= 2;i++)
								for(int j = -2;j<= 2;j++)
								{
									
									pp+=(j*3+stride*i);
									
									sum+=pp[0]*gaussianMatrix[i+2,j+2];
									if (i==0&&j==0) 
									{
										if (pp[0]>240) 
										{
											sum+=p[0]*30;
											dividend+=30;
										}
										else if (pp[0]>230) 
										{
											sum+=pp[0]*20;
											dividend+=20;
										}
										else if (pp[0]>220) 
										{
											sum+=p[0]*15;
											dividend+=15;
										}
										else if (pp[0]>210) 
										{
											sum+=pp[0]*10;
											dividend+=10;
										}
										else if (p[0]>200) 
										{
											sum+=pp[0]*5;
											dividend+=5;
										}
										
										
									}
									
									pp=p;
								}
							sum=sum/dividend;
							if (sum>255) 
							{
								sum = 255;
							}
						
							p[0]=p[1]=p[2]=(byte)(sum);
						  
				
						}
						tt = p[0];
						newGray[tt]++;
						p+=3; 

					} 

					p += nOffset; 

				} 
				
			}
			outGray = newGray;

			b.UnlockBits(bmData); 

			return true; 
		}
	

		
	
	    /*
		 * ��λ����λ��
		 */
		public static bool MarginalFilter(Bitmap b,out int[] outGray,out int outCount,float valve,out int outxu,out int outxd,out int outyl,out int outyr,out int outMaxX,out int outMaxY)

		{
			BitmapData bmData =  b.LockBits(new Rectangle(0, 0,b.Width , b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
				
			int[] newGray = new int[256];

			foreach(int i in newGray)
			{
				newGray[i]=0;
			}
			
			unsafe
			{
				int stride = bmData.Stride;
				System.IntPtr Scan0 = bmData.Scan0;   
				byte * p = (byte *)(void *)Scan0;
				byte * pp;
				int tt;
				int nOffset = stride - b.Width*3;    
    
				int nWidth=b.Width;
				int nHeight=b.Height;
				int Sx = 0;
				int Sy = 0;
				//	float max = 0;
				double sumM = 0;
				double sumCount = 0;
				//sobelģ��
				int[] marginalMx = {-1,0,1,-2,0,2,-1,0,1};
				int[] marginalMy = {1,2,1,0,0,0,-1,-2,-1};


				float[,] m = new float[nHeight,nWidth];
				int[,] dlta = new int[nHeight,nWidth];
				
				int div=7;
				int lv=11;

				long sM=0;
				int ccm=0;
				int pWR,pWL,pWHL,pWHR,pWH;

				int Wmin = 1 ;
				int Wmax = 9 ;
				int Bmin = 1 ;
				int Bmax = 5 ;
				bool getStart ;
				bool[] lineLabel= new bool[(int)(nHeight/div)+1];
				double[] sumC= new double[(int)(nHeight/div)+1];
				int[,] countMatch = new int[(int)(nHeight/div)+1,(int)(nWidth/lv)+1];

				int[,] mark = new int[(int)(nHeight/div)+1,nWidth];

				

				
//////////////////////////////////////////////////////////////////////////
//��ȡͼ���Ե ���Ҷ�ֵ��
//////////////////////////////////////////////////////////////////////////
			
				//sobel����
				for(int y=0;y < nHeight;++y)
				{ 
					for(int x=0; x < nWidth; ++x ) 
					{ 
						if(!(x<=0||x>=nWidth-1||y<=0||y>=nHeight-1))
						{
							pp=p;
							Sx = 0;
							Sy = 0;
							for(int i = -1;i<= 1;i++)
								for(int j = -1;j<= 1;j++)
								{
									pp+=(j*3+stride*i);
									Sx+=pp[0]* marginalMx[(i+1)*3+j+1];
									Sy+=pp[0]* marginalMy[(i+1)*3+j+1];

									pp=p;

								}
						
							m[y,x] =(int)( Math.Sqrt(Sx*Sx+Sy*Sy));

							//��ǿ�׵�
							if (m[y,x]>valve/2) 
							{

								if (p[0]>240) 
								{
									m[y,x]+=valve;
								}
								else if(p[0]>220)
								{
									m[y,x]+=(float)(valve*0.8);
								}
								else if (p[0]>200) 
								{
									m[y,x]+=(float)(valve*0.6);
								}
								else if (p[0]>180) 
								{
									m[y,x]+=(float)(valve*0.4);
								}
								else if (p[0]>160) 
								{
									m[y,x]+=(float)(valve*0.2);
								}
							}
							
							float tan ;
							if (Sx!= 0) 
							{
								tan = Sy/Sx;
							}
							else tan = 10000;
							if (-0.41421356<=tan && tan < 0.41421356) 
							{
								dlta[y,x] = 0;
								//	m[y,x]+=valve;
							
							}
							else if (0.41421356<=tan&&tan<2.41421356) 
							{
								dlta[y,x] = 1;
								//m[y,x] = 0;
								
							}
							else if (tan>=2.41421356||tan<-2.41421356) 
							{
								dlta[y,x] = 2;
								//	m[y,x]+=valve;
								
							}
							else
							{
								dlta[y,x] = 3;
								//m[y,x] = 0;
							}
						
						}
						else
							m[y,x]=0;

						p+=3; 
						if (m[y,x]>0) 
						{
							sumCount++;
							sumM += m[y,x];
						}

					} 

					p += nOffset; 

				} 
				
				//�Ǽ���ֵ���ƺͷ�ֵ
				p = (byte *)(void *)Scan0;
				for(int y=0;y < nHeight;++y)
				{ 
					for(int x=0; x < nWidth; ++x ) 
					{ 

						if (m[y,x]>sumM/sumCount*1.2) 
						{
							p[0] = p[1] = p[2] = (byte)(m[y,x]);
							//m[y,x]=1;
						}
						else 
						{
							m[y,x] = 0;
							p[0] = p[1] = p[2] = 0;
						}
						
						if(x>=1&&x<=nWidth-1&&y>=1&&y<=nHeight-1&&m[y,x]>valve)
						{
							switch(dlta[y,x]) 
							{
								case 0 :
									if (m[y,x]>=m[y,x-1]&&m[y,x]>=m[y,x+1]) 
									{
										p[0] = p[1] = p[2] = 255;
									}
									break;

								case 1:
									if (m[y,x]>=m[y+1,x-1]&&m[y,x]>=m[y-1,x+1])
									{
										p[0] = p[1] = p[2] = 255;
									}
									break;

								case 2:
									if (m[y,x]>=m[y-1,x]&&m[y,x]>=m[y+1,x]) 
									{
										p[0] = p[1] = p[2] = 255;
									}
									break;

								case 3:
									if (m[y,x]>=m[y+1,x+1]&&m[y,x]>=m[y-1,x-1])
									{
										p[0] = p[1] = p[2] = 255;
									}
									break;

							}
							
						}
						if (p[0]==255) 
						{
							m[y,x]=1;
						}
						else
						{
							m[y,x]=0;
							p[0] = p[1] = p[2] = 0;
						}
					
						tt = p[0];
						newGray[tt]++;
						p+=3; 
					}
					p += nOffset; 
		
				}




//////////////////////////////////////////////////////////////////////////	
//ˮƽɨ��(ÿdiv����һ��)��ʶ���ַ�����
//////////////////////////////////////////////////////////////////////////

				for (int i=0;i<(int)(nHeight/div)+1;i++) 
				{
					for ( int j=0;j<(int)(nWidth/lv)+1 ;j++) 
					{
						countMatch[i,j]=0;
					}					
				}

				p = (byte *)(void *)Scan0;
				for(int y=2*div;y < nHeight-2*div;)
				{ 	
					for (int j=0;j<nWidth;j++) 
					{
						mark[y/div,j]=0;
					}
					for (int i =0;i<div;i++) 
					{
						getStart = true;	
						for(pWR=pWL=pWHL=pWHR=pWH=4; pWH< nWidth-1; pWH++ ) 
						{ 
							//���ÿ�еĵ�һ���׵�
							if (getStart) 
							{
								if (m[y+i,pWH]>0) 
								{
									getStart = false;
									pWR=pWL=pWHL=pWHR=pWH;
								}
								else
									continue;
							}
							if (pWR-pWL>nWidth/3||pWHL-pWR>nWidth/3||pWHR-pWHL>nWidth/3) 
							{
								goto L;
							}
							if ( m[y+i,pWH-1]>0&&m[y+i,pWH]<=0)//�ף���>�� 
							{
								pWHR=pWH-1;
								
								if (pWL!=pWHL )
								{
									if( (Wmin<=(pWR-pWL)&&(pWR-pWL)<=Wmax)						
										||( Bmin<=(pWHL-pWR-1)&&(pWHL-pWR-1)<=Bmax)		
										||( Wmin<=(pWHR-pWHL)&&(pWHR-pWHL)<=Wmax ) )
									{
									
										//��¼�õ�
										//if (pWR-pWL+pWHR-pWHL<11) 
										if (-pWL+pWHR<30) 
										{
											double rate1=Wmax/(Math.Abs((pWR-pWL)-(Wmax-Wmin))/2+1);
											double rate2=Wmax/(Math.Abs((pWHR-pWHL)-(Wmax-Wmin))/2+1);
											double rate3=Bmax*3/(pWHL-pWR);
											mark[y/div,pWL+(pWR-pWL)/2]+=(int)(rate3+rate2+rate2);	
										}
									}
									
									if (pWR-pWL>2*lv) 
									{ //�����ף��򣩿���һ���ַ����
										for (int t=pWL+lv/2;t<pWR-lv/2;t+=lv) 
										{
											countMatch[y/div,t/lv]=-1;
										}
										//countMatch[y/div,pWR/lv]=-1;
									}
									if (pWHL-pWR-1>2*lv) 
									{
										for (int t=pWR+lv/2;t<pWHL-lv/2;t+=lv) 
										{
											countMatch[y/div,t/lv]-=1;
										}
										//countMatch[y/div,pWHL/lv]=-1;
									}
									if (pWHR-pWHL>2*lv) 
									{
										for (int t=pWHL+lv/2;t<pWHR-lv/2;t+=lv) 
										{
											countMatch[y/div,t/lv]-=1;
										}
										//countMatch[y/div,pWHR/lv]=-1;
									}
									
								}
								pWR=pWHR;
								pWL=pWHL;
							}
							else if ( m[y+i,pWH-1]<=0&&m[y+i,pWH]>0)//�ڣ���>��
							{
								pWHL=pWH;
							}
						}
					}
				L:y+=div;
				}
		




//////////////////////////////////////////////////////////////////////////
//ȥ������
//////////////////////////////////////////////////////////////////////////
///
				//����������ˮƽ����ȥ��
				int toCheck=-1;

				foreach (int i in sumC) 
				{
					sumC[i]=0;
				}
				sM=0;
				ccm=0;
				
				//�ۼ�������������
				for (int i=2;i<(int)(nHeight/div)-1;i++) 
				{
					toCheck=-1;
					lineLabel[i]=false;
					//sumLX=0;
					pWL=pWR=1;
					getStart = true;
					for (int j=1;j<nWidth;j++)
					{
						//���ÿ�еĵ�һ���׵�
						if (getStart) 
						{
							if (m[i,j]>0) 
							{
								getStart = false;
								pWR=pWL=j;
							}
							else
								continue;
						}

						///*
						if (mark[i,j]>0)
						{
							if (toCheck==-1) 
							{
								toCheck=j;
								continue;
							}
							else 
							{
								if (j-toCheck<=1) 
								{//////////////////////////////////////////////////////////////////////////
									if (countMatch[i,j/lv]>=0) 
									{
										countMatch[i,j/lv]+=(mark[i,toCheck]+mark[i,j]);//�������໥ƥ�䣬�ۼ�2
									}
									toCheck=-1;
									//lineLabel[i]=true;
									continue;
								}
								else
								{
									//mark[i,toCheck]-=(int)(div*0.8);//��ȥ��������
									if (mark[i,toCheck]<(div*0.7)) 
									{
										mark[i,toCheck]=0;
									}
									else
									{
										countMatch[i,j/lv]+=2*mark[i,toCheck];
									}
			
									toCheck=j;
									continue;
								}
							}
						}
					}					

				}
		
				//��ֵ��		
				sM=0;
				ccm=0;
				int va=(int)(lv*div/3);
				int[] countL = new int[(int)(nHeight/div)+1] ;
				
				for (int i=0;i<(int)(nHeight/div)+1;i++) 
				{
					bool ok  ;
					ok = false;
					countL[i]=0;
					lineLabel[i]=false;
					
					for(int j=0; j < (int)(nWidth/lv)+1 ; ++j ) 
					{
						//ͼ���ܱ�������Ϊ��
						if (i==0||i==(int)(nHeight/div)||j==0||j==(int)(nWidth/lv)) 
						{
							countMatch[i,j]=0;
							continue;
						}
						
						if (countMatch[i,j]>va) 
						{//��ֵȥ���� 
							if ( (countMatch[i,j-1]<=va&&countMatch[i,j+1]<=va)||//ȥ��������(ˮƽ��
								(countMatch[i-1,j]<=va&&(countMatch[i+1,j]<=va||(countMatch[i+1,j-1]<=va&&countMatch[i+1,j+1]<=va))) )//ȥ�������㣨��ֱ��			 
							{
								countMatch[i,j]=0;
							}
							else
							{							
								countL[i]+=countMatch[i,j];
								ok=true;
							}
						}
						else
							countMatch[i,j]=0;
					}
					
					if (ok) 
					{
						lineLabel[i]=true;   
						sM+=countL[i];
						ccm++;
					}		
				}
				
				//ȥ���ϰ벿�ִ����������
				
				int v1=0,v2=0;
				int vm1=0,vm2=0;
				int maxL=0,cv=0;
				for (int i=1;i<(int)(nHeight/div)+1;i++) 
				{
					if (lineLabel[i]==true&&lineLabel[i-1]==false) 
					{
						v1=i;
						v2=i;
					}
					else if (lineLabel[i]==false&&lineLabel[i-1]==true) 
					{
						v2=i;
						cv++;
						if (maxL<v2-v1) 
						{
							vm1=v1;
							vm2=v2;
							maxL=v2-v1;
						}	
					}
				}
				if (cv>1&&vm2-vm1>5&&vm1+(vm2-vm1+1)/2<(nHeight/div)/3||vm2-vm1>nHeight/div/2) 
				{
					for (int k=vm1;k<=vm2;k++) 
					{
						lineLabel[k]=false;
					}
				}
				int p1=0,p2=0;
				for (int i=0;i<(int)(nHeight/div)+1;i++)
				{
					if (lineLabel[i]==true) 
					{
						p1=0;p2=0;
						bool ok=false;
						for (int j=1;j<(int)(nWidth/lv)+1;j++) 
						{
							if (countMatch[i,j-1]==0&&countMatch[i,j]>0) 
							{
								p1=p2=j;
							}
							if (countMatch[i,j-1]>0&&countMatch[i,j]==0) 
							{
								p2=j-1;
								if (p2-p1>0) 
								{
									ok=true;
								}
								else
								{
									p2=p1=0;
									countMatch[i,j-1]=0;
								}
							}
						}
						if (!ok&&p2==0&&p1==0) 
						{
							lineLabel[i]=false;
						}
					}
				}


//////////////////////////////////////////////////////////////////////////
//ʹ��2��6����ֶ�λ
//////////////////////////////////////////////////////////////////////////

				int lLenght=5,vLenght=1;
				int maxAverage=0;
				int maxX=0;
				int maxY=0;
				for (int i=0;i<(int)(nHeight/div)+1;i++) 
				{
					if (lineLabel[i]==true&&lineLabel[i+1]==true) 
					{
						for(int j=0; j < (int)(nWidth/lv)-lLenght ; ++j ) 
						{
							int average =countMatch[i,j]  +countMatch[i,j+1] +countMatch[i,j+2]
								+countMatch[i,j+3]+countMatch[i,j+4] +countMatch[i,j+5]// +countMatch[i,j+6]
								+countMatch[i+1,j]+countMatch[i+1,j+1]+countMatch[i+1,j+2]
								+countMatch[i+1,j+3]+countMatch[i+1,j+4] +countMatch[i+1,j+5];// +countMatch[i+1,j+6] ;
							average=average/(lLenght+1)/(vLenght+1);
							if (maxAverage<average) 
							{
								maxAverage=average;
								maxX=i;
								maxY=j;
							}
						}
					}
				}
                

			
				
				
				bool jx1=true,jx2=true;
				int x1=0,x2=0;
				for (int j=0;jx1||jx2;j++)
				{
					if (jx1&&lineLabel[maxX-j]==false)
					{
						jx1=false;
						x1=maxX-j;
					}
					if (jx2&&lineLabel[maxX+j]==false) 
					{
						jx2=false;
						x2=maxX+j;
					}
				}
				for (int i=0;i<x1;i++) 
				{
					lineLabel[i]=false;
				}
				for (int i=x2;i<(int)(nHeight/div)+1;i++) 
				{
					lineLabel[i]=false;
				}
					


//////////////////////////////////////////////////////////////////////////
//Ѱ�ҳ��Ƶ��ı�
//////////////////////////////////////////////////////////////////////////
						
				// λ�õ���	
				int lKZValve=(int)(maxAverage/3);
				int vKZValve=(int)(maxAverage/2.5);
				//int kz1=0,kz2=0;
				int pX1=0,pX2=0,pX3=0,pX4=0,pY1=0,pY2=0,pY3=0,pY4=0; //���������߿�ķ�Χ
				int pXU=0,pXD=0,pYL=0,pYR=0,pXM=0,pYM=0;
				
			



				//��ȥ��������
				bool l=true,r=true;
				pY1=maxY;
				pY4=maxY+lLenght;
				
				for (int j=1;l||r;j++) 
				{
					if (maxY-j<0&&l) 
					{
						l=false;
						//pY1=0;
					}
					else if ( l&&countMatch[maxX,maxY-j]<vKZValve&&countMatch[maxX+1,maxY-j]<vKZValve) 
					{
						if (maxY-j-2>=0&&countMatch[maxX,maxY-j-2]<vKZValve&&countMatch[maxX+1,maxY-j-2]<vKZValve) 
						{
							l=false;
							pY1=maxY-j+1;
						}
						
					}
					if (maxY+lLenght+j>(int)(nWidth/lv)&&r) 
					{
						r=false;
						pY4=(int)(nWidth/lv);
					}
					else if (r&&countMatch[maxX,maxY+lLenght+j]<vKZValve&&countMatch[maxX+1,maxY+lLenght+j]<vKZValve) 
					{
						if (maxY+lLenght+j+2<(int)(nWidth/lv)+1&&countMatch[maxX,maxY+lLenght+j+2]<vKZValve&&countMatch[maxX+1,maxY+lLenght+j+2]<vKZValve) 
						{
						
							r=false;
							pY4=maxY+lLenght+j-1;
						}
					}
				}
				pY2=(pY1+1)*lv;
				pY3=(pY4-1)*lv;
				// ��һ��ȥ������Ҫ�ı���
				bool u=true,d=true;
				pX1=maxX;
				pX4=maxX+vLenght;
				while (u||d) 
				{
					if (u&&pX1-1<0) 
					{
						u=false;
						//pX1=0;
					}
					else if (u&&lineLabel[pX1-1]) 
					{
						bool ok=false;
						for (int j=pY1;j<=pY4;j++) 
						{
							if (pX1-1>=0&&countMatch[pX1-1,j]>lKZValve) 
							{
								ok=true;
								pX1--;
								break;
							}
						}
						if (!ok) 
						{
							u=false;
						}
					}
					else
						u=false;
					
					if (d&&pX4+1>(int)(nHeight/div)) 
					{
						d=false;
					}
					else if (d&&lineLabel[pX4+1]) 
					{
						bool ok=false;
						for (int j=pY1;j<=pY4;j++) 
						{
							if (pX4+1<(int)(nHeight/div)+1&&countMatch[pX4+1,j]>lKZValve) 
							{
								ok=true;
								pX4++;
								break;
							}
						}
						if (!ok) 
						{
							d=false;
						}
					}
					else
						d=false;
				}
				
				pXM=pX1*div+(pX4-pX1)/2*div;
				pYM=pY1*lv+(pY4-pY1)/2*lv;
				//maxX=x1;
				vLenght=x2-x1;
				//ˮƽ�ٵ���
				l=true;r=true;
				while (l||r) 
				{
					if (pY1-1<0&&l) 
					{
						l=false;
					}
					else if(l)
					{
						bool match=false;
						for (int i=0;i<=vLenght;i++) 
						{
							if (countMatch[x1+i,pY1-1]>vKZValve) 
							{
								match=true;
								break;
							}
						}
						if (!match) 
						{
							l=false;
						}
						else 	
							pY1--;

						
					}
					if (pY4+1>(int)(nWidth/lv)&&r) 
					{
						r=false;
					}
					else  if(r)
					{
						bool match=false;
						for (int i=0;i<=vLenght;i++) 
						{
							if (countMatch[x1+i,pY4+1]>vKZValve) 
							{
								match=true;
								break;
							}
						}
						if (!match) 
						{
							r=false;
							
						}
						else
							pY4++;
					}
				}
			
				for (int i=0;i<pX1;i++) 
				{
					lineLabel[i]=false;
				}
				for (int i=pX4+1;i<(int)(nHeight/div)+1;i++) 
				{
					lineLabel[i]=false;
				}

				pX2=x1*div-div/2;
				if (pX2<0) {
					pX2=0;
				}
				pX3=x2*div+div/2;
				if (pX3>=nHeight) {
					pX3=nHeight-1;
				}
				pYL=pY1*lv;//-lv;
				bool kz=false;
				for (int i=x1;i<=x2;i++) 
				{
					if (countMatch[i,pY1]>vKZValve) 
					{
						kz=true;
						break;
					}
					if (pY1-1>=0&&countMatch[i,pY1-1]>vKZValve) 
					{
						pYL-=lv;
						break;
					}
				}
				if (kz) 
				{
					pYL-=lv/2;
				}
				if (pYL<=0) {
					pYL=0;
				}
				pYR=(pY4+1)*lv+lv/2;//+lv;
				kz=false;
				for (int i=x1;i<=x2;i++) {
					if (pY4+1<(int )(nWidth/lv)+1&& countMatch[i,pY4+1]>vKZValve) {
						kz=true;
						break;
					}
					if (pY4+2<(int )(nWidth/lv)+1&&countMatch[i,pY4+2]>vKZValve) 
					{
						pYR+=lv;
						break;
					}
				}
				if (kz) {
					pYR+=lv/2;
				}
				if (pYR>=nWidth) {
					pYR=nWidth-1;
				}
				if (pX4-pX1<=3) 
				{
					if (pX1-1>=0) 
					{
						pXU=(pX1-1)*div;
					}
					else
						pXU=0;
					if (pX4+2>=(int)(nHeight/div)) 
					{
						pXD=nHeight;
					}
					else
						pXD=(int)((pX4+1.5)*div+div);
				}
				else if (4<=pX4-pX1&&pX4-pX1<=5) 
				{
					pXU=pX1*div-div/2;
					pXD=(pX4+1)*div+div/2;
				}
				else 
					pXU=pX1*div;
				pXD=(int)((pX4+1.5)*div-div/2);

				pXD+=div/2;
				if (pXD>nHeight-1) {
					pXD=nHeight-1;
				}
				pYL-=lv/2;
				if (pYL<0) {
					pYL=0;
				}
				
				//������ȡ�ı�Ե
				LR(m,pX2,pX3,pYL,pYR,out pYL,out pYR);
				UD(m,pXU,pXD,pYL,pYR,out pXU,out pXD);


///////////////////////////////////////////////////////////////////////////
//��ͼ������Ӹ�����
//////////////////////////////////////////////////////////////////////////	
				
				//��ʾ�߿�
				p = (byte *)(void *)Scan0;
				pp = p;
				for (int i=0;i<nHeight;i++) 
				{
				
					if(i==pXU||i==pXD)
					{
						for (int j=pYL;j<=pYR;j++) 
						{
							pp=p+i*stride+j*3;
							pp[2]=255;pp[0]=pp[1]=0;
						}
					}
					else if (pXU<i&&i<pXD) 
					{
					
						pp=p+i*stride+pYL*3;
						pp[2]=255;pp[0]=pp[1]=0;

						pp=p+i*stride+pYR*3;
						pp[2]=255;pp[0]=pp[1]=0;
					}
				}

				//��ȡ��������ʾ��ͼ��
				p = (byte *)(void *)Scan0;
				pp = p;
				for (int i=0;i<(int)(nHeight/div)-1;i++) 
				{
					//������
					for (int k=0;k<nWidth+1;k+=lv) 
					{
						pp=p+(i*div+div/2)*stride+k*3;
						pp[2]=255;
					}

					//�ڳ�������ˮƽ���򻭳�����
					if (lineLabel[i]) 
					{
					
						for(int j=0; j < nWidth; ++j ) 
						{ 
							pp=p+(i*div+div/2)*stride+j*3;
							pp[2]=255;
						}
					}
				}
				outCount=ccm;
				outCount=maxAverage;
								
				outxu=pXU;
				outxd=pXD;
				outyl=pYL;
				outyr=pYR;
				outMaxX=maxX*div-pXU;
				outMaxY=maxY*lv-pYL;
			}
			outGray = newGray;

			b.UnlockBits(bmData); 

			return true; 
		}

		

		/*
		 * ������������λ��
		 */
		private static bool  LR( float[,] m, int xu,int xd,int yl,int yr,out int pYL,out int pYR)
		{
			int[] projection=new int[yr-yl+1];
			foreach (int i in projection)
			{
				projection[i]=0;
			}
			//��ֱͶӰ
			for (int i=xu;i<=xd;i++) 
			{
				for (int j=yl;j<=yr;j++) 
				{
					if (m[i,j]>0) 
					{
						projection[j-yl]++;
					}
				}
			}
			bool l=true,r=true;
			int temp_yr=yr,temp_yl=yl;
			while (l||r) 
			{
				if (temp_yr-temp_yl<=60) 
				{
					l=r=false;
				}
				if (l&&projection[temp_yl-yl]<5){
					temp_yl++;
				}
				else 
				{
					l=false;
				}
				
				if (r&&projection[temp_yr-yl]<5)
				{
					temp_yr--;
				}
				else 
				{
					r=false;
				}	
			}

			pYL=temp_yl;
			pYR=temp_yr;

			return true;
		}
	
	
	
		/*
		 * ������������λ��
		 */
		private static bool UD(float[,] m,int pXU,int pXD,int pYL,int pYR,out int  xu,out int xd)
		{
			int[] projection=new int[pXD-pXU+1];
			foreach (int i in projection)
			{
				projection[i]=0;
			}
			//ˮƽͶӰ
			for (int i=pXU;i<=pXD;i++) {
				for (int j=pYL;j<=pYR;j++) {
					if (m[i,j]>0) 
					{
						projection[i-pXU]++;
					}
				}
			}
			bool u=true,d=true;
			int temp_xd=pXD-1,temp_xu=pXU+1;
			while (u||d) 
			{
				if (temp_xd-temp_xu<=60) 
				{
					u=d=false;
				}
				if (u&&projection[temp_xu-pXU]<2)
				{
					temp_xu++;
				}
				else 
				{
					u=false;
				}
				
				if (d&&projection[temp_xd-pXU]<2)
				{
					temp_xd--;
				}
				else 
				{
					d=false;
				}	
			}
			xu=temp_xu;

			xd=temp_xd;

			return true;
		}

		
		/*
		 * �Գ���ͼ���ֵ��
		 */
		public static bool TowValue(Bitmap b,Bitmap c_Bitmap, int maxX,int maxY)
		{ 
			
			int[] c_gray=new int[256];
			int[] c_r=new int[256];
			int[] c_g=new int[256];
			int[] c_b=new int[256];

			zft(b,out c_gray,out c_r,out c_g,out c_b);
			//��ȡ����С����ĻҶȽ�

			int Mr=0;//�ҶȾ�ֵ
			long sum=0;
			int count=0;
			for (int i=0;i<256;i++) {
				sum+=c_gray[i]*i;
				count+=c_gray[i];
			}
			Mr=(int)(sum /count);
			int sum1=0;
			int count1=0;
			for (int i=0;i<=Mr;i++) {
				sum1+=c_gray[i]*i;
				count1+=c_gray[i];
			}
			int g1=sum1/count1;

			int sum2=0;
			int count2=0;
			for (int i=Mr;i<=255;i++) 
			{
				sum2+=c_gray[i]*i;
				count2+=c_gray[i];
			}
			int g2=sum2/count2;
			
			//��ֵ
			int va;
			if (count1<count2) {//�׵׺���
				va=Mr-count1/count2*Math.Abs(g1-Mr);
			}
			else                //�ڵװ���
				va=Mr+count2/count1*Math.Abs(g2-Mr);

			//��ͼ���ֵ��
			BitmapData bmData =  b.LockBits(new Rectangle(0, 0,b.Width , b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);				
			unsafe
			{
				int stride = bmData.Stride;
				System.IntPtr Scan0 = bmData.Scan0;   
				byte * p = (byte *)(void *)Scan0;
				int nOffset = stride - b.Width*3;    
    
				int nWidth=b.Width;
				int nHeight=b.Height;

				
				for(int y=0;y < nHeight;++y)
				{ 
					for(int x=0; x < nWidth; ++x ) 
					{
						if (p[0]>va) 
						{
							p[0]=p[1]=p[2]=255;
						}
						else
							p[0]=p[1]=p[2]=0;

						p+=3; 
					} 
					p += nOffset; 

				} 
				
			}
			b.UnlockBits(bmData); 

			return true; 

		}
	}
}
