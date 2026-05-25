using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using SkiaSharp;

namespace General.Backend.Domain.Shared.Helper
{
    public partial class CaptchaHelper
    {
        private const string Letters = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";

        /// <summary>
        /// 生成随机验证码字符
        /// </summary>
        /// <param name="codeLength">验证码位数</param>
        /// <returns></returns>
        public static string GenerateRandomCaptcha(int codeLength = 4)
        {
            var array = Letters.Split(new[] { ',' });
            var random = new Random();
            var temp = -1;
            var captcheCode = string.Empty;
            for (int i = 0; i < codeLength; i++)
            {
                if (temp != -1)
                {
                    random = new Random(i * temp * unchecked((int)DateTime.UtcNow.Ticks));
                }
                var index = random.Next(array.Length);
                if (temp != -1 && temp == index)
                {
                    return GenerateRandomCaptcha(codeLength);
                }
                temp = index;
                captcheCode += array[index];
            }
            return captcheCode;
        }

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="randomCode"></param>
        /// <returns></returns>
        public static byte[] CreateCaptchaImage(string randomCode = "")
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                System.Runtime.InteropServices.OSPlatform.Windows))
            {
                return CreateCaptchaImageForWindows(randomCode);
            }
            else
            {
                try
                {
                    var verifyCode = new Captcha
                    {
                        SetIsBackgroundLine = true,
                        SetVerifyCodeText = randomCode,

                        SetIsRandomColor = true,
                        SetRandomAngle = 30
                    };
                    verifyCode.SetWith = verifyCode.SetVerifyCodeText.Length * 16;
                    verifyCode.SetHeight = 28;
                    verifyCode.SetFontSize = 14;
                    verifyCode.SetForeNoisePointCount = 0;
                    verifyCode.SetDisturbStyleLine = false;

                    var bytes = verifyCode.GetVerifyCodeImage();
                    return bytes;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="randomCode"></param>
        /// <returns></returns>
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public static byte[] CreateCaptchaImageForWindows(string randomCode)
        {
            // 随机转动角度
            const int randAngle = 45;
            int mapwidth = randomCode.Length * 16;
            // 创建图片背景
            var map = new Bitmap(mapwidth, 28);
            Graphics graph = Graphics.FromImage(map);
            // 清除画面，填充背景
            graph.Clear(Color.AliceBlue);
            var random = new Random();
            // 绘制干扰曲线
            for (int i = 0; i < 2; i++)
            {
                var p1 = new Point(0, random.Next(map.Height));
                var p2 = new Point(random.Next(map.Width), random.Next(map.Height));
                var p3 = new Point(random.Next(map.Width), random.Next(map.Height));
                var p4 = new Point(map.Width, random.Next(map.Height));
                Point[] p = { p1, p2, p3, p4 };
                using (var pen = new Pen(Color.Gray, 1))
                {
                    graph.DrawBeziers(pen, p);
                }
            }
            // 文字距中
            using (StringFormat format = new StringFormat(StringFormatFlags.NoClip))
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                // 定义颜色
                Color[] c = { Color.Black, Color.Red, Color.DarkBlue, 
                    Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
                // 定义字体
                string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
                int cindex = random.Next(7);
                // 验证码旋转，防止机器识别
                // 拆散字符串成单字符数组
                char[] chars = randomCode.ToCharArray();
                foreach (char t in chars)
                {
                    int findex = random.Next(5);
                    // 字体样式(参数2为字体大小)
                    using (Font font = new Font(fonts[findex], 14, FontStyle.Bold))
                    {
                        using (Brush brush = new SolidBrush(c[cindex]))
                        {
                            Point dot = new Point(14, 14);
                            // 转动的度数
                            float angle = random.Next(-randAngle, randAngle);
                            if (t == '+' || t == '-' || t == '*')
                            {
                                // 加减乘运算符不进行旋转
                                // 移动光标到指定位置
                                graph.TranslateTransform(dot.X, dot.Y);
                                graph.DrawString(t.ToString(), font, brush, 1, 1, format);
                                // 移动光标到指定位置，每个字符紧凑显示，避免被软件识别
                                graph.TranslateTransform(-2, -dot.Y);
                            }
                            else
                            {
                                // 移动光标到指定位置
                                graph.TranslateTransform(dot.X, dot.Y);
                                graph.RotateTransform(angle);
                                graph.DrawString(t.ToString(), font, brush, 1, 1, format);
                                // 转回去
                                graph.RotateTransform(-angle);
                                // 移动光标到指定位置，每个字符紧凑显示，避免被软件识别
                                graph.TranslateTransform(-2, -dot.Y);
                            }
                        }
                    }
                }
            }
            // 生成图片
            using (var ms = new MemoryStream())
            {
                map.Save(ms, ImageFormat.Gif);
                graph.Dispose();
                map.Dispose();
                return ms.GetBuffer();
            }
        }

        public class Captcha
        {
            private readonly Random objRandom = new();

            /// <summary>
            /// //验证码长度
            /// </summary>
            public int SetLength { get; set; } = 4;

            /// <summary>
            /// 验证码字符串
            /// </summary>
            public string SetVerifyCodeText { get; set; } = string.Empty;

            /// <summary>
            /// 是否加入小写字母
            /// </summary>
            public bool SetAddLowerLetter { get; set; } = true;

            /// <summary>
            /// 是否加入大写字母
            /// </summary>
            public bool SetAddUpperLetter { get; set; } = true;

            /// <summary>
            /// 字体大小
            /// </summary>
            public int SetFontSize { get; set; } = 36;

            /// <summary>
            ///  //字体颜色
            /// </summary>
            public SKColor SetFontColor { get; set; } = SKColors.Blue;

            /// <summary>
            /// 字体类型
            /// </summary>
            public string SetFontFamily = "Verdana";

            /// <summary>
            /// 背景色
            /// </summary>
            public SKColor SetBackgroundColor { get; set; } = SKColors.AliceBlue;

            /// <summary>
            /// 是否加入背景线
            /// </summary>
            public bool SetIsBackgroundLine { get; set; }

            /// <summary>
            /// 设置干扰线
            /// </summary>
            public bool SetDisturbStyleLine { get; set; }

            /// <summary>
            /// 前景噪点数量
            /// </summary>
            public int SetForeNoisePointCount { get; set; } = 2;

            /// <summary>
            /// 随机码的旋转角度
            /// </summary>
            public int SetRandomAngle { get; set; } = 40;

            /// <summary>
            /// 是否随机字体颜色
            /// </summary>
            public bool SetIsRandomColor { get; set; } = true;

            /// <summary>
            /// 图片宽度
            /// </summary>
            public int SetWith { get; set; } = 200;

            /// <summary>
            /// 图片高度
            /// </summary>
            public int SetHeight { get; set; } = 40;

            /// <summary>
            /// 问题验证码答案，适用于运算符
            /// </summary>
            public string VerifyCodeResult { get; private set; } = string.Empty;

            public Captcha(int length = 4, bool isOperation = false)
            {
                if (isOperation)
                {
                    var dic = GetQuestion();
                    SetVerifyCodeText = dic.Key;
                    VerifyCodeResult = dic.Value;
                    SetRandomAngle = 0;
                }
                else
                {
                    SetLength = length;
                    GetVerifyCodeText();
                }
                SetWith = SetVerifyCodeText.Length * SetFontSize;
                SetHeight = Convert.ToInt32(60.0 / 100 * SetFontSize + SetFontSize);
                InitColors();
            }

            /// <summary>
            /// 得到验证码字符串
            /// </summary>
            private void GetVerifyCodeText()
            {
                // 没有外部输入验证码时随机生成
                if (string.IsNullOrEmpty(SetVerifyCodeText))
                {
                    StringBuilder objStringBuilder = new StringBuilder();
                    // 加入数字1-9
                    for (int i = 1; i <= 9; i++)
                    {
                        objStringBuilder.Append(i.ToString());
                    }
                    // 加入大写字母A-Z，不包括O
                    if (SetAddUpperLetter)
                    {
                        char temp = ' ';
                        for (int i = 0; i < 26; i++)
                        {
                            temp = Convert.ToChar(i + 65);
                            // 如果生成的字母不是'O'
                            if (!temp.Equals('O'))
                            {
                                objStringBuilder.Append(temp);
                            }
                        }
                    }
                    // 加入小写字母a-z，不包括o
                    if (SetAddLowerLetter)
                    {
                        char temp = ' ';
                        for (int i = 0; i < 26; i++)
                        {
                            temp = Convert.ToChar(i + 97);
                            //如果生成的字母不是'o'
                            if (!temp.Equals('o'))
                            {
                                objStringBuilder.Append(temp);
                            }
                        }
                    }
                    // 生成验证码字符串
                    {
                        int index = 0;
                        for (int i = 0; i < SetLength; i++)
                        {
                            index = objRandom.Next(0, objStringBuilder.Length);
                            SetVerifyCodeText += objStringBuilder[index];
                            objStringBuilder.Remove(index, 1);
                        }
                    }
                }
            }

            /// <summary>
            /// 获取随机颜色
            /// </summary>
            /// <returns></returns>
            private SKColor GetRandomColor()
            {
                Random random = new Random();
                return Colors2[random.Next(Colors2.Count)];
            }

            /// <summary>
            /// 获取问题
            /// </summary>
            /// <param name="questionList">默认数字加减验证</param>
            /// <returns></returns>
            public KeyValuePair<string, string> GetQuestion(Dictionary<string, string>? questionList = null)
            {
                if (questionList == null)
                {
                    questionList = new Dictionary<string, string>();
                    var operArray = new string[] { "+", "*", "-", "/" };
                    var left = objRandom.Next(0, 10);
                    var right = objRandom.Next(0, 10);
                    var oper = operArray[objRandom.Next(0, operArray.Length)];
                    string key = string.Empty, val = string.Empty;
                    switch (oper)
                    {
                        case "+":
                            key = string.Format("{0}+{1}=?", left, right);
                            val = (left + right).ToString();
                            questionList.Add(key, val);
                            break;
                        case "*":
                            key = string.Format("{0}×{1}=?", left, right);
                            val = (left * right).ToString();
                            questionList.Add(key, val);
                            break;
                        case "-":
                            if (left < right)
                            {
                                var intTemp = left;
                                left = right;
                                right = intTemp;
                            }
                            questionList.Add(left + "-" + right + "= ?", (left - right).ToString());
                            break;
                        case "/":
                            right = objRandom.Next(1, 10);
                            left = right * objRandom.Next(1, 10);
                            questionList.Add(left + "÷" + right + "= ?", (left / right).ToString());
                            break;
                    }
                }
                return questionList.ToList()[objRandom.Next(0, questionList.Count)];
            }

            /// <summary>
            /// 干扰线的颜色集合
            /// </summary>
            private List<SKColor> Colors { get; set; } = new List<SKColor>();

            private List<SKColor> Colors2 { get; set; } = new List<SKColor>();

            public void InitColors()
            {
                Colors2 = new List<SKColor>
                {
                    SKColors.Orange,
                    SKColors.Purple,
                    SKColors.DarkBlue,
                    SKColors.Green,
                    SKColors.RoyalBlue,
                    SKColors.Black,
                    SKColors.Brown
                };

                Colors = new List<SKColor>
                {
                    SKColors.AliceBlue,
                    SKColors.PaleGreen,
                    SKColors.PaleGoldenrod,
                    SKColors.Orchid,
                    SKColors.OrangeRed,
                    SKColors.Orange,
                    SKColors.OliveDrab,
                    SKColors.Olive,
                    SKColors.OldLace,
                    SKColors.Navy,
                    SKColors.NavajoWhite,
                    SKColors.Moccasin,
                    SKColors.MistyRose,
                    SKColors.MintCream,
                    SKColors.MidnightBlue,
                    SKColors.MediumVioletRed,
                    SKColors.MediumTurquoise,
                    SKColors.MediumSpringGreen,
                    SKColors.LightSlateGray,
                    SKColors.LightSteelBlue,
                    SKColors.LightYellow,
                    SKColors.Lime,
                    SKColors.LimeGreen,
                    SKColors.Linen,
                    SKColors.PaleTurquoise,
                    SKColors.Magenta,
                    SKColors.MediumAquamarine,
                    SKColors.MediumBlue,
                    SKColors.MediumOrchid,
                    SKColors.MediumPurple,
                    SKColors.MediumSeaGreen,
                    SKColors.MediumSlateBlue,
                    SKColors.Maroon,
                    SKColors.PaleVioletRed,
                    SKColors.PapayaWhip,
                    SKColors.PeachPuff,
                    SKColors.Snow,
                    SKColors.SpringGreen,
                    SKColors.SteelBlue,
                    SKColors.Tan,
                    SKColors.Teal,
                    SKColors.Thistle,
                    SKColors.SlateGray,
                    SKColors.Tomato,
                    SKColors.Violet,
                    SKColors.Wheat,
                    SKColors.White,
                    SKColors.WhiteSmoke,
                    SKColors.Yellow,
                    SKColors.YellowGreen,
                    SKColors.Turquoise,
                    SKColors.LightSkyBlue,
                    SKColors.SlateBlue,
                    SKColors.Silver,
                    SKColors.Peru,
                    SKColors.Pink,
                    SKColors.Plum,
                    SKColors.PowderBlue,
                    SKColors.Purple,
                    SKColors.Red,
                    SKColors.SkyBlue,
                    SKColors.RosyBrown,
                    SKColors.SaddleBrown,
                    SKColors.Salmon,
                    SKColors.SandyBrown,
                    SKColors.SeaGreen,
                    SKColors.SeaShell,
                    SKColors.Sienna,
                    SKColors.RoyalBlue,
                    SKColors.LightSeaGreen,
                    SKColors.LightSalmon,
                    SKColors.LightPink,
                    SKColors.Crimson,
                    SKColors.Cyan,
                    SKColors.DarkBlue,
                    SKColors.Green,
                    SKColors.RoyalBlue,
                    SKColors.DarkGoldenrod,
                    SKColors.DarkGray,
                    SKColors.Cornsilk,
                    SKColors.DarkGreen,
                    SKColors.DarkMagenta,
                    SKColors.DarkOliveGreen,
                    SKColors.DarkOrange,
                    SKColors.DarkOrchid,
                    SKColors.DarkRed,
                    SKColors.DarkSalmon,
                    SKColors.DarkKhaki,
                    SKColors.DarkSeaGreen,
                    SKColors.CornflowerBlue,
                    SKColors.Chocolate,
                    SKColors.AntiqueWhite,
                    SKColors.Aqua,
                    SKColors.Aquamarine,
                    SKColors.Azure,
                    SKColors.Beige,
                    SKColors.Bisque,
                    SKColors.Coral,
                    SKColors.Black,
                    SKColors.Blue,
                    SKColors.BlueViolet,
                    SKColors.Brown,
                    SKColors.BurlyWood,
                    SKColors.CadetBlue,
                    SKColors.Chartreuse,
                    SKColors.BlanchedAlmond,
                    SKColors.Transparent,
                    SKColors.DarkSlateBlue,
                    SKColors.DarkTurquoise,
                    SKColors.IndianRed,
                    SKColors.Indigo,
                    SKColors.Ivory,
                    SKColors.Khaki,
                    SKColors.Lavender,
                    SKColors.LavenderBlush,
                    SKColors.HotPink,
                    SKColors.LawnGreen,
                    SKColors.LightBlue,
                    SKColors.LightCoral,
                    SKColors.LightCyan,
                    SKColors.LightGoldenrodYellow,
                    SKColors.LightGray,
                    SKColors.LightGreen,
                    SKColors.LemonChiffon,
                    SKColors.DarkSlateGray,
                    SKColors.Honeydew,
                    SKColors.Green,
                    SKColors.DarkViolet,
                    SKColors.DeepPink,
                    SKColors.DeepSkyBlue,
                    SKColors.DimGray,
                    SKColors.DodgerBlue,
                    SKColors.Firebrick,
                    SKColors.GreenYellow,
                    SKColors.FloralWhite,
                    SKColors.Fuchsia,
                    SKColors.Gainsboro,
                    SKColors.GhostWhite,
                    SKColors.Gold,
                    SKColors.Goldenrod,
                    SKColors.Gray,
                    SKColors.ForestGreen
                };
            }

            /// <summary>
            /// 创建画笔
            /// </summary>
            /// <param name="color"></param>
            /// <param name="fontSize"></param>
            /// <returns></returns>
            private static SKPaint CreatePaint(SKColor color, float fontSize)
            {
                SKTypeface font = SKTypeface.FromFamilyName(
                    null, 
                    SKFontStyleWeight.SemiBold, 
                    SKFontStyleWidth.ExtraCondensed, 
                    SKFontStyleSlant.Upright);
                var paint = new SKPaint
                {
                    IsAntialias = true,
                    Color = color,
                    Typeface = font,
                    TextSize = fontSize
                };
                return paint;
            }

            /// <summary>
            /// 获取验证码
            /// </summary>
            /// <param name="captchaText">验证码文字</param>
            /// <param name="width">图片宽度</param>
            /// <param name="height">图片高度</param>
            /// <param name="lineNum">干扰线数量</param>
            /// <param name="lineStrookeWidth">干扰线宽度</param>
            /// <returns></returns>
            public byte[] GetVerifyCodeImage(int lineNum = 1, int lineStrookeWidth = 1)
            {
                // 创建bitmap位图
                using (var image2d = new SKBitmap(
                    SetWith, 
                    SetHeight, 
                    SKColorType.Bgra8888, 
                    SKAlphaType.Premul))
                {
                    // 创建画笔
                    using (SKCanvas canvas = new SKCanvas(image2d))
                    {
                        // 填充背景颜色为白色
                        if (SetIsRandomColor)
                        {
                            SetFontColor = GetRandomColor();
                        }
                        // 填充白色背景
                        canvas.Clear(SetBackgroundColor);
                        AddForeNoisePoint(image2d);
                        AddBackgroundNoisePoint(image2d, canvas);
                        var drawStyle = new SKPaint();
                        drawStyle.IsAntialias = true;
                        drawStyle.TextSize = SetFontSize;
                        char[] chars = SetVerifyCodeText.ToCharArray();
                        for (int i = 0; i < chars.Length; i++)
                        {
                            var font = SKTypeface.FromFamilyName(
                                SetFontFamily, 
                                SKFontStyleWeight.SemiBold, 
                                SKFontStyleWidth.ExtraCondensed, 
                                SKFontStyleSlant.Upright);
                            // 转动的度数
                            float angle = objRandom.Next(-30, 30);
                            canvas.Translate(12, 12);
                            float px = i * SetFontSize;
                            float py = SetHeight / 3;
                            canvas.RotateDegrees(angle, px, py);
                            drawStyle.Typeface = font;
                            drawStyle.Color = SetFontColor;
                            canvas.DrawText(chars[i].ToString(), px, py, drawStyle);
                            canvas.RotateDegrees(-angle, px, py);
                            canvas.Translate(-12, -12);
                        }
                        if (SetDisturbStyleLine)
                        {
                            // 画随机干扰线
                            using (SKPaint disturbStyle = new SKPaint())
                            {
                                Random random = new Random();
                                for (int i = 0; i < lineNum; i++)
                                {
                                    disturbStyle.Color = Colors[random.Next(Colors.Count)];
                                    disturbStyle.StrokeWidth = lineStrookeWidth;
                                    canvas.DrawLine(
                                        random.Next(0, SetWith), 
                                        random.Next(0, SetHeight), 
                                        random.Next(0, SetWith), 
                                        random.Next(0, SetHeight), 
                                        disturbStyle);
                                }
                            }
                        }
                        // 返回图片byte
                        using (SKImage img = SKImage.FromBitmap(image2d))
                        {
                            using (SKData p = img.Encode(SKEncodedImageFormat.Png, 100))
                            {
                                return p.ToArray();
                            }
                        }
                    }
                }
            }

            private void AddForeNoisePoint(SKBitmap objBitmap)
            {
                for (int i = 0; i < objBitmap.Width * SetForeNoisePointCount; i++)
                {
                    objBitmap.SetPixel(objRandom.Next(
                        objBitmap.Width), 
                        objRandom.Next(objBitmap.Height), 
                        SetFontColor);
                }
            }

            private void AddBackgroundNoisePoint(SKBitmap objBitmap, SKCanvas objGraphics)
            {
                using (SKPaint objPen = CreatePaint(SKColors.Azure, 0))
                {
                    for (int i = 0; i < objBitmap.Width * 2; i++)
                    {
                        objGraphics.DrawRect(
                            objRandom.Next(objBitmap.Width), 
                            objRandom.Next(objBitmap.Height), 
                            1, 
                            1, 
                            objPen);
                    }
                }
                if (SetIsBackgroundLine)
                {
                    // 画图片的背景噪音线
                    for (var i = 0; i < 12; i++)
                    {
                        var x1 = objRandom.Next(objBitmap.Width);
                        var x2 = objRandom.Next(objBitmap.Width);
                        var y1 = objRandom.Next(objBitmap.Height);
                        var y2 = objRandom.Next(objBitmap.Height);

                        objGraphics.DrawLine(x1, y1, x2, y2, CreatePaint(SKColors.Silver, 0));
                    }
                }
            }
        }
    }
}
