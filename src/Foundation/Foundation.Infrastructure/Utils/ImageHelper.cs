/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: ImageHelper.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Helper  for image
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Convert bitmap image to jpeg format with compress quality parameter
        /// </summary>
        /// <param name="bmp">The bitmap image to be converted</param>
        /// <param name="quality">Quality value, between 1 and 100</param>
        /// <param name="outputStream">The stream that the converted image will be saved in</param>
        public static void Bmp2Jpg(Bitmap bmp, int quality, Stream outputStream)
        {
            if (quality > 100)
                quality = 100;
            if (quality <= 0)
                quality = 1;

            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            bmp.Save(outputStream, GetImageEncoder(ImageFormat.Jpeg), encoderParameters);
        }

        /// <summary>
        /// Get the conresponding image encoder of the specified image format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetImageEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// get thumbnail image which keep the width/height ratio
        /// </summary>
        /// <param name="imageSize"></param>
        /// <param name="targetSize"></param>
        /// <returns></returns>
        public static Image GenerateThumbnailImage(Image sourceImage, Size targetSize)
        {
            var imageSize = sourceImage.Size;

            decimal ratioX = (decimal)imageSize.Width / targetSize.Width;
            decimal ratioY = (decimal)imageSize.Height / targetSize.Height;

            if (ratioX <= 1 && ratioY <= 1)
            {
                return sourceImage;
            }
            else
            {
                decimal ratio = ratioX > ratioY ? ratioX : ratioY;

                targetSize = new Size(Convert.ToInt32(Math.Ceiling(imageSize.Width / ratio)), Convert.ToInt32(Math.Ceiling(imageSize.Height / ratio)));

                return sourceImage.GetThumbnailImage(targetSize.Width, targetSize.Height, () => { return true; }, IntPtr.Zero);
            }
        }
    }
}
